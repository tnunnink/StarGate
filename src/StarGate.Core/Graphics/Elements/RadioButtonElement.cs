using System.Collections.Generic;
using StarGate.Core.Graphics.Controls;
using StarGate.Core.Graphics.Layout;
using StarGate.Core.Graphics.Styling;
using YamlDotNet.Serialization;

namespace StarGate.Core.Graphics.Elements;

/// <summary>
/// Represents a group of mutually exclusive radio button controls for selecting a single option.
/// </summary>
public class RadioButtonElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.RadioButton;

    /// <summary>
    /// Gets the arrangement direction of the radio buttons (Horizontal or Vertical).
    /// </summary>
    public GraphicOrientation Layout { get; init; } = GraphicOrientation.Vertical;

    /// <summary>
    /// Gets the visual button style (Flat or ThreeD).
    /// </summary>
    public ControlStyle? ControlStyle { get; init; }

    /// <summary>
    /// Gets the two-way data binding linking the selected value to a Galaxy tag or attribute reference.
    /// </summary>
    public ValueBinding? Binding { get; init; }

    /// <summary>
    /// Gets the source of items for this selector control (static list, enum, or array reference).
    /// </summary>
    public ItemBinding? Items { get; init; }

    /// <summary>
    /// Gets the sort order applied to the displayed items.
    /// </summary>
    public SortOrder? Sort { get; init; }

    /// <summary>
    /// Gets the format string applied to item values when generating captions.
    /// </summary>
    public string? Format { get; init; }
}
