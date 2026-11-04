using FluentAssertions;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestClass]
public class QualityAlarmExtensionHandlerTests : GalaxyFixture
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("  ")]
    public void Extract_NullOrEmptyContext_ShouldReturnNull(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new QualityAlarmExtensionHandler();

        var result = handler.Extract(instance, context);

        result.Should().BeNull();
    }

    [TestMethod]
    public void Extract_PrimitiveWithoutExtension_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        var handler = new QualityAlarmExtensionHandler();

        var result = handler.Extract(instance, "TestString");

        result.Should().BeNull();
    }

    [TestMethod]
    public void Extract_PrimitiveWithExtension_ShouldHaveExpectedValues()
    {
        var instance = GetInstance(TestInstance);

        var config = new QualityAlarmExtensionHandler().Extract(instance, "TestInt");

        config.Should().NotBeNull();
        config!.Message.Should().NotBeNull();
        config.Priority.Should().NotBeNull();
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void Apply_NullOrEmptyContext_ShouldReturnSuccess(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new QualityAlarmExtensionHandler();

        var result = handler.Apply(instance, new QualityAlarmExtension(), context);

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new QualityAlarmExtensionHandler();

        var result = handler.Apply(instance, null, "TestInt");

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_ExistingExtension_ShouldUpdateValues()
    {
        var instance = GetInstance(TestInstance);
        var handler = new QualityAlarmExtensionHandler();
        instance.CheckOut();
        var source = new QualityAlarmExtension { Priority = new AttributeConfig { Value = 123 } };

        var result = handler.Apply(instance, source, "TestInt");
        var config = handler.Extract(instance, "TestInt");
        instance.Rollback();

        result.Success.Should().BeTrue();
        config!.Priority!.Value.Should().Be(123);
    }

    [TestMethod]
    public void Apply_PrimitiveWithoutExtension_ShouldAddExtension()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();
        var source = new QualityAlarmExtension { Priority = new AttributeConfig { Value = 500 } };

        var result = new QualityAlarmExtensionHandler().Apply(instance, source, "TestDouble");
        var hasExtension = instance.HasExtension(typeof(QualityAlarmExtension), "TestDouble");
        instance.Rollback();

        result.Success.Should().BeTrue();
        hasExtension.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        var instance = GetInstance(TestInstance);
        var source = new QualityAlarmExtension { Priority = new AttributeConfig { Value = 500 } };
        var handler = new QualityAlarmExtensionHandler();

        var result = handler.Apply(instance, source, "TestInt");

        result.Success.Should().BeFalse();
    }
}