using System.Text.RegularExpressions;
using Nop.Core.Domain.Customers;

namespace Nop.Web.Framework.Vitalac.Validators;

public static class PhoneNumberExtension
{
    public static bool IsValid(string phoneNumber, CustomerSettings customerSettings)
    {
        if (!customerSettings.PhoneNumberValidationEnabled || string.IsNullOrEmpty(customerSettings.PhoneNumberValidationRule))
            return true;

        if (string.IsNullOrEmpty(phoneNumber))
        {
            return !customerSettings.PhoneRequired;
        }

        return customerSettings.PhoneNumberValidationUseRegex
            ? Regex.IsMatch(phoneNumber, customerSettings.PhoneNumberValidationRule, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
            : phoneNumber.All(customerSettings.PhoneNumberValidationRule.Contains);
    }

    public static string Format(string phoneNumber, CustomerSettings customerSettings)
    {
        if (!customerSettings.PhoneNumberValidationEnabled || string.IsNullOrEmpty(customerSettings.PhoneNumberValidationRule))
            return phoneNumber;

        if (string.IsNullOrEmpty(phoneNumber))
            return null;

        var match = Regex.Match(phoneNumber, customerSettings.PhoneNumberValidationRule);
        if (match.Success)
            return "+880" + phoneNumber[^10..];

        return null;
    }

    public static bool AreSame(string phoneNumber1, string phoneNumber2, CustomerSettings customerSettings)
    { 
        return Format(phoneNumber1, customerSettings) == Format(phoneNumber2, customerSettings);
    }
}
