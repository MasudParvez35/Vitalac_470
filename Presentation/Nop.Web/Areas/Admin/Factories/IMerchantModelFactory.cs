using Nop.Core.Domain.Merchants;
using Nop.Web.Areas.Admin.Models.Merchants;

namespace Nop.Web.Areas.Admin.Factories;
public interface IMerchantModelFactory
{
    Task<MerchantListModel> PrepareMerchantListModelAsync(MerchantSearchModel searchModel);
    Task<MerchantModel> PrepareMerchantModelAsync(MerchantModel model, Merchant merchant, bool excludeProperties = false);
    Task<MerchantSearchModel> PrepareMerchantSearchModelAsync(MerchantSearchModel searchModel);
}