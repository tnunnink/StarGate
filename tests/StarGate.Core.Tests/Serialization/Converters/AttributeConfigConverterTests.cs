using FluentAssertions;

namespace StarGate.Core.Tests.Serialization.Converters;

[TestClass]
public class AttributeConfigConverterTests
{
    private readonly GalaxySerializer _serializer = new();

    [TestMethod]
    public void Accepts_AttributeConfigType_ReturnsTrue()
    {
        var converter = new AttributeConfigConverter();
        converter.Accepts(typeof(AttributeConfig)).Should().BeTrue();
    }

    [TestMethod]
    public void Accepts_OtherType_ReturnsFalse()
    {
        var converter = new AttributeConfigConverter();
        converter.Accepts(typeof(string)).Should().BeFalse();
        converter.Accepts(typeof(int)).Should().BeFalse();
    }

    [TestMethod]
    public void Serialize_DefaultConfig_EmitsScalar()
    {
        var config = new AttributeConfig { Value = 123 };
        var yaml = _serializer.Serialize(config);

        yaml.Trim().Should().Be("123");
    }

    [TestMethod]
    public void Serialize_ConfigWithSecurityAndLocked_EmitsFlowMapping()
    {
        var config = new AttributeConfig
        {
            Value = 123,
            Locked = true,
            Security = Security.Tune
        };

        var yaml = _serializer.Serialize(config);

        yaml.Trim().Should().Be("{value: 123, locked: true, security: Tune}");
    }

    [TestMethod]
    public void Deserialize_ScalarValue_ReturnsAttributeConfig()
    {
        var config = _serializer.Deserialize<AttributeConfig>("123");

        config.Value.Should().Be("123");
        config.Locked.Should().BeFalse();
        config.Security.Should().Be(Security.Default);
    }

    [TestMethod]
    public void Deserialize_FlowMapping_ReturnsAttributeConfig()
    {
        const string yaml = "{value: 123, locked: true, security: Tune}";
        var config = _serializer.Deserialize<AttributeConfig>(yaml);

        config.Value.Should().Be("123");
        config.Locked.Should().BeTrue();
        config.Security.Should().Be(Security.Tune);
    }
}