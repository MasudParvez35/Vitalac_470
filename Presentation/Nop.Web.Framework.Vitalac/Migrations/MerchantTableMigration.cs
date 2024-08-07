using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Merchants;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace Nop.Web.Framework.Vitalac.Migrations;

[NopSchemaMigration("2024/07/28 11:26:08:9037680", "Add Merchant table and relevant fields", MigrationProcessType.Update)]
public class MerchantTableMigration : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.TableFor<Merchant>().Exists())
        {
            Create.TableFor<Merchant>();
            Insert.IntoTable(nameof(Merchant))
                .Row(new { Name = "Merchant 1", Active = true, Email = "merchant@yourStore.com", AddressId = 0, AdminComment = string.Empty, Deleted = false, Description = string.Empty, PictureId = 0 });
        }

        if (!Schema.ColumnFor<Customer>(x => x.MerchantId).Exists())
        {
            Alter.Table(nameof(Customer))
                .AddColumn(nameof(Customer.MerchantId))
                .AsInt32()
                .SetExistingRowsTo(0);
        }

        if (!Schema.ColumnFor<Order>(x => x.MerchantId).Exists())
        {
            Alter.Table(nameof(Order))
                .AddColumn(nameof(Order.MerchantId))
                .AsInt32()
                .ForeignKey<Merchant>()
                .SetExistingRowsTo(1);
        }
    }
}
