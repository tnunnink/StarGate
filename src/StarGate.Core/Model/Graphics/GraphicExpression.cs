namespace StarGate.Core;

/// <summary>
/// Abstract base class representing an expression value for graphic element properties,
/// supporting static literal values and dynamic references.
/// </summary>
public abstract class GraphicExpression
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphicExpression"/> class with the specified expression text.
    /// </summary>
    /// <param name="text">The raw text or identifier of the expression.</param>
    protected GraphicExpression(string text)
    {
        Text = text;
    }

    /// <summary>
    /// Gets the expression text or reference target string.
    /// </summary>
    public string Text { get; init; }

    /// <summary>
    /// Implicitly converts a literal string into a <see cref="StaticExpression"/>.
    /// </summary>
    /// <param name="text">The literal text to convert.</param>
    public static implicit operator GraphicExpression(string text) => new StaticExpression(text);
}

/// <summary>
/// Represents a static, literal expression value within a graphic element.
/// </summary>
/// <param name="text">The literal text value.</param>
public sealed class StaticExpression(string text) : GraphicExpression(text);

/// <summary>
/// Represents a dynamic reference expression that binds to a Galaxy tag, property, or attribute.
/// </summary>
/// <param name="text">The reference target string or identifier.</param>
public sealed class ReferenceExpression(string text) : GraphicExpression(text);