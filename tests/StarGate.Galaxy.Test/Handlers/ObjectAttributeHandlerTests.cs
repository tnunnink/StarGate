using FluentAssertions;
using NUnit.Framework;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestFixture]
public class ObjectAttributeHandlerTests : GalaxyTestFixture
{
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void Extract_NullOrEmptyContext_ShouldReturnNull(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new ObjectAttributeHandler();

        var result = handler.Extract(instance, context);

        result.Should().BeNull();
    }

    [Test]
    public void Extract_MissingAttribute_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        var handler = new ObjectAttributeHandler();

        var result = handler.Extract(instance, "DoesNotExist");

        result.Should().BeNull();
    }

    [Test]
    [TestCase("TestBool", DataType.Boolean)]
    [TestCase("TestInt", DataType.Integer)]
    [TestCase("TestFloat", DataType.Float)]
    [TestCase("TestDouble", DataType.Double)]
    [TestCase("TestString", DataType.String)]
    [TestCase("TestTime", DataType.Time)]
    [TestCase("TestElapsed", DataType.ElapsedTime)]
    [TestCase("TestInternationalized", DataType.InternationalizedString)]
    [TestCase("TestBigString", DataType.BigString)]
    public void Extract_UserDefinedAttribute_ShouldHaveExpectedNameAndType(string name, DataType type)
    {
        var instance = GetInstance(TestInstance);

        var attribute = new ObjectAttributeHandler().Extract(instance, name);

        attribute.Should().NotBeNull();
        attribute.Name.Should().Be(name);
        attribute.DataType.Should().Be(type);
        attribute.Value.Should().NotBeNull();
    }

    [Test]
    public void Extract_ArrayAttribute_ShouldHaveLength()
    {
        var instance = GetInstance(TestInstance);

        var attribute = new ObjectAttributeHandler().Extract(instance, "TestArray");

        attribute.Should().NotBeNull();
        attribute!.Length.Should().Be(10);
    }

    [Test]
    public void Extract_NonArrayAttribute_ShouldNotHaveLength()
    {
        var instance = GetInstance(TestInstance);

        var attribute = new ObjectAttributeHandler().Extract(instance, "TestInt");

        attribute!.Length.Should().BeNull();
    }

    [Test]
    public void Extract_AttributeWithExtensions_ShouldIncludeExtensionConfigs()
    {
        var instance = GetInstance(TestInstance);

        var attribute = new ObjectAttributeHandler().Extract(instance, "TestInt");

        attribute!.IO.Should().NotBeNull();
        attribute.History.Should().NotBeNull();
        attribute.Log.Should().NotBeNull();
        attribute.Alarms.Should().NotBeNull();
    }

    [Test]
    public void Extract_AttributeWithoutExtensions_ShouldNotIncludeExtensionConfigs()
    {
        var instance = GetInstance(TestInstance);

        var attribute = new ObjectAttributeHandler().Extract(instance, "TestString");

        attribute!.IO.Should().BeNull();
        attribute.History.Should().BeNull();
        attribute.Log.Should().BeNull();
    }

    [Test]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new ObjectAttributeHandler();

        var result = handler.Apply(instance, null);

        result.Success.Should().BeTrue();
    }

    [Test]
    public void Apply_NewAttribute_ShouldCreateAttributeWithValue()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        var source = new ObjectAttribute
        {
            Name = "NewAttribute",
            DataType = DataType.Integer,
            Value = new AttributeConfig { Value = 42 }
        };

        var result = new ObjectAttributeHandler().Apply(obj, source);

        result.Success.Should().BeTrue();
        obj.TryGetAttribute("NewAttribute", out var created).Should().BeTrue();
        created.GetValue().Should().Be(42);
    }

    [Test]
    public void Apply_NewAttributeWithLogging_ShouldAddLoggingExtension()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        var source = new ObjectAttribute
        {
            Name = "LoggedAttribute",
            DataType = DataType.Integer,
            Log = new LoggingExtension { LogEvent = new AttributeConfig { Value = true } }
        };

        var result = new ObjectAttributeHandler().Apply(obj, source);

        result.Success.Should().BeTrue();
        obj.HasExtension(typeof(LoggingExtension), "LoggedAttribute").Should().BeTrue();
    }

    [Test]
    public void Apply_NewAttributeWithoutDataType_ShouldFail()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        var source = new ObjectAttribute { Name = "NoType" };

        var result = new ObjectAttributeHandler().Apply(obj, source);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ErrorCode.MissingDataType);
    }

    [Test]
    public void Apply_ExistingAttribute_ShouldMergeWithCurrentConfiguration()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        var handler = new ObjectAttributeHandler();
        handler.Apply(obj, new ObjectAttribute { Name = "Merged", DataType = DataType.Integer, Length = null });

        // Only supply a description; type should be retained from the existing attribute.
        var result = handler.Apply(obj, new ObjectAttribute
        {
            Name = "Merged",
            Description = new AttributeConfig { Value = "updated" }
        });

        result.Success.Should().BeTrue();
        var attribute = handler.Extract(obj, "Merged");
        attribute!.DataType.Should().Be(DataType.Integer);
        attribute.Description!.Value.Should().Be("updated");
    }

    [Test]
    public void Apply_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var source = new ObjectAttribute { Name = "NotCheckedOut", DataType = DataType.Integer };

        var result = new ObjectAttributeHandler().Apply(temp.Instance, source);

        result.Success.Should().BeFalse();
    }
}