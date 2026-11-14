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

        var x = rootDeserializer.Deserialize<double>();
        var y = rootDeserializer.Deserialize<double>();

        if (!parser.TryConsume<SequenceEnd>(out _))
            throw new YamlException(start.Start, parser.Current?.End ?? start.End,
                "GraphicPoint sequence must contain exactly 2 values: [x, y].");

        return new GraphicPoint(x, y);
    }

    /// <inheritdoc />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not GraphicPoint p) 
            return;
        
        emitter.EmitSequence([p.X, p.Y], serializer);
    }
}