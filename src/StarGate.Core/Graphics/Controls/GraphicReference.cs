namespace StarGate.Core.Graphics.Controls;

/// <summary>
/// Represents a graphic value that can be specified as either a literal text string
/// or a dynamic Galaxy reference expression.
/// </summary>
public class GraphicReference
{
    /// <summary>
    /// Gets the literal text value of the graphic or the static content
    /// represented by this instance.
    /// </summary>
    public string? Text { get; init; }

    /// <summary>
    /// Gets the dynamic Galaxy reference expression associated with this instance,
    /// allowing the graphic value to be resolved dynamically at runtime.
    /// </summary>
    public string? Expression { get; init; }

    /// <summary>
    /// Implicitly converts a literal string into a static <see cref="GraphicReference"/>.
    /// </summary>
    public static implicit operator GraphicReference(string text) => new() { Text = text };
}