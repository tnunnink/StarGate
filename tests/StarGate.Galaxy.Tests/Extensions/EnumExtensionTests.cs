using ArchestrA.GRAccess;
using FluentAssertions;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestClass]
public class EnumExtensionTests
{
    [TestMethod]
    [DataRow(EGRCommandResult.cmdUnknownError, ErrorCode.UnknownError)]
    [DataRow(EGRCommandResult.cmdSuccess, ErrorCode.Success)]
    [DataRow(EGRCommandResult.cmdInsufficientPermissions, ErrorCode.InsufficientPermissions)]
    [DataRow(EGRCommandResult.cmdNoSuchGRNode, ErrorCode.NoSuchGalaxyNode)]
    [DataRow(EGRCommandResult.cmdNoSuchUser, ErrorCode.NoSuchUser)]
    [DataRow(EGRCommandResult.cmdPasswordIncorrect, ErrorCode.InvalidPassword)]
    [DataRow(EGRCommandResult.cmdLicenseUnavailable, ErrorCode.LicenseUnavailable)]
    [DataRow(EGRCommandResult.cmdNoSuchFile, ErrorCode.NoSuchFile)]
    [DataRow(EGRCommandResult.cmdCouldntCreateFile, ErrorCode.CouldNotCreateFile)]
    [DataRow(EGRCommandResult.cmdObjectIsCheckedOut, ErrorCode.ObjectIsCheckedOut)]
    [DataRow(EGRCommandResult.cmdObjectIsCheckedOutToSomeoneElse, ErrorCode.ObjectIsCheckedOutToSomeoneElse)]
    [DataRow(EGRCommandResult.cmdObjectNotCheckedOutToMe, ErrorCode.ObjectNotCheckedOutToMe)]
    [DataRow(EGRCommandResult.cmdObjectCannotBeOverwritten, ErrorCode.ObjectCannotBeOverwritten)]
    [DataRow(EGRCommandResult.cmdTemplateInUse, ErrorCode.TemplateInUse)]
    [DataRow(EGRCommandResult.cmdObjectIsAContainer, ErrorCode.ObjectIsAContainer)]
    [DataRow(EGRCommandResult.cmdObjectHostNotFound, ErrorCode.ObjectHostNotFound)]
    [DataRow(EGRCommandResult.cmdInstanceIsHost, ErrorCode.InstanceIsHost)]
    [DataRow(EGRCommandResult.cmdObjectIsRequired, ErrorCode.ObjectIsRequired)]
    [DataRow(EGRCommandResult.cmdObjectNotAnAutomationObject, ErrorCode.ObjectNotAnAutomationObject)]
    [DataRow(EGRCommandResult.cmdObjectInBadState, ErrorCode.ObjectInBadState)]
    [DataRow(EGRCommandResult.cmdCustomConfigurationError, ErrorCode.CustomConfigurationError)]
    [DataRow(EGRCommandResult.cmdObjectInReadOnlyMode, ErrorCode.ObjectInReadOnlyMode)]
    [DataRow(EGRCommandResult.cmdObjectHostNotDeployed, ErrorCode.ObjectHostNotDeployed)]
    [DataRow(EGRCommandResult.cmdInstanceIsDeployed, ErrorCode.InstanceIsDeployed)]
    [DataRow(EGRCommandResult.cmdInvallidGRLoadMode, ErrorCode.InvalidGalaxyLoadMode)]
    [DataRow(EGRCommandResult.cmdOEMVersionIncompatible, ErrorCode.IncompatibleOemVersion)]
    [DataRow(EGRCommandResult.cmdRetryClientSync, ErrorCode.RetryClientSync)]
    [DataRow(EGRCommandResult.cmdSyncSemaphore, ErrorCode.SyncSemaphore)]
    [DataRow(EGRCommandResult.cmdOutOfSync, ErrorCode.OutOfSync)]
    [DataRow(EGRCommandResult.cmdInstanceProtectionDenied, ErrorCode.InstanceProtectionDenied)]
    public void ToCoreType_CommandResult_ShouldReturnExpectedErrorCode(EGRCommandResult result, ErrorCode expected)
    {
        result.ToCoreType().Should().Be(expected);
    }

    [TestMethod]
    public void ToCoreType_UndefinedCommandResult_ShouldThrowArgumentOutOfRange()
    {
        const EGRCommandResult invalid = (EGRCommandResult)int.MaxValue;

        var action = () => invalid.ToCoreType();

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    [DataRow(AttributeDataType.Boolean, MxDataType.MxBoolean)]
    [DataRow(AttributeDataType.Integer, MxDataType.MxInteger)]
    [DataRow(AttributeDataType.Float, MxDataType.MxFloat)]
    [DataRow(AttributeDataType.Double, MxDataType.MxDouble)]
    [DataRow(AttributeDataType.String, MxDataType.MxString)]
    [DataRow(AttributeDataType.Time, MxDataType.MxTime)]
    [DataRow(AttributeDataType.ElapsedTime, MxDataType.MxElapsedTime)]
    [DataRow(AttributeDataType.InternationalizedString, MxDataType.MxInternationalizedString)]
    [DataRow(AttributeDataType.BigString, MxDataType.MxBigString)]
    public void ToMxType_DataType_ShouldReturnExpectedMxDataType(AttributeDataType dataType, MxDataType expected)
    {
        dataType.ToMxType().Should().Be(expected);
    }

    [TestMethod]
    public void ToMxType_UndefinedDataType_ShouldReturnUnknown()
    {
        const AttributeDataType invalid = (AttributeDataType)int.MaxValue;

        invalid.ToMxType().Should().Be(MxDataType.MxDataTypeUnknown);
    }

    [TestMethod]
    [DataRow(MxDataType.MxBoolean, AttributeDataType.Boolean)]
    [DataRow(MxDataType.MxInteger, AttributeDataType.Integer)]
    [DataRow(MxDataType.MxFloat, AttributeDataType.Float)]
    [DataRow(MxDataType.MxDouble, AttributeDataType.Double)]
    [DataRow(MxDataType.MxString, AttributeDataType.String)]
    [DataRow(MxDataType.MxTime, AttributeDataType.Time)]
    [DataRow(MxDataType.MxElapsedTime, AttributeDataType.ElapsedTime)]
    [DataRow(MxDataType.MxInternationalizedString, AttributeDataType.InternationalizedString)]
    [DataRow(MxDataType.MxBigString, AttributeDataType.BigString)]
    public void ToCoreType_MxDataType_ShouldReturnExpectedDataType(MxDataType dataType, AttributeDataType expected)
    {
        dataType.ToCoreType().Should().Be(expected);
    }

    [TestMethod]
    [DataRow(MxDataType.MxDataTypeUnknown)]
    [DataRow(MxDataType.MxNoData)]
    [DataRow(MxDataType.MxReferenceType)]
    [DataRow(MxDataType.MxStatusType)]
    [DataRow(MxDataType.MxDataTypeEnum)]
    [DataRow(MxDataType.MxSecurityClassificationEnum)]
    [DataRow(MxDataType.MxDataQualityType)]
    [DataRow(MxDataType.MxQualifiedEnum)]
    [DataRow(MxDataType.MxQualifiedStruct)]
    [DataRow(MxDataType.MxDataTypeEND)]
    public void ToCoreType_NonStandardMxDataType_ShouldReturnString(MxDataType dataType)
    {
        dataType.ToCoreType().Should().Be(AttributeDataType.String);
    }

    [TestMethod]
    public void ToCoreType_UndefinedMxDataType_ShouldThrowArgumentOutOfRange()
    {
        const MxDataType invalid = (MxDataType)int.MaxValue;

        var action = () => invalid.ToCoreType();

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    [DataRow(Writeability.UserWriteable, MxAttributeCategory.MxCategoryWriteable_USC_Lockable)]
    [DataRow(Writeability.ObjectWriteable, MxAttributeCategory.MxCategoryWriteable_C_Lockable)]
    [DataRow(Writeability.Calculated, MxAttributeCategory.MxCategoryCalculated)]
    [DataRow(Writeability.CalculatedRetentive, MxAttributeCategory.MxCategoryCalculatedRetentive)]
    public void ToMxType_Writeability_ShouldReturnExpectedCategory(Writeability writeability,
        MxAttributeCategory expected)
    {
        ((Writeability?)writeability).ToMxType().Should().Be(expected);
    }

    [TestMethod]
    public void ToMxType_NullWriteability_ShouldReturnUserWriteable()
    {
        Writeability? writeability = null;

        writeability.ToMxType().Should().Be(MxAttributeCategory.MxCategoryWriteable_USC_Lockable);
    }

    [TestMethod]
    public void ToMxType_UndefinedWriteability_ShouldReturnUserWriteable()
    {
        Writeability? writeability = (Writeability)int.MaxValue;

        writeability.ToMxType().Should().Be(MxAttributeCategory.MxCategoryWriteable_USC_Lockable);
    }

    [TestMethod]
    [DataRow(MxAttributeCategory.MxCategoryWriteable_USC_Lockable, Writeability.UserWriteable)]
    [DataRow(MxAttributeCategory.MxCategoryWriteable_C_Lockable, Writeability.ObjectWriteable)]
    [DataRow(MxAttributeCategory.MxCategoryCalculated, Writeability.Calculated)]
    [DataRow(MxAttributeCategory.MxCategoryCalculatedRetentive, Writeability.CalculatedRetentive)]
    public void ToCoreType_AttributeCategory_ShouldReturnExpectedWriteability(MxAttributeCategory category,
        Writeability expected)
    {
        category.ToCoreType().Should().Be(expected);
    }

    [TestMethod]
    [DataRow(MxAttributeCategory.MxCategoryPackageOnly)]
    [DataRow(MxAttributeCategory.MxCategoryWriteable_UC)]
    [DataRow(MxAttributeCategory.MxCategoryWriteable_USC)]
    public void ToCoreType_UnmappedAttributeCategory_ShouldThrowArgumentOutOfRange(MxAttributeCategory category)
    {
        var action = () => category.ToCoreType();

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    [DataRow(Security.FreeAccess, MxSecurityClassification.MxSecurityFreeAccess)]
    [DataRow(Security.Operate, MxSecurityClassification.MxSecurityOperate)]
    [DataRow(Security.SecuredWrite, MxSecurityClassification.MxSecuritySecuredWrite)]
    [DataRow(Security.VerifiedWrite, MxSecurityClassification.MxSecurityVerifiedWrite)]
    [DataRow(Security.Tune, MxSecurityClassification.MxSecurityTune)]
    [DataRow(Security.Configure, MxSecurityClassification.MxSecurityConfigure)]
    [DataRow(Security.ViewOnly, MxSecurityClassification.MxSecurityViewOnly)]
    [DataRow(Security.Undefined, MxSecurityClassification.MxSecurityUndefined)]
    public void ToMxType_Security_ShouldReturnExpectedClassification(Security security,
        MxSecurityClassification expected)
    {
        security.ToMxType().Should().Be(expected);
    }

    [TestMethod]
    [DataRow(MxSecurityClassification.MxSecurityFreeAccess, Security.FreeAccess)]
    [DataRow(MxSecurityClassification.MxSecurityOperate, Security.Operate)]
    [DataRow(MxSecurityClassification.MxSecuritySecuredWrite, Security.SecuredWrite)]
    [DataRow(MxSecurityClassification.MxSecurityVerifiedWrite, Security.VerifiedWrite)]
    [DataRow(MxSecurityClassification.MxSecurityTune, Security.Tune)]
    [DataRow(MxSecurityClassification.MxSecurityConfigure, Security.Configure)]
    [DataRow(MxSecurityClassification.MxSecurityViewOnly, Security.ViewOnly)]
    public void ToSecurity_Classification_ShouldReturnExpectedSecurity(MxSecurityClassification classification,
        Security expected)
    {
        classification.ToSecurity().Should().Be(expected);
    }

    [TestMethod]
    public void ToSecurity_UndefinedClassification_ShouldReturnNull()
    {
        MxSecurityClassification.MxSecurityUndefined.ToSecurity().Should().BeNull();
    }
}