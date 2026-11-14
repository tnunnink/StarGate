using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core;

public class FillStyleConverter : IYamlTypeConverter
{
    /// <inheritdoc />
    public bool Accepts(Type type) => type == typeof(FillStyle);

    /// <inheritdoc />
    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        //Shorthand is solid color brush value
        if (parser.TryConsume<Scalar>(out var color))
            return new FillStyle { Brush = color.Value };

        var fill = new FillStyle();

        parser.ConsumeMap(rootDeserializer, (key, reader) =>
        {
            switch (key.Value.ToLowerInvariant())
            {
                case "brush": fill.Brush = reader.Read<GraphicBrush>() ?? GraphicColor.White; break;
                case "unfilled": fill.Unfilled = reader.Read<GraphicBrush>() ?? GraphicColor.Black; break;
                case "orientation": fill.Orientation = reader.Read<FillOrientation>(); break;
                case "direction": fill.Direction = reader.Read<FillDirection>(); break;
                case "vertical": fill.Vertical = reader.Read<int>(); break;
                case "horizontal": fill.Horizontal = reader.Read<int>(); break;
                default: reader.Skip(); break;
            }
        });

        return fill;
    }

    /// <inheritdoc />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not FillStyle fill) return;

        if (fill.ShouldCollapse())
        {
            serializer(fill.Brush, fill.Brush.GetType());
            return;
        }

        emitter.EmitMap(e =>
        {
            e.EmitProperty(fill, f => f.Brush, serializer);
            e.EmitProperty(fill, f => f.Unfilled, serializer);
            e.EmitProperty(fill, f => f.Orientation, serializer);
            e.EmitProperty(fill, f => f.Direction, serializer);
            e.EmitProperty(fill, f => f.Vertical, serializer);
            e.EmitProperty(fill, f => f.Horizontal, serializer);
        });
    }
}