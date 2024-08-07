using System.Data;
using FluentMigrator;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace Nop.Web.Framework.Vitalac.Migrations;
[NopSchemaMigration("2024-08-01 00:00:00", "SchemaMigration for 4.70.0 Vitalac")]
public class AddressSchemaMigration : ForwardOnlyMigration
{
    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(Thana)).Exists())
            Create.TableFor<Thana>();

        if (!Schema.Table(nameof(Area)).Exists())
            Create.TableFor<Area>();

        var addressTableName = nameof(Address);
        if (!Schema.Table(addressTableName).Column(nameof(Address.ThanaId)).Exists())
            Alter.Table(addressTableName)
                .AddColumn(nameof(Address.ThanaId)).AsInt32().Nullable().ForeignKey<Thana>(onDelete: Rule.None);
        if (!Schema.Table(addressTableName).Column(nameof(Address.AreaId)).Exists())
            Alter.Table(addressTableName)
                .AddColumn(nameof(Address.AreaId)).AsInt32().Nullable().ForeignKey<Area>(onDelete: Rule.None);
    }
}