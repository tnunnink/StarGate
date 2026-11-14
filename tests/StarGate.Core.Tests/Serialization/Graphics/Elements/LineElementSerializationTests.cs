using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class LineElementSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultsElementShouldHaveExpectedValues()
    {
        var element = new LineElement { Name = "LineDefault" };

        element.Kind.Should().Be(ElementKind.Line);
        element.Name.Should().Be("LineDefault");
        element.Start.Should().Be(default(GraphicPoint));
        element.End.Should().Be(default(GraphicPoint));
        element.Snap.Should().BeNull();
    }

    [TestMethod]
    public void ElementShouldSerializeAndDeserializeCorrectly()
    {
        var serializer = new GalaxySerializer();
        var element = new LineElement
        {
            Name = "SerializedLine",
            Start = new GraphicPoint(10, 20),
            End = new GraphicPoint(100, 200),
            Snap = true,
            Stroke = new StrokeStyle { Width = 2, Brush = "#FF0000" }
        };

        var yaml = serializer.Serialize(element);
        var deserialized = serializer.Deserialize<LineElement>(yaml);

        deserialized.Should().NotBeNull();
        deserialized.Name.Should().Be("SerializedLine");
        deserialized.Start.Should().Be(new GraphicPoint(10, 20));
        deserialized.End.Should().Be(new GraphicPoint(100, 200));
        deserialized.Snap.Should().BeTrue();
        deserialized.Kind.Should().Be(ElementKind.Line);
    }

    [TestMethod]
    public Task SerializeDefaultNamedElementShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var element = new LineElement { Name = "SerializedLine" };

        var yaml = serializer.Serialize(element);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializePopulatedLineShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var element = new LineElement
        {
            Name = "SerializedLine",
            Start = new GraphicPoint(0, 10),
            End = new GraphicPoint(50, 100),
            Snap = true,
            Stroke = new StrokeStyle { Width = 3, Brush = "#00FF00" }
        };

        var yaml = serializer.Serialize(element);

        return Verify(yaml);
    }
}
