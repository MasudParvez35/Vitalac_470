namespace Nop.Plugin.DiscountRules.PhoneNumber;

/// <summary>
/// Represents defaults for the discount requirement rule
/// </summary>
public static class DiscountRequirementDefaults
{
    /// <summary>
    /// The system name of the discount requirement rule
    /// </summary>
    public static string SystemName => "DiscountRequirement.PhoneNumber";

    /// <summary>
    /// The key of the settings to save restricted customer roles
    /// </summary>
    public static string SettingsKey => "DiscountRequirement.PhoneNumber-{0}";

    /// <summary>
    /// The HTML field prefix for discount requirements
    /// </summary>
    public static string HtmlFieldPrefix => "DiscountRulesPhoneNumber{0}";
}