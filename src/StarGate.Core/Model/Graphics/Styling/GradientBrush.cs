using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// Represents a gradient brush used to fill graphical elements with a color gradient.
/// A GradientBrush defines a gradient transition between multiple colors using specified
/// stops, direction, angle, and additional properties to control the gradient's appearance.
/// Inherits from the <see cref="GraphicBrush"/> class to support transparency.
/// </summary>
/// <remarks>
/// The GradientBrush can define both linear gradients and other variations depending on
/// the <see cref="GradientType"/>. It uses a sequence of color stops to create smooth transitions,
/// combined with additional configuration options such as angle, center, falloff, and focus scaling.
/// </remarks>
/// <seealso cref="GraphicBrush"/>
/// <seealso cref="GradientType"/>
/// <seealso cref="GraphicColor"/>
public class GradientBrush : GraphicBrush
{
    /// <inheritdoc />
    public override BrushKind Kind => BrushKind.Gradient;

    /// <summary>
    /// Specifies the gradient direction to be applied to a graphical element.
    /// </summary>
    [DefaultValue(GradientType.Horizontal)]
    public GradientType Type { get; set; } = GradientType.Horizontal;

    /// <summary>
    /// Defines the gradient stops, where each stop specifies a color and its relative position
    /// within the gradient. These stops determine how the gradient is rendered across a graphical element.
    /// </summary>
    [MinLength(1, ErrorMessage = "Gradient stops must contain at least 1 color.")]
    [MaxLength(3, ErrorMessage = "Gradient stops cannot contain more than 3 colors.")]
    public GraphicColor[] Stops { get; set; } = [];

    /// <summary>
    /// Specifies the angle, in degrees, for applying a gradient to a graphical element.
    /// Must be within the range of 0.0 to 360.0.
    /// </summary>
    [Range(0.0, 360.0, ErrorMessage = "Angle must be between 0.0 and 360.0.")]
    [DefaultValue(90.0)]
    public double Angle { get; set; } = 90.0;

    /// <summary>
    /// Relative position of the focal peak (0.0 to 1.0).
    /// Corresponds to the 'Center' slider (e.g., 0.50).
    /// </summary>
    [Range(0.0, 1.0, ErrorMessage = "Center must be between 0.0 and 1.0.")]
    [DefaultValue(0.5f)]
    public float Center { get; set; } = 0.5f;

    /// <summary>
    /// Diminishing rate/scale from peak (0.0 to 1.0).
    /// Corresponds to the 'Falloff' slider (e.g., 1.00).
    /// </summary>
    [Range(0.0, 1.0, ErrorMessage = "Falloff must be between 0.0 and 1.0.")]
    [DefaultValue(1.0f)]
    public float Falloff { get; set; } = 1.0f;

    /// <summary>
    /// Expands the inner center color plateau for Radial gradients (0.0 to 1.0).
    /// Corresponds to 'Focus Scale' slider.
    /// </summary>
    [Range(0.0, 1.0, ErrorMessage = "FocusScale must be between 0.0 and 1.0.")]
    [DefaultValue(0.0f)]
    public float FocusScale { get; set; }

    /// <summary>
    /// Determines whether the angle value should be serialized for this gradient brush.
    /// </summary>
    /// <return>
    /// <c>true</c> when the gradient type is custom; otherwise, <c>false</c>.
    /// </return>
    internal bool ShouldSerializeAngle() => Type == GradientType.Custom;

    /// <summary>
    /// Determines whether the focus scale value should be serialized for this gradient brush.
    /// </summary>
    /// <return>
    /// <c>true</c> when the gradient type is radial; otherwise, <c>false</c>.
    /// </return>
    internal bool ShouldSerializeFocusScale() => Type == GradientType.Radial;

    /// <summary>
    /// Determines whether the falloff value should be serialized for this gradient brush.
    /// </summary>
    /// <return>
    /// <c>true</c> when the gradient has fewer than three color stops; otherwise, <c>false</c>.
    /// </return>
    internal bool ShouldSerializeFalloff() => Stops.Length < 3;
}