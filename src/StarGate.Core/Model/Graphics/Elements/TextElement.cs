using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a text label graphic element without background fill or border outline.
/// </summary>
public class TextElement : GraphicElement
{
    /// <inheritdoc />
    
    public override ElementKind Kind => ElementKind.Text;

    /// <summary>
    /// Gets the text string content to be displayed.
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
}