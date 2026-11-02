using FluentAssertions;
using NUnit.Framework;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestFixture]
public class AnalogAlarmExtensionHandlerTests : GalaxyTestFixture
{
    // Assumes TestInt has a limit alarm extension with the HiHi alarm enabled.
    private const string Context = "TestInt.HiHi";

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void Extract_NullOrEmptyContext_ShouldReturnNull(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new AnalogAlarmExtensionHandler();

        var result = handler.Extract(instance, context);

        result.Should().BeNull();
    }

    [Test]
    public void Extract_MissingAlarmedAttribute_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        var handler = new AnalogAlarmExtensionHandler();

        var result = handler.Extract(instance, "TestString.HiHi");

        result.Should().BeNull();
    }

    [Test]
    public void Extract_EnabledAlarm_ShouldHaveExpectedValues()
    {
        var instance = GetInstance(TestInstance);

        var config = new AnalogAlarmExtensionHandler().Extract(instance, Context);

        config.Should().NotBeNull();
        config!.Limit.Should().NotBeNull();
        config.Priority.Should().NotBeNull();
        config.Message.Should().NotBeNull();
    }

    [Test]
    public void Extract_DisabledAlarm_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();
        instance.Attributes[$"{Context}.Alarmed"].SetValue(false);

        var config = new AnalogAlarmExtensionHandler().Extract(instance, Context);
        instance.Rollback();

        config.Should().BeNull();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void Apply_NullOrEmptyContext_ShouldReturnSuccess(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new AnalogAlarmExtensionHandler();

        var result = handler.Apply(instance, new AnalogAlarmExtension(), context);

        result.Success.Should().BeTrue();
    }

    [Test]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new AnalogAlarmExtensionHandler();

        var result = handler.Apply(instance, null, Context);

        result.Success.Should().BeTrue();
    }

    [Test]
    public void Apply_ValidSource_ShouldEnableAlarmAndUpdateValues()
    {
        var instance = GetInstance(TestInstance);
        var handler = new AnalogAlarmExtensionHandler();
        instance.CheckOut();
        instance.Attributes[$"{Context}.Alarmed"].SetValue(false);
        var source = new AnalogAlarmExtension
        {
            Limit = new AttributeConfig { Value = 75f },
            Priority = new AttributeConfig { Value = 400 }
        };

        var result = handler.Apply(instance, source, Context);
        var config = handler.Extract(instance, Context);
        instance.Rollback();

        result.Success.Should().BeTrue();
        config.Should().NotBeNull();
        config!.Limit!.Value.Should().Be(75f);
        config.Priority!.Value.Should().Be(400);
    }

    [Test]
    public void Apply_MissingAlarmedAttribute_ShouldReturnFailure()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();

        var result = new AnalogAlarmExtensionHandler().Apply(instance, new AnalogAlarmExtension(), "TestString.HiHi");
        instance.Rollback();

        result.Success.Should().BeFalse();
    }

    [Test]
    public void Apply_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        var instance = GetInstance(TestInstance);
        var source = new AnalogAlarmExtension { Limit = new AttributeConfig { Value = 75f } };
        var handler = new AnalogAlarmExtensionHandler();

        var result = handler.Apply(instance, source, Context);

        result.Success.Should().BeFalse();
    }
}