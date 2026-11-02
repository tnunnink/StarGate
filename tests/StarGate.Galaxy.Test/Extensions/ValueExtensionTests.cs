using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using StarGate.Galaxy;
using VerifyNUnit;

namespace StarGate.Galaxy.Tests;

[TestFixture]
public class ValueExtensionTests : GalaxyTestFixture
{
    [Test]
    [TestCase("TestBool", typeof(bool), "True")]
    [TestCase("TestInt", typeof(int), "123")]
    [TestCase("TestFloat", typeof(float), "123.4")]
    [TestCase("TestDouble", typeof(double), "1234567.89123")]
    [TestCase("TestString", typeof(string), "This is an initial value")]
    [TestCase("TestTime", typeof(DateTime), "10/9/2026 7:58:20 AM")]
    [TestCase("TestElapsed", typeof(TimeSpan), "11:03:12.3000000")]
    [TestCase("TestInternationalized", typeof(string), "This is the internationalized test string value")]
    [TestCase("TestBigString", typeof(string), "this is a really big string")]
    public void GetValue_UserDefinedAttributes_ShouldReturnExpectedValue(string attribute, Type type, string result)
    {
        var instance = GetInstance(TestInstance);

        var value = instance.Attributes[attribute].GetValue();

        value.Should().BeOfType(type);
        value.ToString().Should().Be(result);
        Console.WriteLine(value.ToString());
    }

    [Test]
    public void GetValue_ArrayAttribute_ShouldHaveExpectedValue()
    {
        var instance = GetInstance(TestInstance);

        var value = instance.Attributes["TestArray"].GetValue();

        value.Should().BeOfType<object[]>();
        value.As<object[]>().Should().HaveCount(10);
        value.As<object[]>().Should().Contain("first");
        value.As<object[]>().Should().Contain("second");
        value.As<object[]>().Should().Contain("third");
        value.As<object[]>().Count(x => string.IsNullOrEmpty(x.ToString())).Should().Be(7);
    }

    [Test]
    [TestCase("ExecutionRelativeOrder", typeof(string), "None")] //qualified enum
    [TestCase("TestInt.InputSource", typeof(string), "Me.TestFloat")] //reference type
    public void GetValue_NonStandardTypes_ShouldReturnExpectedValue(string attribute, Type type, object result)
    {
        var instance = GetInstance(TestInstance);

        var value = instance.Attributes[attribute].GetValue();

        value.Should().BeOfType(type);
        value.Should().Be(result);
    }

    [Test]
    [TestCase("Graphic_001._VisualElementDefinition", typeof(byte[]))]
    public void GetValue_GraphicBinaryData_ShouldReturnData(string attribute, Type type)
    {
        var instance = GetInstance(TestInstance);

        var value = instance.Attributes[attribute].GetValue();

        value.Should().BeOfType(type);
        value.As<byte[]>().Should().NotBeEmpty();
        value.As<byte[]>().ToList().ForEach(b => Console.Write($"{b}"));
    }

    [Test]
    [TestCase("TestBool", false)]
    [TestCase("TestInt", 456)]
    [TestCase("TestFloat", -560.54f)]
    [TestCase("TestDouble", 45267.8492)]
    [TestCase("TestString", "this is an updated value from the galaxy test suite")]
    [TestCase("TestBigString", "this is an updated value from the galaxy test suite")]
    [TestCase("TestInternationalized", "this is an updated value from the galaxy test suite")]
    public void SetValue_UserDefinedAttributes_ShouldUpdateValue(string attribute, object update)
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();

        var target = instance.Attributes[attribute];
        target.SetValue(update);

        target.GetValue().Should().Be(update);

        var result = instance.Rollback();
        Console.WriteLine(result.Message);
    }

    [Test]
    public void SetValue_ArrayAttribute_ShouldUpdateValue()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();

        var target = instance.Attributes["TestArray"];
        target.SetValue(new[] { "third", "first", "Second", "fourth" });
        Console.WriteLine(target.CommandResult.Text);

        var value = target.GetValue();
        value.As<object[]>().Should().HaveCount(4); // since we sent array of 4 items, that becomes the new length
        var result = instance.Rollback();
        Console.WriteLine(result.Message);
    }

    [Test]
    [TestCase("ExecutionRelativeOrder", "Before")]
    public void SetValue_QualifiedEnumeration_ShouldUpdateValue(string attribute, object update)
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();

        var target = instance.Attributes[attribute];
        target.SetValue(update);

        target.GetValue().Should().Be(update);
        instance.Rollback();
    }

    [Test]
    [TestCase("TestInt.InputSource", "---")]
    public void SetValue_ReferenceType_ShouldUpdateValue(string attribute, object update)
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();

        var target = instance.Attributes[attribute];
        target.SetValue(update);

        target.GetValue().Should().Be(update);
        instance.Rollback();
    }
}