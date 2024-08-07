using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.DiscountRules.PhoneNumber.Models;

public class RequirementModel
{
    [NopResourceDisplayName("Plugins.DiscountRules.PhoneNumber.Fields.PhoneNumbers")]
    public string PhoneNumbers { get; set; }

    public int DiscountId { get; set; }

    public int RequirementId { get; set; }
}