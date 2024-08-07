using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Directory;

public partial record AreaModel : BaseNopEntityModel, ILocalizedModel<AreaLocalizedModel>
{
    #region Ctor

    public AreaModel()
    {
        Locales = new List<AreaLocalizedModel>();
    }

    #endregion

    #region Properties
    public int ThanaId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Thanas.Areas.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Thanas.Areas.Fields.Abbreviation")]
    public string Abbreviation { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Thanas.Areas.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Thanas.Areas.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<AreaLocalizedModel> Locales { get; set; }

    #endregion
}

public partial record AreaLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Thanas.Areas.Fields.Name")]
    public string Name { get; set; }
}