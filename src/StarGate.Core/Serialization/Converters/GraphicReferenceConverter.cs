using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Provides custom YAML serialization and deserialization for <see cref="GraphicReference" />,
/// emitting literal strings as plain scalars and dynamic references as flow mappings: { ref: "..." }.
/// </summary>
public class GraphicReferenceConverter : IYamlTypeConverter
{
    public bool Accepts(Type type)
    {
        return type == typeof(GraphicReference);
    }

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        // 1. Literal scalar string (e.g., minDate: "01/01/2025")
        if (parser.TryConsume<Scalar>(out var scalar)) return new GraphicReference { Text = scalar.Value };

        if (!parser.Accept<MappingStart>(out _))
            return null;

        // 2. Mapping (e.g., minDate: { ref: "Me.StartDate" })
        var mapping = (Dictionary<string, string>)rootDeserializer(typeof(Dictionary<string, string>))!;

        if (mapping.TryGetValue("ref", out var reference))
            return new GraphicReference { Expression = reference };

        if (mapping.TryGetValue("text", out var text))
            return new GraphicReference { Text = text };

        return null;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not GraphicReference reference)
            return;

        if (reference.Expression is not null)
        {
            // Flow mapping: { ref: "Me.StartDate" }
            emitter.Emit(new MappingStart(null, null, true, MappingStyle.Flow));
            emitter.Emit(new Scalar("ref"));
            emitter.Emit(new Scalar(reference.Expression));
            emitter.Emit(new MappingEnd());
        }
        else if (reference.Text is not null)
        {
            // Plain scalar string: "01/01/2025"
            emitter.Emit(new Scalar(reference.Text));
        }
    }
}