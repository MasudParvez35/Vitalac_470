using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Directory;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Directory;

/// <summary>
/// Represents a Area entity builder
/// </summary>
public partial class AreaBuilder : NopEntityBuilder<Area>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Area.Name)).AsString(100).NotNullable()
            .WithColumn(nameof(Area.Abbreviation)).AsString(100).Nullable()
            .WithColumn(nameof(Area.ThanaId)).AsInt32().ForeignKey<Thana>();
    }

    #endregion
}
