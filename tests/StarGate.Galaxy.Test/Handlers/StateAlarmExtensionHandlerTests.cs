using ArchestrA.GRAccess;
using FluentAssertions;
using NUnit.Framework;
using StarGate.Core.Extensions;
using StarGate.Core.Objects;
using StarGate.Galaxy.Extensions;
using StarGate.Galaxy.Handlers;

namespace StarGate.Galaxy.Test.Handlers;

[TestFixture]
public class StateAlarmExtensionHandlerTests : GalaxyTestFixture
{
    // Assumes TestBool is configured with a state alarm extension in the test galaxy.
    private const string AlarmedAttribute = "TestBool";

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void Extract_NullOrEmptyContext_ShouldReturnNull(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new StateAlarmExtensionHandler();

        var result = handler.Extract(instance, context);

        result.Should().BeNull();
    }

    [Test]
    public void Extract_PrimitiveWithoutExtension_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        var handler = new StateAlarmExtensionHandler();

        var result = handler.Extract(instance, "TestString");

        result.Should().BeNull();
    }

    [Test]
    public void Extract_PrimitiveWithExtension_ShouldHaveExpectedValues()
    {
        var instance = GetInstance(TestInstance);

        var config = new StateAlarmExtensionHandler().Extract(instance, AlarmedAttribute);

        config.Should().NotBeNull();
        config.Category.Should().NotBeNull();
        config.Priority.Should().NotBeNull();
        config.Message.Should().NotBeNull();
        config.ActiveState.Should().NotBeNull();
        config.Source.Should().NotBeNull();
        config.Delay.Should().NotBeNull();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void Apply_NullOrEmptyContext_ShouldReturnSuccess(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new StateAlarmExtensionHandler();

        var result = handler.Apply(instance, new StateAlarmExtension(), context);

        result.Success.Should().BeTrue();
    }

    [Test]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new StateAlarmExtensionHandler();

        var result = handler.Apply(instance, null, AlarmedAttribute);

        result.Success.Should().BeTrue();
    }

    [Test]
    public void Apply_ExistingExtension_ShouldUpdateValues()
    {
        var instance = GetInstance(TestInstance);
        var handler = new StateAlarmExtensionHandler();
        instance.CheckOut();
        var source = new StateAlarmExtension { Priority = new AttributeConfig { Value = 321 } };

        var result = handler.Apply(instance, source, AlarmedAttribute);
        var config = handler.Extract(instance, AlarmedAttribute);
        instance.Rollback();

        result.Success.Should().BeTrue();
        config!.Priority!.Value.Should().Be(321);
    }

    [Test]
    public void Apply_PrimitiveWithoutExtension_ShouldAddExtension()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();
        instance.AddUDA("TempBool", MxDataType.MxBoolean, MxAttributeCategory.MxCategoryWriteable_USC_Lockable);
        var source = new StateAlarmExtension { Priority = new AttributeConfig { Value = 500 } };

        var result = new StateAlarmExtensionHandler().Apply(instance, source, "TempBool");
        var hasExtension = instance.HasExtension(typeof(StateAlarmExtension), "TempBool");
        instance.Rollback();

        result.Success.Should().BeTrue();
        hasExtension.Should().BeTrue();
    }

    [Test]
    public void Apply_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        var instance = GetInstance(TestInstance);
        var source = new StateAlarmExtension { Priority = new AttributeConfig { Value = 500 } };
        var handler = new StateAlarmExtensionHandler();

        var result = handler.Apply(instance, source, AlarmedAttribute);

        result.Success.Should().BeFalse();
    }
}
