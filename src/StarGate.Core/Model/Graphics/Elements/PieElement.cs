using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a closed pie wedge graphic element formed by an elliptical arc and two lines extending to the center of
/// the ellipse.
/// </summary>
public record PieElement : GraphicElement
{
    /// <inheritdoc />
    
    public override ElementKind Kind => ElementKind.Pie;

    /// <summary>
    /// Gets the starting angle in degrees, measured clockwise from the positive X-axis (-360.0 to 360.0).
    /// </summary>
    [Range(-360.0, 360.0, ErrorMessage = "StartAngle must be between -360.0 and 360.0 degrees.")]
    public double StartAngle { get; init; }

    /// <summary>
    /// Gets the sweep angle in degrees measured clockwise (or negative counter-clockwise) from the start angle (-360.0 to
    /// 360.0).
    /// </summary>
    [Range(-360.0, 360.0, ErrorMessage = "SweepAngle must be between -360.0 and 360.0 degrees.")]
    public double SweepAngle { get; init; }
}