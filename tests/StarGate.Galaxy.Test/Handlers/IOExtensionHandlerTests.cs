using System;
using FluentAssertions;
using NUnit.Framework;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestFixture]
public class IOExtensionHandlerTests : GalaxyTestFixture
{
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void Extract_NullOrEmptyContext_ShouldReturnNull(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new IOExtensionHandler();

        var result = handler.Extract(instance, context);

        result.Should().BeNull();
    }

    [Test]
    public void Extract_PrimitiveWithoutExtension_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        var handler = new IOExtensionHandler();

        var result = handler.Extract(instance, "TestString");

        result.Should().BeNull();
    }

    [Test]
    public void Extract_PrimitiveWithExtension_ShouldHaveExpectedValues()
    {
        var instance = GetInstance(TestInstance);

        var config = new IOExtensionHandler().Extract(instance, "TestInt");

        config.Should().NotBeNull();
        config!.Binding.Should().NotBeNull();
        config.InputSource.Should().NotBeNull();
        config.InputSource!.Value.Should().Be("Me.TestFloat");
    }

    [Test]
    public void Extract_PrimitiveWithScaling_ShouldIncludeScaling()
    {
        var instance = GetInstance(TestInstance);

        var config = new IOExtensionHandler().Extract(instance, "TestFloat");

        config.Should().NotBeNull();
        config!.Scaling.Should().NotBeNull();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void Apply_NullOrEmptyContext_ShouldReturnSuccess(string? context)
    {
        var instance = GetInstance(TestInstance);

        var source = new IOExtension { Binding = BindingType.ReadOnly };
        var handler = new IOExtensionHandler();

        var result = handler.Apply(instance, source, context);

        result.Success.Should().BeTrue();
    }

    [Test]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new IOExtensionHandler();

        var result = handler.Apply(instance, null, "TestInt");

        result.Success.Should().BeTrue();
    }

    [Test]
    public void Apply_ExistingExtension_ShouldUpdateInputSource()
    {
        var instance = GetInstance(TestInstance);
        var handler = new IOExtensionHandler();
        instance.CheckOut();

        var source = new IOExtension
        {
            Binding = BindingType.ReadWrite,
            InputSource = new AttributeConfig { Value = "Me.TestDouble" }
        };

        var result = handler.Apply(instance, source, "TestInt");
        var config = handler.Extract(instance, "TestInt");
        instance.Rollback();

        result.Success.Should().BeTrue();
        config!.InputSource!.Value.Should().Be("Me.TestDouble");
    }

    [Test]
    [TestCase(BindingType.ReadOnly, "InputExtension")]
    [TestCase(BindingType.WriteOnly, "OutputExtension")]
    [TestCase(BindingType.ReadWrite, "InputOutputExtension")]
    public void Apply_PrimitiveWithoutExtension_ShouldAddExpectedExtension(BindingType binding, string expected)
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();
        var source = new IOExtension { Binding = binding };

        var result = new IOExtensionHandler().Apply(instance, source, "TestString");
        var extensions = instance.ListExtensions("TestString");
        instance.Rollback();

        result.Success.Should().BeTrue();
        extensions.Should().Contain(e => e.Equals(expected, StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public void Apply_ReflectBinding_ShouldAddInputAndOutputExtensions()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();
        var source = new IOExtension { Binding = BindingType.Reflect };

        var result = new IOExtensionHandler().Apply(instance, source, "TestString");
        var extensions = instance.ListExtensions("TestString");
        instance.Rollback();

        result.Success.Should().BeTrue();
        extensions.Should().Contain(e => e.Equals("InputExtension", StringComparison.OrdinalIgnoreCase));
        extensions.Should().Contain(e => e.Equals("OutputExtension", StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public void Apply_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        var instance = GetInstance(TestInstance);
        var source = new IOExtension { Binding = BindingType.ReadOnly };
        var handler = new IOExtensionHandler();

        var result = handler.Apply(instance, source, "TestString");

        result.Success.Should().BeFalse();
    }
}