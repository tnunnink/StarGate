using System.Collections.Generic;
using StarGate.Core.Objects;

namespace StarGate.Core.Abstractions;

/// <summary>
/// Represents a repository for managing galaxy configurations, including the retrieval
/// and modification of object definitions.
/// </summary>
public interface IGalaxyRepository
{
    /// <summary>
    /// Retrieves a collection of configured objects based on provided tag names.
    /// </summary>
    /// <param name="tagNames">
    /// An array of tag names used to identify the objects to retrieve.
    /// Tags prefixed with '$' represent templates, while others represent instances.
    /// </param>
    /// <returns>
    /// A <c>GalaxyResult</c> containing a collection of <c>ObjectConfig</c> representing the matching objects.
    /// </returns>
    GalaxyResult<IEnumerable<ObjectDefinition>> GetObjects(params string[] tagNames);

    /// <summary>
    /// Updates the configuration of an existing object in the galaxy repository.
    /// </summary>
    /// <param name="definition">
    /// The definition of the object to update, containing its properties, attributes, and associated configurations.
    /// </param>
    /// <param name="comment">
    /// An optional comment describing the reason or context for the update.
    /// </param>
    /// <returns>
    /// A <c>GalaxyResult</c> indicating the success or failure of the update operation, along with error details if applicable.
    /// </returns>
    GalaxyResult UpdateObject(ObjectDefinition definition, string? comment = null);

    /// <summary>
    /// Updates a collection of object definitions in the repository with optional comments detailing the changes.
    /// </summary>
    /// <param name="definitions">
    /// A collection of <c>ObjectDefinition</c> instances representing the object configurations to update.
    /// Each definition contains metadata and attributes describing the object.
    /// </param>
    /// <param name="comment">
    /// An optional string providing additional context or notes about the update operation.
    /// If not provided, the update will proceed without a comment.
    /// </param>
    /// <returns>
    /// A <c>GalaxyResult</c> object indicating the success or failure of the update operation.
    /// Contains error details if the operation fails.
    /// </returns>
    GalaxyResult UpdateObjects(IEnumerable<ObjectDefinition> definitions, string? comment = null);
}