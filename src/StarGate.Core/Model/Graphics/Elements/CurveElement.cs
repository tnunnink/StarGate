using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents an open cardinal spline curve defined by a sequence of points and a tension factor.
/// </summary>
public record CurveElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.Curve;

    /// <summary>
    /// Gets the collection of control points defining the curve.
    /// </summary>
    [MinLength(2, ErrorMessage = "Curve requires at least 2 points.")]
    public IEnumerable<GraphicPoint> Points { get; init; } = [];

    /// <summary>
    /// Gets the curve tension factor between 0.0 and 2.0.
    /// </summary>
    [Range(0.0, 2.0, ErrorMessage = "Tension must be between 0.0 and 2.0.")]
    public double Tension { get; init; } = 0.5;
}