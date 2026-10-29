using System.ComponentModel.DataAnnotations;

namespace StarGate.Core.Graphics.Styling;

/// <summary>
/// Represents a single stop in a gradient, consisting of a specific color at a specified offset.
/// Gradient stops define how colors are distributed along the gradient.
/// </summary>
public readonly record struct GradientStop
{
    /// <summary>
    /// Gets the color associated with the gradient stop.
    /// This property defines the specific <see cref="Color"/> instance that represents the color
    /// used at the current position within a gradient range.
    /// </summary>
    public GraphicColor Color { get; init; }

    /// <summary>
    /// Gets the position of the gradient stop within the gradient range.
    /// This property specifies a <see cref="double"/> value that represents the relative position
    /// of this gradient stop, typically ranging from 0.0 to 1.0, where 0.0 corresponds to the
    /// start of the gradient and 1.0 corresponds to the end.
    /// </summary>
    [Range(0.0, 1.0, ErrorMessage = "Offset must be between 0.0 and 1.0.")]
    public double Offset { get; init; }
}