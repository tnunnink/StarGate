using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents an anchor point placed on a graphic element where connectors can dock.
/// </summary>
public record ConnectionPointElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.ConnectionPoint;

    /// <summary>
    /// Gets the name of the host graphic element that owns this connection point.
    /// </summary>
    public required string Host { get; init; }
}