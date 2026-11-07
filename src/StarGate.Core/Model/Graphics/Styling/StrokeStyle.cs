using System.ComponentModel;

namespace StarGate.Core;

/// <summary>
/// Represents the styling attributes for drawing strokes or outlines of graphic symbols.
/// </summary>
/// <remarks>
/// Defines visual characteristics such as stroke color, width, dash pattern, and line cap styles.
/// </remarks>
public class StrokeStyle
{
    /// <summary>
    /// Gets the graphical brush used to render the stroke or outline of a visual element.
    /// </summary>
    public GraphicBrush Brush { get; init; } = GraphicColor.Black;

    /// <summary>
    /// Gets the width of the stroke in drawing units.
    /// </summary>
    [DefaultValue(1)]
    public int Width { get; init; } = 1;

    /// <summary>
    /// Gets the pattern style of the stroke, such as solid or dashed.
    /// </summary>
    [DefaultValue(StrokePattern.Solid)]
    public StrokePattern Pattern { get; init; } = StrokePattern.Solid;

    /// <summary>
    /// Gets the cap style applied to the starting point of the stroke.
    /// </summary>
    [DefaultValue(StrokeCap.Flat)]
    public StrokeCap StartCap { get; init; } = StrokeCap.Flat;

    /// <summary>
    /// Gets the cap style applied to the ending point of the stroke.
    /// </summary>
    [DefaultValue(StrokeCap.Flat)]
    public StrokeCap EndCap { get; init; } = StrokeCap.Flat;

    /// <summary>
    /// Implicitly converts a <see cref="GraphicColor"/> to a <see cref="StrokeStyle"/>
    /// with the color as its stroke brush and default stroke settings.
    /// </summary>
    /// <param name="color">The <see cref="GraphicColor"/> representing the stroke color.</param>
    /// <returns>A new <see cref="StrokeStyle"/> instance with default styling attributes and the specified stroke color.</returns>
    public static implicit operator StrokeStyle(GraphicColor color) => new() { Brush = color };

    /// <summary>
    /// Implicitly converts a hex color string to a <see cref="StrokeStyle"/>.
    /// Creates a <see cref="StrokeStyle"/> instance with a <see cref="SolidBrush"/>
    /// based on the specified color and default stroke settings.
    /// </summary>
    /// <param name="hex">The hexadecimal color string representing the stroke color (e.g., #RRGGBB or #RRGGBBAA).</param>
    /// <returns>A new <see cref="StrokeStyle"/> instance with the specified color in its brush.</returns>
    public static implicit operator StrokeStyle(string hex) => new() { Brush = hex };
}