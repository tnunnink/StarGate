using System.Collections.Generic;

namespace StarGate.Core;

/// <summary>
/// Represents a container grouping multiple graphic elements together into a single composite unit.
/// </summary>
public record GroupElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.Group;

    /// <summary>
    /// Gets a value indicating whether the group is treated as an icon during runtime operations.
    /// </summary>
    public bool? TreatAsIcon { get; init; }

    /// <summary>
    /// Gets the collection of child graphic elements contained within this group.
    /// </summary>
    public IEnumerable<GraphicElement> Elements { get; init; } = [];
}