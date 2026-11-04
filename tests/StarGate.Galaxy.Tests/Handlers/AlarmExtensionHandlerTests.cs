using FluentAssertions;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestClass]
public class AlarmExtensionHandlerTests : GalaxyFixture
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("  ")]
    public void Extract_NullOrEmptyContext_ShouldReturnNull(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new AlarmExtensionHandler();

        var result = handler.Extract(instance, context);

        result.Should().BeNull();
    }

    [TestMethod]
    public void Extract_PrimitiveWithLimitAlarm_ShouldHaveLimit()
    {
        var instance = GetInstance(TestInstance);

        var config = new AlarmExtensionHandler().Extract(instance, "TestInt");

        config.Should().NotBeNull();
        config!.Limit.Should().NotBeNull();
    }

    [TestMethod]
    public void Extract_PrimitiveWithoutAlarms_ShouldReturnContainerWithNoAlarms()
    {
        var instance = GetInstance(TestInstance);

        var config = new AlarmExtensionHandler().Extract(instance, "TestString");

        config.Should().NotBeNull();
        config!.State.Should().BeNull();
        config.Limit.Should().BeNull();
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void Apply_NullOrEmptyContext_ShouldReturnSuccess(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new AlarmExtensionHandler();

        var result = handler.Apply(instance, new AlarmExtension(), context);

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new AlarmExtensionHandler();

        var result = handler.Apply(instance, null, "TestInt");

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_EmptyAlarmExtension_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new AlarmExtensionHandler();

        var result = handler.Apply(instance, new AlarmExtension(), "TestInt");

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_LimitAlarm_ShouldUpdateLimitValues()
    {
        var instance = GetInstance(TestInstance);
        var handler = new AlarmExtensionHandler();
        instance.CheckOut();
        var source = new AlarmExtension
        {
            Limit = new LimitAlarmExtension { Deadband = new AttributeConfig { Value = 3f } }
        };

        var result = handler.Apply(instance, source, "TestInt");
        var config = handler.Extract(instance, "TestInt");
        instance.Rollback();

        result.Success.Should().BeTrue();
        config!.Limit!.Deadband!.Value.Should().Be(3f);
    }

    [TestMethod]
    public void Apply_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        var instance = GetInstance(TestInstance);
        var source = new AlarmExtension
        {
            Limit = new LimitAlarmExtension { Deadband = new AttributeConfig { Value = 3f } }
        };
        var handler = new AlarmExtensionHandler();

        var result = handler.Apply(instance, source, "TestInt");

        result.Success.Should().BeFalse();
    }
}