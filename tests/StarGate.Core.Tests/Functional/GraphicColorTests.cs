using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class GraphicColorTests
{
    [TestMethod]
    public void Default_HasZeroValues()
    {
        var color = default(GraphicColor);

        color.R.Should().Be(0);
        color.G.Should().Be(0);
        color.B.Should().Be(0);
    }

    [TestMethod]
    public void Constructor_SetsRgbChannels()
    {
        var color = new GraphicColor(10, 20, 30);

        color.R.Should().Be(10);
        color.G.Should().Be(20);
        color.B.Should().Be(30);
    }

    [TestMethod]
    public void PredefinedColors_HaveExpectedValues()
    {
        GraphicColor.Black.Should().Be(new GraphicColor(0, 0, 0));
        GraphicColor.White.Should().Be(new GraphicColor(255, 255, 255));
    }

    [TestMethod]
    [DataRow("#FF8000", (byte)255, (byte)128, (byte)0)]
    [DataRow("FF8000", (byte)255, (byte)128, (byte)0)]
    [DataRow("#000000", (byte)0, (byte)0, (byte)0)]
    [DataRow("#FFFFFF", (byte)255, (byte)255, (byte)255)]
    [DataRow("#ff8000", (byte)255, (byte)128, (byte)0)]
    public void Parse_WithValid6DigitHex_ReturnsExpectedColor(string input, byte expectedR, byte expectedG,
        byte expectedB)
    {
        var color = GraphicColor.Parse(input);

        color.R.Should().Be(expectedR);
        color.G.Should().Be(expectedG);
        color.B.Should().Be(expectedB);
    }

    [TestMethod]
    [DataRow("255 128 0", (byte)255, (byte)128, (byte)0)]
    [DataRow("  255   128   0  ", (byte)255, (byte)128, (byte)0)]
    [DataRow("0 0 0", (byte)0, (byte)0, (byte)0)]
    public void Parse_WithSpaceDelimitedRgb_ReturnsExpectedColor(string input, byte expectedR, byte expectedG,
        byte expectedB)
    {
        var color = GraphicColor.Parse(input);

        color.R.Should().Be(expectedR);
        color.G.Should().Be(expectedG);
        color.B.Should().Be(expectedB);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("invalid")]
    [DataRow("#12345")]
    [DataRow("#1234567")]
    [DataRow("#123456789")]
    [DataRow("#GGFFFF")]
    [DataRow("255 128")]
    [DataRow("300 128 0")]
    [DataRow("255 300 0")]
    [DataRow("255 128 300")]
    [DataRow("255 abc 0")]
    public void Parse_WithInvalidInput_ThrowsFormatException(string? input)
    {
        var act = () => GraphicColor.Parse(input!);

        act.Should().Throw<FormatException>()
            .WithMessage($"Invalid color format: '{input}'. Expected hex #RRGGBB or 'R G B'.");
    }

    [TestMethod]
    [DataRow("#FF8000", true, (byte)255, (byte)128, (byte)0)]
    [DataRow("FF8000", true, (byte)255, (byte)128, (byte)0)]
    [DataRow("255 128 0", true, (byte)255, (byte)128, (byte)0)]
    public void TryParse_WithValidInput_ReturnsTrueAndSetsColor(string input, bool expectedSuccess, byte expectedR,
        byte expectedG, byte expectedB)
    {
        var success = GraphicColor.TryParse(input, out var color);

        success.Should().Be(expectedSuccess);
        color.R.Should().Be(expectedR);
        color.G.Should().Be(expectedG);
        color.B.Should().Be(expectedB);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("invalid")]
    [DataRow("#12345")]
    [DataRow("#GGFFFF")]
    [DataRow("255 128")]
    [DataRow("300 128 0")]
    [DataRow("255 300 0")]
    [DataRow("255 128 300")]
    public void TryParse_WithInvalidInput_ReturnsFalseAndSetsDefaultColor(string? input)
    {
        var success = GraphicColor.TryParse(input, out var color);

        success.Should().BeFalse();
        color.Should().Be(default(GraphicColor));
    }

    [TestMethod]
    public void ToHex_FormatsAsSixDigitHex()
    {
        var color = new GraphicColor(255, 128, 0);

        color.ToHex().Should().Be("#FF8000");
    }

    [TestMethod]
    public void ToHex_PadsSingleDigitsWithLeadingZeros()
    {
        var color = new GraphicColor(0, 5, 10);

        color.ToHex().Should().Be("#00050A");
    }

    [TestMethod]
    public void ToRGB_FormatsAsSpaceSeparatedValues()
    {
        var color = new GraphicColor(255, 128, 0);

        color.ToRGB().Should().Be("255 128 0");
    }

    [TestMethod]
    public void ToString_ReturnsSixDigitHex()
    {
        var color = new GraphicColor(255, 128, 0);

        color.ToString().Should().Be("#FF8000");
    }

    [TestMethod]
    public void ImplicitOperator_FromValidString_ParsesColor()
    {
        GraphicColor colorFromHex = "#FF8000";
        colorFromHex.Should().Be(new GraphicColor(255, 128, 0));

        GraphicColor colorFromRgb = "255 128 0";
        colorFromRgb.Should().Be(new GraphicColor(255, 128, 0));
    }

    [TestMethod]
    public void ImplicitOperator_FromInvalidString_ThrowsFormatException()
    {
        var act = () =>
        {
            GraphicColor _ = "invalid";
        };

        act.Should().Throw<FormatException>();
    }

    [TestMethod]
    public void Equality_SameValues_AreEqual()
    {
        var color1 = new GraphicColor(10, 20, 30);
        var color2 = new GraphicColor(10, 20, 30);

        (color1 == color2).Should().BeTrue();
        (color1 != color2).Should().BeFalse();
        color1.Equals(color2).Should().BeTrue();
        color1.GetHashCode().Should().Be(color2.GetHashCode());
    }

    [TestMethod]
    public void Equality_DifferentValues_AreNotEqual()
    {
        var color1 = new GraphicColor(10, 20, 30);
        var color2 = new GraphicColor(10, 20, 50);

        (color1 == color2).Should().BeFalse();
        (color1 != color2).Should().BeTrue();
        color1.Equals(color2).Should().BeFalse();
    }
}