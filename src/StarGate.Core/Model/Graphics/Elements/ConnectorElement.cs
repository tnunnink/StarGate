using System.Collections.Generic;
using System.ComponentModel;

namespace StarGate.Core;

/// <summary>
/// Represents a smart schematic or piping connector linking elements with straight or orthogonal routing.
/// </summary>
public class ConnectorElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.Connector;

    /// <summary>
    /// Gets the routing geometry of the connector (Straight or Angled).
    /// </summary>
    [DefaultValue(ConnectorType.Angled)]
    public ConnectorType Type { get; set; } = ConnectorType.Angled;

    /// <summary>
    /// Gets a value indicating whether the connector automatically updates its routing path when connected elements move.
    /// </summary>
    [DefaultValue(true)]
    public bool AutoUpdate { get; set; } = true;

    /// <summary>
    /// Gets the starting point coordinates of the connector.
    /// </summary>
    public GraphicPoint Start { get; set; }

    /// <summary>
    /// Gets the ending point coordinates of the connector.
    /// </summary>
    public GraphicPoint End { get; set; }

    /// <summary>
    /// Gets the name of the graphic element docked at the start of the connector, if any.
    /// </summary>
    public string? StartElement { get; set; }

    /// <summary>
    /// Gets the name of the graphic element docked at the end of the connector, if any.
    /// </summary>
    public string? EndElement { get; set; }

    /// <summary>
    /// Gets the handle position on the start element where the connector is docked.
    /// </summary>
    [DefaultValue(GrabHandle.None)]
    public GrabHandle StartHandle { get; set; } = GrabHandle.None;

    /// <summary>
    /// Gets the handle position on the end element where the connector is docked.
    /// </summary>
    [DefaultValue(GrabHandle.None)]
    public GrabHandle EndHandle { get; set; } = GrabHandle.None;

    /// <summary>
    /// Gets the intermediate waypoint coordinates defining the routing path of the connector.
    /// </summary>
    public IEnumerable<GraphicPoint> Points { get; set; } = [];
}