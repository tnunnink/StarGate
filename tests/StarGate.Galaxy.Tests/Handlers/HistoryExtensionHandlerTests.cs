using FluentAssertions;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestClass]
public class HistoryExtensionHandlerTests : GalaxyFixture
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("  ")]
    public void Extract_NullOrEmptyContext_ShouldReturnNull(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new HistoryExtensionHandler();

        var result = handler.Extract(instance, context);

        result.Should().BeNull();
    }

    [TestMethod]
    public void Extract_PrimitiveWithoutExtension_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        var handler = new HistoryExtensionHandler();

        var result = handler.Extract(instance, "TestString");

        result.Should().BeNull();
    }

    [TestMethod]
    public void Extract_PrimitiveWithExtension_ShouldHaveExpectedValues()
    {
        var instance = GetInstance(TestInstance);

        var config = new HistoryExtensionHandler().Extract(instance, "TestInt");

        config.Should().NotBeNull();
        config!.Description.Should().NotBeNull();
        config.StoragePeriod.Should().NotBeNull();
        config.Deadband.Should().NotBeNull();
        config.TrendHi.Should().NotBeNull();
        config.TrendLo.Should().NotBeNull();
        config.SwingDoorEnabled.Should().NotBeNull();
        config.InterpolationType.Should().NotBeNull();
        config.Rollover.Should().NotBeNull();
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void Apply_NullOrEmptyContext_ShouldReturnSuccess(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new HistoryExtensionHandler();

        var result = handler.Apply(instance, new HistoryExtension(), context);

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new HistoryExtensionHandler();

        var result = handler.Apply(instance, null, "TestInt");

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_ExistingExtension_ShouldUpdateValues()
    {
        var instance = GetInstance(TestInstance);
        var handler = new HistoryExtensionHandler();
        instance.CheckOut();
        var source = new HistoryExtension
        {
            TrendHi = new AttributeConfig { Value = 250f },
            TrendLo = new AttributeConfig { Value = -25f },
            SwingDoorEnabled = new AttributeConfig { Value = true }
        };

        var result = handler.Apply(instance, source, "TestInt");
        var config = handler.Extract(instance, "TestInt");
        instance.Rollback();

        result.Success.Should().BeTrue();
        config!.TrendHi!.Value.Should().Be(250f);
        config.TrendLo!.Value.Should().Be(-25f);
        config.SwingDoorEnabled!.Value.Should().Be(true);
    }

    [TestMethod]
    public void Apply_PrimitiveWithoutExtension_ShouldAddExtension()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();
        var source = new HistoryExtension { TrendHi = new AttributeConfig { Value = 100f } };

        var result = new HistoryExtensionHandler().Apply(instance, source, "TestString");
        var hasExtension = instance.HasExtension(typeof(HistoryExtension), "TestString");
        instance.Rollback();

        result.Success.Should().BeTrue();
        hasExtension.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        var instance = GetInstance(TestInstance);
        var source = new HistoryExtension { TrendHi = new AttributeConfig { Value = 100f } };
        var handler = new HistoryExtensionHandler();

        var result = handler.Apply(instance, source, "TestInt");

        result.Success.Should().BeFalse();
    }
}