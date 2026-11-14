using FluentAssertions;
using YamlDotNet.Core;

namespace StarGate.Core.Tests.Serialization.Converters;

[TestClass]
public class GraphicPointConverterTests
{
    private readonly GalaxySerializer _serializer = new();

    [TestMethod]
    public void Accepts_GraphicPointType_ReturnsTrue()
    {
        var converter = new GraphicPointConverter();
        converter.Accepts(typeof(GraphicPoint)).Should().BeTrue();
    }

    [TestMethod]
    public void Accepts_OtherType_ReturnsFalse()
    {
        var converter = new GraphicPointConverter();
        converter.Accepts(typeof(GraphicBounds)).Should().BeFalse();
        converter.Accepts(typeof(string)).Should().BeFalse();
    }

    [TestMethod]
    public void Serialize_Point_EmitsFlowSequence()
    {
        var point = new GraphicPoint(10, 20);
        var yaml = _serializer.Serialize(point);

        yaml.Trim().Should().Be("[10, 20]");
    }

    [TestMethod]
    public void Deserialize_ValidSequence_ReturnsGraphicPoint()
    {
        var point = _serializer.Deserialize<GraphicPoint>("[10, 20]");

        point.X.Should().Be(10);
        point.Y.Should().Be(20);
    }

    [TestMethod]
    [DataRow("[10]")]
    [DataRow("[10, 20, 30]")]
    public void Deserialize_MalformedSequence_ThrowsYamlException(string yaml)
    {
        var action = () => _serializer.Deserialize<GraphicPoint>(yaml);

        action.Should().Throw<YamlException>();
    }
}
