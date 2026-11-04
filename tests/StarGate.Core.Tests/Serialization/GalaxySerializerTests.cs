namespace StarGate.Core;

[TestClass]
public class GalaxySerializerTests
{
    [TestMethod]
    public Task Serialize_BasicConfig_ShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var data = new GalaxyObject
        {
            TagName = "Test",
            DerivedFrom = "$UserDefined",
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