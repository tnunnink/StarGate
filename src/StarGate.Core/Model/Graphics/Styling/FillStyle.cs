using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// Represents the style and configuration of a fill operation for a graphical element.
/// </summary>
public class FillStyle
{
    /// <summary>
    /// Gets the brush used for rendering graphical content within the element.
    /// Defines the visual properties, such as transparency, of the fill applied to the element.
    /// </summary>
    public GraphicBrush Brush { get; init; } = GraphicColor.White;

    /// <summary>
    /// Gets the brush used to define the unfilled areas of a graphical element.
    /// Controls the visual representation and properties of areas where no fill is applied.
    /// </summary>
    public GraphicBrush Unfilled { get; init; } = GraphicColor.Black;

    /// <summary>
    /// Gets the orientation used for filling graphical content within this element.
    /// Determines whether the fill direction is relative to the graphic or relative to the screen.
    /// </summary>
    [DefaultValue(FillOrientation.RelativeToGraphic)]
    public FillOrientation Orientation { get; init; } = FillOrientation.RelativeToGraphic;

    /// <summary>
    /// Defines how the fill operation is applied to a graphic element.
    /// This property determines whether the fill behavior affects both directions,
    /// only the horizontal direction, or only the vertical direction.
    /// </summary>
    [DefaultValue(FillDirection.UpLeft)]
    public FillDirection Direction { get; init; } = FillDirection.UpRight;

    /// <summary>
    /// Represents the vertical fill percentage of a graphic element, where valid values range from 0 to 100.
    /// A value of 0 indicates no vertical fill, while a value of 100 indicates full vertical fill.
    /// </summary>
    [Range(0.0, 100.0, ErrorMessage = "VerticalFill must be between 0 and 100.")]
    [DefaultValue(100.0)]
    public double Vertical { get; init; } = 100.0;

    /// <summary>
    /// Specifies the horizontal fill percentage of the graphic element.
    /// The value ranges from 0 to 100, representing the percentage of the graphic element's width
    /// that is visually filled in its designated bounds.
    /// </summary>
    [Range(0.0, 100.0, ErrorMessage = "HorizontalFill must be between 0 and 100.")]
    [DefaultValue(100.0)]
    public double Horizontal { get; init; } = 100.0;
}