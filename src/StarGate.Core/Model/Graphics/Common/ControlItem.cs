namespace StarGate.Core;

/// <summary>
/// Represents an individual selectable item in a radio group, combo box, or list box.
/// </summary>
public class ControlItem
{
    /// <summary>
    /// Gets the underlying data value represented by this item.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets the visual label text displayed to the user for this item.
    /// </summary>
    public string Caption { get; set; } = string.Empty;

    /// <summary>
    /// Implicitly converts a single string into a <see cref="ControlItem" /> where Value and Caption are identical.
    /// </summary>
    public static implicit operator ControlItem(string text) => new() { Value = text, Caption = text };
}