using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// Represents a text input box control supporting single-line or multi-line editing, character limits, and tag
/// binding.
/// </summary>
public class EditBoxElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.EditBox;

    /// <summary>
    /// Gets the initial text displayed inside the edit box.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets a value indicating whether the edit box supports multiple lines of text.
    /// </summary>
    public bool? Multiline { get; set; }

    /// <summary>
    /// Gets a value indicating whether the edit box is read-only.
    /// </summary>
    public bool? ReadOnly { get; set; }

    /// <summary>
    /// Gets the maximum number of characters permitted in the edit box (0 to 10,000).
    /// </summary>
    [Range(0, 10000, ErrorMessage = "MaxChars must be between 0 and 10000.")]
    public int? MaxChars { get; set; }

    /// <summary>
    /// Gets the two-way data binding linking the text value to a Galaxy tag or attribute reference.
    /// </summary>
    public ValueBinding? Binding { get; set; }
}