using FluentAssertions;
using NUnit.Framework;
using StarGate.Core.Extensions;
using StarGate.Core.Objects;
using StarGate.Galaxy.Extensions;
using StarGate.Galaxy.Handlers;

namespace StarGate.Galaxy.Test.Handlers;

[TestFixture]
public class ScalingExtensionHandlerTests : GalaxyTestFixture
{
    [Test]
    public void Extract_PrimitiveWithScalingEnabled_ShouldHaveExpectedValues()
    {
        var instance = GetInstance(TestInstance);
        var handler = new ScalingExtensionHandler();

        var config = handler.Extract(instance, "TestFloat");

        config?.RawMax.Should().NotBeNull();
        config?.RawMin.Should().NotBeNull();
        config?.EngMax.Should().NotBeNull();
        config?.EngMin.Should().NotBeNull();
        config?.RangeMax.Should().NotBeNull();
        config?.RangeMin.Should().NotBeNull();
        config?.ClampEnabled.Should().NotBeNull();
        config?.ConversionMode.Should().NotBeNull();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void Extract_NullOrEmptyContext_ShouldReturnNull(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new ScalingExtensionHandler();

        handler.Extract(instance, context).Should().BeNull();
    }

    [Test]
    public void Extract_PrimitiveWithoutScalingExtension_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        var handler = new ScalingExtensionHandler();

        handler.Extract(instance, "TestString").Should().BeNull();
    }

    [Test]
    public void Extract_PrimitiveWithScalingEnabled_ShouldNotIncludeLockOrSecurityForInstance()
    {
        var instance = GetInstance(TestInstance);
        var handler = new ScalingExtensionHandler();

        var config = handler.Extract(instance, "TestFloat");

        config.Should().NotBeNull();
        config!.RawMax!.Locked.Should().BeNull();
        config.RawMax.Security.Should().BeNull();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void Apply_NullOrEmptyContext_ShouldReturnSuccess(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new ScalingExtensionHandler();

        var result = handler.Apply(instance, new ScalingExtension(), context);

        result.Success.Should().BeTrue();
    }

    [Test]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new ScalingExtensionHandler();

        var result = handler.Apply(instance, null, "TestFloat");

        result.Success.Should().BeTrue();
    }

    [Test]
    public void Apply_ExistingScalingExtension_ShouldUpdateValues()
    {
        var instance = GetInstance(TestInstance);
        var handler = new ScalingExtensionHandler();
        instance.CheckOut();
        var source = new ScalingExtension
        {
            RawMax = new AttributeConfig { Value = 1000f },
            RawMin = new AttributeConfig { Value = 10f },
            EngMax = new AttributeConfig { Value = 500f },
            EngMin = new AttributeConfig { Value = 5f }
        };

        var result = handler.Apply(instance, source, "TestFloat");
        var config = handler.Extract(instance, "TestFloat");
        instance.Rollback();

        result.Success.Should().BeTrue();
        config!.RawMax!.Value.Should().Be(1000f);
        config.RawMin!.Value.Should().Be(10f);
        config.EngMax!.Value.Should().Be(500f);
        config.EngMin!.Value.Should().Be(5f);
    }

    [Test]
    public void Apply_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        var instance = GetInstance(TestInstance);
        var handler = new ScalingExtensionHandler();
        var source = new ScalingExtension { RawMax = new AttributeConfig { Value = 1000f } };

        var result = handler.Apply(instance, source, "TestFloat");

        result.Success.Should().BeFalse();
    }

    [Test]
    public void Apply_PrimitiveWithoutExtension_ShouldAddExtension()
    {
        var instance = GetInstance(TestInstance);
        var handler = new ScalingExtensionHandler();
        instance.CheckOut();
        var source = new ScalingExtension { RawMax = new AttributeConfig { Value = 100f } };

        var result = handler.Apply(instance, source, "TestString");
        var hasExtension = instance.HasExtension(typeof(ScalingExtension), "TestString");
        instance.Rollback();

        result.Success.Should().BeTrue();
        hasExtension.Should().BeTrue();
    }
}