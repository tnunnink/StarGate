using FluentAssertions;
using StarGate.Core.Graphics.Styling;

namespace StarGate.Core.Tests.Graphics;

[TestFixture]
public class GraphicColorTests
{
    [Test]
    public void Default_HasZeroValues()
    {
        var color = default(GraphicColor);

        color.R.Should().Be(0);
        color.G.Should().Be(0);
        color.B.Should().Be(0);
        color.A.Should().Be(0);
    }

    [Test]
    public void Constructor_WithRgbOnly_SetsRgbAndDefaultsAlphaTo255()
    {
        var color = new GraphicColor(10, 20, 30);

        color.R.Should().Be(10);
        color.G.Should().Be(20);
        color.B.Should().Be(30);
        color.A.Should().Be(255);
    }

    [Test]
    public void Constructor_WithRgba_SetsAllChannels()
    {
        var color = new GraphicColor(10, 20, 30, 100);

        color.R.Should().Be(10);
        color.G.Should().Be(20);
        color.B.Should().Be(30);
        color.A.Should().Be(100);
    }

    [Test]
    public void PredefinedColors_HaveExpectedValues()
    {
        GraphicColor.Transparent.Should().Be(new GraphicColor(0, 0, 0, 0));
        GraphicColor.Black.Should().Be(new GraphicColor(0, 0, 0));
        GraphicColor.White.Should().Be(new GraphicColor(255, 255, 255));
    }

    [TestCase("#FF8000", (byte)255, (byte)128, (byte)0, (byte)255)]
    [TestCase("FF8000", (byte)255, (byte)128, (byte)0, (byte)255)]
    [TestCase("#000000", (byte)0, (byte)0, (byte)0, (byte)255)]
    [TestCase("#FFFFFF", (byte)255, (byte)255, (byte)255, (byte)255)]
    [TestCase("#ff8000", (byte)255, (byte)128, (byte)0, (byte)255)]
    public void Parse_WithValid6DigitHex_ReturnsExpectedColor(string input, byte expectedR, byte expectedG, byte expectedB, byte expectedA)
    {
        var color = GraphicColor.Parse(input);

        color.R.Should().Be(expectedR);
        color.G.Should().Be(expectedG);
        color.B.Should().Be(expectedB);
        color.A.Should().Be(expectedA);
    }

    [TestCase("#FF800080", (byte)255, (byte)128, (byte)0, (byte)128)]
    [TestCase("FF800080", (byte)255, (byte)128, (byte)0, (byte)128)]
    [TestCase("#00000000", (byte)0, (byte)0, (byte)0, (byte)0)]
    [TestCase("#FFFFFFFF", (byte)255, (byte)255, (byte)255, (byte)255)]
    [TestCase("#ff800080", (byte)255, (byte)128, (byte)0, (byte)128)]
    public void Parse_WithValid8DigitHex_ReturnsExpectedColor(string input, byte expectedR, byte expectedG, byte expectedB, byte expectedA)
    {
        var color = GraphicColor.Parse(input);

        color.R.Should().Be(expectedR);
        color.G.Should().Be(expectedG);
        color.B.Should().Be(expectedB);
        color.A.Should().Be(expectedA);
    }

    [TestCase("255 128 0", (byte)255, (byte)128, (byte)0, (byte)255)]
    [TestCase("  255   128   0  ", (byte)255, (byte)128, (byte)0, (byte)255)]
    [TestCase("0 0 0", (byte)0, (byte)0, (byte)0, (byte)255)]
    public void Parse_WithSpaceDelimitedRgb_ReturnsExpectedColor(string input, byte expectedR, byte expectedG, byte expectedB, byte expectedA)
    {
        var color = GraphicColor.Parse(input);

        color.R.Should().Be(expectedR);
        color.G.Should().Be(expectedG);
        color.B.Should().Be(expectedB);
        color.A.Should().Be(expectedA);
    }

    [TestCase("255 128 0 128", (byte)255, (byte)128, (byte)0, (byte)128)]
    [TestCase("  255   128   0   64  ", (byte)255, (byte)128, (byte)0, (byte)64)]
    [TestCase("0 0 0 0", (byte)0, (byte)0, (byte)0, (byte)0)]
    public void Parse_WithSpaceDelimitedRgba_ReturnsExpectedColor(string input, byte expectedR, byte expectedG, byte expectedB, byte expectedA)
    {
        var color = GraphicColor.Parse(input);

        color.R.Should().Be(expectedR);
        color.G.Should().Be(expectedG);
        color.B.Should().Be(expectedB);
        color.A.Should().Be(expectedA);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("invalid")]
    [TestCase("#12345")]
    [TestCase("#1234567")]
    [TestCase("#123456789")]
    [TestCase("#GGFFFF")]
    [TestCase("255 128")]
    [TestCase("300 128 0")]
    [TestCase("255 300 0")]
    [TestCase("255 128 300")]
    [TestCase("255 abc 0")]
    public void Parse_WithInvalidInput_ThrowsFormatException(string? input)
    {
        var act = () => GraphicColor.Parse(input!);

        act.Should().Throw<FormatException>()
            .WithMessage($"Invalid color format: '{input}'. Expected hex (#RRGGBB, #RRGGBBAA) or 'R G B'.");
    }

    [TestCase("#FF8000", true, (byte)255, (byte)128, (byte)0, (byte)255)]
    [TestCase("FF8000", true, (byte)255, (byte)128, (byte)0, (byte)255)]
    [TestCase("#FF800080", true, (byte)255, (byte)128, (byte)0, (byte)128)]
    [TestCase("255 128 0", true, (byte)255, (byte)128, (byte)0, (byte)255)]
    [TestCase("255 128 0 64", true, (byte)255, (byte)128, (byte)0, (byte)64)]
    [TestCase("255 128 0 invalidAlpha", true, (byte)255, (byte)128, (byte)0, (byte)255)]
    public void TryParse_WithValidInput_ReturnsTrueAndSetsColor(string input, bool expectedSuccess, byte expectedR, byte expectedG, byte expectedB, byte expectedA)
    {
        var success = GraphicColor.TryParse(input, out var color);

        success.Should().Be(expectedSuccess);
        color.R.Should().Be(expectedR);
        color.G.Should().Be(expectedG);
        color.B.Should().Be(expectedB);
        color.A.Should().Be(expectedA);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("invalid")]
    [TestCase("#12345")]
    [TestCase("#GGFFFF")]
    [TestCase("255 128")]
    [TestCase("300 128 0")]
    [TestCase("255 300 0")]
    [TestCase("255 128 300")]
    public void TryParse_WithInvalidInput_ReturnsFalseAndSetsDefaultColor(string? input)
    {
        var success = GraphicColor.TryParse(input, out var color);

        success.Should().BeFalse();
        color.Should().Be(default(GraphicColor));
    }

    [Test]
    public void ToHex_WithoutAlpha_FormatsAsSixDigitHex()
    {
        var color = new GraphicColor(255, 128, 0, 100);

        color.ToHex().Should().Be("#FF8000");
        color.ToHex().Should().Be("#FF8000");
    }

    [Test]
    public void ToHex_WithAlpha_FormatsAsEightDigitHex()
    {
        var color = new GraphicColor(255, 128, 0, 100);

        color.ToHex(true).Should().Be("#FF800064");
    }

    [Test]
    public void ToHex_PadsSingleDigitsWithLeadingZeros()
    {
        var color = new GraphicColor(0, 5, 10, 1);

        color.ToHex().Should().Be("#00050A");
        color.ToHex(true).Should().Be("#00050A01");
    }

    [Test]
    public void ToRGB_FormatsAsSpaceSeparatedValues()
    {
        var color = new GraphicColor(255, 128, 0, 64);

        color.ToRGB().Should().Be("255 128 0");
    }

    [Test]
    public void ToRGBA_FormatsAsSpaceSeparatedValues()
    {
        var color = new GraphicColor(255, 128, 0, 64);

        color.ToRGBA().Should().Be("255 128 0 64");
    }

    [Test]
    public void ToString_WhenFullyOpaque_ReturnsSixDigitHex()
    {
        var color = new GraphicColor(255, 128, 0);

        color.ToString().Should().Be("#FF8000");
    }

    [Test]
    public void ToString_WhenNotFullyOpaque_ReturnsEightDigitHex()
    {
        var color = new GraphicColor(255, 128, 0, 128);

        color.ToString().Should().Be("#FF800080");
    }

    [Test]
    public void ImplicitOperator_FromValidString_ParsesColor()
    {
        GraphicColor colorFromHex = "#FF8000";
        colorFromHex.Should().Be(new GraphicColor(255, 128, 0));

        GraphicColor colorFromRgb = "255 128 0";
        colorFromRgb.Should().Be(new GraphicColor(255, 128, 0));
    }

    [Test]
    public void ImplicitOperator_FromInvalidString_ThrowsFormatException()
    {
        var act = () =>
        {
            GraphicColor _ = "invalid";
        };

        act.Should().Throw<FormatException>();
    }

    [Test]
    public void Equality_SameValues_AreEqual()
    {
        var color1 = new GraphicColor(10, 20, 30, 40);
        var color2 = new GraphicColor(10, 20, 30, 40);

        (color1 == color2).Should().BeTrue();
        (color1 != color2).Should().BeFalse();
        color1.Equals(color2).Should().BeTrue();
        color1.GetHashCode().Should().Be(color2.GetHashCode());
    }

    [Test]
    public void Equality_DifferentValues_AreNotEqual()
    {
        var color1 = new GraphicColor(10, 20, 30, 40);
        var color2 = new GraphicColor(10, 20, 30, 50);

        (color1 == color2).Should().BeFalse();
        (color1 != color2).Should().BeTrue();
        color1.Equals(color2).Should().BeFalse();
    }
}
