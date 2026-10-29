using System;
using System.Collections;
using StarGate.Core.Objects;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core.Serialization.Converters;

public class AttributeConfigConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(AttributeConfig);

    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        // Shorthand: scalar value directly
        if (parser.TryConsume<Scalar>(out var scalar))
        {
            return new AttributeConfig { Value = scalar.Value };
        }

        // Full mapping representation
        parser.Consume<MappingStart>();
        object? value = null;
        bool? locked = null;
        Security? security = null;

        while (!parser.TryConsume<MappingEnd>(out _))
        {
            var key = parser.Consume<Scalar>().Value;
            switch (key.ToLowerInvariant())
            {
                case "value":
                    value = rootDeserializer(typeof(object));
                    break;
                case "locked":
                    locked = (bool?)rootDeserializer(typeof(bool?));
                    break;
                case "security":
                    security = (Security?)rootDeserializer(typeof(Security?));
                    break;
                default:
                    parser.SkipThisAndNestedEvents();
                    break;
            }
        }

        return new AttributeConfig
        {
            Value = value,
            Locked = locked,
            Security = security
        };
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not AttributeConfig config)
        {
            emitter.Emit(new Scalar("null"));
            return;
        }

        // Shorthand: emit only the underlying value if Locked and Security are default/null/omitted
        if (config.Locked is null or false && config.Security is null)
        {
            WriteValue(emitter, config.Value, serializer);
            return;
        }

        // Otherwise emit the full mapping (using flow style mapping: "{value: 123, locked: false ... }")
        emitter.Emit(new MappingStart(null, null, false, MappingStyle.Flow));

        emitter.Emit(new Scalar("Value"));
        WriteValue(emitter, config.Value, serializer);

        if (config.Locked is true)
        {
            emitter.Emit(new Scalar("Locked"));
            serializer(config.Locked, typeof(bool));
        }

        if (config.Security.HasValue)
        {
            emitter.Emit(new Scalar("Security"));
            serializer(config.Security, typeof(Security));
        }

        emitter.Emit(new MappingEnd());
    }

    /// <summary>
    /// Serializes the given value into YAML format using the specified emitter and serializer.
    /// Handles enumerable objects separately by emitting a flow-style sequence,
    /// and for non-enumerable objects, emits the serialized value directly.
    /// </summary>
    /// <param name="emitter">The YAML emitter used to write the serialized output.</param>
    /// <param name="value">The object to be serialized. Can be a primitive, an enumerable, or any other type.</param>
    /// <param name="serializer">The serializer delegate for writing individual objects or values.</param>
    private static void WriteValue(IEmitter emitter, object? value, ObjectSerializer serializer)
    {
        if (value is IEnumerable enumerable and not string)
        {
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Flow));

            foreach (var item in enumerable)
                serializer(item, item.GetType());

            emitter.Emit(new SequenceEnd());
            return;
        }

        serializer(value, value?.GetType() ?? typeof(object));
    }
}