using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// Represents a rectangle graphic element, with an optional corner radius for rounded rectangles.
/// </summary>
public class RectangleElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.Rectangle;

    /// <summary>
    /// Gets the corner radius for rounded rectangles (0 to 70).
    /// When greater than zero, this element represents a rounded rectangle.
    /// </summary>
    [Range(0, 70, ErrorMessage = "Corner radius must be between 0 and 70.")]
    [DefaultValue(0)]
    public int? Radius { get; set; }
}