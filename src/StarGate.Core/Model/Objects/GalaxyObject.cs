using System;
using System.Collections.Generic;
using System.Linq;

namespace StarGate.Core;

/// <summary>
/// Represents the complete definition of an object, including its metadata, assignment hierarchy, attributes, scripts,
/// and graphics.
/// </summary>
public class GalaxyObject
{
    /// <summary>
    /// Gets or sets the unique tag name of the object.
    /// </summary>
    public required string TagName { get; init; }

    /// <summary>
    /// Gets or sets the name of the template or object from which this object is derived.
    /// </summary>
    public required string Template { get; init; }

    /// <summary>
    /// Gets or sets the base template or object model this definition is based on.
    /// </summary>
    public string? BasedOn { get; init; }

    /// <summary>
    /// Gets or sets the category or classification of the object.
    /// </summary>
    public string? Category { get; init; }

    /// <summary>
    /// Gets or sets the fully qualified hierarchical name within the object containment hierarchy.
    /// </summary>
    public string? HierarchicalName { get; init; }

    /// <summary>
    /// Gets or sets the local name of the object within its parent container.
    /// </summary>
    public string? ContainedName { get; init; }

    /// <summary>
    /// Gets or sets the logical plant area assigned to the object.
    /// </summary>
    public string? Area { get; init; }

    /// <summary>
    /// Gets or sets the host execution platform or engine where the object resides.
    /// </summary>
    public string? Host { get; init; }

    /// <summary>
    /// Gets or sets the identifier or name of the parent container object.
    /// </summary>
    public string? Container { get; init; }

    /// <summary>
    /// Gets or sets the alias name for the object.
    /// </summary>
    public string? Alias { get; init; }

    /// <summary>
    /// Gets or sets the collection of configured attributes associated with the object.
    /// </summary>
    public IEnumerable<ObjectAttribute> Attributes { get; init; } = [];

    /// <summary>
    /// Gets or sets the collection of script extensions associated with the object.
    /// </summary>
    public IEnumerable<ScriptExtension> Scripts { get; init; } = [];

    /// <summary>
    /// Gets or sets the collection of graphic extensions associated with the object.
    /// </summary>
    public IEnumerable<GraphicExtension> Graphics { get; init; } = [];
}

public static class ObjectDefinitionExtensions
{
    /// <summary>
    /// Orders a collection of <see cref="GalaxyObject" /> objects based on their inheritance hierarchy,
    /// ensuring that parent objects appear before their child objects.
    /// </summary>
    /// <param name="items">The collection of <see cref="GalaxyObject" /> objects to be ordered.</param>
    /// <returns>An <see cref="IEnumerable{T}" /> containing the <see cref="GalaxyObject" /> objects in inheritance order.</returns>
    public static IEnumerable<GalaxyObject> OrderByInheritance(this IEnumerable<GalaxyObject> items)
    {
        var list = items.ToList();
        var lookup = list.ToDictionary(x => x.TagName, StringComparer.OrdinalIgnoreCase);
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var ordered = new List<GalaxyObject>();

        foreach (var item in list)
            Visit(item);

        return ordered;

        void Visit(GalaxyObject item)
        {
            if (!visited.Add(item.TagName)) return;

            // If its parent is ALSO in the import batch, process the parent first
            if (lookup.TryGetValue(item.Template, out var parent))
                Visit(parent);

            ordered.Add(item);
        }
    }
}