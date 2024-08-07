using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Merchants;

/// <summary>
/// Represents a merchant associated customer model
/// </summary>
public partial record MerchantAssociatedCustomerModel : BaseNopEntityModel
{
    #region Properties

    public string Email { get; set; }

    #endregion
}