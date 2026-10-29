using System.Linq;
using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Core.Objects;
using StarGate.Galaxy.Abstractions;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Handlers;

public class ObjectDefinitionHandler : IConfigurationHandler<ObjectDefinition>
{
    private readonly ObjectAttributeHandler _attributeHandler = new();
    private readonly ScriptExtensionHandler _scriptHandler = new();
    private readonly GraphicExtensionHandler _graphicHandler = new();

    public ObjectDefinition Extract(IgObject target, string? context = null)
    {
        return new ObjectDefinition
        {
            TagName = target.Tagname,
            BasedOn = target.basedOn,
            Template = target.DerivedFrom,
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
                .Cast<ObjectAttribute>()
                .ToArray(),
            Scripts = [],
            Graphics = []
        };
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, ObjectDefinition? source, string? context = null)
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