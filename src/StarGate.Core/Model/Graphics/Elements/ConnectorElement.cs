using System.Collections.Generic;
using System.ComponentModel;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a smart schematic or piping connector linking elements with straight or orthogonal routing.
/// </summary>
public record ConnectorElement : GraphicElement
{
    /// <inheritdoc />
    [YamlIgnore]
    public override ElementKind Kind => ElementKind.Connector;

    /// <summary>
    /// Gets the routing geometry of the connector (Straight or Angled).
    /// </summary>
    [DefaultValue(ConnectorType.Angled)]
    public ConnectorType Type { get; init; } = ConnectorType.Angled;

    /// <summary>
    /// Gets a value indicating whether the connector automatically updates its routing path when connected elements move.
    /// </summary>
    [DefaultValue(true)]
    public bool AutoUpdate { get; init; } = true;

    /// <summary>
    /// Gets the starting point coordinates of the connector.
    /// </summary>
    public GraphicPoint Start { get; init; }

    /// <summary>
    /// Gets the ending point coordinates of the connector.
    /// </summary>
    public GraphicPoint End { get; init; }

    /// <summary>
    /// Gets the name of the graphic element docked at the start of the connector, if any.
    /// </summary>
    public string? StartElement { get; init; }

    /// <summary>
    /// Gets the name of the graphic element docked at the end of the connector, if any.
    /// </summary>
    public string? EndElement { get; init; }

    /// <summary>
    /// Gets the handle position on the start element where the connector is docked.
    /// </summary>
    [DefaultValue(GrabHandle.None)]
    public GrabHandle StartHandle { get; init; } = GrabHandle.None;

    /// <summary>
    /// Gets the handle position on the end element where the connector is docked.
    /// </summary>
    [DefaultValue(GrabHandle.None)]
    public GrabHandle EndHandle { get; init; } = GrabHandle.None;

    /// <summary>
    /// Gets the intermediate waypoint coordinates defining the routing path of the connector.
    /// </summary>
    public IEnumerable<GraphicPoint> Points { get; init; } = [];
}