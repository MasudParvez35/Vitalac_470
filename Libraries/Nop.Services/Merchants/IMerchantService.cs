using Nop.Core;
using Nop.Core.Domain.Merchants;

namespace Nop.Services.Merchants;
public interface IMerchantService
{
    /// <summary>
    /// Deletes a merchant
    /// </summary>
    /// <param name="merchant">Merchant</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteMerchantAsync(Merchant merchant);

    /// <summary>
    /// Gets all merchants
    /// </summary>
    /// <param name="name">Merchant name</param>
    /// <param name="email">Merchant email</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the merchants
    /// </returns>
    Task<IPagedList<Merchant>> GetAllMerchantsAsync(string name = "", string email = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

    /// <summary>
    /// Gets a merchant by merchant identifier
    /// </summary>
    /// <param name="merchantId">Merchant identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the merchant
    /// </returns>
    Task<Merchant> GetMerchantByIdAsync(int merchantId);

    /// <summary>
    /// Gets a merchants by customers identifiers
    /// </summary>
    /// <param name="customerIds">Array of customer identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the merchants
    /// </returns>
    Task<IList<Merchant>> GetMerchantsByCustomerIdsAsync(int[] customerIds);

    /// <summary>
    /// Inserts a merchant
    /// </summary>
    /// <param name="merchant">Merchant</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertMerchantAsync(Merchant merchant);

    /// <summary>
    /// Updates the merchant
    /// </summary>
    /// <param name="merchant">Merchant</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateMerchantAsync(Merchant merchant);

    /// <summary>
    /// Get merchant by area identifier
    /// </summary>
    /// <param name="areaId">Area identifier</param>
    /// <returns>Merchant</returns>
    Task<Merchant> GetMerchantByAreaIdAsync(int areaId);
}