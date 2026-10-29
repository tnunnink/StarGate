namespace StarGate.Core.Graphics.Styling;

/// <summary>
/// Represents the styling attributes for drawing strokes or outlines of graphic symbols.
/// </summary>
/// <remarks>
/// Defines visual characteristics such as stroke color, width, dash pattern, and line cap styles.
/// </remarks>
public class StrokeStyle
{
    /// <summary>
    /// Gets the color of the stroke.
    /// </summary>
    public GraphicColor Color { get; init; } = GraphicColor.Black;

    /// <summary>
    /// Gets the width of the stroke in drawing units.
    /// </summary>
    public int Width { get; init; } = 1;

    /// <summary>
    /// Gets the pattern style of the stroke, such as solid or dashed.
    /// </summary>
    public StrokePattern Pattern { get; init; } = StrokePattern.Solid;

    /// <summary>
    /// Gets the cap style applied to the starting point of the stroke.
    /// </summary>
    public StrokeCap? StartCap { get; init; } = StrokeCap.Flat;

    /// <summary>
    /// Gets the cap style applied to the ending point of the stroke.
    /// </summary>
    public StrokeCap? EndCap { get; init; } = StrokeCap.Flat;

    /// <summary>
    /// Implicitly converts a <see cref="Color"/> to a <see cref="StrokeStyle"/> with default stroke settings.
    /// </summary>
    /// <param name="color">The color to apply to the stroke.</param>
    public static implicit operator StrokeStyle(GraphicColor color) => new() { Color = color };

    /// <summary>
    /// Implicitly converts a hexadecimal color string to a <see cref="StrokeStyle"/> with default stroke settings.
    /// </summary>
    /// <param name="hex">The hexadecimal string representing the stroke color.</param>
    public static implicit operator StrokeStyle(string hex) => new() { Color = GraphicColor.Parse(hex) };
}