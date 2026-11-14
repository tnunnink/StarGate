using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a compound vector path combining multiple shapes with a unified fill and stroke.
/// </summary>
public class PathElement : GraphicElement
{
    /// <inheritdoc />
    
    public override ElementKind Kind => ElementKind.Path;

    /// <summary>
    /// Gets the collection of sub-elements that define the geometry of this compound path.
    /// </summary>
    public IEnumerable<GraphicElement> Elements { get; set; } = [];
}