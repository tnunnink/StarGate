using System.Collections.Generic;

namespace StarGate.Core;

/// <summary>
/// Represents the definition of a graphic element, including its metadata and visual content markup.
/// </summary>
public class GalaxySymbol
{
    /// <summary>
    /// Gets or sets the unique tag name of the object.
    /// </summary>
    public required string TagName { get; init; }

    /// <summary>
    /// Gets or sets the folder path associated with the graphic definition.
    /// </summary>
    public string? Folder { get; init; }

    /// <summary>
    /// </summary>
    public IEnumerable<GraphicElement> Elements { get; init; } = [];
}