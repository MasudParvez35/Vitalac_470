using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Vitalac.Validators;

namespace Nop.Plugin.DiscountRules.PhoneNumber;

public class PhoneNumberDiscountRequirementRule : BasePlugin, IDiscountRequirementRule
{
    #region Fields

    protected readonly IActionContextAccessor _actionContextAccessor;
    protected readonly ICustomerService _customerService;
    protected readonly IDiscountService _discountService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ISettingService _settingService;
    protected readonly IUrlHelperFactory _urlHelperFactory;
    protected readonly IWebHelper _webHelper;
    private readonly CustomerSettings _customerSettings;

    #endregion

    #region Ctor

    public PhoneNumberDiscountRequirementRule(IActionContextAccessor actionContextAccessor,
        IDiscountService discountService,
        ICustomerService customerService,
        ILocalizationService localizationService,
        ISettingService settingService,
        IUrlHelperFactory urlHelperFactory,
        IWebHelper webHelper,
        CustomerSettings customerSettings)
    {
        _actionContextAccessor = actionContextAccessor;
        _customerService = customerService;
        _discountService = discountService;
        _localizationService = localizationService;
        _settingService = settingService;
        _urlHelperFactory = urlHelperFactory;
        _webHelper = webHelper;
        _customerSettings = customerSettings;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Check discount requirement
    /// </summary>
    /// <param name="request">Object that contains all information required to check the requirement (Current customer, discount, etc)</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    public async Task<DiscountRequirementValidationResult> CheckRequirementAsync(DiscountRequirementValidationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        //invalid by default
        var result = new DiscountRequirementValidationResult();

        if (request.Customer == null)
            return result;

        //try to get saved restricted customer phone number
        var phoneNumbers = await _settingService.GetSettingByKeyAsync(string.Format(DiscountRequirementDefaults.SettingsKey, request.DiscountRequirementId), new List<string>());
        if (!phoneNumbers.Any())
            return result;

        //result is valid if the customer has any of these phone number
        result.IsValid = phoneNumbers.Any(x => PhoneNumberExtension.AreSame(x, request.Customer.Phone, _customerSettings));

        return result;
    }

    /// <summary>
    /// Get URL for rule configuration
    /// </summary>
    /// <param name="discountId">Discount identifier</param>
    /// <param name="discountRequirementId">Discount requirement identifier (if editing)</param>
    /// <returns>URL</returns>
    public string GetConfigurationUrl(int discountId, int? discountRequirementId)
    {
        var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

        return urlHelper.Action("Configure", "DiscountRulesPhoneNumber",
            new { discountId, discountRequirementId }, _webHelper.GetCurrentRequestProtocol());
    }

    /// <summary>
    /// Install the plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task InstallAsync()
    {
        //locales
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.DiscountRules.PhoneNumber.Fields.PhoneNumbers"] = "Phone number(s)",
            ["Plugins.DiscountRules.PhoneNumber.Fields.PhoneNumbers.Hint"] = "Discount will be applied if customer has any of these phone numbers. You can enter multiple phone numbers separated by comma or new line.",
            ["Plugins.DiscountRules.PhoneNumber.Fields.PhoneNumbers.Required"] = "Phone numbers are required",
            ["Plugins.DiscountRules.PhoneNumber.Fields.DiscountId.Required"] = "Discount is required"
        });

        await base.InstallAsync();
    }

    /// <summary>
    /// Uninstall the plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
        //discount requirements
        var discountRequirements = (await _discountService.GetAllDiscountRequirementsAsync())
            .Where(discountRequirement => discountRequirement.DiscountRequirementRuleSystemName == DiscountRequirementDefaults.SystemName);
        foreach (var discountRequirement in discountRequirements)
            await _discountService.DeleteDiscountRequirementAsync(discountRequirement, false);

        //locales
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.DiscountRules.PhoneNumber");

        await base.UninstallAsync();
    }

    #endregion
}