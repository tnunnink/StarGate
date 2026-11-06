using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace StarGate.Core;

public class GraphicBrushConverter : IYamlTypeConverter
{
    /// <inheritdoc />
    public bool Accepts(Type type) => typeof(GraphicBrush).IsAssignableFrom(type);

    /// <inheritdoc />
    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        throw new NotImplementedException();
    }
}