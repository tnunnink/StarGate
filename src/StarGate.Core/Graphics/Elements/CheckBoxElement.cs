using StarGate.Core.Graphics.Controls;
using StarGate.Core.Graphics.Styling;
using YamlDotNet.Serialization;

namespace StarGate.Core.Graphics.Elements;

/// <summary>
/// Represents a common Windows checkbox control with two-way data binding and optional caption overrides.
/// </summary>
public class CheckBoxElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.CheckBox;

    /// <summary>
    /// Gets the label text displayed adjacent to the checkbox.
    /// </summary>
    public string Caption { get; init; } = string.Empty;

    /// <summary>
    /// Gets the default initial checked state of the control.
    /// </summary>
    public bool? Checked { get; init; }

    /// <summary>
    /// Gets the visual border and box style (Flat or ThreeD).
    /// </summary>
    public ControlStyle? ControlStyle { get; init; }

    /// <summary>
    /// Gets the two-way data binding linking the checked state to a Galaxy attribute reference.
    /// </summary>
    public ValueBinding? Binding { get; init; }

    /// <summary>
    /// Gets an optional runtime expression that dynamically overrides the caption text.
    /// </summary>
    public string? CaptionOverride { get; init; }
}
