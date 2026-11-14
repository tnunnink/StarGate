using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

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
    /// Gets the graphical brush used to render the font's appearance.
    /// </summary>
    public GraphicBrush Brush { get; set; } = GraphicColor.Black;

    /// <summary>
    /// Gets the name of the font-family.
    /// </summary>
    [DefaultValue("Arial")]
    public string Family { get; set; } = "Arial";

    /// <summary>
    /// Gets the size of the font in points or drawing units.
    /// </summary>
    [DefaultValue(12)]
    [Range(1, 2000, ErrorMessage = "Font size must be between 1 and 2000.")]
    public int Size { get; set; } = 12;

    /// <summary>
    /// Gets a value indicating whether the font is styled in bold weight.
    /// </summary>
    public bool Bold { get; set; }

    /// <summary>
    /// Gets a value indicating whether the font is styled in italics.
    /// </summary>
    public bool Italic { get; set; }

    /// <summary>
    /// Gets a value indicating whether the font text is underlined.
    /// </summary>
    public bool Underline { get; set; }

    /// <summary>
    /// Gets a value indicating whether the font text has a strikethrough line.
    /// </summary>
    public bool Strikeout { get; set; }

    /// <summary>
    /// Gets the horizontal and vertical alignment of the text within its bounding area.
    /// </summary>
    [DefaultValue(GraphicAlignment.TopLeft)]
    public GraphicAlignment Alignment { get; set; } = GraphicAlignment.TopLeft;

    /// <summary>
    /// Determines whether the brush value should be serialized.
    /// </summary>
    /// <return>
    /// true if the brush is not the default opaque black solid brush; otherwise, false.
    /// </return>
    internal bool ShouldSerializeBrush()
    {
        return Brush is not SolidBrush { Transparency: 0 } solid || solid.Color != GraphicColor.Black;
    }
}