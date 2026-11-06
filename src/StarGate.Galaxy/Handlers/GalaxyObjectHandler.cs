using System.Linq;
using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

public class GalaxyObjectHandler : IConfigurationHandler<GalaxyObject>
{
    private readonly ObjectAttributeHandler _attributeHandler = new();
    private readonly GraphicExtensionHandler _graphicHandler = new();
    private readonly ScriptExtensionHandler _scriptHandler = new();

    public GalaxyObject Extract(IgObject target, string? context = null)
    {
        return new GalaxyObject
        {
            TagName = target.Tagname,
            BasedOn = target.basedOn,
            DerivedFrom = target.DerivedFrom,
            Category = target.category.ToString().Replace("idxCategory", string.Empty),
            HierarchicalName = target.HierarchicalName,
            ContainedName = !string.IsNullOrEmpty(target.ContainedName) ? target.ContainedName : null,
            Container = target.Container,
            Host = target.Host,
            Area = target.Area,
            Alias = target.Alias,
            Attributes = target.ListUda()
                .Select(name => _attributeHandler.Extract(target, name))
                .Where(a => a is not null)
                .Cast<GalaxyAttribute>()
                .ToArray(),
            Scripts = [],
            Graphics = []
        };
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, GalaxyObject? source, string? context = null)
    {
        if (source is null) return GalaxyResult.Ok();

        // todo we want to also handle reassigning the object

        var result = target
            .Execute(x => x.CheckOut())
            .ThenAll(source.Attributes, x => _attributeHandler.Apply(target, x))
            .ThenAll(source.Scripts, x => _scriptHandler.Apply(target, x))
            .ThenAll(source.Graphics, x => _graphicHandler.Apply(target, x));

        if (!result.Success)
            return result.Merge(target.Rollback());

        return target.Commit(context ?? string.Empty);
    }
}