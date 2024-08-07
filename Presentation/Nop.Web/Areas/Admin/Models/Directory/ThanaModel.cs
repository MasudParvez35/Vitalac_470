using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Directory;

public partial record ThanaModel : BaseNopEntityModel, ILocalizedModel<ThanaLocalizedModel>
{
    #region Ctor

    public ThanaModel()
    {
        Locales = new List<ThanaLocalizedModel>();
        AreaSearchModel = new AreaSearchModel();
    }

    #endregion

    #region Properties

    public int StateProvinceId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.States.Thanas.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.States.Thanas.Fields.Abbreviation")]
    public string Abbreviation { get; set; }

    [NopResourceDisplayName("Admin.Configuration.States.Thanas.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Admin.Configuration.States.Thanas.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [NopResourceDisplayName("Admin.Configuration.States.Fields.NumberOfAreas")]
    public int NumberOfAreas { get; set; }

    public IList<ThanaLocalizedModel> Locales { get; set; }
    public AreaSearchModel AreaSearchModel { get; set; }

    #endregion
}

public partial record ThanaLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.States.Thanas.Fields.Name")]
    public string Name { get; set; }
}
