using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class PieElementSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultsElementShouldHaveExpectedValues()
    {
        var element = new PieElement { Name = "PieDefault" };

        element.Kind.Should().Be(ElementKind.Pie);
        element.Name.Should().Be("PieDefault");
        element.StartAngle.Should().Be(0.0);
        element.SweepAngle.Should().Be(0.0);
    }

    [TestMethod]
    public void ElementShouldSerializeAndDeserializeCorrectly()
    {
        var serializer = new GalaxySerializer();
        var element = new PieElement
        {
            Name = "SerializedPie",
            Bounds = new GraphicBounds(0, 0, 100, 100),
            StartAngle = 0.0,
            SweepAngle = 90.0,
            Fill = new FillStyle { Brush = "#FF0000" }
        };

        var yaml = serializer.Serialize(element);
        var deserialized = serializer.Deserialize<PieElement>(yaml);

        deserialized.Should().NotBeNull();
        deserialized.Name.Should().Be("SerializedPie");
        deserialized.Bounds.Should().Be(new GraphicBounds(0, 0, 100, 100));
        deserialized.StartAngle.Should().Be(0.0);
        deserialized.SweepAngle.Should().Be(90.0);
        deserialized.Kind.Should().Be(ElementKind.Pie);
    }

    [TestMethod]
    public Task SerializeDefaultNamedElementShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var element = new PieElement { Name = "SerializedPie" };

        var yaml = serializer.Serialize(element);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializePopulatedPieShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var element = new PieElement
        {
            Name = "SerializedPie",
            Bounds = new GraphicBounds(10, 10, 80, 80),
            StartAngle = 45.0,
            SweepAngle = 135.0,
            Fill = new FillStyle { Brush = "#00FF00" },
            Stroke = new StrokeStyle { Width = 1, Brush = "#000000" }
        };

        var yaml = serializer.Serialize(element);

        return Verify(yaml);
    }
}
