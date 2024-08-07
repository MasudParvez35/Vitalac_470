using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Plugin.DiscountRules.PhoneNumber.Models;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Vitalac.Validators;

namespace Nop.Plugin.DiscountRules.PhoneNumber.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class DiscountRulesPhoneNumberController : BasePluginController
{
    #region Fields

    protected readonly ICustomerService _customerService;
    protected readonly IDiscountService _discountService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPermissionService _permissionService;
    protected readonly ISettingService _settingService;
    private readonly CustomerSettings _customerSettings;

    #endregion

    #region Ctor

    public DiscountRulesPhoneNumberController(ICustomerService customerService,
        IDiscountService discountService,
        ILocalizationService localizationService,
        IPermissionService permissionService,
        ISettingService settingService,
        CustomerSettings customerSettings)
    {
        _customerService = customerService;
        _discountService = discountService;
        _localizationService = localizationService;
        _permissionService = permissionService;
        _settingService = settingService;
        _customerSettings = customerSettings;
    }

    #endregion

    #region Utilities

    protected IEnumerable<string> GetErrorsFromModelState(ModelStateDictionary modelState)
    {
        return ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
    }

    protected List<string> GetFomattedNumbers(string phoneNumbers)
    {
        if (string.IsNullOrEmpty(phoneNumbers))
            return null;

        return phoneNumbers
            .Replace("\n", ",")
            .Split(new[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries)
            .Where(x => PhoneNumberExtension.IsValid(x, _customerSettings))
            .Select(x => PhoneNumberExtension.Format(x, _customerSettings))
            .Distinct()
            .Order()
            .ToList();
    }

    #endregion

    #region Methods

    public async Task<IActionResult> Configure(int discountId, int? discountRequirementId)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDiscounts))
            return Content("Access denied");

        //load the discount
        var discount = await _discountService.GetDiscountByIdAsync(discountId)
                       ?? throw new ArgumentException("Discount could not be loaded");

        //check whether the discount requirement exists
        if (discountRequirementId.HasValue && await _discountService.GetDiscountRequirementByIdAsync(discountRequirementId.Value) is null)
            return Content("Failed to load requirement.");

        //try to get previously saved restricted customer role identifier
        var phoneNumbers = await _settingService.GetSettingByKeyAsync(string.Format(DiscountRequirementDefaults.SettingsKey, discountRequirementId ?? 0), new List<string>());

        var model = new RequirementModel
        {
            RequirementId = discountRequirementId ?? 0,
            DiscountId = discountId,
            PhoneNumbers = string.Join($",{Environment.NewLine}", phoneNumbers),
        };

        //set the HTML field prefix
        ViewData.TemplateInfo.HtmlFieldPrefix = string.Format(DiscountRequirementDefaults.HtmlFieldPrefix, discountRequirementId ?? 0);

        return View("~/Plugins/DiscountRules.PhoneNumber/Views/Configure.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> Configure(RequirementModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDiscounts))
            return Content("Access denied");

        if (ModelState.IsValid)
        {
            //load the discount
            var discount = await _discountService.GetDiscountByIdAsync(model.DiscountId);
            if (discount == null)
                return NotFound(new { Errors = new[] { "Discount could not be loaded" } });

            //get the discount requirement
            var discountRequirement = await _discountService.GetDiscountRequirementByIdAsync(model.RequirementId);

            //the discount requirement does not exist, so create a new one
            if (discountRequirement == null)
            {
                discountRequirement = new DiscountRequirement
                {
                    DiscountId = discount.Id,
                    DiscountRequirementRuleSystemName = DiscountRequirementDefaults.SystemName
                };

                await _discountService.InsertDiscountRequirementAsync(discountRequirement);
            }

            //save restricted customer role identifier
            await _settingService.SetSettingAsync(string.Format(DiscountRequirementDefaults.SettingsKey, discountRequirement.Id), GetFomattedNumbers(model.PhoneNumbers));

            return Ok(new { NewRequirementId = discountRequirement.Id });
        }

        return Ok(new { Errors = GetErrorsFromModelState(ModelState) });
    }

    #endregion
}