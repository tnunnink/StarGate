namespace StarGate.Core;

/// <summary>
/// Represents a common Windows checkbox control with two-way data binding and optional caption overrides.
/// </summary>
public class CheckBoxElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.CheckBox;

    /// <summary>
    /// Gets the label text displayed adjacent to the checkbox.
    /// </summary>
    public string Caption { get; set; } = string.Empty;

    /// <summary>
    /// Gets the default initial checked state of the control.
    /// </summary>
    public bool? Checked { get; set; }

    /// <summary>
    /// Gets the visual border and box style (Flat or ThreeD).
    /// </summary>
    public ControlStyle? ControlStyle { get; set; }

    /// <summary>
    /// Gets the two-way data binding linking the checked state to a Galaxy attribute reference.
    /// </summary>
    public ValueBinding? Binding { get; set; }

    /// <summary>
    /// Gets an optional runtime expression that dynamically overrides the caption text.
    /// </summary>
    public string? CaptionOverride { get; set; }
}