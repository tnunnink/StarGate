using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace StarGate.Core.Graphics.Elements;

/// <summary>
/// Represents a quality and data status indicator element that reflects the communication or alarm health of expressions or elements.
/// </summary>
public class StatusElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.Status;

    /// <summary>
    /// Gets the collection of attribute references or script expressions whose status is monitored.
    /// </summary>
    public IEnumerable<string> Expressions { get; init; } = [];

    /// <summary>
    /// Gets the collection of target element names whose status is monitored.
    /// </summary>
    public IEnumerable<string> Elements { get; init; } = [];
}
