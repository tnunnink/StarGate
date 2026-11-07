using ArchestrA.GRAccess;
using FluentAssertions;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestClass]
public class GalaxyObjectHandlerTests : GalaxyFixture
{
    [TestMethod]
    public void Extract_Instance_ShouldHaveExpectedIdentity()
    {
        var instance = GetInstance(TestInstance);

        var definition = new GalaxyObjectHandler().Extract(instance);

        definition.TagName.Should().Be(TestInstance);
        definition.DerivedFrom.Should().Be(instance.DerivedFrom);
        definition.BasedOn.Should().Be(instance.basedOn);
        definition.Category.Should().NotBeNullOrEmpty();
        definition.Category.Should().NotContain("idxCategory");
        definition.HierarchicalName.Should().Be(instance.HierarchicalName);
    }

    [TestMethod]
    public void Extract_Instance_ShouldIncludeUserDefinedAttributes()
    {
        var instance = GetInstance(TestInstance);

        var definition = new GalaxyObjectHandler().Extract(instance);

        definition.Attributes.Select(a => a.Name)
            .Should().Contain(["TestBool", "TestInt", "TestFloat", "TestString", "TestArray"]);
    }

    [TestMethod]
    public void Extract_Template_ShouldHaveTemplateTagName()
    {
        var template = GetTemplate(TestTemplate);

        var definition = new GalaxyObjectHandler().Extract(template);

        definition.TagName.Should().Be(TestTemplate);
        definition.Attributes.Should().NotBeEmpty();
    }

    [TestMethod]
    public void Extract_InstanceWithoutUserAttributes_ShouldHaveNoAttributes()
    {
        var instance = GetInstance(EmptyInstance);

        var definition = new GalaxyObjectHandler().Extract(instance);

        definition.Attributes.Should().BeEmpty();
    }

    [TestMethod]
    public void Extract_Instance_ShouldNotIncludeScriptsOrGraphics()
    {
        var instance = GetInstance(TestInstance);

        var definition = new GalaxyObjectHandler().Extract(instance);

        definition.Scripts.Should().BeEmpty();
        definition.Graphics.Should().BeEmpty();
    }

    [TestMethod]
    public void Extract_InstanceWithoutContainedName_ShouldHaveNullContainedName()
    {
        var instance = GetInstance(TestInstance);

        var definition = new GalaxyObjectHandler().Extract(instance);

        if (string.IsNullOrEmpty(instance.ContainedName))
            definition.ContainedName.Should().BeNull();
        else
            definition.ContainedName.Should().Be(instance.ContainedName);
    }

    [TestMethod]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new GalaxyObjectHandler();

        var result = handler.Apply(instance, null);

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_NewAttributes_ShouldCheckOutAddAttributesAndCheckIn()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var source = new GalaxyObject
        {
            TagName = temp.TagName,
            DerivedFrom = "$UserDefined",
            Attributes =
            [
                new GalaxyAttribute
                {
                    Name = "First",
                    DataType = AttributeDataType.Integer,
                    Value = new AttributeConfig { Value = 1 }
                },
                new GalaxyAttribute
                {
                    Name = "Second",
                    DataType = AttributeDataType.String,
                    Value = new AttributeConfig { Value = "two" }
                }
            ]
        };

        var result = new GalaxyObjectHandler().Apply(temp.Instance, source, "test apply");

        result.Success.Should().BeTrue();
        temp.Instance.CheckoutStatus.Should().Be(ECheckoutStatus.notCheckedOut);
        temp.Instance.Attributes["First"].GetValue().Should().Be(1);
        temp.Instance.Attributes["Second"].GetValue().Should().Be("two");
    }

    [TestMethod]
    public void Apply_NoAttributes_ShouldCheckOutAndCheckIn()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var source = new GalaxyObject { TagName = temp.TagName, DerivedFrom = "$UserDefined" };

        var result = new GalaxyObjectHandler().Apply(temp.Instance, source);

        result.Success.Should().BeTrue();
        temp.Instance.CheckoutStatus.Should().Be(ECheckoutStatus.notCheckedOut);
    }

    [TestMethod]
    public void Apply_InvalidAttribute_ShouldFailAndRollback()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var source = new GalaxyObject
        {
            TagName = temp.TagName,
            DerivedFrom = "$UserDefined",
            Attributes = [new GalaxyAttribute { Name = "Invalid" }] // missing data type
        };

        var result = new GalaxyObjectHandler().Apply(temp.Instance, source);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ErrorCode.MissingDataType);
        temp.Instance.CheckoutStatus.Should().Be(ECheckoutStatus.notCheckedOut);
    }

    [TestMethod]
    public void ExtractThenApply_Instance_ShouldRoundTripAttributeNames()
    {
        var handler = new GalaxyObjectHandler();
        var original = handler.Extract(GetInstance(TestInstance));
        using var temp = new GalaxyTestObject(Galaxy);

        var result = handler.Apply(temp.Instance, original);
        var copy = handler.Extract(temp.Instance);

        result.Success.Should().BeTrue();
        copy.Attributes.Select(a => a.Name).Should().BeEquivalentTo(original.Attributes.Select(a => a.Name));
    }
}