using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Directory;

public partial record AreaSearchModel : BaseSearchModel
{
    #region Properties

    public int ThanaId { get; set; }

    #endregion
}
