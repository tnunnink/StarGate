using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class ArcElementSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultsElementShouldHaveExpectedValues()
    {
        var element = new ArcElement { Name = "ArcDefault" };

        element.Kind.Should().Be(ElementKind.Arc);
        element.Name.Should().Be("ArcDefault");
        element.StartAngle.Should().Be(0.0);
        element.SweepAngle.Should().Be(0.0);
    }

    [TestMethod]
    public void ElementShouldSerializeAndDeserializeCorrectly()
    {
        var serializer = new GalaxySerializer();
        var element = new ArcElement
        {
            Name = "SerializedArc",
            Bounds = new GraphicBounds(0, 0, 100, 100),
            StartAngle = 45.0,
            SweepAngle = 180.0,
            Stroke = new StrokeStyle { Width = 2, Brush = "#0000FF" }
        };

        var yaml = serializer.Serialize(element);
        var deserialized = serializer.Deserialize<ArcElement>(yaml);

        deserialized.Should().NotBeNull();
        deserialized.Name.Should().Be("SerializedArc");
        deserialized.Bounds.Should().Be(new GraphicBounds(0, 0, 100, 100));
        deserialized.StartAngle.Should().Be(45.0);
        deserialized.SweepAngle.Should().Be(180.0);
        deserialized.Kind.Should().Be(ElementKind.Arc);
    }

    [TestMethod]
    public Task SerializeDefaultNamedElementShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var element = new ArcElement { Name = "SerializedArc" };

        var yaml = serializer.Serialize(element);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializePopulatedArcShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var element = new ArcElement
        {
            Name = "SerializedArc",
            Bounds = new GraphicBounds(10, 20, 150, 100),
            StartAngle = 30.0,
            SweepAngle = 270.0,
            Stroke = new StrokeStyle { Width = 2, Brush = "#FF8000" }
        };

        var yaml = serializer.Serialize(element);

        return Verify(yaml);
    }
}
