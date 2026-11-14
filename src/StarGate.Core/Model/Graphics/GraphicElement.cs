using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// Abstract base class for all visual and interactive elements contained within an ArchestrA graphic symbol.
/// </summary>
public abstract class GraphicElement
{
    /// <summary>
    /// Gets the specific kind of graphic element represented by this instance.
    /// This corresponds to the type of visual or interactive element within an ArchestrA graphic symbol.
    /// </summary>
    public abstract ElementKind Kind { get; }

    /// <summary>
    /// Gets the unique identifier name of the graphic element within the symbol.
    /// Corresponds to the Name attribute in the ArchestrA schema (max 373 characters).
    /// </summary>
    [MaxLength(373, ErrorMessage = "Element name cannot exceed 373 characters.")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets the rectangular bounding box defining the position and size of the graphic element.
    /// </summary>
    public GraphicBounds Bounds { get; set; }

    /// <summary>
    /// Gets the rotational origin center point of the graphic element.
    /// </summary>
    public GraphicPoint Origin { get; set; }

    /// <summary>
    /// Gets the clockwise rotation angle of the element in degrees (0 to 360).
    /// </summary>
    [Range(0, 360, ErrorMessage = "Angle must be between 0 and 360 degrees.")]
    [DefaultValue(0)]
    public int Angle { get; set; }

    /// <summary>
    /// Gets the axis along which the graphic element is mirrored or flipped.
    /// </summary>
    [DefaultValue(GraphicFlip.None)]
    public GraphicFlip Flip { get; set; } = GraphicFlip.None;

    /// <summary>
    /// Gets a value indicating whether the graphic element is enabled for runtime interaction.
    /// </summary>
    [DefaultValue(true)]
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether the graphic element is locked against design-time modifications.
    /// </summary>
    public bool Locked { get; set; }

    /// <summary>
    /// Gets a value indicating whether the graphic element is visible at runtime.
    /// </summary>
    [DefaultValue(true)]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether the graphic element can receive focus during keyboard tab navigation.
    /// </summary>
    [DefaultValue(true)]
    public bool TabStop { get; set; } = true;

    /// <summary>
    /// Gets the sequence position of the graphic element within the keyboard tab navigation order.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "TabOrder must be a non-negative integer.")]
    public int TabOrder { get; set; }

    /// <summary>
    /// Gets the docking behavior of the graphic element within its container boundary.
    /// </summary>
    [DefaultValue(DockType.None)]
    public DockType Dock { get; set; }

    /// <summary>
    /// Gets the anchor placement behavior of the graphic element relative to its container edges.
    /// </summary>
    [DefaultValue(AnchorType.None)]
    public AnchorType Anchor { get; set; }

    /// <summary>
    /// Gets a value indicating whether the graphic element operates in faceplate mode for contextual binding.
    /// </summary>
    [DefaultValue(false)]
    public bool? Faceplate { get; set; }

    /// <summary>
    /// Gets the interior fill styling applied to the graphic element, or <see langword="null" /> if unfilled.
    /// </summary>
    public FillStyle? Fill { get; set; }

    /// <summary>
    /// Gets the stroke outline styling applied to the graphic element, or <see langword="null" /> if unstroked.
    /// </summary>
    public StrokeStyle? Stroke { get; set; }

    /// <summary>
    /// Gets the font and text styling for the checkbox caption.
    /// </summary>
    public FontStyle? Font { get; set; }
}