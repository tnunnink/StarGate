using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Galaxy.Abstractions;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Handlers;

public class GraphicExtensionHandler : IConfigurationHandler<GraphicExtension>
{
    public GraphicExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        if (!target.HasExtension(typeof(GraphicExtension), context))
            return null;

        return null;
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, GraphicExtension? source, string? context = null)
    {
        return GalaxyResult.Ok();
    }
}