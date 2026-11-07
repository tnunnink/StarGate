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
    /// Gets or initializes the custom properties defined for the graphic symbol.
    /// </summary>
    public IEnumerable<GraphicProperty> Properties { get; init; } = [];

    /// <summary>
    /// Gets the wizard options that define configurable choices for the graphic symbol.
    /// </summary>
    public IEnumerable<WizardOption> Options { get; init; } = [];

    /// <summary>
    /// Gets the collection of wizard rules that define conditional layers for the symbol.
    /// </summary>
    public IEnumerable<WizardRule> Layers { get; init; } = [];

    /// <summary>
    /// Gets the visual and interactive elements that compose the graphic symbol.
    /// </summary>
    public IEnumerable<GraphicElement> Elements { get; init; } = [];

    /// <summary>
    /// Gets the scripts that define runtime behavior for the graphic element.
    /// </summary>
    public IEnumerable<GraphicScript> Scripts { get; init; } = [];
}