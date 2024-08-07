using Nop.Core.Domain.Merchants;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Merchants;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Merchants;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the merchant model factory implementation
/// </summary>
public partial class MerchantModelFactory : IMerchantModelFactory
{
    #region Fields

    protected readonly IAddressModelFactory _addressModelFactory;
    protected readonly IAddressService _addressService;
    protected readonly ICustomerService _customerService;
    protected readonly IMerchantService _merchantService;

    #endregion

    #region Ctor

    public MerchantModelFactory(
        IAddressModelFactory addressModelFactory,
        IAddressService addressService,
        ICustomerService customerService,
        IMerchantService merchantService)
    {
        _addressModelFactory = addressModelFactory;
        _addressService = addressService;
        _customerService = customerService;
        _merchantService = merchantService;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Prepare merchant associated customer models
    /// </summary>
    /// <param name="models">List of merchant associated customer models</param>
    /// <param name="merchant">Merchant</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task PrepareAssociatedCustomerModelsAsync(IList<MerchantAssociatedCustomerModel> models, Merchant merchant)
    {
        ArgumentNullException.ThrowIfNull(models);

        ArgumentNullException.ThrowIfNull(merchant);

        var associatedCustomers = await _customerService.GetAllCustomersAsync(merchantId: merchant.Id);
        foreach (var customer in associatedCustomers)
        {
            models.Add(new MerchantAssociatedCustomerModel
            {
                Id = customer.Id,
                Email = customer.Email
            });
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare merchant search model
    /// </summary>
    /// <param name="searchModel">Merchant search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the merchant search model
    /// </returns>
    public virtual Task<MerchantSearchModel> PrepareMerchantSearchModelAsync(MerchantSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    /// <summary>
    /// Prepare paged merchant list model
    /// </summary>
    /// <param name="searchModel">Merchant search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the merchant list model
    /// </returns>
    public virtual async Task<MerchantListModel> PrepareMerchantListModelAsync(MerchantSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get merchants
        var merchants = await _merchantService.GetAllMerchantsAsync(showHidden: true,
            name: searchModel.SearchName,
            email: searchModel.SearchEmail,
            pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

        //prepare list model
        var model = await new MerchantListModel().PrepareToGridAsync(searchModel, merchants, () =>
        {
            //fill in model values from the entity
            return merchants.SelectAwait(async merchant =>
            {
                var merchantModel = merchant.ToModel<MerchantModel>();
                await Task.CompletedTask;
                return merchantModel;
            });
        });

        return model;
    }

    /// <summary>
    /// Prepare merchant model
    /// </summary>
    /// <param name="model">Merchant model</param>
    /// <param name="merchant">Merchant</param>
    /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the merchant model
    /// </returns>
    public virtual async Task<MerchantModel> PrepareMerchantModelAsync(MerchantModel model, Merchant merchant, bool excludeProperties = false)
    {
        if (merchant != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = merchant.ToModel<MerchantModel>();
            }

            //prepare associated customers
            await PrepareAssociatedCustomerModelsAsync(model.AssociatedCustomers, merchant);
        }

        //set default values for the new model
        if (merchant == null)
        {
            model.Active = true;
        }

        //prepare address model
        var address = await _addressService.GetAddressByIdAsync(merchant?.AddressId ?? 0);
        if (!excludeProperties && address != null)
            model.Address = address.ToModel(model.Address);
        await _addressModelFactory.PrepareAddressModelAsync(model.Address, address);

        return model;
    }

    #endregion
}