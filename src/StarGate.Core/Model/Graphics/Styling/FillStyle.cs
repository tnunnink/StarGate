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
    public GraphicBrush Brush { get; set; } = GraphicColor.White;

    /// <summary>
    /// Gets the brush used to define the unfilled areas of a graphical element.
    /// Controls the visual representation and properties of areas where no fill is applied.
    /// </summary>
    public GraphicBrush Unfilled { get; set; } = GraphicColor.Black;

    /// <summary>
    /// Gets the orientation used for filling graphical content within this element.
    /// Determines whether the fill direction is relative to the graphic or relative to the screen.
    /// </summary>
    [DefaultValue(FillOrientation.RelativeToGraphic)]
    public FillOrientation Orientation { get; set; } = FillOrientation.RelativeToGraphic;

    /// <summary>
    /// Defines how the fill operation is applied to a graphic element.
    /// This property determines whether the fill behavior affects both directions,
    /// only the horizontal direction, or only the vertical direction.
    /// </summary>
    [DefaultValue(FillDirection.UpRight)]
    public FillDirection Direction { get; set; } = FillDirection.UpRight;

    /// <summary>
    /// Represents the vertical fill percentage of a graphic element, where valid values range from 0 to 100.
    /// A value of 0 indicates no vertical fill, while a value of 100 indicates full vertical fill.
    /// </summary>
    [Range(0, 100, ErrorMessage = "VerticalFill must be between 0 and 100.")]
    [DefaultValue(100)]
    public int Vertical { get; set; } = 100;

    /// <summary>
    /// Specifies the horizontal fill percentage of the graphic element.
    /// The value ranges from 0 to 100, representing the percentage of the graphic element's width
    /// that is visually filled in its designated bounds.
    /// </summary>
    [Range(0, 100, ErrorMessage = "HorizontalFill must be between 0 and 100.")]
    [DefaultValue(100)]
    public int Horizontal { get; set; } = 100;

    /// <summary>
    /// Determines whether the fill style can be collapsed because it matches the default fill configuration.
    /// </summary>
    /// <return>
    /// true if the fill uses a fully opaque black solid brush, relative-to-graphic orientation,
    /// up-right direction, and default vertical and horizontal scaling; otherwise, false.
    /// </return>
    internal bool ShouldCollapse()
    {
        return
            Brush is SolidBrush {Transparency: 0} &&
            Unfilled is SolidBrush { Transparency: 0 } solid && solid.Color == GraphicColor.Black &&
            Orientation == FillOrientation.RelativeToGraphic &&
            Direction == FillDirection.UpRight &&
            Vertical == 100 &&
            Horizontal == 100;
    }

    /// <summary>
    /// Determines whether the brush configuration should be included during serialization.
    /// </summary>
    /// <return>
    /// true if the brush is not a fully opaque solid brush, or if it is a fully opaque solid brush with a color other than white; otherwise, false.
    /// </return>
    internal bool ShouldSerializeBrush()
    {
        return Brush is not SolidBrush { Transparency: 0 } solid || solid.Color != GraphicColor.White;
    }

    /// <summary>
    /// Determines whether the unfilled brush configuration should be included during serialization.
    /// </summary>
    /// <return>
    /// true if the unfilled brush is not a fully opaque solid brush, or if it is a fully opaque solid brush with the black color; otherwise, false.
    /// </return>
    internal bool ShouldSerializeUnfilled()
    {
        return Unfilled is not SolidBrush { Transparency: 0 } solid || solid.Color != GraphicColor.Black;
    }
}