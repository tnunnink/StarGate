using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class TextureBrushSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultsShouldHaveExpectedValues()
    {
        var brush = new TextureBrush { Image = "images/texture.png" };

        brush.Kind.Should().Be(BrushKind.Texture);
        brush.Image.Should().Be("images/texture.png");
        brush.Wrap.Should().Be(WrapMode.Stretch);
        brush.Transparency.Should().Be(0.0);
    }

    [TestMethod]
    public Task SerializeDefaultTextureShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var brush = new TextureBrush
        {
            Image = "images/texture.png"
        };

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializeCustomTextureShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var brush = new TextureBrush
        {
            Image = "images/texture.png",
            Wrap = WrapMode.Tile,
            Transparency = 25.0
        };

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }

    [TestMethod]
    public void DeserializeFromObjectWithDefaultsShouldHaveExpectedValues()
    {
        var serializer = new GalaxySerializer();
        var yaml = """
                   kind: Texture
                   image: images/texture.png
                   """;

        var brush = serializer.Deserialize<GraphicBrush>(yaml);

        brush.Should().NotBeNull();
        brush.Should().BeOfType<TextureBrush>();

        var texture = (TextureBrush)brush;
        texture.Kind.Should().Be(BrushKind.Texture);
        texture.Image.Should().Be("images/texture.png");
        texture.Wrap.Should().Be(WrapMode.Stretch);
        texture.Transparency.Should().Be(0.0);
    }

    [TestMethod]
    public void DeserializeFromObjectWithCustomTextureShouldHaveExpectedValues()
    {
        var serializer = new GalaxySerializer();
        var yaml = """
                   kind: Texture
                   image: images/texture.png
                   wrap: Tile
                   transparency: 25.0
                   """;

        var brush = serializer.Deserialize<GraphicBrush>(yaml);

        brush.Should().NotBeNull();
        brush.Should().BeOfType<TextureBrush>();

        var texture = (TextureBrush)brush;
        texture.Kind.Should().Be(BrushKind.Texture);
        texture.Image.Should().Be("images/texture.png");
        texture.Wrap.Should().Be(WrapMode.Tile);
        texture.Transparency.Should().Be(25.0);
    }
}
