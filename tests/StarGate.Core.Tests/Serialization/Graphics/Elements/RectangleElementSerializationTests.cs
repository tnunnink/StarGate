using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class RectangleElementSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultsElementShouldHaveExpectedValues()
    {
        var element = new RectangleElement { Name = "RectDefault" };

        element.Kind.Should().Be(ElementKind.Rectangle);
        element.Name.Should().Be("RectDefault");
        element.Bounds.Should().Be(default(GraphicBounds));
        element.Origin.Should().Be(default(GraphicPoint));
        element.Angle.Should().Be(0.0);
        element.Flip.Should().Be(default);
        element.Enabled.Should().BeTrue();
        element.Locked.Should().BeFalse();
        element.Visible.Should().BeTrue();
        element.TabStop.Should().BeTrue();
        element.TabOrder.Should().Be(0);
        element.Dock.Should().Be(default);
        element.Anchor.Should().Be(default);
        element.Faceplate.Should().BeNull();
        element.Fill.Should().BeNull();
        element.Stroke.Should().BeNull();
        element.Font.Should().BeNull();
        element.Radius.Should().BeNull();
    }

    #region SealizationTests

    [TestMethod]
    public void ElementShouldSerializeAndDeserializeCorrectly()
    {
        var serializer = new GalaxySerializer();
        var element = new RectangleElement
        {
            Name = "SerializedRect",
            Radius = 25,
            Bounds = new GraphicBounds { Left = 5, Top = 10, Width = 100, Height = 50 },
            Angle = 90.0,
            Visible = true,
            Locked = false
        };

        var yaml = serializer.Serialize(element);
        var deserialized = serializer.Deserialize<RectangleElement>(yaml);

        deserialized.Should().NotBeNull();
        deserialized.Name.Should().Be("SerializedRect");
        deserialized.Radius.Should().Be(25);
        deserialized.Bounds.Should().Be(element.Bounds);
        deserialized.Angle.Should().Be(90.0);
        deserialized.Visible.Should().BeTrue();
        deserialized.Locked.Should().BeFalse();
        deserialized.Kind.Should().Be(ElementKind.Rectangle);
    }

    [TestMethod]
    public Task SerializeDefaultNamedElementShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var element = new RectangleElement { Name = "SerializedRect" };

        var yaml = serializer.Serialize(element);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializeOverriddenShouldBeVerified()
    {
        var serializer = new GalaxySerializer();

        var element = new RectangleElement
        {
            Name = "SerializedRect",
            Radius = 25,
            Bounds = new GraphicBounds { Left = 5, Top = 10, Width = 100, Height = 50 },
            Angle = 90.0,
            Visible = false,
            Locked = true,
            Fill = new FillStyle
            {
                Brush = new GraphicColor(255, 0, 0),
                Unfilled = new GraphicColor(0, 255, 0),
                Orientation = FillOrientation.RelativeToScreen,
                Direction = FillDirection.DownRight,
                Vertical = 75.0,
                Horizontal = 50.0
            }
        };

        var yaml = serializer.Serialize(element);

        return Verify(yaml);
    }

    #endregion
}