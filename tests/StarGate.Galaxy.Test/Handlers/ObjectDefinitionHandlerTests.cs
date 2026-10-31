using System.Linq;
using ArchestrA.GRAccess;
using FluentAssertions;
using NUnit.Framework;
using StarGate.Core;
using StarGate.Galaxy.Extensions;
using StarGate.Galaxy.Handlers;

namespace StarGate.Galaxy.Test.Handlers;

[TestFixture]
public class ObjectDefinitionHandlerTests : GalaxyTestFixture
{
    [Test]
    public void Extract_Instance_ShouldHaveExpectedIdentity()
    {
        var instance = GetInstance(TestInstance);

        var definition = new ObjectDefinitionHandler().Extract(instance);

        definition.TagName.Should().Be(TestInstance);
        definition.Template.Should().Be(instance.DerivedFrom);
        definition.BasedOn.Should().Be(instance.basedOn);
        definition.Category.Should().NotBeNullOrEmpty();
        definition.Category.Should().NotContain("idxCategory");
        definition.HierarchicalName.Should().Be(instance.HierarchicalName);
    }

    [Test]
    public void Extract_Instance_ShouldIncludeUserDefinedAttributes()
    {
        var instance = GetInstance(TestInstance);

        var definition = new ObjectDefinitionHandler().Extract(instance);

        definition.Attributes.Select(a => a.Name)
            .Should().Contain(["TestBool", "TestInt", "TestFloat", "TestString", "TestArray"]);
    }

    [Test]
    public void Extract_Template_ShouldHaveTemplateTagName()
    {
        var template = GetTemplate(TestTemplate);

        var definition = new ObjectDefinitionHandler().Extract(template);

        definition.TagName.Should().Be(TestTemplate);
        definition.Attributes.Should().NotBeEmpty();
    }

    [Test]
    public void Extract_InstanceWithoutUserAttributes_ShouldHaveNoAttributes()
    {
        var instance = GetInstance(EmptyInstance);

        var definition = new ObjectDefinitionHandler().Extract(instance);

        definition.Attributes.Should().BeEmpty();
    }

    [Test]
    public void Extract_Instance_ShouldNotIncludeScriptsOrGraphics()
    {
        var instance = GetInstance(TestInstance);

        var definition = new ObjectDefinitionHandler().Extract(instance);

        definition.Scripts.Should().BeEmpty();
        definition.Graphics.Should().BeEmpty();
    }

    [Test]
    public void Extract_InstanceWithoutContainedName_ShouldHaveNullContainedName()
    {
        var instance = GetInstance(TestInstance);

        var definition = new ObjectDefinitionHandler().Extract(instance);

        if (string.IsNullOrEmpty(instance.ContainedName))
            definition.ContainedName.Should().BeNull();
        else
            definition.ContainedName.Should().Be(instance.ContainedName);
    }

    [Test]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new ObjectDefinitionHandler();

        var result = handler.Apply(instance, null);

        result.Success.Should().BeTrue();
    }

    [Test]
    public void Apply_NewAttributes_ShouldCheckOutAddAttributesAndCheckIn()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var source = new GalaxyObject
        {
            TagName = temp.TagName,
            Template = "$UserDefined",
            Attributes =
            [
                new ObjectAttribute
                {
                    Name = "First",
                    DataType = DataType.Integer,
                    Value = new AttributeConfig { Value = 1 }
                },
                new ObjectAttribute
                {
                    Name = "Second",
                    DataType = DataType.String,
                    Value = new AttributeConfig { Value = "two" }
                }
            ]
        };

        var result = new ObjectDefinitionHandler().Apply(temp.Instance, source, "test apply");

        result.Success.Should().BeTrue();
        temp.Instance.CheckoutStatus.Should().Be(ECheckoutStatus.notCheckedOut);
        temp.Instance.Attributes["First"].GetValue().Should().Be(1);
        temp.Instance.Attributes["Second"].GetValue().Should().Be("two");
    }

    [Test]
    public void Apply_NoAttributes_ShouldCheckOutAndCheckIn()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var source = new GalaxyObject { TagName = temp.TagName, Template = "$UserDefined" };

        var result = new ObjectDefinitionHandler().Apply(temp.Instance, source);

        result.Success.Should().BeTrue();
        temp.Instance.CheckoutStatus.Should().Be(ECheckoutStatus.notCheckedOut);
    }

    [Test]
    public void Apply_InvalidAttribute_ShouldFailAndRollback()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var source = new GalaxyObject
        {
            TagName = temp.TagName,
            Template = "$UserDefined",
            Attributes = [new ObjectAttribute { Name = "Invalid" }] // missing data type
        };

        var result = new ObjectDefinitionHandler().Apply(temp.Instance, source);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ErrorCode.MissingDataType);
        temp.Instance.CheckoutStatus.Should().Be(ECheckoutStatus.notCheckedOut);
    }

    [Test]
    public void ExtractThenApply_Instance_ShouldRoundTripAttributeNames()
    {
        var handler = new ObjectDefinitionHandler();
        var original = handler.Extract(GetInstance(TestInstance));
        using var temp = new GalaxyTestObject(Galaxy);

        var result = handler.Apply(temp.Instance, original);
        var copy = handler.Extract(temp.Instance);

        result.Success.Should().BeTrue();
        copy.Attributes.Select(a => a.Name).Should().BeEquivalentTo(original.Attributes.Select(a => a.Name));
    }
}