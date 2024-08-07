using Nop.Core.Caching;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Data;
using Nop.Services.Localization;

namespace Nop.Services.Directory;
public partial class AreaService : IAreaService
{
    #region Fields

    protected readonly IRepository<Area> _areaRepository;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly ILocalizationService _localizationService;

    #endregion

    #region Ctor

    public AreaService(IRepository<Area> areaRepository,
        IStaticCacheManager staticCacheManager,
        ILocalizationService localizationService)
    {
        _areaRepository = areaRepository;
        _staticCacheManager = staticCacheManager;
        _localizationService = localizationService;
    }

    #endregion

    #region Methods

    public virtual async Task<IList<Area>> GetAreasByThanaIdAsync(int thanaId, int languageId = 0, bool showHidden = false)
    {
        var key = _staticCacheManager.PrepareKeyForDefaultCache(NopDirectoryDefaults.AreaByThanasCacheKey, thanaId, languageId, showHidden);

        return await _staticCacheManager.GetAsync(key, async () =>
        {
            var query = from ar in _areaRepository.Table
            orderby ar.DisplayOrder, ar.Name
                        where ar.ThanaId == thanaId &&
                        (showHidden || ar.Published)
                        select ar;
            var area = await query.ToListAsync();

            if (languageId > 0)
                area = await area.ToAsyncEnumerable()
                    .OrderBy(t => t.DisplayOrder)
                    .ThenByAwait(async t => await _localizationService.GetLocalizedAsync(t, x => x.Name, languageId))
                    .ToListAsync();

            return area;
        });
    }

    public virtual async Task<IList<Area>> GetAreaAsync(bool showHidden = false)
    {
        var query = from ar in _areaRepository.Table
                    orderby ar.ThanaId, ar.DisplayOrder, ar.Name
                    where showHidden || ar.Published
                    select ar;
        var area = await _staticCacheManager.GetAsync(_staticCacheManager.PrepareKeyForDefaultCache(NopDirectoryDefaults.AreasAllCacheKey, showHidden), async () => await query.ToListAsync());

        return area;
    }

    public virtual async Task DeleteAreaAsync(Area area)
    {
        await _areaRepository.DeleteAsync(area);
    }

    public virtual async Task<Area> GetAreaByIdAsync(int areaId)
    {
        return await _areaRepository.GetByIdAsync(areaId);
    }

    public virtual async Task<IList<Area>> GetAreasByIdsAsync(int[] areaIds)
    {
        return await _areaRepository.GetByIdsAsync(areaIds);
    }

    public virtual async Task InsertAreaAsync(Area area)
    {
        await _areaRepository.InsertAsync(area);
    }

    public virtual async Task UpdateAreaAsync(Area area)
    {
        await _areaRepository.UpdateAsync(area);
    }

    public virtual async Task<Area> GetAreaByAddressAsync(Address address)
    {
        return await GetAreaByIdAsync(address?.AreaId ?? 0);
    }

    #endregion
}
