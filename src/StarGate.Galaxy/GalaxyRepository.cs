using System;
using System.Collections.Generic;
using System.Linq;
using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Core.Abstractions;
using StarGate.Core.Objects;
using StarGate.Galaxy.Extensions;
using StarGate.Galaxy.Handlers;

namespace StarGate.Galaxy;

/// <summary>
/// Provides a repository for querying and retrieving configuration objects from an ArchestrA Galaxy.
/// </summary>
/// <param name="galaxy">The <see cref="IGalaxy"/> instance used to access and query the Galaxy repository.</param>
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
    public GalaxyResult<IEnumerable<ObjectDefinition>> GetObjects(params string[] tagNames)
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
    public GalaxyResult UpdateObject(ObjectDefinition definition, string? comment = null)
    {
        var created = galaxy.EnsureObjectCreated(definition, out var wasCreated);
        if (!created.Success) return created;

        comment ??= $"Configuration applied to '{definition.TagName}' via StarGate";
        var result = ObjectHandler.Apply(created.Value, definition, comment);

        if (!result.Success && wasCreated)
            return result.Merge(created.Value.Delete());

        return result;
    }

    /// <inheritdoc />
    public GalaxyResult UpdateObjects(IEnumerable<ObjectDefinition> definitions, string? comment = null)
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
    /// A GalaxyResult containing a collection of <see cref="ObjectDefinition"/> objects matching the provided tag names.
    /// </returns>
    private GalaxyResult<IEnumerable<ObjectDefinition>> GetObjectsByName(string[] tagNames,
        EgObjectIsTemplateOrInstance option)
    {
        if (tagNames.Length == 0)
            return new GalaxyResult<IEnumerable<ObjectDefinition>>([]);

        var results = galaxy.QueryObjectsByName(option, tagNames)
            .AsEnumerable()
            .Select(x => ObjectHandler.Extract(x))
            .ToArray();

        return galaxy.CommandResults.ToResult<IEnumerable<ObjectDefinition>>(results);
    }

    /// <summary>
    /// Retrieves instances from the Galaxy repository by their tag names.
    /// </summary>
    /// <param name="tagNames">An array of tag names corresponding to the instances to be retrieved.</param>
    /// <returns>
    /// A <see cref="GalaxyResult{T}"/> containing an enumerable collection of <see cref="ObjectDefinition"/>
    /// objects that represent the retrieved instances.
    /// </returns>
    private GalaxyResult<IEnumerable<ObjectDefinition>> GetInstancesAndSymbolsByName(string[] tagNames)
    {
        if (tagNames.Length == 0)
            return new GalaxyResult<IEnumerable<ObjectDefinition>>([]);

        const EgObjectIsTemplateOrInstance option = EgObjectIsTemplateOrInstance.gObjectIsInstance;
        var handler = new ObjectDefinitionHandler();
        var results = new List<ObjectDefinition>();

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

        return galaxy.CommandResults.ToResult<IEnumerable<ObjectDefinition>>(results.ToArray());
    }

    /// <summary>
    /// Retrieves template information for the specified tag names from the Galaxy repository.
    /// </summary>
    /// <param name="tagNames">An array of tag names for which to retrieve template information.</param>
    /// <returns>A <see cref="GalaxyResult{T}"/> containing a collection of <see cref="ObjectInfo"/>
    /// representing the template information, along with any associated errors.</returns>
    private GalaxyResult<IEnumerable<ObjectInfo>> GetTemplateInfo(string[] tagNames)
    {
        if (tagNames.Length == 0)
            return new GalaxyResult<IEnumerable<ObjectInfo>>([]);

        const EgObjectIsTemplateOrInstance option = EgObjectIsTemplateOrInstance.gObjectIsTemplate;

        var results = galaxy.QueryObjectsByName(option, tagNames)
            .AsEnumerable()
            .Select(x => x.ToInfo())
            .ToArray();

        return galaxy.CommandResults.ToResult<IEnumerable<ObjectInfo>>(results);
    }

    /// <summary>
    /// Retrieves instance information for the specified tag names from the Galaxy repository.
    /// </summary>
    /// <param name="tagNames">An array of tag names to query for instance information.</param>
    /// <returns>A <see cref="GalaxyResult{T}"/> containing a collection of <see cref="ObjectInfo"/> instances for the specified tag names.</returns>
    private GalaxyResult<IEnumerable<ObjectInfo>> GetInstanceInfo(string[] tagNames)
    {
        if (tagNames.Length == 0)
            return new GalaxyResult<IEnumerable<ObjectInfo>>([]);

        const EgObjectIsTemplateOrInstance option = EgObjectIsTemplateOrInstance.gObjectIsInstance;
        var results = new List<ObjectInfo>();

        foreach (var tagName in tagNames)
        {
            var conditions = galaxy.CreateConditionsObject();
            //todo I'm not sure if this is the most performant way. We could test splitting the instances/symbols out to separate methods.
            // This will effectively make it search for both AutomationObjects and VisualElements (symbols)
            // during testing it does seem that adding a tagname is much faster than querying the entire namespace and post filtering
            conditions.Add(EConditionType.NameSpaceIdIs, 2, false);
            conditions.Add(EConditionType.NameEquals, tagName);

            var instances = galaxy.QueryObjectsMultiCondition(option, conditions)
                .AsEnumerable()
                .Select(x => x.ToInfo());

            results.AddRange(instances);
        }

        return galaxy.CommandResults.ToResult<IEnumerable<ObjectInfo>>(results.ToArray());
    }
}