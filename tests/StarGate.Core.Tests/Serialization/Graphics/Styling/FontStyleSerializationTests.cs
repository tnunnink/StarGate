using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class FontStyleSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultsShouldHaveExpectedValues()
    {
        var font = new FontStyle();

        font.Family.Should().Be("Arial");
        font.Size.Should().Be(12);
        font.Bold.Should().BeFalse();
        font.Italic.Should().BeFalse();
        font.Underline.Should().BeFalse();
        font.Strikeout.Should().BeFalse();
        font.Alignment.Should().Be(GraphicAlignment.TopLeft);
    }

    [TestMethod]
    public Task SerializeDefaultShouldHaveVerifiedOutput()
    {
        var serializer = new GalaxySerializer();
        var font = new FontStyle();

        var yaml = serializer.Serialize(font);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializeCustomFontShouldHaveVerifiedOutput()
    {
        var serializer = new GalaxySerializer();
        var font = new FontStyle
        {
            Brush = "#0000FF",
            Family = "Segoe UI",
            Size = 14,
            Bold = true,
            Italic = true,
            Underline = true,
            Strikeout = true,
            Alignment = GraphicAlignment.Centers
        };

        var yaml = serializer.Serialize(font);

        return Verify(yaml);
    }
}