using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core;

public class GraphicBoundsConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(GraphicBounds);

    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        // Custom flow mapping reader or rely on default deserialization
        throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not GraphicBounds b) return;

        emitter.Emit(new MappingStart(AnchorName.Empty, TagName.Empty, false, MappingStyle.Flow));

        emitter.Emit(new Scalar("left"));
        emitter.Emit(new Scalar(b.Left.ToString()));
        emitter.Emit(new Scalar("top"));
        emitter.Emit(new Scalar(b.Top.ToString()));
        emitter.Emit(new Scalar("width"));
        emitter.Emit(new Scalar(b.Width.ToString()));
        emitter.Emit(new Scalar("height"));
        emitter.Emit(new Scalar(b.Height.ToString()));

        emitter.Emit(new MappingEnd());
    }
}
