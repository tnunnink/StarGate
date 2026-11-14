using FluentAssertions;
using YamlDotNet.Core;

namespace StarGate.Core.Tests.Serialization.Converters;

[TestClass]
public class GraphicExpressionConverterTests
{
    private readonly GalaxySerializer _serializer = new();

    [TestMethod]
    public void Accepts_GraphicExpressionTypes_ReturnsTrue()
    {
        var converter = new GraphicExpressionConverter();
        converter.Accepts(typeof(GraphicExpression)).Should().BeTrue();
        converter.Accepts(typeof(StaticExpression)).Should().BeTrue();
        converter.Accepts(typeof(ReferenceExpression)).Should().BeTrue();
    }

    [TestMethod]
    public void Accepts_OtherType_ReturnsFalse()
    {
        var converter = new GraphicExpressionConverter();
        converter.Accepts(typeof(string)).Should().BeFalse();
        converter.Accepts(typeof(int)).Should().BeFalse();
    }

    [TestMethod]
    public void Serialize_StaticExpression_EmitsDoubleQuotedScalar()
    {
        var expr = new StaticExpression("01/01/2025");
        var yaml = _serializer.Serialize(expr);

        yaml.Trim().Should().Be("\"01/01/2025\"");
    }

    [TestMethod]
    public void Serialize_ReferenceExpression_EmitsFlowMapping()
    {
        var expr = new ReferenceExpression("Me.StartDate");
        var yaml = _serializer.Serialize(expr);

        yaml.Trim().Should().Be("{ref: Me.StartDate}");
    }

    [TestMethod]
    public void Deserialize_ScalarString_ReturnsStaticExpression()
    {
        var expr = _serializer.Deserialize<GraphicExpression>("\"01/01/2025\"");

        expr.Should().BeOfType<StaticExpression>();
        expr.Text.Should().Be("01/01/2025");
    }

    [TestMethod]
    public void Deserialize_MappingWithRef_ReturnsReferenceExpression()
    {
        var expr = _serializer.Deserialize<GraphicExpression>("{ ref: \"Me.StartDate\" }");

        expr.Should().BeOfType<ReferenceExpression>();
        expr.Text.Should().Be("Me.StartDate");
    }

    [TestMethod]
    public void Deserialize_InvalidMapping_ThrowsYamlException()
    {
        var action = () => _serializer.Deserialize<GraphicExpression>("{ invalid: \"abc\" }");

        action.Should().Throw<YamlException>();
    }
}
