using FluentAssertions;
using YamlDotNet.Core;

namespace StarGate.Core.Tests.Serialization.Converters;

[TestClass]
public class GraphicColorConverterTests
{
    private readonly GalaxySerializer _serializer = new();

    [TestMethod]
    public void Accepts_GraphicColorType_ReturnsTrue()
    {
        var converter = new GraphicColorConverter();
        converter.Accepts(typeof(GraphicColor)).Should().BeTrue();
    }

    [TestMethod]
    public void Accepts_OtherType_ReturnsFalse()
    {
        var converter = new GraphicColorConverter();
        converter.Accepts(typeof(string)).Should().BeFalse();
        converter.Accepts(typeof(int)).Should().BeFalse();
    }

    [TestMethod]
    [DataRow("#FF8000", "\"#FF8000\"")]
    [DataRow("#000000", "\"#000000\"")]
    [DataRow("#FFFFFF", "\"#FFFFFF\"")]
    public void Serialize_GraphicColor_EmitsDoubleQuotedScalar(string hex, string expectedYaml)
    {
        var color = GraphicColor.Parse(hex);
        var yaml = _serializer.Serialize(color);

        yaml.Trim().Should().Be(expectedYaml);
    }

    [TestMethod]
    [DataRow("\"#FF8000\"", 255, 128, 0)]
    [DataRow("\"255 128 0\"", 255, 128, 0)]
    public void Deserialize_ValidColor_ReturnsGraphicColor(string yaml, int r, int g, int b)
    {
        var color = _serializer.Deserialize<GraphicColor>(yaml);

        color.R.Should().Be((byte)r);
        color.G.Should().Be((byte)g);
        color.B.Should().Be((byte)b);
    }

    [TestMethod]
    public void Deserialize_MalformedColor_ThrowsYamlException()
    {
        var action = () => _serializer.Deserialize<GraphicColor>("\"invalid-color\"");

        action.Should().Throw<YamlException>().WithMessage("*Invalid color*");
    }
}