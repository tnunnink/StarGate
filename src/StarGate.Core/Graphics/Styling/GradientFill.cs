using System.ComponentModel.DataAnnotations;
using StarGate.Core.Graphics.Layout;

namespace StarGate.Core.Graphics.Styling;

/// <summary>
/// Represents a fill style that applies a gradient pattern to a graphical element.
/// Provides various gradient types and customization options, such as direction,
/// center point, and peak intensity.
/// </summary>
public class GradientFill : FillStyle
{
    /// <summary>
    /// Specifies the type of gradient to be applied within the fill style. This property determines
    /// how the gradient is rendered, such as horizontal, vertical, radial, or other custom patterns.
    /// The value is defined by the <see cref="GradientType"/> enumeration.
    /// </summary>
    public GradientType Type { get; init; }

    /// <summary>
    /// Defines the angle of the gradient's orientation in degrees. This property determines
    /// the direction of the gradient when applied to a graphical element, measured clockwise
    /// from the horizontal axis. A null value indicates that the orientation is determined
    /// by the gradient type or other properties.
    /// </summary>
    [Range(0.0, 360.0, ErrorMessage = "Angle must be between 0.0 and 360.0.")]
    public double Angle { get; init; }

    /// <summary>
    /// Specifies the central point of the gradient, represented as coordinates
    /// within the applicable drawing space. The position of this point determines
    /// the focal origin for gradient calculations, influencing how gradient colors
    /// are distributed across the rendered area. Adjusting the center allows control
    /// over the direction or spread of the gradient effect.
    /// </summary>
    public GraphicPoint Center { get; init; }

    /// <summary>
    /// Defines the collection of gradient stops that specify the color and position of each transition
    /// within the gradient. Each stop represents a distinct color at a specific offset in the gradient's
    /// color band. This property enables fine-tuned customization of the gradient appearance.
    /// </summary>
    public GradientStop[] Stops { get; init; } = [];

    /// <summary>
    /// Defines the intensity level at the highest point of the gradient. This value
    /// determines how bright or prominent the gradient appears at its peak.
    /// A higher value results in a more intense peak, while a lower value creates a
    /// subtler effect. The peak value can be adjusted to control the visual emphasis
    /// of the gradient's center or focal point.
    /// </summary>
    [Range(0.0, 1.0, ErrorMessage = "Peak must be between 0.0 and 1.0.")]
    public float Peak { get; init; }

    /// <summary>
    /// Specifies the rate at which the gradient intensity diminishes from its peak
    /// as distance increases. The falloff value determines the steepness of this
    /// fade-out effect, influencing the overall appearance of the gradient.
    /// A smaller value results in a more gradual decrease, while a larger value
    /// creates a sharper intensity drop.
    /// </summary>
    [Range(0.0, 1.0, ErrorMessage = "Falloff must be between 0.0 and 1.0.")]
    public float Falloff { get; init; }
}