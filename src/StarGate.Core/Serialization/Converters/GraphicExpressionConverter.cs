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
        if (parser.TryConsume<MappingStart>(out _))
        {
            var key = parser.Consume<Scalar>();
            var value = parser.Consume<Scalar>();
            parser.Consume<MappingEnd>();

            if (string.Equals(key.Value, "ref", StringComparison.OrdinalIgnoreCase))
                return new ReferenceExpression(value.Value);

            throw new YamlException(
                key.Start,
                key.End,
                $"Invalid expression mapping key '{key.Value}'. Expected 'ref'."
            );
        }

        throw new YamlException(
            parser.Current?.Start ?? Mark.Empty,
            parser.Current?.End ?? Mark.Empty,
            $"Expected a scalar string or mapping {{ ref: ... }} for {nameof(GraphicExpression)}."
        );
    }

    /// <inheritdoc />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not GraphicExpression expression)
            return;

        // Flow mapping: { ref: "Me.StartDate" }
        if (value is ReferenceExpression reference)
        {
            emitter.Emit(new MappingStart(null, null, true, MappingStyle.Flow));
            emitter.Emit(new Scalar("ref"));
            emitter.Emit(new Scalar(reference.Text));
            emitter.Emit(new MappingEnd());
            return;
        }

        // Plain scalar string: "01/01/2025"
        emitter.Emit(new Scalar(expression.Text));
    }
}