using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// Represents an open cardinal spline curve defined by a sequence of points and a tension factor.
/// </summary>
public class CurveElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.Curve;

    /// <summary>
    /// Gets the collection of control points defining the curve.
    /// </summary>
    [MinLength(2, ErrorMessage = "Curve requires at least 2 points.")]
    public IEnumerable<GraphicPoint> Points { get; set; } = [];

    /// <summary>
    /// Gets the curve tension factor between 0.0 and 2.0.
    /// </summary>
    [Range(0.0, 2.0, ErrorMessage = "Tension must be between 0.0 and 2.0.")]
    public double Tension { get; set; } = 0.5;
}