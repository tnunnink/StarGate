using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core.Helpers;

/// <summary>
/// Provides scoped helpers for reading the value associated with a YAML mapping property.
/// </summary>
/// <param name="parser">
/// The YAML parser positioned at the property value to be consumed or skipped.
/// </param>
/// <param name="deserializer">
/// The deserializer callback used to read complex, nested, or polymorphic values through the configured YamlDotNet pipeline.
/// </param>
/// <remarks>
/// This reader is intended for use while handling a single YAML property. It can read scalar values directly, delegate value materialization to the deserializer, or skip the current value including any nested content.
/// </remarks>
public readonly struct PropertyReader(IParser parser, ObjectDeserializer deserializer)
{
    /// <summary>
    /// Consumes the current YAML scalar event and returns its string value.
    /// </summary>
    /// <return>
    /// The value of the consumed scalar event.
    /// </return>
    public string Read() => parser.Consume<Scalar>().Value;

    /// <summary>
    /// Deserializes the current YAML property value as the specified type.
    /// </summary>
    /// <typeparam name="T">
    /// The type to deserialize the current value as.
    /// </typeparam>
    /// <return>
    /// The deserialized value, or <c>null</c> if the deserializer returns no value.
    /// </return>
    public T? Read<T>() => (T?)deserializer(typeof(T));

    /// <summary>
    /// Skips the current YAML value, including any nested events it contains.
    /// </summary>
    public void Skip() => parser.SkipThisAndNestedEvents();
}