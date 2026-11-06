using System;
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
        var left = ReadCoordinate(parser, "left");
        var top = ReadCoordinate(parser, "top");
        var width = ReadCoordinate(parser, "width");
        var height = ReadCoordinate(parser, "height");

        // 2. Consume<SequenceEnd>() ensures there were no extra elements (e.g., 5 numbers)
        if (!parser.TryConsume<SequenceEnd>(out _))
        {
            throw new YamlException(start.Start, parser.Current?.End ?? start.End,
                "GraphicBounds sequence must contain exactly 4 values: [left, top, width, height].");
        }

        return new GraphicBounds(left, top, width, height);

        static double ReadCoordinate(IParser p, string c)
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
        if (value is not GraphicBounds b) return;

        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));
        emitter.Emit(new Scalar(b.Left.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(b.Top.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(b.Width.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(b.Height.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new SequenceEnd());
    }
}