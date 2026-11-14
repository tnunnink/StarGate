using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// Represents an open elliptical arc defined by a bounding rectangle, start angle, and sweep angle.
/// </summary>
public class ArcElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.Arc;

    /// <summary>
    /// Gets the starting angle in degrees, measured clockwise from the positive X-axis (0 to 360).
    /// </summary>
    [Range(-360.0, 360.0, ErrorMessage = "StartAngle must be between -360.0 and 360.0 degrees.")]
    public double StartAngle { get; set; }

    /// <summary>
    /// Gets the sweep angle in degrees measured clockwise (or negative counter-clockwise) from the start angle.
    /// </summary>
    [Range(-360.0, 360.0, ErrorMessage = "SweepAngle must be between -360.0 and 360.0 degrees.")]
    public double SweepAngle { get; set; }
}