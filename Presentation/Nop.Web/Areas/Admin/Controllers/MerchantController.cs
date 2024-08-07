using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Merchants;
using Nop.Services.Attributes;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Merchants;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Merchants;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class MerchantController : BaseAdminController
{
    #region Fields

    protected readonly IAddressService _addressService;
    protected readonly IAttributeParser<AddressAttribute, AddressAttributeValue> _addressAttributeParser;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerService _customerService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly IPictureService _pictureService;
    protected readonly IMerchantModelFactory _merchantModelFactory;
    protected readonly IMerchantService _merchantService;

    #endregion

    #region Ctor

    public MerchantController(IAddressService addressService,
        IAttributeParser<AddressAttribute, AddressAttributeValue> addressAttributeParser,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IGenericAttributeService genericAttributeService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IPermissionService permissionService,
        IPictureService pictureService,
        IMerchantModelFactory merchantModelFactory,
        IMerchantService merchantService)
    {
        _addressService = addressService;
        _addressAttributeParser = addressAttributeParser;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _genericAttributeService = genericAttributeService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _pictureService = pictureService;
        _merchantModelFactory = merchantModelFactory;
        _merchantService = merchantService;
    }

    #endregion

    #region Utilities

    protected virtual async Task UpdatePictureSeoNamesAsync(Merchant merchant)
    {
        var picture = await _pictureService.GetPictureByIdAsync(merchant.PictureId);
        if (picture != null)
            await _pictureService.SetSeoFilenameAsync(picture.Id, await _pictureService.GetPictureSeNameAsync(merchant.Name));
    }

    #endregion

    #region Merchants

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    public virtual async Task<IActionResult> List()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageMerchants))
            return AccessDeniedView();

        //prepare model
        var model = await _merchantModelFactory.PrepareMerchantSearchModelAsync(new MerchantSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> List(MerchantSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageMerchants))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _merchantModelFactory.PrepareMerchantListModelAsync(searchModel);

        return Json(model);
    }

    public virtual async Task<IActionResult> Create()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageMerchants))
            return AccessDeniedView();

        //prepare model
        var model = await _merchantModelFactory.PrepareMerchantModelAsync(new MerchantModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [FormValueRequired("save", "save-continue")]
    public virtual async Task<IActionResult> Create(MerchantModel model, bool continueEditing, IFormCollection form)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageMerchants))
            return AccessDeniedView();

        if (ModelState.IsValid)
        {
            var merchant = model.ToEntity<Merchant>();
            await _merchantService.InsertMerchantAsync(merchant);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewMerchant",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewMerchant"), merchant.Id), merchant);

            //address
            var address = model.Address.ToEntity<Address>();
            address.CreatedOnUtc = DateTime.UtcNow;

            //some validation
            if (address.CountryId == 0)
                address.CountryId = null;
            if (address.StateProvinceId == 0)
                address.StateProvinceId = null;
            await _addressService.InsertAddressAsync(address);
            merchant.AddressId = address.Id;
            await _merchantService.UpdateMerchantAsync(merchant);

            //update picture seo file name
            await UpdatePictureSeoNamesAsync(merchant);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Merchants.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = merchant.Id });
        }

        //prepare model
        model = await _merchantModelFactory.PrepareMerchantModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    public virtual async Task<IActionResult> Edit(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageMerchants))
            return AccessDeniedView();

        //try to get a merchant with the specified id
        var merchant = await _merchantService.GetMerchantByIdAsync(id);
        if (merchant == null || merchant.Deleted)
            return RedirectToAction("List");

        //prepare model
        var model = await _merchantModelFactory.PrepareMerchantModelAsync(null, merchant);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    public virtual async Task<IActionResult> Edit(MerchantModel model, bool continueEditing, IFormCollection form)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageMerchants))
            return AccessDeniedView();

        //try to get a merchant with the specified id
        var merchant = await _merchantService.GetMerchantByIdAsync(model.Id);
        if (merchant == null || merchant.Deleted)
            return RedirectToAction("List");

        //custom address attributes
        var customAttributes = await _addressAttributeParser.ParseCustomAttributesAsync(form, NopCommonDefaults.AddressAttributeControlName);
        var customAttributeWarnings = await _addressAttributeParser.GetAttributeWarningsAsync(customAttributes);
        foreach (var error in customAttributeWarnings)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        if (ModelState.IsValid)
        {
            var prevPictureId = merchant.PictureId;
            merchant = model.ToEntity(merchant);
            await _merchantService.UpdateMerchantAsync(merchant);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditMerchant",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditMerchant"), merchant.Id), merchant);

            //address
            var address = await _addressService.GetAddressByIdAsync(merchant.AddressId);
            if (address == null)
            {
                address = model.Address.ToEntity<Address>();
                address.CustomAttributes = customAttributes;
                address.CreatedOnUtc = DateTime.UtcNow;

                //some validation
                if (address.CountryId == 0)
                    address.CountryId = null;
                if (address.StateProvinceId == 0)
                    address.StateProvinceId = null;

                await _addressService.InsertAddressAsync(address);
                merchant.AddressId = address.Id;
                await _merchantService.UpdateMerchantAsync(merchant);
            }
            else
            {
                address = model.Address.ToEntity(address);
                address.CustomAttributes = customAttributes;

                //some validation
                if (address.CountryId == 0)
                    address.CountryId = null;
                if (address.StateProvinceId == 0)
                    address.StateProvinceId = null;

                await _addressService.UpdateAddressAsync(address);
            }

            //delete an old picture (if deleted or updated)
            if (prevPictureId > 0 && prevPictureId != merchant.PictureId)
            {
                var prevPicture = await _pictureService.GetPictureByIdAsync(prevPictureId);
                if (prevPicture != null)
                    await _pictureService.DeletePictureAsync(prevPicture);
            }
            //update picture seo file name
            await UpdatePictureSeoNamesAsync(merchant);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Merchants.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = merchant.Id });
        }

        //prepare model
        model = await _merchantModelFactory.PrepareMerchantModelAsync(model, merchant, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Delete(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageMerchants))
            return AccessDeniedView();

        //try to get a merchant with the specified id
        var merchant = await _merchantService.GetMerchantByIdAsync(id);
        if (merchant == null)
            return RedirectToAction("List");

        //clear associated customer references
        var associatedCustomers = await _customerService.GetAllCustomersAsync(merchantId: merchant.Id);
        foreach (var customer in associatedCustomers)
        {
            customer.MerchantId = 0;
            await _customerService.UpdateCustomerAsync(customer);
        }

        //delete a merchant
        await _merchantService.DeleteMerchantAsync(merchant);

        //activity log
        await _customerActivityService.InsertActivityAsync("DeleteMerchant",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteMerchant"), merchant.Id), merchant);

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Merchants.Deleted"));

        return RedirectToAction("List");
    }

    #endregion
}