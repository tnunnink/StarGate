namespace StarGate.Core;

/// <summary>
/// Represents an interactive push-button graphic element with text, styling, and optional image states.
/// </summary>
public class ButtonElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.Button;

    /// <summary>
    /// Gets the visual style of the button (Standard or Image).
    /// </summary>
    public ButtonStyle ButtonStyle { get; set; } = ButtonStyle.Standard;

    /// <summary>
    /// Gets the text string displayed on the button.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets the format string applied to the text representation.
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// Gets a value indicating whether text wraps across multiple lines when exceeding bounds.
    /// </summary>
    public bool? WordWrap { get; set; }

    /// <summary>
    /// Gets a value indicating whether the text size automatically scales to fit within its container.
    /// </summary>
    public bool? AutoScale { get; set; }

    /// <summary>
    /// Specifies the rendering style for the image associated with a button element.
    /// </summary>
    public ImageStyle ImageStyle { get; set; }

    /// <summary>
    /// Gets the image displayed when the button is in its unpressed (up) state.
    /// </summary>
    public string? UpImage { get; set; }

    /// <summary>
    /// Gets the image displayed when the button is in its pressed (down) state.
    /// </summary>
    public string? DownImage { get; set; }
}