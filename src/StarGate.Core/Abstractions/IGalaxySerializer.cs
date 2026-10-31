namespace StarGate.Core;

/// <summary>
/// Provides an abstraction for serializing and deserializing objects and collections
/// of objects to and from various formats.
/// </summary>
public interface IGalaxySerializer
{
    /// <summary>
    /// Serializes an object into its string representation in a specified format.
    /// </summary>
    /// <param name="data">The object to be serialized.</param>
    /// <returns>A string representation of the serialized object.</returns>
    string Serialize(object data);

    /// <summary>
    /// Deserializes a string representation of data into a collection of items of type T.
    /// </summary>
    /// <param name="data">The string containing the serialized data to be deserialized.</param>
    /// <returns>A collection of items of type T that have been deserialized from the provided string.</returns>
    T Deserialize<T>(string data);
}