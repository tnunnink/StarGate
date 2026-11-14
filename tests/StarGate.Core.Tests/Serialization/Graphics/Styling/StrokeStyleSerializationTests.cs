using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class StrokeStyleSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultsShouldHaveExpectedValues()
    {
        var stroke = new StrokeStyle();

        stroke.Width.Should().Be(1);
        stroke.Pattern.Should().Be(StrokePattern.Solid);
        stroke.StartCap.Should().Be(StrokeCap.Flat);
        stroke.EndCap.Should().Be(StrokeCap.Flat);
    }

    [TestMethod]
    public Task SerializeDefaultShouldHaveVerifiedOutput()
    {
        var serializer = new GalaxySerializer();
        var stroke = new StrokeStyle();

        var yaml = serializer.Serialize(stroke);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializeWithBrushShouldHaveVerifiedOutput()
    {
        var serializer = new GalaxySerializer();
        var stroke = new StrokeStyle
        {
            Brush = "#FF8000"
        };

        var yaml = serializer.Serialize(stroke);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializeCustomStrokeShouldHaveVerifiedOutput()
    {
        var serializer = new GalaxySerializer();
        var stroke = new StrokeStyle
        {
            Brush = "#FF8000",
            Width = 3,
            Pattern = StrokePattern.DashDot,
            StartCap = StrokeCap.Round,
            EndCap = StrokeCap.ArrowAnchor
        };

        var yaml = serializer.Serialize(stroke);

        return Verify(yaml);
    }
}