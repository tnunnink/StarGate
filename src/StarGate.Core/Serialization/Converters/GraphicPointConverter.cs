using System;
using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core;

public class GraphicPointConverter : IYamlTypeConverter
{
    /// <inheritdoc />
    public bool Accepts(Type type) => type == typeof(GraphicPoint);

    /// <inheritdoc />
    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        var start = parser.Consume<SequenceStart>();
        var x = ReadNumber(parser, "x");
        var y = ReadNumber(parser, "y");

        if (!parser.TryConsume<SequenceEnd>(out _))
        {
            throw new YamlException(start.Start, parser.Current?.End ?? start.End,
                "GraphicPoint sequence must contain exactly 2 values: [x, y]");
        }

        return new GraphicPoint(x, y);

        static double ReadNumber(IParser p, string c)
        {
            var scalar = p.Consume<Scalar>();

            if (!double.TryParse(scalar.Value, out var value))
                throw new YamlException(scalar.Start, scalar.End,
                    $"Invalid numeric value '{scalar.Value}' for bounds component '{c}'.");

            return value;
        }
    }

    /// <inheritdoc />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not GraphicPoint p) return;

        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));
        emitter.Emit(new Scalar(p.X.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(p.Y.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new SequenceEnd());
    }
}