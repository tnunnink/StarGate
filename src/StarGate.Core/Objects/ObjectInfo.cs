namespace StarGate.Core.Objects;

/// <summary>
/// Represents metadata and configuration details about an object within the system.
/// </summary>
/// <remarks>
/// The <c>ObjectInfo</c> class contains core properties describing the identity, classification,
/// and configuration version of an object. It serves as a foundational component to model object characteristics
/// across different contexts.
/// </remarks>
public class ObjectInfo
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
}