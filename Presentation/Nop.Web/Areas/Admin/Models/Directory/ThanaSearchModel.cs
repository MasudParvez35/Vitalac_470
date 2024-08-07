using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Directory;

public partial record ThanaSearchModel : BaseSearchModel
{
    #region Properties

    public int StateProvinceId { get; set; }

    #endregion
}
