using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a quality and data status indicator element that reflects the communication or alarm health of
/// expressions or elements.
/// </summary>
public class StatusElement : GraphicElement
{
    /// <inheritdoc />
    
    public override ElementKind Kind => ElementKind.Status;

    /// <summary>
    /// Gets the collection of attribute references or script expressions whose status is monitored.
    /// </summary>
    public IEnumerable<string> Expressions { get; set; } = [];

    /// <summary>
    /// Gets the collection of target element names whose status is monitored.
    /// </summary>
    public IEnumerable<string> Elements { get; set; } = [];
}