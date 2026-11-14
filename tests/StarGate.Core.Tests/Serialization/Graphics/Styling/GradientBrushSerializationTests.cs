using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class GradientBrushSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultsShouldHaveExpectedValues()
    {
        var brush = new GradientBrush();

        brush.Kind.Should().Be(BrushKind.Gradient);
        brush.Type.Should().Be(GradientType.Horizontal);
        brush.Angle.Should().Be(90.0);
        brush.Center.Should().Be(0.5f);
        brush.Falloff.Should().Be(1.0f);
        brush.FocusScale.Should().Be(0);
        brush.Stops.Should().BeEmpty();
    }

    [TestMethod]
    public Task SerializeDefaultGradientShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var brush = new GradientBrush();

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializeLinearGradientShouldBeVerified()
    {
        //NOTE Angle should only get serialized for custom gradients
        var serializer = new GalaxySerializer();
        var brush = new GradientBrush
        {
            Type = GradientType.Horizontal,
            Angle = 45.0,
            Center = 0.5f,
            Falloff = 1.0f,
            Stops = [GraphicColor.Parse("#FF0000"), GraphicColor.Parse("#0000FF")]
        };

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }
    
    [TestMethod]
    public Task SerializeCustomGradientShouldBeVerified()
    {
        //NOTE Angle should only get serialized for radial gradients
        var serializer = new GalaxySerializer();
        var brush = new GradientBrush
        {
            Type = GradientType.Custom,
            Angle = 45.0,
            Stops = [GraphicColor.Parse("#FF0000"), GraphicColor.Parse("#0000FF")]
        };

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializeRadialGradientShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var brush = new GradientBrush
        {
            Type = GradientType.Radial,
            Center = 0.25f,
            Falloff = 0.75f,
            FocusScale = 0.5f,
            Stops = [GraphicColor.White, GraphicColor.Black]
        };

        var yaml = serializer.Serialize(brush);

        return Verify(yaml);
    }
}