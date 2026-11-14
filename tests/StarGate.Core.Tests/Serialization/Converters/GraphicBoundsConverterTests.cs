using FluentAssertions;
using YamlDotNet.Core;

namespace StarGate.Core.Tests.Serialization.Converters;

[TestClass]
public class GraphicBoundsConverterTests
{
    private readonly GalaxySerializer _serializer = new();

    [TestMethod]
    public void Accepts_GraphicBoundsType_ReturnsTrue()
    {
        var converter = new GraphicBoundsConverter();
        converter.Accepts(typeof(GraphicBounds)).Should().BeTrue();
    }

    [TestMethod]
    public void Accepts_OtherType_ReturnsFalse()
    {
        var converter = new GraphicBoundsConverter();
        converter.Accepts(typeof(GraphicPoint)).Should().BeFalse();
        converter.Accepts(typeof(string)).Should().BeFalse();
    }

    [TestMethod]
    public void Serialize_DefaultBounds_EmitsFlowSequence()
    {
        var bounds = new GraphicBounds();
        var yaml = _serializer.Serialize(bounds);

        yaml.Trim().Should().Be("[0, 0, 0, 0]");
    }

    [TestMethod]
    public void Serialize_PopulatedBounds_EmitsFlowSequence()
    {
        var bounds = new GraphicBounds(10, 20, 100, 200);
        var yaml = _serializer.Serialize(bounds);

        yaml.Trim().Should().Be("[10, 20, 100, 200]");
    }

    [TestMethod]
    public void Deserialize_ValidSequence_ReturnsGraphicBounds()
    {
        var bounds = _serializer.Deserialize<GraphicBounds>("[10, 20, 100, 200]");

        bounds.Left.Should().Be(10);
        bounds.Top.Should().Be(20);
        bounds.Width.Should().Be(100);
        bounds.Height.Should().Be(200);
    }

    [TestMethod]
    [DataRow("[10, 20, 100]")]
    [DataRow("[10, 20, 100, 200, 300]")]
    public void Deserialize_MalformedSequence_ThrowsYamlException(string yaml)
    {
        var action = () => _serializer.Deserialize<GraphicBounds>(yaml);

        action.Should().Throw<YamlException>();
    }
}
