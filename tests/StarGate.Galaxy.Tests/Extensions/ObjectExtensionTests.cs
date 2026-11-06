using ArchestrA.GRAccess;
using FluentAssertions;
using StarGate.Core;

// ReSharper disable SuspiciousTypeConversion.Global

namespace StarGate.Galaxy.Tests;

[TestClass]
public class ObjectExtensionTests : GalaxyFixture
{
    [TestMethod]
    public void IsTemplate_WhenCalledOnTemplate_ShouldBeTrue()
    {
        var obj = GetTemplate(TestTemplate);

        var result = obj.IsTemplate;

        obj.Should().NotBeNull();
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsTemplate_WhenCalledOnInstance_ShouldBeFalse()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.IsTemplate;

        obj.Should().NotBeNull();
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsInstance_WhenCalledOnInstance_ShouldBeTrue()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.IsInstance;

        obj.Should().NotBeNull();
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsInstance_WhenCalledOnTemplate_ShouldBeFalse()
    {
        var obj = GetTemplate(TestTemplate);

        var result = obj.IsInstance;

        obj.Should().NotBeNull();
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Execute_ActionSucceeds_ShouldReturnSuccess()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.Execute(x => x.CheckOut());

        obj.Rollback();
        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Execute_ActionFails_ShouldReturnFailureWithErrors()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.Execute(x => x.Save()); // not checked out

        result.Success.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [TestMethod]
    public void Execute_ValidAction_ShouldInvokeAction()
    {
        var obj = GetInstance(TestInstance);
        var invoked = false;

        obj.Execute(_ => invoked = true);

        invoked.Should().BeTrue();
    }

    [TestMethod]
    public void Commit_CheckedOutObjectWithChange_ShouldSaveAndCheckIn()
    {
        using var temp = new GalaxyTestObject(Galaxy);

        var obj = temp.Instance;
        obj.CheckOut();
        obj.AddUDA("CommitTest",
            MxDataType.MxInteger,
            MxAttributeCategory.MxCategoryWriteable_USC_Lockable,
            MxSecurityClassification.MxSecurityOperate,
            false,
            0
        );

        var result = obj.Commit("test commit");

        result.Success.Should().BeTrue();
        obj.CheckoutStatus.Should().Be(ECheckoutStatus.notCheckedOut);
    }

    [TestMethod]
    public void Commit_ObjectNotCheckedOut_ShouldFail()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.Commit("should fail");

        result.Success.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [TestMethod]
    public void Rollback_CheckedOutObjectWithChange_ShouldRevertChangeAndUndoCheckOut()
    {
        var obj = GetInstance(TestInstance);
        var original = obj.Attributes["TestInt"].GetValue();
        obj.CheckOut();
        obj.Attributes["TestInt"].SetValue(999);

        var result = obj.Rollback();

        result.Success.Should().BeTrue();
        obj.CheckoutStatus.Should().Be(ECheckoutStatus.notCheckedOut);
        GetInstance(TestInstance).Attributes["TestInt"].GetValue().Should().Be(original);
    }

    [TestMethod]
    public void Rollback_ObjectNotCheckedOut_ShouldStillAttemptAllStepsAndReportFailure()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.Rollback();

        result.Success.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [TestMethod]
    public void TryGetAttribute_ExistingAttribute_ShouldReturnTrueAndAttribute()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.TryGetAttribute("TestInt", out var attribute);

        result.Should().BeTrue();
        attribute.Should().NotBeNull();
        attribute.Name.Should().Be("TestInt");
    }

    [TestMethod]
    public void TryGetAttribute_MissingAttribute_ShouldReturnFalse()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.TryGetAttribute("DoesNotExist", out var attribute);

        result.Should().BeFalse();
        attribute.Should().BeNull();
    }

    [TestMethod]
    public void TryGetAttribute_NullName_ShouldReturnFalse()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.TryGetAttribute(null, out var attribute);

        result.Should().BeFalse();
        attribute.Should().BeNull();
    }

    [TestMethod]
    public void GetConfig_ExistingExtensionAttributeOnInstance_ShouldReturnValueOnly()
    {
        var obj = GetInstance(TestInstance);

        var config = obj.GetConfig<LoggingExtension>(x => x.LogEvent, "TestInt");

        config.Should().NotBeNull();
        config.Value.Should().NotBeNull();
        config.Locked.Should().BeNull();
        config.Security.Should().BeNull();
    }

    [TestMethod]
    public void GetConfig_ExistingExtensionAttributeOnTemplate_ShouldReturnLockedState()
    {
        var obj = GetTemplate(TestTemplate);

        var config = obj.GetConfig<LoggingExtension>(x => x.LogEvent, "ParentInt");

        config.Should().NotBeNull();
        config.Locked.Should().NotBeNull();
    }

    [TestMethod]
    public void GetConfig_MissingAttribute_ShouldReturnNull()
    {
        var obj = GetInstance(TestInstance);

        var config = obj.GetConfig<LoggingExtension>(x => x.LogEvent, "DoesNotExist");

        config.Should().BeNull();
    }

    [TestMethod]
    public void GetConfig_AttributeLockedInParent_ShouldReturnNull()
    {
        var template = GetTemplate(TestTemplate);

        var instance = GetInstance(TestInstance);
        var config = instance.GetConfig<LoggingExtension>(x => x.LogEvent, "ParentInt");

        template.Rollback();
        config.Should().BeNull();
    }

    [TestMethod]
    public void GetConfig_SelectorIsNotMemberExpression_ShouldThrowArgumentException()
    {
        var obj = GetInstance(TestInstance);

        var action = () => obj.GetConfig<LoggingExtension>(_ => null, "TestInt");

        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Configure_InstanceAttribute_ShouldSetValue()
    {
        var obj = GetInstance(TestInstance);
        obj.CheckOut();
        var extension = new LoggingExtension { LogEvent = new AttributeConfig { Value = false } };

        var result = obj.Configure(extension, x => x.LogEvent, "TestInt");

        var value = obj.Attributes["TestInt.LogDataChangeEvent"].GetValue();
        obj.Rollback();
        result.Success.Should().BeTrue();
        value.Should().Be(false);
    }

    [TestMethod]
    public void Configure_TemplateAttribute_ShouldSetValueLockAndSecurity()
    {
        var obj = GetTemplate(TestTemplate);
        obj.CheckOut();
        var extension = new LoggingExtension
        {
            LogEvent = new AttributeConfig { Value = false, Locked = true, Security = Security.Configure }
        };

        var result = obj.Configure(extension, x => x.LogEvent, "ParentInt");

        var attribute = obj.Attributes["ParentInt.LogDataChangeEvent"];
        var value = attribute.GetValue();
        var locked = attribute.Locked;
        var security = attribute.SecurityClassification;
        obj.Rollback();
        result.Success.Should().BeTrue();
        value.Should().Be(false);
        locked.Should().Be(MxPropertyLockedEnum.MxLockedInMe);
        security.Should().Be(MxSecurityClassification.MxSecurityConfigure);
    }

    [TestMethod]
    public void Configure_MissingAttribute_ShouldReturnSuccessAndDoNothing()
    {
        var obj = GetInstance(TestInstance);
        var extension = new LoggingExtension { LogEvent = new AttributeConfig { Value = true } };

        var result = obj.Configure(extension, x => x.LogEvent, "DoesNotExist");

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Configure_NullConfig_ShouldReturnSuccess()
    {
        var obj = GetInstance(TestInstance);
        obj.CheckOut();
        var extension = new LoggingExtension { LogEvent = null };

        var result = obj.Configure(extension, x => x.LogEvent, "TestInt");

        obj.Rollback();
        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Configure_ObjectNotCheckedOut_ShouldReturnFailure()
    {
        var obj = GetInstance(TestInstance);
        var extension = new LoggingExtension { LogEvent = new AttributeConfig { Value = false } };

        var result = obj.Configure(extension, x => x.LogEvent, "TestInt");

        result.Success.Should().BeFalse();
    }

    [TestMethod]
    public void Configure_SelectorIsNotMemberExpression_ShouldThrowArgumentException()
    {
        var obj = GetInstance(TestInstance);

        var action = () => obj.Configure(new LoggingExtension(), _ => null, "TestInt");

        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void ListUda_InstanceWithUserDefinedAttributes_ShouldContainConfiguredNames()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.ListUda().ToList();

        result.Should().Contain(["TestBool", "TestInt", "TestFloat", "TestString", "TestArray"]);
    }

    [TestMethod]
    public void ListUda_InstanceWithoutUserDefinedAttributes_ShouldBeEmpty()
    {
        var obj = GetInstance(EmptyInstance);

        var result = obj.ListUda();

        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ListUda_InheritedAttributes_ShouldNotBeIncluded()
    {
        var template = GetTemplate(TestTemplate);
        var instance = GetInstance(TestInstance);

        var templateUdas = template.ListUda().ToList();
        var instanceUdas = instance.ListUda().ToList();

        // Anything declared on the template is inherited by the instance and so should not be listed on it.
        instanceUdas.Should().NotContain(templateUdas);
    }

    [TestMethod]
    public void ListExtensions_PrimitiveWithExtensions_ShouldReturnExtensionTypes()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.ListExtensions("TestInt").ToList();

        result.Should().NotBeEmpty();
        result.Should().Contain("logdatachangeeventextension");
    }

    [TestMethod]
    public void ListExtensions_PrimitiveWithoutExtensions_ShouldBeEmpty()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.ListExtensions("TestString");

        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ListExtensions_UnknownPrimitive_ShouldBeEmpty()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.ListExtensions("DoesNotExist");

        result.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(typeof(LoggingExtension), "TestInt")]
    [DataRow(typeof(IOExtension), "TestInt")]
    [DataRow(typeof(HistoryExtension), "TestInt")]
    [DataRow(typeof(LimitAlarmExtension), "TestInt")]
    [DataRow(typeof(RocAlarmExtension), "TestInt")]
    [DataRow(typeof(DeviationAlarmExtension), "TestInt")]
    [DataRow(typeof(QualityAlarmExtension), "TestInt")]
    public void HasExtension_HasConfiguredExtension_ShouldReturnTrue(Type type, string attribute)
    {
        var obj = GetInstance(TestInstance);

        var result = obj.HasExtension(type, attribute);

        result.Should().BeTrue();
    }

    [TestMethod]
    [DataRow(typeof(LoggingExtension), "TestString")]
    [DataRow(typeof(HistoryExtension), "DoesNotExist")]
    public void HasExtension_DoesNotHaveExtension_ShouldReturnFalse(Type type, string attribute)
    {
        var obj = GetInstance(TestInstance);

        var result = obj.HasExtension(type, attribute);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasExtension_TypeWithoutExtensionAttribute_ShouldThrowInvalidOperation()
    {
        var obj = GetInstance(TestInstance);

        var action = () => obj.HasExtension(typeof(string), "TestInt");

        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void ReplaceAttribute_NoDataType_ShouldFailWithMissingDataType()
    {
        var obj = GetInstance(TestInstance);
        var attribute = new GalaxyAttribute { Name = "NoType" };

        var result = obj.ReplaceAttribute(attribute);

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == ErrorCode.MissingDataType);
    }

    [TestMethod]
    public void ReplaceAttribute_NewAttribute_ShouldAddAttribute()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        var attribute = new GalaxyAttribute { Name = "NewAttribute", DataType = DataType.Integer };

        var result = obj.ReplaceAttribute(attribute);

        result.Success.Should().BeTrue();
        obj.TryGetAttribute("NewAttribute", out var added).Should().BeTrue();
        added.DataType.Should().Be(MxDataType.MxInteger);
    }

    [TestMethod]
    public void ReplaceAttribute_ExistingAttribute_ShouldReplaceWithNewDefinition()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        obj.ReplaceAttribute(new GalaxyAttribute { Name = "Replaced", DataType = DataType.Integer });

        var result = obj.ReplaceAttribute(new GalaxyAttribute { Name = "Replaced", DataType = DataType.String });

        result.Success.Should().BeTrue();
        obj.Attributes["Replaced"].DataType.Should().Be(MxDataType.MxString);
    }

    [TestMethod]
    public void ReplaceAttribute_ArrayAttribute_ShouldAddWithLength()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        var attribute = new GalaxyAttribute { Name = "NewArray", DataType = DataType.Integer, Length = 5 };

        var result = obj.ReplaceAttribute(attribute);

        result.Success.Should().BeTrue();
        obj.Attributes["NewArray"].GetLength().Should().Be(5);
    }

    [TestMethod]
    public void ReplaceAttribute_CalculatedWriteability_ShouldAddWithExpectedCategory()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        var attribute = new GalaxyAttribute
        {
            Name = "NewCalculated",
            DataType = DataType.Integer,
            Writeability = Writeability.Calculated
        };

        var result = obj.ReplaceAttribute(attribute);

        result.Success.Should().BeTrue();
        obj.Attributes["NewCalculated"].AttributeCategory.Should().Be(MxAttributeCategory.MxCategoryCalculated);
    }

    [TestMethod]
    public void AddExtension_NewExtension_ShouldAddExtensionPrimitive()
    {
        using var temp = new GalaxyTestObject(Galaxy);
        var obj = temp.Instance;
        obj.CheckOut();
        obj.ReplaceAttribute(new GalaxyAttribute { Name = "Target", DataType = DataType.Integer });

        var result = obj.AddExtension<LoggingExtension>("Target");

        result.Success.Should().BeTrue();
        result.Value.Should().BeSameAs(obj);
        obj.HasExtension(typeof(LoggingExtension), "Target").Should().BeTrue();
    }

    [TestMethod]
    public void AddExtension_ExistingExtension_ShouldReturnSuccessWithoutChange()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.AddExtension<LoggingExtension>("TestInt");

        result.Success.Should().BeTrue();
        result.Value.Should().BeSameAs(obj);
    }

    [TestMethod]
    public void EnableAttribute_BooleanAttribute_ShouldSetTrue()
    {
        var obj = GetInstance(TestInstance);
        obj.CheckOut();
        obj.Attributes["TestBool"].SetValue(false);

        var result = obj.EnableAttribute("TestBool");

        var value = obj.Attributes["TestBool"].GetValue();
        obj.Rollback();
        result.Success.Should().BeTrue();
        value.Should().Be(true);
    }

    [TestMethod]
    public void EnableAttribute_MissingAttribute_ShouldFailWithMissingAttribute()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.EnableAttribute("DoesNotExist");

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == ErrorCode.MissingAttribute);
    }

    [TestMethod]
    public void EnableAttribute_NonBooleanAttribute_ShouldFailWithInvalidDataType()
    {
        var obj = GetInstance(TestInstance);

        var result = obj.EnableAttribute("TestInt");

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == ErrorCode.InvalidDataType);
    }

    [TestMethod]
    public void CreateObject_FromTemplateWithInstanceName_ShouldCreateInstance()
    {
        const string tagName = "TempCreateObjectInstance";
        var template = GetTemplate(TestTemplate);

        var result = template.CreateObject(tagName);

        try
        {
            result.Success.Should().BeTrue();
            result.Value.Tagname.Should().Be(tagName);
            result.Value.IsInstance.Should().BeTrue();
        }
        finally
        {
            GetInstance(tagName).Delete();
        }
    }

    [TestMethod]
    public void CreateObject_FromTemplateWithTemplateName_ShouldCreateTemplate()
    {
        const string tagName = "$TempCreateObjectTemplate";
        var template = GetTemplate(TestTemplate);

        var result = template.CreateObject(tagName);

        try
        {
            result.Success.Should().BeTrue();
            result.Value.Tagname.Should().Be(tagName);
            result.Value.IsTemplate.Should().BeTrue();
        }
        finally
        {
            GetTemplate(tagName).Delete();
        }
    }

    [TestMethod]
    public void CreateObject_CalledOnInstance_ShouldFailWithUnknownError()
    {
        var instance = GetInstance(TestInstance);

        var result = instance.CreateObject("TempShouldNotExist");

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == ErrorCode.UnknownError);
    }

    [TestMethod]
    public void CreateObject_DuplicateTagName_ShouldFail()
    {
        var template = GetTemplate(TestTemplate);

        var result = template.CreateObject(TestInstance);

        result.Success.Should().BeFalse();
    }

    [TestMethod]
    public void DeleteObject_Instance_ShouldDeleteInstance()
    {
        const string tagName = "TempDeleteObjectInstance";
        var template = GetTemplate(TestTemplate);
        var created = template.CreateObject(tagName);
        created.Success.Should().BeTrue();

        var result = created.Value.Delete();

        result.Success.Should().BeTrue();
        GetInstance(tagName).Should().BeNull();
    }

    [TestMethod]
    public void DeleteObject_Template_ShouldDeleteTemplate()
    {
        const string tagName = "$TempDeleteObjectTemplate";
        var template = GetTemplate(TestTemplate);
        var created = template.CreateObject(tagName);
        created.Success.Should().BeTrue();

        var result = created.Value.Delete();

        result.Success.Should().BeTrue();
        GetTemplate(tagName).Should().BeNull();
    }

    [TestMethod]
    public Task ReadSymbol_FromAutomationObject_ShouldReturnVerifiedXml()
    {
        var instance = GetInstance(TestInstance);

        var doc = instance.ReadSymbol("Graphic_001");

        return VerifyXml(doc.ToString());
    }
}