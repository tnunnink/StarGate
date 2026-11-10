using System;
using StarGate.Core.Helpers;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Provides extension methods that support serialization and deserialization workflows.
/// </summary>
public static class SerializationExtensions
{
    /// <summary>
    /// Deserializes an object from the current YAML value using the specified object deserializer.
    /// </summary>
    /// <param name="deserializer">The object deserializer used to deserialize the current value.</param>
    /// <typeparam name="T">The type to deserialize the value as.</typeparam>
    /// <returns>The deserialized value as an instance of <typeparamref name="T" />.</returns>
    public static T Deserialize<T>(this ObjectDeserializer deserializer)
    {
        return (T)deserializer(typeof(T))!;
    }

    /// <param name="parser">The YAML parser positioned at the start of a mapping.</param>
    extension(IParser parser)
    {
        /// <summary>
        /// Consumes the current YAML mapping from the parser and invokes the specified handler for each key-value pair.
        /// </summary>
        /// <param name="deserializer">The object deserializer used by property readers to deserialize mapping values.</param>
        /// <param name="handler">The callback to invoke for each property encountered in the mapping.</param>
        public void ConsumeMap(ObjectDeserializer deserializer, Action<Scalar, PropertyReader> handler)
        {
            parser.Consume<MappingStart>();

            while (!parser.TryConsume<MappingEnd>(out _))
            {
                var key = parser.Consume<Scalar>();
                var reader = new PropertyReader(parser, deserializer);
                handler(key, reader);
            }
        }
    }

    /// <param name="emitter">The YAML emitter that writes the mapping events.</param>
    extension(IEmitter emitter)
    {
        /// <summary>
        /// Emits a YAML mapping by writing the mapping start event, invoking the specified action to emit the mapping contents,
        /// and then writing the mapping end event.
        /// </summary>
        /// <param name="mapping">The action that emits the contents of the mapping.</param>
        /// <param name="style">The mapping style that specifies the output format (block or flow).</param>
        public void EmitMap(Action<IEmitter> mapping, MappingStyle style = MappingStyle.Block)
        {
            emitter.Emit(new MappingStart(AnchorName.Empty, TagName.Empty, true, style));
            mapping(emitter);
            emitter.Emit(new MappingEnd());
        }

        /// <summary>
        /// Emits a YAML sequence by writing the sequence start event, invoking the specified action
        /// to emit the sequence items, and then writing the sequence end event.
        /// </summary>
        /// <param name="sequence">The action that emits the contents of the sequence.</param>
        /// <param name="style">The sequence style (flow [a, b] or block).</param>
        public void EmitSequence(Action<IEmitter> sequence, SequenceStyle style = SequenceStyle.Flow)
        {
            emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, style));
            sequence(emitter);
            emitter.Emit(new SequenceEnd());
        }

        /// <summary>
        /// Emits a key and delegates serialization of the value to the YamlDotNet serializer.
        /// </summary>
        public void Emit<T>(T value, ObjectSerializer serializer)
        {
            serializer(value, typeof(T));
        }

        /// <summary>
        /// Emits a YAML scalar value using the specified scalar style.
        /// </summary>
        /// <param name="value">The scalar value to emit.</param>
        /// <param name="style">The scalar style to apply to the emitted value.</param>
        public void Emit(string value, ScalarStyle style = ScalarStyle.Plain)
        {
            emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value, style, true, false));
        }

        /// <summary>
        /// Emits a key and delegates serialization of the value to the YamlDotNet serializer.
        /// </summary>
        public void Emit<T>(string key, T value, ObjectSerializer serializer)
        {
            emitter.Emit(new Scalar(key));
            serializer(value, typeof(T));
        }

        /// <summary>
        /// Emits a YAML scalar key and scalar value using the specified value style, then returns the emitter.
        /// </summary>
        /// <param name="key">The scalar key to emit.</param>
        /// <param name="value">The scalar value to emit.</param>
        /// <param name="style">The scalar style to apply to the emitted value.</param>
        /// <returns>The same emitter after the key and value have been emitted.</returns>
        public void Emit(string key, string value, ScalarStyle style = ScalarStyle.Plain)
        {
            emitter.Emit(new Scalar(key));
            emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value, style, true, false));
        }

        /// <summary>
        /// Emits a key and serializes the associated value only when the specified condition is true.
        /// </summary>
        /// <typeparam name="T">The type of the value to serialize.</typeparam>
        /// <param name="condition">A value indicating whether the key and value should be emitted.</param>
        /// <param name="key">The YAML mapping key to emit when the condition is true.</param>
        /// <param name="value">The value to serialize when the condition is true.</param>
        /// <param name="serializer">The serializer used to emit the value.</param>
        public void EmitIf<T>(bool condition, string key, T value, ObjectSerializer serializer)
        {
            if (condition)
            {
                emitter.Emit(new Scalar(key));
                serializer(value, typeof(T));
            }
        }

        /// <summary>
        /// Emits a YAML key-value scalar pair when the specified value is not null or empty.
        /// </summary>
        /// <param name="key">The YAML mapping key to emit.</param>
        /// <param name="value">The scalar value to emit when it is not null or empty.</param>
        /// <param name="style">The scalar style to apply to the emitted value.</param>
        public void EmitIfNotEmpty(string key, string? value, ScalarStyle style = ScalarStyle.Plain)
        {
            if (value is not null && !string.IsNullOrEmpty(value))
            {
                emitter.Emit(new Scalar(key));
                emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value, style, true, false));
            }
        }
    }
}