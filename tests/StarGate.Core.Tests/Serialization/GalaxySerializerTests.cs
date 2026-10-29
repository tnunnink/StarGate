using StarGate.Core.Objects;
using StarGate.Core.Serialization;

namespace StarGate.Core.Tests.Serialization;

[TestFixture]
public class GalaxySerializerTests
{
    [Test]
    public Task Serialize_BasicConfig_ShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var data = new ObjectDefinition
        {
            TagName = "Test",
            Template = "$UserDefined",
            Attributes =
            [
                new ObjectAttribute
                {
                    Name = "MyAttribute",
                    Value = new AttributeConfig { Value = 123 }
                }
            ]
        };

        var yaml = serializer.Serialize(data);

        return Verify(yaml);
    }
}