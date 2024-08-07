using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Directory;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Directory;

/// <summary>
/// Represents a Thana entity builder
/// </summary>
public partial class ThanaBuilder : NopEntityBuilder<Thana>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Thana.Name)).AsString(100).NotNullable()
            .WithColumn(nameof(Thana.Abbreviation)).AsString(100).Nullable()
            .WithColumn(nameof(Thana.StateProvinceId)).AsInt32().ForeignKey<StateProvince>();
    }

    #endregion
}
