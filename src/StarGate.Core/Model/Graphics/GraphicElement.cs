using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Abstract base class for all visual and interactive elements contained within an ArchestrA graphic symbol.
/// </summary>
public abstract record GraphicElement
{
    /// <summary>
    /// Gets the specific kind of graphic element represented by this instance.
    /// This corresponds to the type of visual or interactive element within an ArchestrA graphic symbol.
    /// </summary>
    [YamlIgnore]
    public abstract ElementKind Kind { get; }

    /// <summary>
    /// Gets the unique identifier name of the graphic element within the symbol.
    /// Corresponds to the Name attribute in the ArchestrA schema (max 373 characters).
    /// </summary>
    [MaxLength(373, ErrorMessage = "Element name cannot exceed 373 characters.")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the rectangular bounding box defining the position and size of the graphic element.
    /// </summary>
    public GraphicBounds Bounds { get; init; }

    /// <summary>
    /// Gets the rotational origin center point of the graphic element.
    /// </summary>
    public GraphicPoint Origin { get; init; }

    /// <summary>
    /// Gets the clockwise rotation angle of the element in degrees (0.0 to 360.0).
    /// </summary>
    [Range(0.0, 360.0, ErrorMessage = "Angle must be between 0.0 and 360.0 degrees.")]
    [DefaultValue(0.0)]
    public double? Angle { get; init; } = 0.0;

    /// <summary>
    /// Gets the axis along which the graphic element is mirrored or flipped.
    /// </summary>
    [DefaultValue(GraphicFlip.None)]
    public GraphicFlip Flip { get; init; } = GraphicFlip.None;

    /// <summary>
    /// Gets a value indicating whether the graphic element is enabled for runtime interaction.
    /// </summary>
    [DefaultValue(true)]
    public bool Enabled { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the graphic element is locked against design-time modifications.
    /// </summary>
    public bool Locked { get; init; }

    /// <summary>
    /// Gets a value indicating whether the graphic element is visible at runtime.
    /// </summary>
    [DefaultValue(true)]
    public bool Visible { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the graphic element can receive focus during keyboard tab navigation.
    /// </summary>
    [DefaultValue(true)]
    public bool TabStop { get; init; } = true;

    /// <summary>
    /// Gets the sequence position of the graphic element within the keyboard tab navigation order.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "TabOrder must be a non-negative integer.")]
    public int TabOrder { get; init; }

    /// <summary>
    /// Gets the docking behavior of the graphic element within its container boundary.
    /// </summary>
    [DefaultValue(DockType.None)]
    public DockType Dock { get; init; }

    /// <summary>
    /// Gets the anchor placement behavior of the graphic element relative to its container edges.
    /// </summary>
    [DefaultValue(AnchorType.None)]
    public AnchorType Anchor { get; init; }

    /// <summary>
    /// Gets a value indicating whether the graphic element operates in faceplate mode for contextual binding.
    /// </summary>
    [DefaultValue(false)]
    public bool? Faceplate { get; init; }

    /// <summary>
    /// Gets the interior fill styling applied to the graphic element, or <see langword="null" /> if unfilled.
    /// </summary>
    public FillStyle? Fill { get; init; }

    /// <summary>
    /// Gets the stroke outline styling applied to the graphic element, or <see langword="null" /> if unstroked.
    /// </summary>
    public StrokeStyle? Stroke { get; init; }

    /// <summary>
    /// Gets the font and text styling for the checkbox caption.
    /// </summary>
    public FontStyle? Font { get; init; }
}