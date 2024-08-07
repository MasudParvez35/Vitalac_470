using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Controllers;

public partial class CountryController : BasePublicController
{
    #region Fields

    protected readonly ICountryModelFactory _countryModelFactory;
        
    #endregion

    #region Ctor

    public CountryController(ICountryModelFactory countryModelFactory)
    {
        _countryModelFactory = countryModelFactory;
    }

    #endregion

    #region States / provinces

    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    //ignore SEO friendly URLs checks
    [CheckLanguageSeoCode(ignore: true)]
    public virtual async Task<IActionResult> GetStatesByCountryId(int countryId, bool addSelectStateItem)
    {
        var model = await _countryModelFactory.GetStatesByCountryIdAsync(countryId, addSelectStateItem);

        return Json(model);
    }

    #endregion

    #region Thanas

    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    //ignore SEO friendly URLs checks
    [CheckLanguageSeoCode(ignore: true)]
    public virtual async Task<IActionResult> GetThanasByStateProvinceId(int stateId, bool addSelectThanaItem)
    {
        var model = await _countryModelFactory.GetThanasByStateProvinceIdAsync(stateId, addSelectThanaItem);

        return Json(model);
    }

    #endregion

    #region Areas

    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    //ignore SEO friendly URLs checks
    [CheckLanguageSeoCode(ignore: true)]
    public virtual async Task<IActionResult> GetAreasByThanaId(int thanaId, bool addSelectAreaItem)
    {
        var model = await _countryModelFactory.GetAreasByThanaIdAsync(thanaId, addSelectAreaItem);

        return Json(model);
    }

    #endregion
}