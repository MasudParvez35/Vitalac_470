using Nop.Core.Domain.Customers;
using Nop.Services.Customers;

namespace Nop.Web.Framework.Vitalac.Migrations;

public partial class DataMigration : IBaseMigration
{
    private readonly ICustomerService _customerService;

    public DataMigration(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task InitAsync()
    {
        var merchantCustomerRole = await _customerService.GetCustomerRoleBySystemNameAsync(NopCustomerDefaults.MerchantsRoleName);
        if (merchantCustomerRole == null)
        {
            await _customerService.InsertCustomerRoleAsync(new CustomerRole
            {
                Active = true,
                IsSystemRole = true,
                SystemName = NopCustomerDefaults.MerchantsRoleName,
                Name = "Merchants"
            });
        }
    }

    public int Order => 1;
}
