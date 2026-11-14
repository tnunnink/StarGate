using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Decorates a YamlDotNet type inspector to customize type metadata used during Galaxy serialization.
/// </summary>
/// <remarks>
/// Filters discovered properties according to Galaxy serialization rules and applies deterministic property ordering.
/// Delegates property lookup, enum conversion, parse-method detection, and parsing behavior to the wrapped inspector.
/// </remarks>
/// <param name="inner">The underlying type inspector that provides the base type metadata.</param>
/// <exception cref="ArgumentNullException"><paramref name="inner"/> is null.</exception>
public sealed class GalaxyTypeInspector(ITypeInspector inner) : ITypeInspector
{
    private readonly ITypeInspector _inner = inner ?? throw new ArgumentNullException(nameof(inner));

    /// <inheritdoc />
    public IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
    {
        return _inner
            .GetProperties(type, container)
            .Where(p => ShouldSerialize(type, container, p))
            .OrderBy(p => GetExplicitOrder(type, p))
            .ThenBy(p => GetPropertyDepth(type, p));
    }

    /// <inheritdoc />
    public IPropertyDescriptor GetProperty(
        Type type,
        object? container,
        string name,
        bool ignoreUnmatched,
        bool caseInsensitivePropertyMatching) =>
        _inner.GetProperty(type, container, name, ignoreUnmatched, caseInsensitivePropertyMatching);

    /// <inheritdoc />
    public string GetEnumName(Type enumType, string name) => _inner.GetEnumName(enumType, name);

    /// <inheritdoc />
    public string GetEnumValue(object enumValue) => _inner.GetEnumValue(enumValue);

    /// <inheritdoc />
    public bool HasParseMethod(Type type) => _inner.HasParseMethod(type);

    /// <inheritdoc />
    public object? Parse(string value, Type expectedType) => _inner.Parse(value, expectedType);

    /// <summary>
    /// Gets the explicitly configured serialization order for the specified property.
    /// </summary>
    /// <param name="targetType">The type to inspect for the property metadata.</param>
    /// <param name="property">The property descriptor whose order is evaluated.</param>
    /// <returns>The configured nonzero order value, or <see cref="int.MaxValue" /> when no explicit order is defined.</returns>
    private static int GetExplicitOrder(Type targetType, IPropertyDescriptor property)
    {
        var propertyInfo = targetType.GetProperty(
            property.Name,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
        );

        var member = propertyInfo?.GetCustomAttribute<YamlMemberAttribute>();
        if (member is not null && member.Order != 0)
        {
            return member.Order;
        }

        return int.MaxValue;
    }

    /// <summary>
    /// Calculates the inheritance depth used to order a property descriptor for the specified target type.
    /// </summary>
    /// <param name="targetType">The type used to locate the matching declared property.</param>
    /// <param name="property">The property descriptor whose name is used to determine the declaring type depth.</param>
    /// <returns>The depth of the declaring type in the inheritance hierarchy, excluding object.</returns>
    private static int GetPropertyDepth(Type targetType, IPropertyDescriptor property)
    {
        // Find the PropertyInfo on targetType matching the descriptor's Name
        var propertyInfo = targetType.GetProperty(
            property.Name,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
        );

        if (propertyInfo is null)
            return int.MaxValue;

        // If it's an overridden property, walk back to where it was originally defined
        var declaringType = propertyInfo.GetMethod?.GetBaseDefinition().DeclaringType ?? propertyInfo.DeclaringType;

        // Calculate depth from System.Object
        var depth = 0;
        var current = declaringType;

        while (current is not null && current != typeof(object))
        {
            depth++;
            current = current.BaseType;
        }

        return depth;
    }

    /// <summary>
    /// Determines whether the specified property should be serialized for the given target instance.
    /// </summary>
    /// <param name="targetType">The type used to locate a matching ShouldSerialize method for the property.</param>
    /// <param name="container">The object instance on which the ShouldSerialize method is invoked.</param>
    /// <param name="property">The property descriptor whose name is used to locate the corresponding ShouldSerialize method.</param>
    /// <returns>true if the property should be serialized; otherwise, false.</returns>
    private static bool ShouldSerialize(Type targetType, object? container, IPropertyDescriptor property)
    {
        var method = targetType.GetMethod(
            $"ShouldSerialize{property.Name}",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase,
            null,
            Type.EmptyTypes,
            null
        );

        if (container is not null && method is not null && method.ReturnType == typeof(bool))
        {
            return (bool)method.Invoke(container, null)!;
        }

        return true;
    }
}