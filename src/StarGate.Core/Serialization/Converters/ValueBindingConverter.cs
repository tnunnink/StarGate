using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Provides custom YAML serialization and deserialization for <see cref="ValueBinding" />,
/// emitting immediate tag bindings as plain scalar strings and deferred trigger bindings
/// as compact flow mappings: { reference: "...", trigger: "..." }.
/// </summary>
public class ValueBindingConverter : IYamlTypeConverter
{
    /// <inheritdoc />
    public bool Accepts(Type type) => type == typeof(ValueBinding);

    /// <inheritdoc />
    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        // 1. Shorthand: scalar reference string directly (e.g., binding: "Pump1.AutoMode")
        if (parser.TryConsume<Scalar>(out var scalar))
            return new ValueBinding { Reference = scalar.Value };

        var binding = new ValueBinding();

        parser.ConsumeMap(rootDeserializer, (key, reader) =>
        {
            switch (key.Value.ToLowerInvariant())
            {
                case "ref": binding.Reference = reader.Read(); break;
                case "trigger": binding.Trigger = reader.Read(); break;
                default: reader.Skip(); break;
            }
        });

        return binding;
    }

    /// <inheritdoc />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not ValueBinding binding)
            return;

        // If no trigger is specified, emit the clean scalar shorthand: "Pump1.AutoMode"
        if (binding.Trigger is null || string.IsNullOrEmpty(binding.Trigger))
        {
            emitter.EmitValue(binding.Reference);
            return;
        }

        // When a trigger is present, emit flow mapping: { ref: "...", trigger: "..." }
        emitter.EmitMap(e =>
        {
            e.EmitProperty("ref", binding.Reference);
            e.EmitProperty("trigger", binding.Trigger);
        });
    }
}