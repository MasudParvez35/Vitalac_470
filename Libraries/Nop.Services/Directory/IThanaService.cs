using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;

namespace Nop.Services.Directory;

public partial interface IThanaService
{
    Task InsertThanaAsync(Thana thana);

    Task UpdateThanaAsync(Thana thana);

    Task DeleteThanaAsync(Thana thana);

    Task<Thana> GetThanaByAddressAsync(Address address);

    Task<Thana> GetThanaByIdAsync(int thanaId);

    Task<IList<Thana>> GetThanasByIdsAsync(int[] thanaIds);

    Task<IList<Thana>> GetThanasByStateProvinceIdAsync(int stateProvinceId, int languageId = 0, bool showHidden = false);

    Task <IList<Thana>> GetThanaAsync(bool showHidden = false);
}
