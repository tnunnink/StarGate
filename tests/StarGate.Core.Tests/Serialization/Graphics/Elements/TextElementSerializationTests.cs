using FluentAssertions;

namespace StarGate.Core;

[TestClass]
public class TextElementSerializationTests : VerifyBase
{
    [TestMethod]
    public void DefaultsElementShouldHaveExpectedValues()
    {
        var element = new TextElement { Name = "TextDefault" };

        element.Kind.Should().Be(ElementKind.Text);
        element.Name.Should().Be("TextDefault");
        element.Text.Should().Be(string.Empty);
        element.Format.Should().BeNull();
        element.WordWrap.Should().BeNull();
        element.AutoScale.Should().BeNull();
    }

    [TestMethod]
    public void ElementShouldSerializeAndDeserializeCorrectly()
    {
        var serializer = new GalaxySerializer();
        var element = new TextElement
        {
            Name = "SerializedText",
            Text = "Hello World",
            Format = "{0:N2}",
            WordWrap = true,
            AutoScale = false,
            Font = new FontStyle { Family = "Segoe UI", Size = 16, Bold = true }
        };

        var yaml = serializer.Serialize(element);
        var deserialized = serializer.Deserialize<TextElement>(yaml);

        deserialized.Should().NotBeNull();
        deserialized.Name.Should().Be("SerializedText");
        deserialized.Text.Should().Be("Hello World");
        deserialized.Format.Should().Be("{0:N2}");
        deserialized.WordWrap.Should().BeTrue();
        deserialized.AutoScale.Should().BeFalse();
        deserialized.Kind.Should().Be(ElementKind.Text);
    }

    [TestMethod]
    public Task SerializeDefaultNamedElementShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var element = new TextElement { Name = "SerializedText" };

        var yaml = serializer.Serialize(element);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializePopulatedTextShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var element = new TextElement
        {
            Name = "SerializedText",
            Text = "Status: OK",
            Format = "{0}",
            WordWrap = true,
            AutoScale = true,
            Font = new FontStyle { Family = "Tahoma", Size = 11, Bold = true }
        };

        var yaml = serializer.Serialize(element);

        return Verify(yaml);
    }
}