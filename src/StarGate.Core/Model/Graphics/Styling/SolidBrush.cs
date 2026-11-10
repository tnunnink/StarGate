using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a brush with a single solid color, used for rendering graphical elements.
/// A SolidBrush is characterized by a specific color and transparency level.
/// </summary>
public class SolidBrush : GraphicBrush
{
    /// <inheritdoc />
    public override BrushKind Kind => BrushKind.Solid;
    
    /// <summary>
    /// Specifies the color of the brush, represented as an RGBA value.
    /// The color determines the visual appearance of the rendered brush.
    /// </summary>
    [YamlMember(Order = 1)]
    public GraphicColor Color { get; set; }
}