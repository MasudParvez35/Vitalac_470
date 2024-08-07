using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Merchants;

/// <summary>
/// Represents a merchant search model
/// </summary>
public partial record MerchantSearchModel : BaseSearchModel
{
    #region Properties

    [NopResourceDisplayName("Admin.Merchants.List.SearchName")]
    public string SearchName { get; set; }

    [NopResourceDisplayName("Admin.Merchants.List.SearchEmail")]
    public string SearchEmail { get; set; }

    #endregion
}