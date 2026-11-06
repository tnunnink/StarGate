using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class SolidBrushTests
{
    [TestMethod]
    public void DefaultShouldHaveExpectedValues()
    {
        var brush = new SolidBrush();

        brush.Color.Should().Be(default(GraphicColor));
        brush.Transparency.Should().Be(0.0);
    }

    [TestMethod]
    public void SerializedShouldHaveVerifiedOutput()
    {
        var brush = new SolidBrush() { Color = "" };
    }
}