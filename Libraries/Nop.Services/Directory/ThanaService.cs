using Nop.Core.Caching;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Data;
using Nop.Services.Localization;

namespace Nop.Services.Directory;
public partial class ThanaService : IThanaService
{
    #region Fields

    protected readonly IRepository<Thana> _thanaRepository;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly ILocalizationService _localizationService;

    #endregion

    #region Ctor

    public ThanaService(IRepository<Thana> thanaRepository, 
        IStaticCacheManager staticCacheManager, 
        ILocalizationService localizationService)
    {
        _thanaRepository = thanaRepository;
        _staticCacheManager = staticCacheManager;
        _localizationService = localizationService;
    }

    #endregion

    #region Methods

    public virtual async Task<IList<Thana>> GetThanasByStateProvinceIdAsync(int stateProvinceId, int languageId = 0, bool showHidden = false)
    {
        var key = _staticCacheManager.PrepareKeyForDefaultCache(NopDirectoryDefaults.ThanaByStateProvincesCacheKey, stateProvinceId, languageId, showHidden);

        return await _staticCacheManager.GetAsync(key, async () =>
        {
            var query = from th in _thanaRepository.Table
                orderby th.DisplayOrder, th.Name
                where th.StateProvinceId == stateProvinceId && 
                (showHidden || th.Published)
                select th;
            var thana = await query.ToListAsync();

            if (languageId > 0)
                thana = await thana.ToAsyncEnumerable()
                    .OrderBy(t => t.DisplayOrder)
                    .ThenByAwait(async t => await _localizationService.GetLocalizedAsync(t, x => x.Name, languageId))
                    .ToListAsync();

            return thana;
        });
    }

    public virtual async Task<IList<Thana>> GetThanaAsync(bool showHidden = false)
    {
        var query = from th in _thanaRepository.Table
                    orderby th.StateProvinceId, th.DisplayOrder, th.Name
                    where showHidden || th.Published
                    select th;

        var thana = await _staticCacheManager.GetAsync(_staticCacheManager.PrepareKeyForDefaultCache(NopDirectoryDefaults.ThanasAllCacheKey, showHidden), async () => await query.ToListAsync());

        return thana;
    }

    public virtual async Task<Thana> GetThanaByIdAsync(int thanaId)
    {
        return await _thanaRepository.GetByIdAsync(thanaId);
    }

    public virtual async Task<IList<Thana>> GetThanasByIdsAsync(int[] thanaIds)
    {
        return await _thanaRepository.GetByIdsAsync(thanaIds);
    }

    public virtual async Task InsertThanaAsync(Thana thana)
    {
        await _thanaRepository.InsertAsync(thana);
    }

    public virtual async Task UpdateThanaAsync(Thana thana)
    {
        await _thanaRepository.UpdateAsync(thana);
    }

    public virtual async Task DeleteThanaAsync(Thana thana)
    {
        await _thanaRepository.DeleteAsync(thana);
    }

    public virtual async Task<Thana> GetThanaByAddressAsync(Address address)
    {
        return await GetThanaByIdAsync(address?.ThanaId ?? 0);
    }

    #endregion
}
