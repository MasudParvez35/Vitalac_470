using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.Directory;

/// <summary>
/// Represents a Area
/// </summary>
public partial class Area : BaseEntity, ILocalizedEntity
{
    /// <summary>
    /// Gets or sets the Thana identifier
    /// </summary>
    public int ThanaId { get; set; }

    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the abbreviation
    /// </summary>
    public string Abbreviation { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is published
    /// </summary>
    public bool Published { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
}
