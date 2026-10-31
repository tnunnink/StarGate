using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a graphical line element in a 2D coordinate system.
/// A line element is defined by its start and end points.
/// Inherits from the <see cref="GraphicElement" /> base class to provide common graphical properties.
/// </summary>
public record LineElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.Line;

    /// <summary>
    /// Gets the starting point of the line element in the 2D coordinate system.
    /// The value represents the initial position of the line's graphical representation,
    /// defined by its X and Y coordinates.
    /// </summary>
    [Required]
    public GraphicPoint Start { get; init; }

    /// <summary>
    /// Gets the ending point of the line element in a 2D coordinate system.
    /// This property, along with <see cref="Start" />, defines the geometric bounds of the line.
    /// </summary>
    [Required]
    public GraphicPoint End { get; init; }

    /// <summary>
    /// Gets a value indicating whether the line element should snap to a grid or specific alignment points
    /// </summary>
    /// <remarks>
    /// This corresponds to the attribute <c>HVLine</c> which means horizontal/vertical line and represent
    /// a different element type from the IDE editor. We need to preserve this so that reimporting
    /// doesn't create HV lines as normal lines.
    /// </remarks>
    public bool? Snap { get; init; }
}