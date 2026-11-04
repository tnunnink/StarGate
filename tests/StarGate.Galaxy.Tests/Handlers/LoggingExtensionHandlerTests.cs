using FluentAssertions;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestClass]
public class LoggingExtensionHandlerTests : GalaxyFixture
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("  ")]
    public void Extract_NullOrEmptyContext_ShouldReturnNull(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new LoggingExtensionHandler();

        var result = handler.Extract(instance, context);

        result.Should().BeNull();
    }

    [TestMethod]
    public void Extract_PrimitiveWithoutExtension_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        var handler = new LoggingExtensionHandler();

        var result = handler.Extract(instance, "TestString");

        result.Should().BeNull();
    }

    [TestMethod]
    public void Extract_PrimitiveWithExtension_ShouldHaveExpectedValues()
    {
        var instance = GetInstance(TestInstance);

        var config = new LoggingExtensionHandler().Extract(instance, "TestInt");

        config.Should().NotBeNull();
        config!.LogEvent.Should().NotBeNull();
        config.LogEvent!.Value.Should().NotBeNull();
    }

    [TestMethod]
    public void Extract_TemplatePrimitiveWithExtension_ShouldIncludeLockState()
    {
        var template = GetTemplate(TestTemplate);

        var config = new LoggingExtensionHandler().Extract(template, "ParentInt");

        config.Should().NotBeNull();
        config.LogEvent?.Locked.Should().NotBeNull();
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void Apply_NullOrEmptyContext_ShouldReturnSuccess(string? context)
    {
        var instance = GetInstance(TestInstance);

        var result = new LoggingExtensionHandler().Apply(instance, new LoggingExtension(), context);

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new LoggingExtensionHandler();

        var result = handler.Apply(instance, null, "TestInt");

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_ExistingExtension_ShouldUpdateValue()
    {
        var instance = GetInstance(TestInstance);
        var handler = new LoggingExtensionHandler();
        instance.CheckOut();
        var source = new LoggingExtension { LogEvent = new AttributeConfig { Value = false } };

        var result = handler.Apply(instance, source, "TestInt");
        var config = handler.Extract(instance, "TestInt");
        instance.Rollback();

        result.Success.Should().BeTrue();
        config?.LogEvent?.Value.Should().Be(false);
    }

    [TestMethod]
    public void Apply_PrimitiveWithoutExtension_ShouldAddExtension()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();
        var source = new LoggingExtension { LogEvent = new AttributeConfig { Value = true } };

        var result = new LoggingExtensionHandler().Apply(instance, source, "TestDouble");
        var hasExtension = instance.HasExtension(typeof(LoggingExtension), "TestDouble");
        instance.Rollback();

        result.Success.Should().BeTrue();
        hasExtension.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        var instance = GetInstance(TestInstance);
        var source = new LoggingExtension { LogEvent = new AttributeConfig { Value = false } };
        var handler = new LoggingExtensionHandler();

        var result = handler.Apply(instance, source, "TestInt");

        result.Success.Should().BeFalse();
    }
}