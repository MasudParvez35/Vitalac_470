using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Merchants;
using Nop.Data;
using Nop.Services.Html;

namespace Nop.Services.Merchants;

/// <summary>
/// Merchant service
/// </summary>
public partial class MerchantService : IMerchantService
{
    #region Fields

    protected readonly IHtmlFormatter _htmlFormatter;
    protected readonly IRepository<Customer> _customerRepository;
    protected readonly IRepository<Merchant> _merchantRepository;

    #endregion

    #region Ctor

    public MerchantService(IHtmlFormatter htmlFormatter,
        IRepository<Customer> customerRepository,
        IRepository<Merchant> merchantRepository)
    {
        _htmlFormatter = htmlFormatter;
        _customerRepository = customerRepository;
        _merchantRepository = merchantRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets a merchant by merchant identifier
    /// </summary>
    /// <param name="merchantId">Merchant identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the merchant
    /// </returns>
    public virtual async Task<Merchant> GetMerchantByIdAsync(int merchantId)
    {
        return await _merchantRepository.GetByIdAsync(merchantId, cache => default);
    }

    /// <summary>
    /// Gets a merchants by customers identifiers
    /// </summary>
    /// <param name="customerIds">Array of customer identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the merchants
    /// </returns>
    public virtual async Task<IList<Merchant>> GetMerchantsByCustomerIdsAsync(int[] customerIds)
    {
        ArgumentNullException.ThrowIfNull(customerIds);

        return await (from v in _merchantRepository.Table
                      join c in _customerRepository.Table on v.Id equals c.MerchantId
                      where customerIds.Contains(c.Id) && !v.Deleted && v.Active
                      select v).Distinct().ToListAsync();
    }

    /// <summary>
    /// Delete a merchant
    /// </summary>
    /// <param name="merchant">Merchant</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteMerchantAsync(Merchant merchant)
    {
        await _merchantRepository.DeleteAsync(merchant);
    }

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
    public virtual async Task<IPagedList<Merchant>> GetAllMerchantsAsync(string name = "", string email = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
    {
        var merchants = await _merchantRepository.GetAllPagedAsync(query =>
        {
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(v => v.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(v => v.Email.Contains(email));

            if (!showHidden)
                query = query.Where(v => v.Active);

            query = query.Where(v => !v.Deleted);
            query = query.OrderBy(v => v.Name).ThenBy(v => v.Email);

            return query;
        }, pageIndex, pageSize);

        return merchants;
    }

    /// <summary>
    /// Inserts a merchant
    /// </summary>
    /// <param name="merchant">Merchant</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertMerchantAsync(Merchant merchant)
    {
        await _merchantRepository.InsertAsync(merchant);
    }

    /// <summary>
    /// Updates the merchant
    /// </summary>
    /// <param name="merchant">Merchant</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateMerchantAsync(Merchant merchant)
    {
        await _merchantRepository.UpdateAsync(merchant);
    }

    /// <summary>
    /// Get merchant by area identifier
    /// </summary>
    /// <param name="areaId">Area identifier</param>
    /// <returns>Merchant</returns>
    public async Task<Merchant> GetMerchantByAreaIdAsync(int areaId)
    {
        // TODO: get merchant based on location
        var merchants = await GetAllMerchantsAsync(pageSize: 1);
        return merchants.FirstOrDefault();
    }

    #endregion
}