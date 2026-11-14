using System.ComponentModel;

namespace StarGate.Core;

/// <summary>
/// Represents a brush that uses a patterned style to render visual elements.
/// A PatternBrush defines its appearance through a combination of foreground and background colors,
/// and a specific hatch pattern style.
/// </summary>
public class PatternBrush : GraphicBrush
{
    /// <inheritdoc />
    public override BrushKind Kind => BrushKind.Pattern;

    /// <summary>
    /// Specifies the foreground color used in the rendering of a pattern.
    /// This color defines the primary visual element of the pattern, contrasting with the background color.
    /// </summary>
    public GraphicColor ForeColor { get; set; } = GraphicColor.Black;

    /// <summary>
    /// Specifies the background color used for rendering elements in a patterned style.
    /// </summary>
    public GraphicColor BackColor { get; set; } = GraphicColor.White;

    /// <summary>
    /// Specifies the hatch pattern style to be applied by the PatternBrush.
    /// The style determines the type of visual pattern, such as lines, shapes, or grids,
    /// used to render areas within the graphical element.
    /// </summary>
    [DefaultValue(HatchStyle.Horizontal)]
    public HatchStyle Hatch { get; set; } = HatchStyle.Horizontal;

    /// <summary>
    /// Determines whether the foreground color should be serialized.
    /// </summary>
    /// <return>
    /// <c>true</c> when the foreground color is black; otherwise, <c>false</c>.
    /// </return>
    internal bool ShouldSerializeForeColor() => ForeColor == GraphicColor.Black;
    
    /// <summary>
    /// Determines whether the foreground color should be serialized.
    /// </summary>
    /// <return>
    /// <c>true</c> when the foreground color is black; otherwise, <c>false</c>.
    /// </return>
    internal bool ShouldSerializeBackColor() => ForeColor == GraphicColor.White;
}