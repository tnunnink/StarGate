using System.Xml.Linq;
using StarGate.Core;

namespace StarGate.Galaxy;

/// <summary>
/// Defines a contract for translating between an ArchestrA XML representation and a domain model.
/// </summary>
/// <typeparam name="T">The type of the domain model object being translated.</typeparam>
public interface IGraphicTranslator<T>
{
    /// <summary>
    /// Reads and converts an ArchestrA XML element into its strongly typed domain model representation.
    /// </summary>
    /// <param name="target">The source XML element to read from.</param>
    /// <returns>The deserialized domain model instance of type <typeparamref name="T" />.</returns>
    T? Read(XElement? target);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    XElement? Write(T? source);
}