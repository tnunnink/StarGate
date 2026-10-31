using System;

namespace StarGate.Core;

/// <summary>
/// An attribute used to mark and provide metadata for galaxy attribute and object extensions.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class GalaxyExtensionAttribute(string name, ExtensionType type) : Attribute
{
    /// <summary>
    /// Gets the name of the galaxy extension. This property represents a unique identifier or descriptive name
    /// assigned to the extension for identifying its purpose or context within the system.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the type of the galaxy extension. This property indicates the category or classification
    /// of the extension, defining its functional role or significance within the system.
    /// </summary>
    public ExtensionType Type { get; } = type;
}

/// <summary>
/// Represents the type of extension that can be attributed to a galaxy-level component.
/// </summary>
public enum ExtensionType
{
    /// <summary>
    /// Specifies that the extension type is defined as an attribute level extension.
    /// </summary>
    Attribute,

    /// <summary>
    /// Indicates that the extension type is defined as an object level extension.
    /// </summary>
    Object
}