using FluentAssertions;

namespace StarGate.Core.Tests.Serialization.Converters;

[TestClass]
public class SolidBrushConverterTests
{
    private readonly GalaxySerializer _serializer = new();

    [TestMethod]
    public void Accepts_SolidBrushType_ReturnsTrue()
    {
        var converter = new SolidBrushConverter();
        converter.Accepts(typeof(SolidBrush)).Should().BeTrue();
    }

    [TestMethod]
    public void Accepts_OtherType_ReturnsFalse()
    {
        var converter = new SolidBrushConverter();
        converter.Accepts(typeof(GradientBrush)).Should().BeFalse();
        converter.Accepts(typeof(string)).Should().BeFalse();
    }

    [TestMethod]
    public void Serialize_OpaqueSolidBrush_EmitsDoubleQuotedHex()
    {
        var brush = new SolidBrush { Color = GraphicColor.Parse("#FF8000"), Transparency = 0.0 };
        var yaml = _serializer.Serialize(brush);

        yaml.Trim().Should().Be("\"#FF8000\"");
    }

    [TestMethod]
    public void Serialize_TransparentSolidBrush_EmitsMapping()
    {
        var brush = new SolidBrush { Color = GraphicColor.Parse("#FF8000"), Transparency = 25.0 };
        var yaml = _serializer.Serialize(brush);

        yaml.Should().Contain("kind: Solid");
        yaml.Should().Contain("color: \"#FF8000\"");
        yaml.Should().Contain("transparency: 25");
    }

    [TestMethod]
    public void Deserialize_ScalarColor_ReturnsSolidBrush()
    {
        var brush = _serializer.Deserialize<SolidBrush>("\"#FF8000\"");

        brush.Color.R.Should().Be(255);
        brush.Color.G.Should().Be(128);
        brush.Color.B.Should().Be(0);
        brush.Transparency.Should().Be(0);
    }

    [TestMethod]
    public void Deserialize_Mapping_ReturnsSolidBrushWithTransparency()
    {
        var yaml = """
                   kind: solid
                   color: "#FF8000"
                   transparency: 50.0
                   """;

        var brush = _serializer.Deserialize<SolidBrush>(yaml);

        brush.Color.R.Should().Be(255);
        brush.Color.G.Should().Be(128);
        brush.Color.B.Should().Be(0);
        brush.Transparency.Should().Be(50.0);
    }
}
