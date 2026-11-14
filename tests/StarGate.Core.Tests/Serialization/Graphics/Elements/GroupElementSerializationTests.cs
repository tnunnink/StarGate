using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class GroupElementSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultsElementShouldHaveExpectedValues()
    {
        var element = new GroupElement { Name = "GroupDefault" };

        element.Kind.Should().Be(ElementKind.Group);
        element.Name.Should().Be("GroupDefault");
        element.TreatAsIcon.Should().BeNull();
        element.Elements.Should().BeEmpty();
    }

    [TestMethod]
    public Task SerializeDefaultNamedElementShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var element = new GroupElement { Name = "SerializedGroup" };

        var yaml = serializer.Serialize(element);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializeNestedGroupHierarchyShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var element = new GroupElement
        {
            Name = "RootGroup",
            TreatAsIcon = false,
            Elements =
            [
                new RectangleElement
                {
                    Name = "BackgroundRect",
                    Bounds = new GraphicBounds(0, 0, 200, 100),
                    Fill = new FillStyle { Brush = "#333333" }
                },
                new GroupElement
                {
                    Name = "SubGroup",
                    Elements =
                    [
                        new TextElement { Name = "Label", Text = "Nested Text" },
                        new LineElement { Name = "Divider", Start = new GraphicPoint(0, 50), End = new GraphicPoint(200, 50) }
                    ]
                }
            ]
        };

        var yaml = serializer.Serialize(element);

        return Verify(yaml);
    }
}
