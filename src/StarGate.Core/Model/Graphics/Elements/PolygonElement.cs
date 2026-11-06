using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a closed polygon graphic element defined by a collection of vertices.
/// </summary>
public record PolygonElement : GraphicElement
{
    /// <inheritdoc />
    
    public override ElementKind Kind => ElementKind.Polygon;

    /// <summary>
    /// Gets the collection of vertices defining the closed polygon.
    /// </summary>
    [MinLength(2, ErrorMessage = "Polygon requires at least 2 points.")]
    public IEnumerable<GraphicPoint> Points { get; init; } = [];
}