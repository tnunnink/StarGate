using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using StarGate.Core.Graphics.Controls;
using StarGate.Core.Graphics.Styling;
using YamlDotNet.Serialization;

namespace StarGate.Core.Graphics.Elements;

/// <summary>
/// Represents a Windows combo box control combining a drop-down list with an optional text entry box.
/// </summary>
public class ComboBoxElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.ComboBox;

    /// <summary>
    /// Gets the visual drop-down style (Simple, DropDown, or DropDownList).
    /// </summary>
    public ComboBoxStyle? Style { get; init; }

    /// <summary>
    /// Gets the width in pixels of the drop-down list when expanded.
    /// </summary>
    public int? Width { get; init; }

    /// <summary>
    /// Gets the maximum number of items visible in the drop-down list before scrolling occurs.
    /// </summary>
    public int? MaxItems { get; init; }

    /// <summary>
    /// Gets the maximum number of characters permitted when typing into the combo box (0 to 10,000).
    /// </summary>
    [Range(0, 10000, ErrorMessage = "MaxChars must be between 0 and 10000.")]
    public int? MaxChars { get; init; }

    /// <summary>
    /// Gets a value indicating whether the drop-down list automatically resizes to display complete rows without clipping.
    /// </summary>
    [DefaultValue(true)]
    public bool IntegralHeight { get; init; } = true;

    /// <summary>
    /// Gets the two-way data binding linking the selected value to a Galaxy tag or attribute reference.
    /// </summary>
    public ValueBinding? Binding { get; init; }

    /// <summary>
    /// Gets the source of items for this combo box (static list, enum, or array reference).
    /// </summary>
    public ItemBinding? Items { get; init; }

    /// <summary>
    /// Gets a value indicating whether static items in the combo box are sorted alphabetically.
    /// </summary>
    public bool? Sorted { get; init; }

    /// <summary>
    /// Gets a value indicating whether duplicate items are permitted in the list.
    /// </summary>
    public bool? Duplicates { get; init; }
}
