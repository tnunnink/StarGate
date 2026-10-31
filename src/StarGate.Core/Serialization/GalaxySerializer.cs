using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace StarGate.Core;

/// <summary>
/// Provides functionality for serializing and deserializing objects to and from YAML format.
/// </summary>
public class GalaxySerializer : IGalaxySerializer
{
    /// <summary>
    /// The YAML deserializer instance used for converting YAML-formatted data into .NET objects,
    /// configured with specific options for naming conventions and property handling.
    /// </summary>
    private readonly IDeserializer _deserializer;

    /// <summary>
    /// The YAML serializer instance used for serializing and deserializing objects
    /// based on the specified <see cref="SerializationOptions" /> configuration.
    /// </summary>
    private readonly ISerializer _serializer;

    /// <summary>
    /// Provides functionality for serializing and deserializing objects to and from YAML format.
    /// </summary>
    public GalaxySerializer(SerializationOptions? options = null)
    {
        options ??= new SerializationOptions();

        _serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithIndentedSequences()
            .WithTypeInspector(inner => new InheritanceTypeInspector(inner))
            .ConfigureDefaultValuesHandling(
                DefaultValuesHandling.OmitNull
                | DefaultValuesHandling.OmitDefaults
                | DefaultValuesHandling.OmitEmptyCollections
            )
            .WithCoreConverters()
            .Build();

        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .WithCoreConverters()
            .Build();
    }

    /// <inheritdoc />
    public string Serialize(object data)
    {
        return _serializer.Serialize(data);
    }

    /// <inheritdoc />
    public T Deserialize<T>(string data)
    {
        return _deserializer.Deserialize<T>(data);
    }
}

/// <summary>
/// Provides extension methods for registering YAML type converters in a builder,
/// facilitating custom serialization and deserialization for specific types.
/// </summary>
public static class YamlTypeConverterRegistration
{
    /// <summary>
    /// Registers core YAML type converters for the builder, enabling support for custom serialization and deserialization
    /// of types like AttributeConfig, ItemBinding, and GraphicReference.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder being extended.</typeparam>
    /// <param name="builder">The builder instance to be configured with core type converters.</param>
    /// <returns>The builder instance with the core type converters added.</returns>
    public static TBuilder WithCoreConverters<TBuilder>(this TBuilder builder)
        where TBuilder : BuilderSkeleton<TBuilder>
    {
        return builder
            .WithTypeConverter(new AttributeConfigConverter())
            .WithTypeConverter(new ValueBindingConverter())
            .WithTypeConverter(new ItemBindingConverter())
            .WithTypeConverter(new GraphicReferenceConverter());
    }
}