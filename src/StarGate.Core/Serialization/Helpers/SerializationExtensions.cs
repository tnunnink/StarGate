using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
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

        /// <summary>
        /// Consumes the current YAML sequence from the parser and deserializes each item as the specified type.
        /// </summary>
        /// <param name="type">The type to deserialize each sequence item as.</param>
        /// <param name="deserializer">The object deserializer used to deserialize sequence items.</param>
        /// <returns>The non-null deserialized sequence items.</returns>
        public IEnumerable<object> ConsumeSequence(Type type, ObjectDeserializer deserializer)
        {
            var items = new List<object>();
            parser.Consume<SequenceStart>();

            while (!parser.TryConsume<SequenceEnd>(out _))
            {
                var item = deserializer(type);
                if (item is null) continue;
                items.Add(item);
            }

            return items;
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
        /// Emits a YAML flow sequence by writing the sequence start event, serializing each item in the sequence,
        /// and then writing the sequence end event.
        /// </summary>
        /// <param name="sequence">The items to emit as elements of the YAML sequence.</param>
        /// <param name="serializer">The serializer used to emit each sequence item.</param>
        /// <typeparam name="T">The type of the sequence items.</typeparam>
        public void EmitSequence<T>(IEnumerable<T> sequence, ObjectSerializer serializer)
        {
            emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));

            foreach (var item in sequence)
                serializer(item, typeof(T));

            emitter.Emit(new SequenceEnd());
        }

        /// <summary>
        /// Emits a YAML scalar value using the specified scalar style.
        /// </summary>
        /// <param name="value">The scalar value to emit.</param>
        /// <param name="style">The scalar style to apply to the emitted value.</param>
        public void EmitValue(string value, ScalarStyle style = ScalarStyle.Plain)
        {
            emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value, style, true, false));
        }

        /// <summary>
        /// Emits a YAML property by writing the specified key as a scalar and serializing the associated value.
        /// </summary>
        /// <param name="key">The property key to emit.</param>
        /// <param name="value">The property value to serialize.</param>
        /// <param name="serializer">The object serializer used to emit the property value.</param>
        /// <typeparam name="T">The type of the property value.</typeparam>
        public void EmitProperty<T>(string key, T value, ObjectSerializer serializer)
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
        public void EmitProperty(string key, string value, ScalarStyle style = ScalarStyle.Plain)
        {
            emitter.Emit(new Scalar(key));
            emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value, style, true, false));
        }

        /// <summary>
        /// Emits a YAML mapping entry for the property selected from the specified container, when the property value should be serialized.
        /// </summary>
        /// <param name="container">The object that contains the property value to emit.</param>
        /// <param name="selector">An expression that selects the property to emit.</param>
        /// <param name="serializer">The serializer used to write the selected property value.</param>
        /// <typeparam name="TContainer">The type of the object that contains the selected property.</typeparam>
        /// <typeparam name="TValue">The type of the selected property value.</typeparam>
        /// <exception cref="ArgumentException">Thrown when <paramref name="selector" /> does not refer to a property.</exception>
        public void EmitProperty<TContainer, TValue>(
            TContainer container,
            Expression<Func<TContainer, TValue>> selector,
            ObjectSerializer serializer)
        {
            var expression = selector.Body switch
            {
                MemberExpression m => m,
                UnaryExpression { Operand: MemberExpression m } => m,
                _ => throw new ArgumentException("Selector must refer to a property.")
            };

            if (expression.Member is not PropertyInfo propertyInfo)
                throw new ArgumentException("Member must be a property.");

            if (!ShouldSerialize(propertyInfo, container, out var value))
                return;

            var key = char.ToLowerInvariant(propertyInfo.Name[0]) + propertyInfo.Name[1..];
            emitter.Emit(new Scalar(key));
            serializer(value, value.GetType());
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
            if (!condition) return;

            emitter.Emit(new Scalar(key));
            serializer(value, typeof(T));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="property"></param>
    /// <param name="instance"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static bool ShouldSerialize(PropertyInfo property, object? instance, out object value)
    {
        value = property.GetValue(instance);
        var attribute = property.GetCustomAttribute<DefaultValueAttribute>();

        if (attribute is not null)
        {
            return !Equals(attribute.Value, value);
        }

        var method = property.DeclaringType?.GetMethod(
            $"ShouldSerialize{property.Name}",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase,
            null,
            Type.EmptyTypes,
            null
        );

        if (instance is not null && method is not null && method.ReturnType == typeof(bool))
        {
            return (bool)method.Invoke(instance, null)!;
        }

        return true;
    }
}