using FluentValidation;
using Nop.Plugin.DiscountRules.PhoneNumber.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.DiscountRules.PhoneNumber.Validators;

public class RequirementModelValidator : BaseNopValidator<RequirementModel>
{
    public RequirementModelValidator(ILocalizationService localizationService)
    {
        RuleFor(model => model.DiscountId)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.DiscountRules.PhoneNumber.Fields.DiscountId.Required"));
        RuleFor(model => model.PhoneNumbers)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.DiscountRules.PhoneNumber.Fields.PhoneNumbers.Required"));
    }
}