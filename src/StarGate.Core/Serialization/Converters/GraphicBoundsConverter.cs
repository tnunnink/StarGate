using System;
using System.Collections.Generic;
using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Provides YAML conversion support for <see cref="GraphicBounds"/> values.
/// </summary>
/// <remarks>
/// Serializes graphic bounds as a flow-style YAML sequence containing the left, top, width, and height values in that order.
/// Numeric values are written using invariant culture formatting.
/// </remarks>
public class GraphicBoundsConverter : IYamlTypeConverter
{
    /// <inheritdoc />
    public bool Accepts(Type type) => type == typeof(GraphicBounds);

    /// <inheritdoc />
    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        var start = parser.Consume<SequenceStart>();

        var left = rootDeserializer.Deserialize<double>();
        var top = rootDeserializer.Deserialize<double>();
        var width = rootDeserializer.Deserialize<double>();
        var height = rootDeserializer.Deserialize<double>();

        if (!parser.TryConsume<SequenceEnd>(out _))
            throw new YamlException(start.Start, parser.Current?.End ?? start.End,
                "GraphicBounds sequence must contain exactly 4 values: [left, top, width, height].");

        return new GraphicBounds(left, top, width, height);
    }

    /// <inheritdoc />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not GraphicBounds b) return;

        // outputs: [left, top, width, height]
        emitter.EmitSequence(e =>
        {
            e.Emit(b.Left, serializer);
            e.Emit(b.Top, serializer);
            e.Emit(b.Width, serializer);
            e.Emit(b.Height, serializer);
        });
    }
}