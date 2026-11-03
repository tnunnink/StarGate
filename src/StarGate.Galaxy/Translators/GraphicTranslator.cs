using System.Xml.Linq;

namespace StarGate.Galaxy;

/// <summary>
/// Defines a contract for translating graphics between domain models and XML representations.
/// Provides methods to read and write graphical representations.
/// </summary>
public interface IGraphicTranslator
{
    /// <summary>
    /// Reads and converts the specified XML element into an instance of the target type or domain model.
    /// </summary>
    /// <param name="target">The XML element to be read. Can be null.</param>
    /// <returns>An instance of the translated object, or null if the input element is null or invalid.</returns>
    object? Read(XElement? target);

    /// <summary>
    /// Converts the specified source object into an XML representation.
    /// </summary>
    /// <param name="source">The source object to translate. Can be null.</param>
    /// <returns>An XML representation of the source object, or null if the input is null or invalid.</returns>
    XElement? Write(object? source);
}

/// <summary>
/// Represents an abstract translator responsible for converting graphics-related data
/// between different forms, such as between domain objects and XML representations.
/// </summary>
/// <typeparam name="T">The type of object to be translated.</typeparam>
public abstract class GraphicTranslator<T> : IGraphicTranslator
{
    /// <summary>
    /// Reads and converts the specified XML element into an instance of the target type.
    /// </summary>
    /// <param name="target">The XML element to be read and translated. Can be null.</param>
    /// <returns>An instance of the target type, or null if the input element is null or invalid.</returns>
    public abstract T? Read(XElement? target);

    /// <summary>
    /// Converts the specified source object of type <typeparamref name="T"/> into an XML representation.
    /// </summary>
    /// <param name="source">The source object to translate. Can be null.</param>
    /// <returns>An XML representation of the source object, or null if the input is null or invalid.</returns>
    public abstract XElement? Write(T? source);

    /// <inheritdoc />
    object? IGraphicTranslator.Read(XElement? target) => Read(target);

    /// <inheritdoc />
    XElement? IGraphicTranslator.Write(object? source) => source is T typed ? Write(typed) : null;
}