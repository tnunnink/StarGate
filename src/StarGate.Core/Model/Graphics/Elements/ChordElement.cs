using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a closed chord graphic element formed by connecting the endpoints of an elliptical arc with a straight
/// line.
/// </summary>
public class ChordElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.Chord;

    /// <summary>
    /// Gets the starting angle in degrees, measured clockwise from the positive X-axis (-360.0 to 360.0).
    /// </summary>
    [Range(-360.0, 360.0, ErrorMessage = "StartAngle must be between -360.0 and 360.0 degrees.")]
    public double StartAngle { get; set; }

    /// <summary>
    /// Gets the sweep angle in degrees measured clockwise (or negative counter-clockwise) from the start angle (-360.0 to
    /// 360.0).
    /// </summary>
    [Range(-360.0, 360.0, ErrorMessage = "SweepAngle must be between -360.0 and 360.0 degrees.")]
    public double SweepAngle { get; set; }
}