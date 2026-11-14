using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a group of mutually exclusive radio button controls for selecting a single option.
/// </summary>
public class RadioButtonElement : GraphicElement
{
    /// <inheritdoc />
    
    public override ElementKind Kind => ElementKind.RadioButton;

    /// <summary>
    /// Gets the arrangement direction of the radio buttons (Horizontal or Vertical).
    /// </summary>
    public GraphicOrientation Layout { get; set; } = GraphicOrientation.Vertical;

    /// <summary>
    /// Gets the visual button style (Flat or ThreeD).
    /// </summary>
    public ControlStyle? ControlStyle { get; set; }

    /// <summary>
    /// Gets the two-way data binding linking the selected value to a Galaxy tag or attribute reference.
    /// </summary>
    public ValueBinding? Binding { get; set; }

    /// <summary>
    /// Gets the source of items for this selector control (static list, enum, or array reference).
    /// </summary>
    public ItemBinding? Items { get; set; }

    /// <summary>
    /// Gets the sort order applied to the displayed items.
    /// </summary>
    public SortOrder? Sort { get; set; }

    /// <summary>
    /// Gets the format string applied to item values when generating captions.
    /// </summary>
    public string? Format { get; set; }
}