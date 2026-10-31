using System;
using ArchestrA.GRAccess;
using FluentAssertions;
using NUnit.Framework;
using StarGate.Core;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Test.Extensions;

[TestFixture]
public class EnumExtensionTests
{
    [Test]
    [TestCase(EGRCommandResult.cmdUnknownError, ErrorCode.UnknownError)]
    [TestCase(EGRCommandResult.cmdSuccess, ErrorCode.Success)]
    [TestCase(EGRCommandResult.cmdInsufficientPermissions, ErrorCode.InsufficientPermissions)]
    [TestCase(EGRCommandResult.cmdNoSuchGRNode, ErrorCode.NoSuchGalaxyNode)]
    [TestCase(EGRCommandResult.cmdNoSuchUser, ErrorCode.NoSuchUser)]
    [TestCase(EGRCommandResult.cmdPasswordIncorrect, ErrorCode.InvalidPassword)]
    [TestCase(EGRCommandResult.cmdLicenseUnavailable, ErrorCode.LicenseUnavailable)]
    [TestCase(EGRCommandResult.cmdNoSuchFile, ErrorCode.NoSuchFile)]
    [TestCase(EGRCommandResult.cmdCouldntCreateFile, ErrorCode.CouldNotCreateFile)]
    [TestCase(EGRCommandResult.cmdObjectIsCheckedOut, ErrorCode.ObjectIsCheckedOut)]
    [TestCase(EGRCommandResult.cmdObjectIsCheckedOutToSomeoneElse, ErrorCode.ObjectIsCheckedOutToSomeoneElse)]
    [TestCase(EGRCommandResult.cmdObjectNotCheckedOutToMe, ErrorCode.ObjectNotCheckedOutToMe)]
    [TestCase(EGRCommandResult.cmdObjectCannotBeOverwritten, ErrorCode.ObjectCannotBeOverwritten)]
    [TestCase(EGRCommandResult.cmdTemplateInUse, ErrorCode.TemplateInUse)]
    [TestCase(EGRCommandResult.cmdObjectIsAContainer, ErrorCode.ObjectIsAContainer)]
    [TestCase(EGRCommandResult.cmdObjectHostNotFound, ErrorCode.ObjectHostNotFound)]
    [TestCase(EGRCommandResult.cmdInstanceIsHost, ErrorCode.InstanceIsHost)]
    [TestCase(EGRCommandResult.cmdObjectIsRequired, ErrorCode.ObjectIsRequired)]
    [TestCase(EGRCommandResult.cmdObjectNotAnAutomationObject, ErrorCode.ObjectNotAnAutomationObject)]
    [TestCase(EGRCommandResult.cmdObjectInBadState, ErrorCode.ObjectInBadState)]
    [TestCase(EGRCommandResult.cmdCustomConfigurationError, ErrorCode.CustomConfigurationError)]
    [TestCase(EGRCommandResult.cmdObjectInReadOnlyMode, ErrorCode.ObjectInReadOnlyMode)]
    [TestCase(EGRCommandResult.cmdObjectHostNotDeployed, ErrorCode.ObjectHostNotDeployed)]
    [TestCase(EGRCommandResult.cmdInstanceIsDeployed, ErrorCode.InstanceIsDeployed)]
    [TestCase(EGRCommandResult.cmdInvallidGRLoadMode, ErrorCode.InvalidGalaxyLoadMode)]
    [TestCase(EGRCommandResult.cmdOEMVersionIncompatible, ErrorCode.IncompatibleOemVersion)]
    [TestCase(EGRCommandResult.cmdRetryClientSync, ErrorCode.RetryClientSync)]
    [TestCase(EGRCommandResult.cmdSyncSemaphore, ErrorCode.SyncSemaphore)]
    [TestCase(EGRCommandResult.cmdOutOfSync, ErrorCode.OutOfSync)]
    [TestCase(EGRCommandResult.cmdInstanceProtectionDenied, ErrorCode.InstanceProtectionDenied)]
    public void ToCoreType_CommandResult_ShouldReturnExpectedErrorCode(EGRCommandResult result, ErrorCode expected)
    {
        result.ToCoreType().Should().Be(expected);
    }

    [Test]
    public void ToCoreType_UndefinedCommandResult_ShouldThrowArgumentOutOfRange()
    {
        const EGRCommandResult invalid = (EGRCommandResult)int.MaxValue;

        var action = () => invalid.ToCoreType();

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    [TestCase(DataType.Boolean, MxDataType.MxBoolean)]
    [TestCase(DataType.Integer, MxDataType.MxInteger)]
    [TestCase(DataType.Float, MxDataType.MxFloat)]
    [TestCase(DataType.Double, MxDataType.MxDouble)]
    [TestCase(DataType.String, MxDataType.MxString)]
    [TestCase(DataType.Time, MxDataType.MxTime)]
    [TestCase(DataType.ElapsedTime, MxDataType.MxElapsedTime)]
    [TestCase(DataType.InternationalizedString, MxDataType.MxInternationalizedString)]
    [TestCase(DataType.BigString, MxDataType.MxBigString)]
    public void ToMxType_DataType_ShouldReturnExpectedMxDataType(DataType dataType, MxDataType expected)
    {
        dataType.ToMxType().Should().Be(expected);
    }

    [Test]
    public void ToMxType_UndefinedDataType_ShouldReturnUnknown()
    {
        const DataType invalid = (DataType)int.MaxValue;

        invalid.ToMxType().Should().Be(MxDataType.MxDataTypeUnknown);
    }

    [Test]
    [TestCase(MxDataType.MxBoolean, DataType.Boolean)]
    [TestCase(MxDataType.MxInteger, DataType.Integer)]
    [TestCase(MxDataType.MxFloat, DataType.Float)]
    [TestCase(MxDataType.MxDouble, DataType.Double)]
    [TestCase(MxDataType.MxString, DataType.String)]
    [TestCase(MxDataType.MxTime, DataType.Time)]
    [TestCase(MxDataType.MxElapsedTime, DataType.ElapsedTime)]
    [TestCase(MxDataType.MxInternationalizedString, DataType.InternationalizedString)]
    [TestCase(MxDataType.MxBigString, DataType.BigString)]
    public void ToCoreType_MxDataType_ShouldReturnExpectedDataType(MxDataType dataType, DataType expected)
    {
        dataType.ToCoreType().Should().Be(expected);
    }

    [Test]
    [TestCase(MxDataType.MxDataTypeUnknown)]
    [TestCase(MxDataType.MxNoData)]
    [TestCase(MxDataType.MxReferenceType)]
    [TestCase(MxDataType.MxStatusType)]
    [TestCase(MxDataType.MxDataTypeEnum)]
    [TestCase(MxDataType.MxSecurityClassificationEnum)]
    [TestCase(MxDataType.MxDataQualityType)]
    [TestCase(MxDataType.MxQualifiedEnum)]
    [TestCase(MxDataType.MxQualifiedStruct)]
    [TestCase(MxDataType.MxDataTypeEND)]
    public void ToCoreType_NonStandardMxDataType_ShouldReturnString(MxDataType dataType)
    {
        dataType.ToCoreType().Should().Be(DataType.String);
    }

    [Test]
    public void ToCoreType_UndefinedMxDataType_ShouldThrowArgumentOutOfRange()
    {
        const MxDataType invalid = (MxDataType)int.MaxValue;

        var action = () => invalid.ToCoreType();

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    [TestCase(Writeability.UserWriteable, MxAttributeCategory.MxCategoryWriteable_USC_Lockable)]
    [TestCase(Writeability.ObjectWriteable, MxAttributeCategory.MxCategoryWriteable_C_Lockable)]
    [TestCase(Writeability.Calculated, MxAttributeCategory.MxCategoryCalculated)]
    [TestCase(Writeability.CalculatedRetentive, MxAttributeCategory.MxCategoryCalculatedRetentive)]
    public void ToMxType_Writeability_ShouldReturnExpectedCategory(Writeability writeability,
        MxAttributeCategory expected)
    {
        ((Writeability?)writeability).ToMxType().Should().Be(expected);
    }

    [Test]
    public void ToMxType_NullWriteability_ShouldReturnUserWriteable()
    {
        Writeability? writeability = null;

        writeability.ToMxType().Should().Be(MxAttributeCategory.MxCategoryWriteable_USC_Lockable);
    }

    [Test]
    public void ToMxType_UndefinedWriteability_ShouldReturnUserWriteable()
    {
        Writeability? writeability = (Writeability)int.MaxValue;

        writeability.ToMxType().Should().Be(MxAttributeCategory.MxCategoryWriteable_USC_Lockable);
    }

    [Test]
    [TestCase(MxAttributeCategory.MxCategoryWriteable_USC_Lockable, Writeability.UserWriteable)]
    [TestCase(MxAttributeCategory.MxCategoryWriteable_C_Lockable, Writeability.ObjectWriteable)]
    [TestCase(MxAttributeCategory.MxCategoryCalculated, Writeability.Calculated)]
    [TestCase(MxAttributeCategory.MxCategoryCalculatedRetentive, Writeability.CalculatedRetentive)]
    public void ToCoreType_AttributeCategory_ShouldReturnExpectedWriteability(MxAttributeCategory category,
        Writeability expected)
    {
        category.ToCoreType().Should().Be(expected);
    }

    [Test]
    [TestCase(MxAttributeCategory.MxCategoryPackageOnly)]
    [TestCase(MxAttributeCategory.MxCategoryWriteable_UC)]
    [TestCase(MxAttributeCategory.MxCategoryWriteable_USC)]
    public void ToCoreType_UnmappedAttributeCategory_ShouldThrowArgumentOutOfRange(MxAttributeCategory category)
    {
        var action = () => category.ToCoreType();

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    [TestCase(Security.FreeAccess, MxSecurityClassification.MxSecurityFreeAccess)]
    [TestCase(Security.Operate, MxSecurityClassification.MxSecurityOperate)]
    [TestCase(Security.SecuredWrite, MxSecurityClassification.MxSecuritySecuredWrite)]
    [TestCase(Security.VerifiedWrite, MxSecurityClassification.MxSecurityVerifiedWrite)]
    [TestCase(Security.Tune, MxSecurityClassification.MxSecurityTune)]
    [TestCase(Security.Configure, MxSecurityClassification.MxSecurityConfigure)]
    [TestCase(Security.ViewOnly, MxSecurityClassification.MxSecurityViewOnly)]
    [TestCase(Security.Undefined, MxSecurityClassification.MxSecurityUndefined)]
    public void ToMxType_Security_ShouldReturnExpectedClassification(Security security,
        MxSecurityClassification expected)
    {
        security.ToMxType().Should().Be(expected);
    }

    [Test]
    [TestCase(MxSecurityClassification.MxSecurityFreeAccess, Security.FreeAccess)]
    [TestCase(MxSecurityClassification.MxSecurityOperate, Security.Operate)]
    [TestCase(MxSecurityClassification.MxSecuritySecuredWrite, Security.SecuredWrite)]
    [TestCase(MxSecurityClassification.MxSecurityVerifiedWrite, Security.VerifiedWrite)]
    [TestCase(MxSecurityClassification.MxSecurityTune, Security.Tune)]
    [TestCase(MxSecurityClassification.MxSecurityConfigure, Security.Configure)]
    [TestCase(MxSecurityClassification.MxSecurityViewOnly, Security.ViewOnly)]
    public void ToSecurity_Classification_ShouldReturnExpectedSecurity(MxSecurityClassification classification,
        Security expected)
    {
        classification.ToSecurity().Should().Be(expected);
    }

    [Test]
    public void ToSecurity_UndefinedClassification_ShouldReturnNull()
    {
        MxSecurityClassification.MxSecurityUndefined.ToSecurity().Should().BeNull();
    }
}