using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Merchants;

namespace Nop.Data.Mapping.Builders.Merchants;

/// <summary>
/// Represents a merchant entity builder
/// </summary>
public partial class MerchantBuilder : NopEntityBuilder<Merchant>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Merchant.Name)).AsString(400).NotNullable()
            .WithColumn(nameof(Merchant.Email)).AsString(400).Nullable();
    }

    #endregion
}