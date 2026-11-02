using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

public class ScriptExtensionHandler : IConfigurationHandler<ScriptExtension>
{
    public ScriptExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        if (!target.HasExtension(typeof(ScriptExtension), context))
            return null;

        return null;
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, ScriptExtension? source, string? context = null)
    {
        return GalaxyResult.Ok();
    }
}