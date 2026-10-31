using System;
using System.Collections.Generic;
using System.Linq;
using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Galaxy.Extensions;
using StarGate.Galaxy.Handlers;

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
    private static readonly ObjectDefinitionHandler ObjectHandler = new();

    /// <inheritdoc />
    public GalaxyResult<IEnumerable<GalaxyObject>> GetObjects(params string[] tagNames)
    {
        if (tagNames is null)
            throw new ArgumentNullException(nameof(tagNames));

        var templateNames = tagNames.Where(t => t.StartsWith(TemplateToken)).ToArray();
        var instanceNames = tagNames.Where(t => !t.StartsWith(TemplateToken)).ToArray();

        var templates = GetObjectsByName(templateNames, EgObjectIsTemplateOrInstance.gObjectIsTemplate);
        var instances = GetObjectsByName(instanceNames, EgObjectIsTemplateOrInstance.gObjectIsInstance);

        return templates.Merge(instances, (t, i) => t.Concat(i).ToArray());
    }

    /// <inheritdoc />
    public GalaxyResult UpdateObject(GalaxyObject config, string? comment = null)
    {
        var created = galaxy.EnsureObjectCreated(config, out var wasCreated);
        if (!created.Success) return created;

        comment ??= $"Configuration applied to '{config.TagName}' via StarGate";
        var result = ObjectHandler.Apply(created.Value, config, comment);

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

    /// <summary>
    /// Retrieves a collection of object definitions based on the provided tag names and their type (template or instance).
    /// </summary>
    /// <param name="tagNames">An array of strings representing the names of the tags to query.</param>
    /// <param name="option">Specifies whether the query should target object templates or instances.</param>
    /// <returns>
    /// A GalaxyResult containing a collection of <see cref="GalaxyObject" /> objects matching the provided tag names.
    /// </returns>
    private GalaxyResult<IEnumerable<GalaxyObject>> GetObjectsByName(string[] tagNames,
        EgObjectIsTemplateOrInstance option)
    {
        if (tagNames.Length == 0)
            return new GalaxyResult<IEnumerable<GalaxyObject>>([]);

        var results = galaxy.QueryObjectsByName(option, tagNames)
            .AsEnumerable()
            .Select(x => ObjectHandler.Extract(x))
            .ToArray();

        return galaxy.CommandResults.ToResult<IEnumerable<GalaxyObject>>(results);
    }

    /// <summary>
    /// Retrieves instances from the Galaxy repository by their tag names.
    /// </summary>
    /// <param name="tagNames">An array of tag names corresponding to the instances to be retrieved.</param>
    /// <returns>
    /// A <see cref="GalaxyResult{T}" /> containing an enumerable collection of <see cref="GalaxyObject" />
    /// objects that represent the retrieved instances.
    /// </returns>
    private GalaxyResult<IEnumerable<GalaxyObject>> GetInstancesAndSymbolsByName(string[] tagNames)
    {
        if (tagNames.Length == 0)
            return new GalaxyResult<IEnumerable<GalaxyObject>>([]);

        const EgObjectIsTemplateOrInstance option = EgObjectIsTemplateOrInstance.gObjectIsInstance;
        var handler = new ObjectDefinitionHandler();
        var results = new List<GalaxyObject>();

        foreach (var tagName in tagNames)
        {
            var conditions = galaxy.CreateConditionsObject();
            //todo I'm not sure if this is the most performant way. We could test splitting the instances/symbols out to separate methods.
            // This will effectively make it search for both AutomationObjects and VisualElements (symbols)
            conditions.Add(EConditionType.NameSpaceIdIs, 2, false);
            conditions.Add(EConditionType.NameEquals, tagName);

            var instances = galaxy.QueryObjectsMultiCondition(option, conditions)
                .AsEnumerable()
                .Select(x => handler.Extract(x));

            results.AddRange(instances);
        }

        return galaxy.CommandResults.ToResult<IEnumerable<GalaxyObject>>(results.ToArray());
    }
}