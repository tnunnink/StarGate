using FluentAssertions;

namespace StarGate.Core.Tests.Serialization.Converters;

[TestClass]
public class ValueBindingConverterTests
{
    private readonly GalaxySerializer _serializer = new();

    [TestMethod]
    public void Accepts_ValueBindingType_ReturnsTrue()
    {
        var converter = new ValueBindingConverter();
        converter.Accepts(typeof(ValueBinding)).Should().BeTrue();
    }

    [TestMethod]
    public void Accepts_OtherType_ReturnsFalse()
    {
        var converter = new ValueBindingConverter();
        converter.Accepts(typeof(string)).Should().BeFalse();
        converter.Accepts(typeof(ItemBinding)).Should().BeFalse();
    }

    [TestMethod]
    public void Serialize_WithoutTrigger_EmitsScalar()
    {
        var binding = new ValueBinding { Reference = "Pump1.AutoMode" };
        var yaml = _serializer.Serialize(binding);

        yaml.Trim().Should().Be("Pump1.AutoMode");
    }

    [TestMethod]
    public void Serialize_WithTrigger_EmitsMapping()
    {
        var binding = new ValueBinding { Reference = "Pump1.AutoMode", Trigger = "OnDemand" };
        var yaml = _serializer.Serialize(binding);

        yaml.Should().Contain("ref: Pump1.AutoMode");
        yaml.Should().Contain("trigger: OnDemand");
    }

    [TestMethod]
    public void Deserialize_ScalarString_ReturnsValueBinding()
    {
        var binding = _serializer.Deserialize<ValueBinding>("Pump1.AutoMode");

        binding.Reference.Should().Be("Pump1.AutoMode");
        binding.Trigger.Should().BeNull();
    }

    [TestMethod]
    public void Deserialize_FlowMapping_ReturnsValueBinding()
    {
        var binding = _serializer.Deserialize<ValueBinding>("{ ref: \"Pump1.AutoMode\", trigger: \"OnDemand\" }");

        binding.Reference.Should().Be("Pump1.AutoMode");
        binding.Trigger.Should().Be("OnDemand");
    }
}
