using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class SolidBrushTests : VerifyBase
{
    [TestMethod]
    public void DefaultShouldHaveExpectedValues()
    {
        var brush = new SolidBrush();

        brush.Color.Should().Be(default(GraphicColor));
        brush.Transparency.Should().Be(0.0);
    }

    [TestMethod]
    public Task SerializedWithColorShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var brush = new SolidBrush { Color = "FFFFFF" };

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }
    
    [TestMethod]
    public Task SerializedWithAllShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var brush = new SolidBrush { Color = "FFFFFF", Transparency = 12.0 };

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }
}