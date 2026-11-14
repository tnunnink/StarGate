using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Converts <see cref="GraphicBrush"/> instances to and from YAML.
/// </summary>
/// <remarks>
/// Supports scalar color values as shorthand for solid brushes and mapping values for full brush definitions.
/// During serialization, an opaque <see cref="SolidBrush"/> is emitted as a scalar color value; other brush instances
/// are serialized using their concrete runtime type.
/// </remarks>
public class SolidBrushConverter : IYamlTypeConverter
{
    /// <inheritdoc />
    public bool Accepts(Type type) => type == typeof(SolidBrush);

    /// <inheritdoc />
    public object ReadYaml(IParser parser, Type type, ObjectDeserializer deserializer)
    {
        if (parser.TryConsume<Scalar>(out var scalar))
            return new SolidBrush { Color = GraphicColor.Parse(scalar.Value) };

        var brush = new SolidBrush();

        parser.ConsumeMap(deserializer, (key, reader) =>
        {
            switch (key.Value.ToLowerInvariant())
            {
                case "color": brush.Color = reader.Read<GraphicColor>(); break;
                case "transparency": brush.Transparency = reader.Read<double>(); break;
                default: reader.Skip(); break;
            }
        });

        return brush;
    }

    /// <inheritdoc />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not SolidBrush brush) return;

        if (brush.Transparency <= 0)
        {
            emitter.EmitValue(brush.Color.ToString(), ScalarStyle.DoubleQuoted);
            return;
        }

        emitter.EmitMap(e =>
        {
            e.EmitProperty("kind", brush.Kind, serializer);
            e.EmitProperty("color", brush.Color, serializer);
            e.EmitProperty("transparency", brush.Transparency, serializer);
        });
    }
}