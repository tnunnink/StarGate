using System;
using System.Collections;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core;

public class AttributeConfigConverter : IYamlTypeConverter
{
    public bool Accepts(Type type)
    {
        return type == typeof(AttributeConfig);
    }

    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        // Shorthand: scalar value directly
        if (parser.TryConsume<Scalar>(out var scalar))
            return new AttributeConfig { Value = scalar.Value };

        var config = new AttributeConfig();

        // Full mapping representation using our helper
        parser.ConsumeMap(rootDeserializer, (key, reader) =>
        {
            switch (key.Value.ToLowerInvariant())
            {
                case "value": config.Value = reader.Read<object>(); break;
                case "locked": config.Locked = reader.Read<bool>(); break;
                case "security": config.Security = reader.Read<Security>(); break;
                default: reader.Skip(); break;
            }
        });

        return config;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not AttributeConfig config)
        {
            emitter.Emit(new Scalar("null"));
            return;
        }

        // Shorthand: emit only the underlying value if Locked and Security are default/null/omitted
        if (config is { Locked: false, Security: Security.Default })
        {
            WriteValue(emitter, config.Value, serializer);
            return;
        }

        // Otherwise emit the full mapping (using flow style mapping: "{value: 123, locked: false ... }")
        emitter.EmitMap(e =>
        {
            e.Emit("value");
            WriteValue(e, config.Value, serializer);

            e.EmitIf(config.Locked, "locked", config.Locked, serializer);
            e.EmitIf(config.Security != Security.Default, "security", config.Security, serializer);
        }, MappingStyle.Flow);
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