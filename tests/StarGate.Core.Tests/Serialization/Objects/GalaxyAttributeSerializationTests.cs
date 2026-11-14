using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class GalaxyAttributeSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultsShouldHaveExpectedValues()
    {
        var attribute = new GalaxyAttribute { Name = "Attr1" };

        attribute.Name.Should().Be("Attr1");
        attribute.DataType.Should().BeNull();
        attribute.Length.Should().BeNull();
        attribute.Writeability.Should().BeNull();
        attribute.Value.Should().BeNull();
        attribute.Description.Should().BeNull();
        attribute.Units.Should().BeNull();
        attribute.IO.Should().BeNull();
        attribute.History.Should().BeNull();
        attribute.Alarms.Should().BeNull();
    }

    [TestMethod]
    public Task SerializeBasicAttributeShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var attribute = new GalaxyAttribute
        {
            Name = "Pressure",
            DataType = AttributeDataType.Float,
            Value = new AttributeConfig { Value = 101.3 },
            Description = new AttributeConfig { Value = "Line pressure in PSI" },
            Units = new AttributeConfig { Value = "PSI" }
        };

        var yaml = serializer.Serialize(attribute);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializePopulatedAttributeWithExtensionsShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var attribute = new GalaxyAttribute
        {
            Name = "Temperature",
            DataType = AttributeDataType.Float,
            Value = new AttributeConfig { Value = 72.5 },
            Description = new AttributeConfig { Value = "Ambient Temperature" },
            Units = new AttributeConfig { Value = "DegF" },
            IO = new IOExtension
            {
                Binding = BindingType.ReadOnly,
                InputSource = new AttributeConfig { Value = "PLC.TempSensor1" }
            },
            History = new HistoryExtension
            {
                Deadband = new AttributeConfig { Value = 0.5 },
                StoragePeriod = new AttributeConfig { Value = 60000 }
            }
        };

        var yaml = serializer.Serialize(attribute);

        return Verify(yaml);
    }
}
