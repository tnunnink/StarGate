using System;
using ArchestrA.GRAccess;
using FluentAssertions;
using NUnit.Framework;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestFixture]
public class AttributeExtensionTests : GalaxyTestFixture
{
    [Test]
    public void InheritsLock_AttributeNotLockedInParent_ShouldBeFalse()
    {
        var instance = GetInstance(TestInstance);

        var attribute = instance.Attributes["TestInt"];

        attribute.Locked.Should().NotBe(MxPropertyLockedEnum.MxLockedInParent);
        attribute.InheritsLock.Should().BeFalse();
    }

    [Test]
    public void InheritsLock_AttributeLockedInTemplate_ShouldBeTrueOnDerivedInstance()
    {
        var template = GetTemplate(TestTemplate);
        template.CheckOut();
        template.Attributes["ParentInt"].SetLocked(true);

        var instance = GetInstance(TestInstance);
        var result = instance.Attributes["ParentBool"].InheritsLock;

        template.Rollback();
        result.Should().BeTrue();
    }

    [Test]
    [TestCase("TestBool", typeof(bool))]
    [TestCase("TestInt", typeof(int))]
    [TestCase("TestString", typeof(string))]
    public void GetValue_UserDefinedAttribute_ShouldReturnValueOfExpectedType(string name, Type type)
    {
        var instance = GetInstance(TestInstance);

        var value = instance.Attributes[name].GetValue();

        value.Should().BeOfType(type);
    }

    [Test]
    public void GetLength_ArrayAttribute_ShouldReturnArrayLength()
    {
        var instance = GetInstance(TestInstance);

        var length = instance.Attributes["TestArray"].GetLength();

        length.Should().Be(10);
    }

    [Test]
    [TestCase("TestInt")]
    [TestCase("TestString")]
    public void GetLength_NonArrayAttribute_ShouldBeNull(string name)
    {
        var instance = GetInstance(TestInstance);

        var length = instance.Attributes[name].GetLength();

        length.Should().BeNull();
    }

    [Test]
    public void GetWriteability_UserWriteableLockableAttribute_ShouldBeNull()
    {
        var instance = GetInstance(TestInstance);
        var attribute = instance.Attributes["TestInt"];

        attribute.AttributeCategory.Should().Be(MxAttributeCategory.MxCategoryWriteable_USC_Lockable);
        attribute.GetWriteability().Should().BeNull();
    }

    [Test]
    public void GetWriteability_CalculatedAttribute_ShouldReturnCalculated()
    {
        var instance = GetInstance(TestInstance);
        var attribute = instance.Attributes["TestCalculated"];

        attribute.GetWriteability().Should().Be(Writeability.Calculated);
    }

    [Test]
    public void SetValue_NullValue_ShouldReturnSuccessAndNotChangeValue()
    {
        var instance = GetInstance(TestInstance);
        var attribute = instance.Attributes["TestInt"];
        var before = attribute.GetValue();

        var result = attribute.SetValue((object?)null);

        result.Success.Should().BeTrue();
        attribute.GetValue().Should().Be(before);
    }

    [Test]
    public void SetValue_ValidValueOnCheckedOutObject_ShouldReturnSuccessAndUpdateValue()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();

        var attribute = instance.Attributes["TestInt"];
        var result = attribute.SetValue(999);

        var value = attribute.GetValue();
        instance.Rollback();
        result.Success.Should().BeTrue();
        value.Should().Be(999);
    }

    [Test]
    public void SetValue_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        var instance = GetInstance(TestInstance);

        var result = instance.Attributes["TestInt"].SetValue(999);

        result.Success.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Test]
    public void SetLocked_NullValue_ShouldReturnSuccessAndNotChangeLock()
    {
        var template = GetTemplate(TestTemplate);
        var attribute = template.Attributes["ParentInt"];
        var before = attribute.Locked;

        var result = attribute.SetLocked(null);

        result.Success.Should().BeTrue();
        attribute.Locked.Should().Be(before);
    }

    [Test]
    [TestCase(true, MxPropertyLockedEnum.MxLockedInMe)]
    [TestCase(false, MxPropertyLockedEnum.MxUnLocked)]
    public void SetLocked_TemplateAttribute_ShouldUpdateLockState(bool locked, MxPropertyLockedEnum expected)
    {
        var template = GetTemplate(TestTemplate);
        template.CheckOut();

        var attribute = template.Attributes["ParentInt"];
        var result = attribute.SetLocked(locked);

        var state = attribute.Locked;
        template.Rollback();
        result.Success.Should().BeTrue();
        state.Should().Be(expected);
    }

    [Test]
    public void SetSecurity_NullValue_ShouldReturnSuccessAndNotChangeSecurity()
    {
        var template = GetTemplate(TestTemplate);
        var attribute = template.Attributes["ParentInt"];
        var before = attribute.SecurityClassification;

        var result = attribute.SetSecurity(null);

        result.Success.Should().BeTrue();
        attribute.SecurityClassification.Should().Be(before);
    }

    [Test]
    [TestCase(Security.FreeAccess, MxSecurityClassification.MxSecurityFreeAccess)]
    [TestCase(Security.Tune, MxSecurityClassification.MxSecurityTune)]
    [TestCase(Security.Configure, MxSecurityClassification.MxSecurityConfigure)]
    public void SetSecurity_TemplateAttribute_ShouldUpdateSecurityClassification(Security security,
        MxSecurityClassification expected)
    {
        var template = GetTemplate(TestTemplate);
        template.CheckOut();

        var attribute = template.Attributes["ParentInt"];
        var result = attribute.SetSecurity(security);

        var classification = attribute.SecurityClassification;
        template.Rollback();
        result.Success.Should().BeTrue();
        classification.Should().Be(expected);
    }
}