using StarGate.Core.Graphics.Styling;
using YamlDotNet.Serialization;

namespace StarGate.Core.Graphics.Elements;

/// <summary>
/// Represents a text box graphic element with customizable background fill, border stroke, and text content.
/// </summary>
public class TextBoxElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.TextBox;
    
    /// <summary>
    /// Gets the text string content to be displayed.
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
}
