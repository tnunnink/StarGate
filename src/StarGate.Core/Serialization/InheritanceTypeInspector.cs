using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// A type inspector implementation that modifies the behavior of property and type inspection,
/// specifically designed to enhance YAML serialization and deserialization. This class works
/// as an adapter or decorator around an existing <see cref="ITypeInspector"/> instance.
/// </summary>
/// <remarks>
/// The primary purpose of this class is to reorder properties based on their inheritance depth in
/// the class hierarchy, ensuring that inherited properties are prioritized over properties declared
/// in the current type. Other inspection behaviors remain consistent with the wrapped type inspector.
/// </remarks>
public sealed class InheritanceTypeInspector(ITypeInspector inner) : ITypeInspector
{
    private readonly ITypeInspector _inner = inner ?? throw new ArgumentNullException(nameof(inner));

    public IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
    {
        return _inner
            .GetProperties(type, container)
            .OrderBy(p => GetExplicitOrder(type, p))
            .ThenBy(p => GetPropertyDepth(type, p));
    }

    public IPropertyDescriptor GetProperty(
        Type type,
        object? container,
        string name,
        bool ignoreUnmatched,
        bool caseInsensitivePropertyMatching) =>
        _inner.GetProperty(type, container, name, ignoreUnmatched, caseInsensitivePropertyMatching);

    public string GetEnumName(Type enumType, string name) => _inner.GetEnumName(enumType, name);

    public string GetEnumValue(object enumValue) => _inner.GetEnumValue(enumValue);

    public bool HasParseMethod(Type type) => _inner.HasParseMethod(type);

    public object? Parse(string value, Type expectedType) => _inner.Parse(value, expectedType);

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
}