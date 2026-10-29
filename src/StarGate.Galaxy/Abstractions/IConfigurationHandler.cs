using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy.Abstractions;

/// <summary>
/// Defines a contract for handling the extraction and application of configuration data
/// for Galaxy objects of a specific type.
/// </summary>
/// <typeparam name="T">The type of the configuration object being handled.</typeparam>
public interface IConfigurationHandler<T>
{
    /// <summary>
    /// Extracts configuration or mapping data from the specified target Galaxy object.
    /// </summary>
    /// <param name="target">The target Galaxy object from which data is extracted.</param>
    /// <param name="context">Optional context for extraction, such as an attribute/extension primitive name or path, or a check-in comment for root objects.</param>
    /// <returns>The extracted configuration object of type <typeparamref name="T"/>, or <c>null</c> if not found.</returns>
    T? Extract(IgObject target, string? context = null);

    /// <summary>
    /// Applies the provided source configuration data to the specified target Galaxy object.
    /// </summary>
    /// <param name="target">The target Galaxy object to which the source configuration will be applied.</param>
    /// <param name="source">The source configuration object containing the data to be applied. Can be null.</param>
    /// <param name="context">Optional context for applying changes, such as an attribute/extension primitive name or path, or a check-in comment for root objects.</param>
    /// <returns>A <see cref="GalaxyResult"/> indicating the result of the apply operation.</returns>
    GalaxyResult Apply(IgObject target, T? source, string? context = null);
}