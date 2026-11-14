using FluentAssertions;

namespace StarGate.Core.Tests.Serialization.Converters;

[TestClass]
public class ItemBindingConverterTests
{
    private readonly GalaxySerializer _serializer = new();

    [TestMethod]
    public void Accepts_ItemBindingTypes_ReturnsTrue()
    {
        var converter = new ItemBindingConverter();
        converter.Accepts(typeof(ItemBinding)).Should().BeTrue();
        converter.Accepts(typeof(EnumItemBinding)).Should().BeTrue();
        converter.Accepts(typeof(ArrayItemBinding)).Should().BeTrue();
        converter.Accepts(typeof(StaticItemBinding)).Should().BeTrue();
    }

    [TestMethod]
    public void Accepts_OtherType_ReturnsFalse()
    {
        var converter = new ItemBindingConverter();
        converter.Accepts(typeof(string)).Should().BeFalse();
        converter.Accepts(typeof(ValueBinding)).Should().BeFalse();
    }

    [TestMethod]
    public void Serialize_EnumItemBinding_EmitsEnumScalar()
    {
        var binding = new EnumItemBinding();
        var yaml = _serializer.Serialize(binding);

        yaml.Trim().Should().Be("enum");
    }

    [TestMethod]
    public void Serialize_ArrayItemBinding_EmitsFlowMapping()
    {
        var binding = new ArrayItemBinding("Plant.RecipeNames");
        var yaml = _serializer.Serialize(binding);

        yaml.Trim().Should().Be("{array: Plant.RecipeNames}");
    }

    [TestMethod]
    public void Serialize_StaticItemBinding_MatchingCaptions_EmitsFlowSequence()
    {
        var binding = new StaticItemBinding([
            new ControlItem { Value = "Off", Caption = "Off" },
            new ControlItem { Value = "Manual", Caption = "Manual" },
            new ControlItem { Value = "Auto", Caption = "Auto" }
        ]);
        var yaml = _serializer.Serialize(binding);

        yaml.Trim().Should().Be("[Off, Manual, Auto]");
    }

    [TestMethod]
    public void Serialize_StaticItemBinding_DifferingCaptions_EmitsBlockSequenceOfMappings()
    {
        var binding = new StaticItemBinding([
            new ControlItem { Value = "0", Caption = "Off" },
            new ControlItem { Value = "1", Caption = "On" }
        ]);

        var yaml = _serializer.Serialize(binding);

        yaml.Should().Contain("value: 0");
        yaml.Should().Contain("caption: Off");
    }

    [TestMethod]
    public void Deserialize_EnumKeyword_ReturnsEnumItemBinding()
    {
        var binding = _serializer.Deserialize<ItemBinding>("enum");

        binding.Should().BeOfType<EnumItemBinding>();
    }

    [TestMethod]
    public void Deserialize_ArrayMapping_ReturnsArrayItemBinding()
    {
        var binding = _serializer.Deserialize<ItemBinding>("{ array: \"Plant.RecipeNames\" }");

        binding.Should().BeOfType<ArrayItemBinding>();
        ((ArrayItemBinding)binding).Reference.Should().Be("Plant.RecipeNames");
    }

    [TestMethod]
    public void Deserialize_SequenceOfStrings_ReturnsStaticItemBinding()
    {
        var binding = _serializer.Deserialize<ItemBinding>("[Off, Manual, Auto]");

        binding.Should().BeOfType<StaticItemBinding>();
        var staticBinding = (StaticItemBinding)binding;
        staticBinding.Items.Should().HaveCount(3);
    }
}