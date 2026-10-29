using System.ComponentModel.DataAnnotations;
using StarGate.Core.Graphics.Controls;
using YamlDotNet.Serialization;

namespace StarGate.Core.Graphics.Elements;

/// <summary>
/// Represents a text input box control supporting single-line or multi-line editing, character limits, and tag binding.
/// </summary>
public class EditBoxElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.EditBox;

    /// <summary>
    /// Gets the initial text displayed inside the edit box.
    /// </summary>
    public string? Text { get; init; }

    /// <summary>
    /// Gets a value indicating whether the edit box supports multiple lines of text.
    /// </summary>
    public bool? Multiline { get; init; }

    /// <summary>
    /// Gets a value indicating whether the edit box is read-only.
    /// </summary>
    public bool? ReadOnly { get; init; }

    /// <summary>
    /// Gets the maximum number of characters permitted in the edit box (0 to 10,000).
    /// </summary>
    [Range(0, 10000, ErrorMessage = "MaxChars must be between 0 and 10000.")]
    public int? MaxChars { get; init; }

    /// <summary>
    /// Gets the two-way data binding linking the text value to a Galaxy tag or attribute reference.
    /// </summary>
    public ValueBinding? Binding { get; init; }
}