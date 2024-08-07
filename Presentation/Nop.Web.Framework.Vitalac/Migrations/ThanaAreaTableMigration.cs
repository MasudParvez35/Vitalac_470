using FluentMigrator;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace Nop.Web.Framework.Vitalac.Migrations;

[NopSchemaMigration("2024/07/30 11:24:16:2551771", "Thana, area migration", MigrationProcessType.Update)]
public class ThanaAreaTableMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        if (!Schema.TableFor<Thana>().Exists())
            Create.TableFor<Thana>();
        if (!Schema.TableFor<Area>().Exists())
            Create.TableFor<Area>();

        if (!Schema.TableFor<Customer>().Column(nameof(Customer.AreaId)).Exists())
            Alter.Table(nameof(Customer)).AddColumn(nameof(Customer.AreaId)).AsInt32().SetExistingRowsTo(0);

        if (!Schema.TableFor<Address>().Column(nameof(Address.AreaId)).Exists())
            Alter.Table(nameof(Address)).AddColumn(nameof(Address.AreaId)).AsInt32().Nullable();

        if (!Schema.TableFor<Address>().Column(nameof(Address.ThanaId)).Exists())
            Alter.Table(nameof(Address)).AddColumn(nameof(Address.ThanaId)).AsInt32().Nullable();
    }
}
