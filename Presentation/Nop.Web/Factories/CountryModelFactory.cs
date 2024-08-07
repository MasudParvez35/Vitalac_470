using Nop.Core;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Web.Models.Directory;

namespace Nop.Web.Factories;

/// <summary>
/// Represents the country model factory
/// </summary>
public partial class CountryModelFactory : ICountryModelFactory
{
    #region Fields

    protected readonly ICountryService _countryService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IStateProvinceService _stateProvinceService;
    protected readonly IWorkContext _workContext;
    protected readonly IThanaService _thanaService;
    protected readonly IAreaService _areaService;

    #endregion

    #region Ctor

    public CountryModelFactory(ICountryService countryService,
        ILocalizationService localizationService,
        IStateProvinceService stateProvinceService,
        IWorkContext workContext,
        IThanaService thanaService,
        IAreaService areaService)
    {
        _countryService = countryService;
        _localizationService = localizationService;
        _stateProvinceService = stateProvinceService;
        _workContext = workContext;
        _thanaService = thanaService;
        _areaService = areaService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get states and provinces by country identifier
    /// </summary>
    /// <param name="countryId">Country identifier</param>
    /// <param name="addSelectStateItem">Whether to add "Select state" item to list of states</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of identifiers and names of states and provinces
    /// </returns>
    public virtual async Task<IList<StateProvinceModel>> GetStatesByCountryIdAsync(int countryId, bool addSelectStateItem)
    {
        var country = await _countryService.GetCountryByIdAsync(countryId);
        var states =
            (await _stateProvinceService.GetStateProvincesByCountryIdAsync(country?.Id ?? 0,
                (await _workContext.GetWorkingLanguageAsync()).Id)).ToList();
        
        var result = new List<StateProvinceModel>();
        
        foreach (var state in states)
            result.Add(new StateProvinceModel
            {
                id = state.Id,
                name = await _localizationService.GetLocalizedAsync(state, x => x.Name)
            });

        if (country == null)
        {
            //country is not selected ("choose country" item)
            if (addSelectStateItem)
                result.Insert(0, new StateProvinceModel
                {
                    id = 0,
                    name = await _localizationService.GetResourceAsync("Address.SelectState")
                });
            else
                result.Insert(0, new StateProvinceModel
                {
                    id = 0,
                    name = await _localizationService.GetResourceAsync("Address.Other")
                });
        }
        else
        {
            //some country is selected
            if (!result.Any())
                //country does not have states
                result.Insert(0, new StateProvinceModel
                {
                    id = 0,
                    name = await _localizationService.GetResourceAsync("Address.Other")
                });
            else
            {
                //country has some states
                if (addSelectStateItem)
                    result.Insert(0, new StateProvinceModel
                    {
                        id = 0,
                        name = await _localizationService.GetResourceAsync("Address.SelectState")
                    });
            }
        }

        return result;
    }

    public virtual async Task<IList<ThanaModel>> GetThanasByStateProvinceIdAsync(int stateProvinceId, bool addSelectStateItem)
    {
        var state = await _stateProvinceService.GetStateProvinceByIdAsync(stateProvinceId);
        var thanas = 
            (await _thanaService.GetThanasByStateProvinceIdAsync(state?.Id ?? 0, 
                (await _workContext.GetWorkingLanguageAsync()).Id)).ToList();

        var result = new List<ThanaModel>();

        foreach (var thana in thanas)
            result.Add(new ThanaModel
            {
                id = thana.Id,
                name = await _localizationService.GetLocalizedAsync(thana, x => x.Name)
            });

        if (state == null)
        {
            //state is not selected ("choose state" item)
            if (addSelectStateItem)
                result.Insert(0, new ThanaModel
                {
                    id = 0,
                    name = await _localizationService.GetResourceAsync("Address.SelectThana")
                });
            else
                result.Insert(0, new ThanaModel
                {
                    id = 0,
                    name = await _localizationService.GetResourceAsync("Address.Other")
                });
        }
        else
        {
            // some state is selected
            if (!result.Any())
                //state does not have thanas
                result.Insert(0, new ThanaModel
                {
                    id = 0,
                    name = await _localizationService.GetResourceAsync("Address.Other")
                });
            else
            {
                //state has some thanas
                if (addSelectStateItem)
                    result.Insert(0, new ThanaModel
                    {
                        id = 0,
                        name = await _localizationService.GetResourceAsync("Address.SelectThana")
                    });
            
            }
        }

        return result;
    }

    public virtual async Task<IList<AreaModel>> GetAreasByThanaIdAsync(int thanaId, bool addSelectStateItem)
    {
        var thana = await _thanaService.GetThanaByIdAsync(thanaId);
        var areas = 
            (await _areaService.GetAreasByThanaIdAsync(thana?.Id ?? 0, 
                           (await _workContext.GetWorkingLanguageAsync()).Id)).ToList();

        var result = new List<AreaModel>();

        foreach (var area in areas)
            result.Add(new AreaModel
            {
                id = area.Id,
                name = await _localizationService.GetLocalizedAsync(area, x => x.Name)
            });

        if (thana == null)
        {
            //thana is not selected ("choose thana" item)
            if (addSelectStateItem)
                result.Insert(0, new AreaModel
                {
                    id = 0,
                    name = await _localizationService.GetResourceAsync("Address.SelectArea")
                });
            else
                result.Insert(0, new AreaModel
                {
                    id = 0,
                    name = await _localizationService.GetResourceAsync("Address.Other")
                });
        }
        else
        {
            // some thana is selected
            if (!result.Any())
                //thana does not have areas
                result.Insert(0, new AreaModel
                {
                    id = 0,
                    name = await _localizationService.GetResourceAsync("Address.Other")
                });
            else
            {
                //thana has some areas
                if (addSelectStateItem)
                    result.Insert(0, new AreaModel
                    {
                        id = 0,
                        name = await _localizationService.GetResourceAsync("Address.SelectArea")
                    });
            }
        }

        return result;
    }

    #endregion
}