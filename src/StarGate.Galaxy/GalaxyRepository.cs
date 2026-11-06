using System;
using System.Collections.Generic;
using System.Linq;
using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

/// <summary>
/// Provides a repository for querying and retrieving configuration objects from an ArchestrA Galaxy.
/// </summary>
/// <param name="galaxy">The <see cref="IGalaxy" /> instance used to access and query the Galaxy repository.</param>
public class GalaxyRepository(IGalaxy galaxy) : IGalaxyRepository
{
    /// <summary>
    /// Represents the identifier prefix used to differentiate template objects
    /// from instance objects within the Galaxy repository.
    /// </summary>
    private const string TemplateToken = "$";

    /// <summary>
    /// Handles the extraction and application of object definitions within the context of the Galaxy repository.
    /// Used to streamline operations related to managing object definitions for instances and templates.
    /// </summary>
    private static readonly GalaxyObjectHandler GalaxyObjectHandler = new();

    /// <inheritdoc />
    public GalaxyResult<IEnumerable<GalaxyObject>> GetObjects(params string[] tagNames)
    {
        if (tagNames is null)
            throw new ArgumentNullException(nameof(tagNames));

        var templateNames = tagNames.Where(t => t.StartsWith(TemplateToken)).ToArray();
        var instanceNames = tagNames.Where(t => !t.StartsWith(TemplateToken)).ToArray();

        var templates = galaxy.GetObjectsByName(templateNames, EgObjectIsTemplateOrInstance.gObjectIsTemplate);
        var instances = galaxy.GetObjectsByName(instanceNames, EgObjectIsTemplateOrInstance.gObjectIsInstance);

        return templates.Merge(instances, (t, i) => t.Concat(i).ToArray());
    }

    /// <inheritdoc />
    public GalaxyResult<IEnumerable<GalaxySymbol>> GetSymbols(params string[] tagNames)
    {
        if (tagNames is null)
            throw new ArgumentNullException(nameof(tagNames));

        var handler = new GalaxySymbolHandler(galaxy);
        var symbols = galaxy.GetSymbols(tagNames).AsEnumerable().Select(x => handler.Extract(x)).ToArray();

        return galaxy.CommandResults.ToResult<IEnumerable<GalaxySymbol>>(symbols);
    }

    /// <inheritdoc />
    public GalaxyResult UpdateObject(GalaxyObject config, string? comment = null)
    {
        var created = galaxy.EnsureObjectCreated(config, out var wasCreated);
        if (!created.Success) return created;

        comment ??= $"Configuration applied to '{config.TagName}' via StarGate";
        var result = GalaxyObjectHandler.Apply(created.Value, config, comment);

        if (!result.Success && wasCreated)
            return result.Merge(created.Value.Delete());

        return result;
    }

    /// <inheritdoc />
    public GalaxyResult UpdateObjects(IEnumerable<GalaxyObject> definitions, string? comment = null)
    {
        var ordered = definitions.OrderByInheritance();
        var result = GalaxyResult.Ok();

        foreach (var definition in ordered)
        {
            var update = UpdateObject(definition, comment);
            result = result.Merge(update);
        }

        return result;
    }
}