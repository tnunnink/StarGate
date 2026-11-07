using FluentAssertions;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestClass]
public class ObjectAttributeHandlerTests : GalaxyFixture
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("  ")]
    public void Extract_NullOrEmptyContext_ShouldReturnNull(string? context)
    {
        var instance = GetInstance(TestInstance);
        var handler = new ObjectAttributeHandler();

        var result = handler.Extract(instance, context);

        result.Should().BeNull();
    }

    [TestMethod]
    public void Extract_MissingAttribute_ShouldReturnNull()
    {
        var instance = GetInstance(TestInstance);
        var handler = new ObjectAttributeHandler();

        var result = handler.Extract(instance, "DoesNotExist");

        result.Should().BeNull();
    }

    [TestMethod]
    [DataRow("TestBool", AttributeDataType.Boolean)]
    [DataRow("TestInt", AttributeDataType.Integer)]
    [DataRow("TestFloat", AttributeDataType.Float)]
    [DataRow("TestDouble", AttributeDataType.Double)]
    [DataRow("TestString", AttributeDataType.String)]
    [DataRow("TestTime", AttributeDataType.Time)]
    [DataRow("TestElapsed", AttributeDataType.ElapsedTime)]
    [DataRow("TestInternationalized", AttributeDataType.InternationalizedString)]
    [DataRow("TestBigString", AttributeDataType.BigString)]
    public void Extract_UserDefinedAttribute_ShouldHaveExpectedNameAndType(string name, AttributeDataType type)
    {
        var instance = GetInstance(TestInstance);

        var attribute = new ObjectAttributeHandler().Extract(instance, name);

        attribute.Should().NotBeNull();
        attribute.Name.Should().Be(name);
        attribute.DataType.Should().Be(type);
        attribute.Value.Should().NotBeNull();
    }

    [TestMethod]
    public void Extract_ArrayAttribute_ShouldHaveLength()
    {
        var instance = GetInstance(TestInstance);

        var attribute = new ObjectAttributeHandler().Extract(instance, "TestArray");

        attribute.Should().NotBeNull();
        attribute!.Length.Should().Be(10);
    }

    [TestMethod]
    public void Extract_NonArrayAttribute_ShouldNotHaveLength()
    {
        var instance = GetInstance(TestInstance);

        var attribute = new ObjectAttributeHandler().Extract(instance, "TestInt");

        attribute!.Length.Should().BeNull();
    }

    [TestMethod]
    public void Extract_AttributeWithExtensions_ShouldIncludeExtensionConfigs()
    {
        var instance = GetInstance(TestInstance);

        var attribute = new ObjectAttributeHandler().Extract(instance, "TestInt");

        attribute!.IO.Should().NotBeNull();
        attribute.History.Should().NotBeNull();
        attribute.Log.Should().NotBeNull();
        attribute.Alarms.Should().NotBeNull();
    }

    [TestMethod]
    public void Extract_AttributeWithoutExtensions_ShouldNotIncludeExtensionConfigs()
    {
        var instance = GetInstance(TestInstance);

        var attribute = new ObjectAttributeHandler().Extract(instance, "TestString");

        attribute!.IO.Should().BeNull();
        attribute.History.Should().BeNull();
        attribute.Log.Should().BeNull();
    }

    [TestMethod]
    public void Apply_NullSource_ShouldReturnSuccess()
    {
        var instance = GetInstance(TestInstance);
        var handler = new ObjectAttributeHandler();

        var result = handler.Apply(instance, null);

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Apply_NewAttribute_ShouldCreateAttributeWithValue()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        var source = new GalaxyAttribute
        {
            Name = "NewAttribute",
            DataType = AttributeDataType.Integer,
            Value = new AttributeConfig { Value = 42 }
        };

        var result = new ObjectAttributeHandler().Apply(obj, source);

        result.Success.Should().BeTrue();
        obj.TryGetAttribute("NewAttribute", out var created).Should().BeTrue();
        created.GetValue().Should().Be(42);
    }

    [TestMethod]
    public void Apply_NewAttributeWithLogging_ShouldAddLoggingExtension()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        var source = new GalaxyAttribute
        {
            Name = "LoggedAttribute",
            DataType = AttributeDataType.Integer,
            Log = new LoggingExtension { LogEvent = new AttributeConfig { Value = true } }
        };

        var result = new ObjectAttributeHandler().Apply(obj, source);

        result.Success.Should().BeTrue();
        obj.HasExtension(typeof(LoggingExtension), "LoggedAttribute").Should().BeTrue();
    }

    [TestMethod]
    public void Apply_NewAttributeWithoutDataType_ShouldFail()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        var source = new GalaxyAttribute { Name = "NoType" };

        var result = new ObjectAttributeHandler().Apply(obj, source);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ErrorCode.MissingDataType);
    }

    [TestMethod]
    public void Apply_ExistingAttribute_ShouldMergeWithCurrentConfiguration()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        var handler = new ObjectAttributeHandler();
        handler.Apply(obj, new GalaxyAttribute { Name = "Merged", DataType = AttributeDataType.Integer, Length = null });

        // Only supply a description; type should be retained from the existing attribute.
        var result = handler.Apply(obj, new GalaxyAttribute
        {
            Name = "Merged",
            Description = new AttributeConfig { Value = "updated" }
        });

        result.Success.Should().BeTrue();
        var attribute = handler.Extract(obj, "Merged");
        attribute!.DataType.Should().Be(AttributeDataType.Integer);
        attribute.Description!.Value.Should().Be("updated");
    }

    [TestMethod]
    public void Apply_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var source = new GalaxyAttribute { Name = "NotCheckedOut", DataType = AttributeDataType.Integer };

        var result = new ObjectAttributeHandler().Apply(temp.Instance, source);

        result.Success.Should().BeFalse();
    }
}