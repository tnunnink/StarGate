using StarGate.Core.Objects;

namespace StarGate.Core.Abstractions;

/// <summary>
/// Represents an interface for implementing attribute-based extensions.
/// Attribute extensions allow additional functionality or metadata handling
/// for specific data types in the system.
/// </summary>
public interface IAttributeExtension
{
    /// <summary>
    /// Determines whether the extension supports the specified data type for a given attribute.
    /// </summary>
    /// <param name="dataType">The data type to check for support.</param>
    /// <returns>True if the specified data type is supported; otherwise, false.</returns>
    bool Supports(DataType dataType);
}