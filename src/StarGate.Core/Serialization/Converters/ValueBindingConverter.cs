using System;
using System.Collections.Generic;
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
    public bool Accepts(Type type)
    {
        return type == typeof(ValueBinding);
    }

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        // 1. Shorthand: scalar reference string directly (e.g., binding: "Pump1.AutoMode")
        if (parser.TryConsume<Scalar>(out var scalar))
            return new ValueBinding { Reference = scalar.Value };

        if (!parser.Accept<MappingStart>(out _))
            return null;

        // 2. Full mapping (e.g., binding: { ref: "Pump1.AutoMode", trigger: "Batch.Confirm" })
        var dict = (Dictionary<string, string>)rootDeserializer(typeof(Dictionary<string, string>))!;

        if (!dict.TryGetValue("ref", out var reference))
            return null;

        dict.TryGetValue("trigger", out var trigger);

        return new ValueBinding
        {
            Reference = reference,
            Trigger = trigger
        };
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not ValueBinding binding)
            return;

        // If no trigger is specified, emit the clean scalar shorthand: "Pump1.AutoMode"
        if (binding.Trigger is null || string.IsNullOrEmpty(binding.Trigger))
        {
            emitter.Emit(new Scalar(binding.Reference));
            return;
        }

        // When a trigger is present, emit flow mapping: { ref: "...", trigger: "..." }
        emitter.Emit(new MappingStart(null, null, true, MappingStyle.Flow));

        emitter.Emit(new Scalar("ref"));
        emitter.Emit(new Scalar(binding.Reference));

        emitter.Emit(new Scalar("trigger"));
        emitter.Emit(new Scalar(binding.Trigger));

        emitter.Emit(new MappingEnd());
    }
}