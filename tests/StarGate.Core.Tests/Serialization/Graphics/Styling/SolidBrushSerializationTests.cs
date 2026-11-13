using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class SolidBrushSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultShouldHaveExpectedValues()
    {
        var brush = new SolidBrush();

        brush.Kind.Should().Be(BrushKind.Solid);
        brush.Color.Should().Be(default(GraphicColor));
        brush.Transparency.Should().Be(0.0);
    }

    [TestMethod]
    public Task SerializedDefaultShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var brush = new SolidBrush();

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializedWithColorShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var brush = new SolidBrush { Color = "#35A6E2" };

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializedWithAllShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var brush = new SolidBrush { Color = "#FFFFFF", Transparency = 12.0 };

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }
}