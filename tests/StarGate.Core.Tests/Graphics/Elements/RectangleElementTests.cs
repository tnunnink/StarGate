using System.Reflection;
using FluentAssertions;
using YamlDotNet.Serialization;

namespace StarGate.Core;

[TestClass]
[UsesVerify]
public partial class RectangleElementTests
{
    [TestMethod]
    public void Kind_ShouldBeRectangle()
    {
        var element = new RectangleElement { Name = "Rect1" };

        element.Kind.Should().Be(ElementKind.Rectangle);
    }

    [TestMethod]
    public void Kind_ShouldHaveYamlIgnoreAttribute()
    {
        var property = typeof(RectangleElement).GetProperty(nameof(RectangleElement.Kind));

        property.Should().NotBeNull();
        property.GetCustomAttribute<YamlIgnoreAttribute>().Should().NotBeNull();
    }

    [TestMethod]
    public void Defaults_ShouldHaveExpectedValues()
    {
        var element = new RectangleElement { Name = "RectDefault" };

        element.Name.Should().Be("RectDefault");
        element.Radius.Should().BeNull();
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
    }

    [TestMethod]
    public void Properties_WhenSet_ShouldRetainValues()
    {
        var fill = new FillStyle
        {
            Brush = new GraphicColor(255, 0, 0),
            Unfilled = new GraphicColor(0, 255, 0),
            Orientation = FillOrientation.RelativeToScreen,
            Direction = FillDirection.DownRight,
            Vertical = 75.0,
            Horizontal = 50.0
        };
        var stroke = new StrokeStyle { Brush = new GraphicColor(0, 0, 255), Width = 2 };
        var font = new FontStyle { Family = "Arial", Size = 12 };
        var bounds = new GraphicBounds { Left = 10, Top = 20, Width = 100, Height = 50 };
        var origin = new GraphicPoint { X = 50, Y = 25 };

        var element = new RectangleElement
        {
            Name = "CustomRect",
            Radius = 15,
            Bounds = bounds,
            Origin = origin,
            Angle = 45.0,
            Flip = GraphicFlip.Horizontal,
            Enabled = true,
            Locked = false,
            Visible = true,
            TabStop = true,
            TabOrder = 3,
            Dock = DockType.Top,
            Anchor = AnchorType.Center,
            Faceplate = false,
            Fill = fill,
            Stroke = stroke,
            Font = font
        };

        element.Name.Should().Be("CustomRect");
        element.Radius.Should().Be(15);
        element.Bounds.Should().Be(bounds);
        element.Origin.Should().Be(origin);
        element.Angle.Should().Be(45.0);
        element.Flip.Should().Be(GraphicFlip.Horizontal);
        element.Enabled.Should().BeTrue();
        element.Locked.Should().BeFalse();
        element.Visible.Should().BeTrue();
        element.TabStop.Should().BeTrue();
        element.TabOrder.Should().Be(3);
        element.Dock.Should().Be(DockType.Top);
        element.Anchor.Should().Be(AnchorType.Center);
        element.Faceplate.Should().BeFalse();
        element.Fill.Should().Be(fill);
        element.Stroke.Should().Be(stroke);
        element.Font.Should().Be(font);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(35)]
    [DataRow(70)]
    public void Validation_WithValidRadius_Passes(int? radius)
    {
        var element = new RectangleElement
        {
            Name = "ValidRadiusRect",
            Radius = radius
        };

        var result = element.Validate();

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(-10)]
    [DataRow(71)]
    [DataRow(100)]
    public void Validation_WithInvalidRadius_Fails(int? radius)
    {
        var element = new RectangleElement
        {
            Name = "InvalidRadiusRect",
            Radius = radius
        };

        var result = element.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "Corner radius must be between 0 and 70.");
    }

    [TestMethod]
    public void Serialization_ShouldSerializeAndDeserializeCorrectly()
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

        yaml.Should().NotContain("kind");
        yaml.Should().Contain("name: SerializedRect");
        yaml.Should().Contain("radius: 25");

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
    public Task Serialization_ShouldBeVerified()
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
}