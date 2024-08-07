using System.ComponentModel.DataAnnotations;
using Nop.Web.Areas.Admin.Models.Common;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Merchants;

public partial record MerchantModel : BaseNopEntityModel
{
    #region Ctor

    public MerchantModel()
    {
        Address = new();
        AssociatedCustomers = [];
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Merchants.Fields.Name")]
    public string Name { get; set; }

    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Admin.Merchants.Fields.Email")]
    public string Email { get; set; }

    [NopResourceDisplayName("Admin.Merchants.Fields.Description")]
    public string Description { get; set; }

    [UIHint("Picture")]
    [NopResourceDisplayName("Admin.Merchants.Fields.Picture")]
    public int PictureId { get; set; }

    [NopResourceDisplayName("Admin.Merchants.Fields.AdminComment")]
    public string AdminComment { get; set; }

    public AddressModel Address { get; set; }

    [NopResourceDisplayName("Admin.Merchants.Fields.Active")]
    public bool Active { get; set; }

    [NopResourceDisplayName("Admin.Merchants.Fields.AssociatedCustomerEmails")]
    public IList<MerchantAssociatedCustomerModel> AssociatedCustomers { get; set; }

    #endregion
}