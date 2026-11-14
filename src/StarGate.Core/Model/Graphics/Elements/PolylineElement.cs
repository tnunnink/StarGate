using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// Represents a polyline graphic element. A polyline is a series of connected line segments defined by a set of
/// points.
/// </summary>
/// <remarks>
/// The <see cref="PolylineElement" /> requires a minimum of two points to define the polyline. This element
/// inherits from <see cref="GraphicElement" /> and provides specific functionality for handling polyline graphics.
/// </remarks>
public class PolylineElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.Polyline;

    /// <summary>
    /// Gets the collection of points that define the vertices of the polyline.
    /// </summary>
    /// <remarks>
    /// A polyline requires at least two points to form its structure. Each point in the collection
    /// is specified as a <see cref="GraphicPoint" /> containing X and Y coordinates. This property is
    /// immutable and must be initialized with the required points during the creation of a <see cref="PolylineElement" />.
    /// </remarks>
    [MinLength(2, ErrorMessage = "Polyline requires at least 2 points.")]
    public IEnumerable<GraphicPoint> Points { get; set; } = [];
}