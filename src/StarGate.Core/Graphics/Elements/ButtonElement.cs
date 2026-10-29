using StarGate.Core.Graphics.Styling;
using YamlDotNet.Serialization;

namespace StarGate.Core.Graphics.Elements;

/// <summary>
/// Represents an interactive push-button graphic element with text, styling, and optional image states.
/// </summary>
public class ButtonElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.Button;

    /// <summary>
    /// Gets the visual style of the button (Standard or Image).
    /// </summary>
    public ButtonStyle ButtonStyle { get; init; } = ButtonStyle.Standard;

    /// <summary>
    /// Gets the text string displayed on the button.
    /// </summary>
    public string Text { get; init; } = string.Empty;

    /// <summary>
    /// Gets the format string applied to the text representation.
    /// </summary>
    public string? Format { get; init; }

    /// <summary>
    /// Gets a value indicating whether text wraps across multiple lines when exceeding bounds.
    /// </summary>
    public bool? WordWrap { get; init; }

    /// <summary>
    /// Gets a value indicating whether the text size automatically scales to fit within its container.
    /// </summary>
    public bool? AutoScale { get; init; }

    /// <summary>
    /// Specifies the rendering style for the image associated with a button element.
    /// </summary>
    public ImageStyle ImageStyle { get; init; }
    
    /// <summary>
    /// Gets the image displayed when the button is in its unpressed (up) state.
    /// </summary>
    public string? UpImage { get; init; }

    /// <summary>
    /// Gets the image displayed when the button is in its pressed (down) state.
    /// </summary>
    public string? DownImage { get; init; }
}
