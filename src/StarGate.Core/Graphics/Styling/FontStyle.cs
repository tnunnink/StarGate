using StarGate.Core.Graphics.Layout;

namespace StarGate.Core.Graphics.Styling;

/// <summary>
/// Represents the style properties of a font used for graphical elements.
/// </summary>
/// <remarks>
/// This class provides configuration options for font appearance,
/// including font family, size, weight, style, and text decorations.
/// </remarks>
public class FontStyle
{
    /// <summary>
    /// Gets the color used to render the text.
    /// </summary>
    public GraphicColor Color { get; init; } = GraphicColor.Black;

    /// <summary>
    /// Gets the name of the font-family.
    /// </summary>
    public string Family { get; init; } = "Arial";

    /// <summary>
    /// Gets the size of the font in points or drawing units.
    /// </summary>
    public double Size { get; init; } = 12;

    /// <summary>
    /// Gets a value indicating whether the font is styled in bold weight.
    /// </summary>
    public bool? Bold { get; init; }

    /// <summary>
    /// Gets a value indicating whether the font is styled in italics.
    /// </summary>
    public bool? Italic { get; init; }

    /// <summary>
    /// Gets a value indicating whether the font text is underlined.
    /// </summary>
    public bool? Underline { get; init; }

    /// <summary>
    /// Gets a value indicating whether the font text has a strikethrough line.
    /// </summary>
    public bool? Strikeout { get; init; }

    /// <summary>
    /// Gets the horizontal and vertical alignment of the text within its bounding area.
    /// </summary>
    public GraphicAlignment? Alignment { get; init; }
}