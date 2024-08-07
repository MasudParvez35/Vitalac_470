using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;

namespace Nop.Services.Directory;
public partial interface IAreaService
{
    Task InsertAreaAsync(Area area);

    Task UpdateAreaAsync(Area area);

    Task DeleteAreaAsync(Area area);

    Task<Area> GetAreaByAddressAsync(Address address);

    Task<Area> GetAreaByIdAsync(int areaId);

    Task<IList<Area>> GetAreasByIdsAsync(int[] areaIds);

    Task<IList<Area>> GetAreasByThanaIdAsync(int thanaId, int languageId = 0, bool showHidden = false);

    Task<IList<Area>> GetAreaAsync(bool showHidden = false);
}
