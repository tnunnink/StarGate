namespace StarGate.Core.Graphics.Styling;

/// <summary>
/// Represents a solid fill style for rendering graphic symbols, where a single color is used to fill the visual area.
/// </summary>
/// <remarks>
/// This class is a specialization of <see cref="FillStyle"/> that defines a uniform color fill.
/// The transparency level of the fill can be controlled via the <see cref="FillStyle.Transparency"/> property.
/// </remarks>
public class SolidFill : FillStyle
{
    /// <summary>
    /// Gets the <see cref="Color"/> used to define the solid fill color for a graphic element.
    /// This property specifies the RGBA color value that determines the visual appearance
    /// of the solid fill styling applied to the symbol.
    /// </summary>
    public GraphicColor Color { get; init; } = GraphicColor.White;
}