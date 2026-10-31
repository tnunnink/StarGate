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
        return _inner.GetProperties(type, container)
            .OrderBy(p => GetPropertyDepth(type, p))
            .ThenBy(p => p.Order);
    }

    public IPropertyDescriptor GetProperty(
        Type type,
        object? container,
        string name,
        bool ignoreUnmatched,
        bool caseInsensitivePropertyMatching)
    {
        return _inner.GetProperty(type, container, name, ignoreUnmatched, caseInsensitivePropertyMatching);
    }

    public string GetEnumName(Type enumType, string name)
    {
        return _inner.GetEnumName(enumType, name);
    }

    public string GetEnumValue(object enumValue)
    {
        return _inner.GetEnumValue(enumValue);
    }

    public bool HasParseMethod(Type type)
    {
        return _inner.HasParseMethod(type);
    }

    public object? Parse(string value, Type expectedType)
    {
        return _inner.Parse(value, expectedType);
    }

    private static int GetPropertyDepth(Type targetType, IPropertyDescriptor property)
    {
        // Find the PropertyInfo on targetType matching the descriptor's Name
        var properties = targetType.GetProperty(
            property.Name,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
        );

        var declaringType = properties?.DeclaringType ?? targetType;

        // Calculate depth from System.Object
        var depth = 0;
        var current = declaringType;
        while (current != null && current != typeof(object))
        {
            depth++;
            current = current.BaseType;
        }

        return depth;
    }
}