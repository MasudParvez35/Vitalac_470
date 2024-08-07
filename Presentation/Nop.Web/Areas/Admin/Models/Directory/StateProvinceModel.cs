using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Directory;

/// <summary>
/// Represents a state and province model
/// </summary>
public partial record StateProvinceModel : BaseNopEntityModel, ILocalizedModel<StateProvinceLocalizedModel>
{
    #region Ctor

    public StateProvinceModel()
    {
        Locales = new List<StateProvinceLocalizedModel>();
        ThanaSearchModel = new ThanaSearchModel();
    }

    #endregion

    #region Properties

    public int CountryId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Countries.States.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Countries.States.Fields.Abbreviation")]
    public string Abbreviation { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Countries.States.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Countries.States.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Countries.Fields.NumberOfThanas")]
    public int NumberOfThanas { get; set; }

    public IList<StateProvinceLocalizedModel> Locales { get; set; }

    public ThanaSearchModel ThanaSearchModel { get; set; }

    #endregion
}

public partial record StateProvinceLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Countries.States.Fields.Name")]
    public string Name { get; set; }
}