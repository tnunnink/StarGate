using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Provides custom YAML serialization and deserialization for <see cref="GraphicExpression" />,
/// emitting literal strings as plain scalars and dynamic references as flow mappings: { ref: "..." }.
/// </summary>
public class GraphicExpressionConverter : IYamlTypeConverter
{
    /// <inheritdoc />
    public bool Accepts(Type type) => typeof(GraphicExpression).IsAssignableFrom(type);

    /// <inheritdoc />
    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        // 1. Literal scalar string (e.g., minDate: "01/01/2025")
        if (parser.TryConsume<Scalar>(out var scalar))
            return new StaticExpression(scalar.Value);

        // 2. Mapping: { ref: "Me.StartDate" }
        string? reference = null;

        parser.ConsumeMap(rootDeserializer, (key, reader) =>
        {
            switch (key.Value.ToLowerInvariant())
            {
                case "ref": reference = reader.Read(); break;
                default: reader.Skip(); break;
            }
        });

        if (reference is null || string.IsNullOrWhiteSpace(reference))
            throw new YamlException(
                "Expected a scalar string or mapping with 'ref' property for GraphicExpression.");

        return new ReferenceExpression(reference);
    }

    /// <inheritdoc />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not GraphicExpression expression) return;

        // Flow mapping: { ref: "Me.StartDate" }
        if (value is ReferenceExpression reference)
        {
            emitter.EmitMap(e => { e.Emit("ref", reference.Text); }, MappingStyle.Flow);
            return;
        }

        // Plain scalar string: "01/01/2025"
        emitter.Emit(expression.Text, ScalarStyle.DoubleQuoted);
    }
}