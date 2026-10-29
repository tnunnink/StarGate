namespace StarGate.Core.Graphics.Styling;

/// <summary>
/// Represents a specific fill style that uses a pattern to fill graphical elements.
/// This style is defined by a hatch pattern, foreground color, and background color.
/// </summary>
public class PatternFill : FillStyle
{
    /// <summary>
    /// Gets the hatch style used for filling graphical elements with a specific pattern.
    /// The hatch style determines the visual pattern applied to the fill area,
    /// such as horizontal lines, diagonal lines, or dots. This property is part of the
    /// <see cref="PatternFill"/> style and provides various predefined pattern options
    /// through the <see cref="HatchStyle"/> enumeration.
    /// </summary>
    public HatchStyle Style { get; init; }

    /// <summary>
    /// Gets the foreground color used in a pattern-based fill style.
    /// The foreground color determines the primary color applied
    /// to the visible elements of the hatch pattern, contributing
    /// to the overall visual appearance of the fill.
    /// </summary>
    public GraphicColor ForeColor { get; init; }

    /// <summary>
    /// Gets the background color used in conjunction with a hatch pattern to fill graphical elements.
    /// The background color is applied beneath the defined pattern, working together with the
    /// foreground color to create the desired visual effect.
    /// </summary>
    public GraphicColor BackColor { get; init; }
}