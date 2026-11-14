namespace StarGate.Core;

/// <summary>
/// Represents an ellipse or circle graphic element defined by its bounding rectangle.
/// </summary>
public class EllipseElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.Ellipse;
}