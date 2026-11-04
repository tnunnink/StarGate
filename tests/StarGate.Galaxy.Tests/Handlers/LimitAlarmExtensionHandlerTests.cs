using FluentAssertions;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestClass]
public class LimitAlarmExtensionHandlerTests : GalaxyFixture
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("  ")]
    public void Extract_NullOrEmptyContext_ShouldReturnNull(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new LimitAlarmExtensionHandler();

        var result = handler.Extract(instance, context);

        result.Should().BeNull();
    }

    [TestMethod]
    public void Extract_PrimitiveWithoutExtension_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        var handler = new LimitAlarmExtensionHandler();

        var result = handler.Extract(instance, "TestString");

        result.Should().BeNull();
    }

    [TestMethod]
    public void Extract_LimitAlarmDisabled_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();
        instance.Attributes["TestInt.LimitAlarmed"].SetValue(false);

        var config = new LimitAlarmExtensionHandler().Extract(instance, "TestInt");
        instance.Rollback();

        config.Should().BeNull();
    }

    [TestMethod]
    public void Extract_PrimitiveWithLimitAlarm_ShouldHaveExpectedValues()
    {
        var instance = GetInstance(TestInstance);

        var config = new LimitAlarmExtensionHandler().Extract(instance, "TestInt");

        config.Should().NotBeNull();
        config!.Deadband.Should().NotBeNull();
        config.Delay.Should().NotBeNull();
        config.HiHi.Should().NotBeNull();
        config.Hi.Should().NotBeNull();
        config.Lo.Should().NotBeNull();
        config.LoLo.Should().NotBeNull();
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void Apply_NullOrEmptyContext_ShouldReturnSuccess(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new LimitAlarmExtensionHandler();

        var result = handler.Apply(instance, new LimitAlarmExtension(), context);

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new LimitAlarmExtensionHandler();

        var result = handler.Apply(instance, null, "TestInt");

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_ValidSource_ShouldUpdateValues()
    {
        var instance = GetInstance(TestInstance);
        var handler = new LimitAlarmExtensionHandler();
        instance.CheckOut();
        var source = new LimitAlarmExtension
        {
            Deadband = new AttributeConfig { Value = 2f },
            Hi = new AnalogAlarmExtension { Limit = new AttributeConfig { Value = 80f } }
        };

        var result = handler.Apply(instance, source, "TestInt");
        var config = handler.Extract(instance, "TestInt");
        instance.Rollback();

        result.Success.Should().BeTrue();
        config!.Deadband!.Value.Should().Be(2f);
        config.Hi!.Limit!.Value.Should().Be(80f);
    }

    [TestMethod]
    public void Apply_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        var instance = GetInstance(TestInstance);
        var source = new LimitAlarmExtension { Deadband = new AttributeConfig { Value = 2f } };
        var handler = new LimitAlarmExtensionHandler();

        var result = handler.Apply(instance, source, "TestInt");

        result.Success.Should().BeFalse();
    }
}