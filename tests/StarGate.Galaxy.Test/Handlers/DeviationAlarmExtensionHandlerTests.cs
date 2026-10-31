using FluentAssertions;
using NUnit.Framework;
using StarGate.Core;
using StarGate.Galaxy.Extensions;
using StarGate.Galaxy.Handlers;

namespace StarGate.Galaxy.Test.Handlers;

[TestFixture]
public class DeviationAlarmExtensionHandlerTests : GalaxyTestFixture
{
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void Extract_NullOrEmptyContext_ShouldReturnNull(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new DeviationAlarmExtensionHandler();

        var result = handler.Extract(instance, context);

        result.Should().BeNull();
    }

    [Test]
    public void Extract_PrimitiveWithoutExtension_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        var handler = new DeviationAlarmExtensionHandler();

        var result = handler.Extract(instance, "TestString");

        result.Should().BeNull();
    }

    [Test]
    public void Extract_PrimitiveWithDeviationAlarm_ShouldHaveExpectedValues()
    {
        var instance = GetInstance(TestInstance);

        var config = new DeviationAlarmExtensionHandler().Extract(instance, "TestInt");

        config.Should().NotBeNull();
        config!.Target.Should().NotBeNull();
        config.Deadband.Should().NotBeNull();
        config.SettlingPeriod.Should().NotBeNull();
        config.Major.Should().NotBeNull();
        config.Minor.Should().NotBeNull();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void Apply_NullOrEmptyContext_ShouldReturnSuccess(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new DeviationAlarmExtensionHandler();

        var result = handler.Apply(instance, new DeviationAlarmExtension(), context);

        result.Success.Should().BeTrue();
    }

    [Test]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new DeviationAlarmExtensionHandler();

        var result = handler.Apply(instance, null, "TestInt");

        result.Success.Should().BeTrue();
    }

    [Test]
    public void Apply_ValidSource_ShouldUpdateValues()
    {
        var instance = GetInstance(TestInstance);
        var handler = new DeviationAlarmExtensionHandler();
        instance.CheckOut();
        var source = new DeviationAlarmExtension
        {
            Target = new AttributeConfig { Value = 50f },
            Deadband = new AttributeConfig { Value = 1f },
            Major = new AnalogAlarmExtension { Limit = new AttributeConfig { Value = 20f } }
        };

        var result = handler.Apply(instance, source, "TestInt");
        var config = handler.Extract(instance, "TestInt");
        instance.Rollback();

        result.Success.Should().BeTrue();
        config!.Target!.Value.Should().Be(50f);
        config.Deadband!.Value.Should().Be(1f);
        config.Major!.Limit!.Value.Should().Be(20f);
    }

    [Test]
    public void Apply_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        var instance = GetInstance(TestInstance);
        var source = new DeviationAlarmExtension { Target = new AttributeConfig { Value = 50f } };
        var handler = new DeviationAlarmExtensionHandler();

        var result = handler.Apply(instance, source, "TestInt");

        result.Success.Should().BeFalse();
    }
}