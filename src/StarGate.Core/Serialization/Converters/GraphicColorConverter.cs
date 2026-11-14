using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Converts <see cref="GraphicColor" /> values to and from YAML scalar values.
/// </summary>
/// <remarks>
/// Accepts only <see cref="GraphicColor" /> values. Deserialization reads a YAML scalar and parses it as a graphic color; invalid scalar content results in a <see cref="YamlDotNet.Core.YamlException" />. Serialization writes the color string representation as a double-quoted YAML scalar.
/// </remarks>
public class GraphicColorConverter : IYamlTypeConverter
{
    /// <inheritdoc />
    public bool Accepts(Type type) => type == typeof(GraphicColor);

    /// <inheritdoc />
    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        var scalar = parser.Consume<Scalar>();

        if (!GraphicColor.TryParse(scalar.Value, out var color))
            throw new YamlException(scalar.Start, scalar.End,
                $"Invalid color '{scalar.Value}'. Expected hex format (e.g., '#RRGGBB', '#RRGGBBAA') or space-delimited RGB ('R G B').");

        return color;
    }

    /// <inheritdoc />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not GraphicColor color)
            return;

        emitter.EmitValue(color.ToString(), ScalarStyle.DoubleQuoted);
    }
}