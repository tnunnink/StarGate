using YamlDotNet.Serialization;

namespace StarGate.Core.Graphics.Elements;

/// <summary>
/// Represents an ellipse or circle graphic element defined by its bounding rectangle.
/// </summary>
public class EllipseElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.Ellipse;
}