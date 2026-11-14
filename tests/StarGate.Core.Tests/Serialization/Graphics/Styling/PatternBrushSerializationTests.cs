using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class PatternBrushSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultsShouldHaveExpectedValues()
    {
        var brush = new PatternBrush();

        brush.Kind.Should().Be(BrushKind.Pattern);
        brush.ForeColor.Should().Be(GraphicColor.Black);
        brush.BackColor.Should().Be(GraphicColor.White);
        brush.Hatch.Should().Be(HatchStyle.Horizontal);
        brush.Transparency.Should().Be(0.0);
    }

    [TestMethod]
    public Task SerializeDefaultPatternShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var brush = new PatternBrush();

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializeCustomPatternShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var brush = new PatternBrush
        {
            ForeColor = GraphicColor.Parse("#FF0000"),
            BackColor = GraphicColor.Parse("#0000FF"),
            Hatch = HatchStyle.Cross
        };

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializePatternWithTransparencyShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var brush = new PatternBrush
        {
            ForeColor = GraphicColor.Parse("#FF0000"),
            BackColor = GraphicColor.Parse("#0000FF"),
            Hatch = HatchStyle.DiagonalCross,
            Transparency = 25.0
        };

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }

    [TestMethod]
    public void DeserializeFromObjectWithDefaultsShouldHaveExpectedValues()
    {
        var serializer = new GalaxySerializer();
        const string yaml = "kind: Pattern";

        var brush = serializer.Deserialize<GraphicBrush>(yaml);

        brush.Should().NotBeNull();
        brush.Should().BeOfType<PatternBrush>();

        var pattern = (PatternBrush)brush;
        pattern.Kind.Should().Be(BrushKind.Pattern);
        pattern.ForeColor.Should().Be(GraphicColor.Black);
        pattern.BackColor.Should().Be(GraphicColor.White);
        pattern.Hatch.Should().Be(HatchStyle.Horizontal);
        pattern.Transparency.Should().Be(0.0);
    }

    [TestMethod]
    public void DeserializeFromObjectWithCustomPatternShouldHaveExpectedValues()
    {
        var serializer = new GalaxySerializer();
        const string yaml =
            """
            kind: Pattern
            foreColor: "#FF0000"
            backColor: "#0000FF"
            hatch: Cross
            """;

        var brush = serializer.Deserialize<GraphicBrush>(yaml);

        brush.Should().NotBeNull();
        brush.Should().BeOfType<PatternBrush>();

        var pattern = (PatternBrush)brush;
        pattern.Kind.Should().Be(BrushKind.Pattern);
        pattern.ForeColor.Should().Be(new GraphicColor(255, 0, 0));
        pattern.BackColor.Should().Be(new GraphicColor(0, 0, 255));
        pattern.Hatch.Should().Be(HatchStyle.Cross);
        pattern.Transparency.Should().Be(0.0);
    }

    [TestMethod]
    public void DeserializeFromObjectWithTransparencyShouldHaveExpectedValues()
    {
        var serializer = new GalaxySerializer();
        const string yaml =
            """
            kind: Pattern
            foreColor: "#FF0000"
            backColor: "#0000FF"
            hatch: DiagonalCross
            transparency: 25.0
            """;

        var brush = serializer.Deserialize<GraphicBrush>(yaml);

        brush.Should().NotBeNull();
        brush.Should().BeOfType<PatternBrush>();

        var pattern = (PatternBrush)brush;
        pattern.Kind.Should().Be(BrushKind.Pattern);
        pattern.ForeColor.Should().Be(new GraphicColor(255, 0, 0));
        pattern.BackColor.Should().Be(new GraphicColor(0, 0, 255));
        pattern.Hatch.Should().Be(HatchStyle.DiagonalCross);
        pattern.Transparency.Should().Be(25.0);
    }
}