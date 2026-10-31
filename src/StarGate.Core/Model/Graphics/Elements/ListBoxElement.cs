using System.ComponentModel;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a Windows list box control for displaying and selecting from a scrollable list of items.
/// </summary>
public record ListBoxElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.ListBox;

    /// <summary>
    /// Gets a value indicating whether the list box automatically resizes to display complete rows of items without
    /// clipping.
    /// </summary>
    [DefaultValue(true)] //todo we need to go back and add this for all properties where the default is not false/0/empty so that yAML will not serialize when the are default
    public bool? IntegralHeight { get; init; }

    /// <summary>
    /// Gets a value indicating whether a horizontal scrollbar is displayed for items wider than the control.
    /// </summary>
    public bool? Scroll { get; init; }

    /// <summary>
    /// Gets the two-way data binding linking the selected item value to a Galaxy tag or attribute reference.
    /// </summary>
    public ValueBinding? Binding { get; init; }

    /// <summary>
    /// Gets the source of items for this list box (static list, enum, or array reference).
    /// </summary>
    public ItemBinding? Items { get; init; }

    /// <summary>
    /// Gets a value indicating whether static items in the list box are sorted alphabetically.
    /// </summary>
    public bool? Sorted { get; init; }

    /// <summary>
    /// Gets a value indicating whether duplicate items are permitted in the list.
    /// </summary>
    public bool? Duplicates { get; init; }
}