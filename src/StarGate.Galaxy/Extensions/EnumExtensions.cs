using System;
using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Core.Objects;

namespace StarGate.Galaxy.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Converts a given <see cref="EGRCommandResult"/> value to its corresponding <see cref="ErrorCode"/> representation.
    /// </summary>
    /// <param name="result">The <see cref="EGRCommandResult"/> value to be converted.</param>
    /// <returns>An <see cref="ErrorCode"/> that represents the equivalent type for the specified <paramref name="result"/> value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="result"/> value is not recognized.</exception>
    public static ErrorCode ToCoreType(this EGRCommandResult result)
    {
        return result switch
        {
            EGRCommandResult.cmdUnknownError => ErrorCode.UnknownError,
            EGRCommandResult.cmdSuccess => ErrorCode.Success,
            EGRCommandResult.cmdInsufficientPermissions => ErrorCode.InsufficientPermissions,
            EGRCommandResult.cmdNoSuchGRNode => ErrorCode.NoSuchGalaxyNode,
            EGRCommandResult.cmdNoSuchUser => ErrorCode.NoSuchUser,
            EGRCommandResult.cmdPasswordIncorrect => ErrorCode.InvalidPassword,
            EGRCommandResult.cmdLicenseUnavailable => ErrorCode.LicenseUnavailable,
            EGRCommandResult.cmdNoSuchFile => ErrorCode.NoSuchFile,
            EGRCommandResult.cmdCouldntCreateFile => ErrorCode.CouldNotCreateFile,
            EGRCommandResult.cmdObjectIsCheckedOut => ErrorCode.ObjectIsCheckedOut,
            EGRCommandResult.cmdObjectIsCheckedOutToSomeoneElse => ErrorCode.ObjectIsCheckedOutToSomeoneElse,
            EGRCommandResult.cmdObjectNotCheckedOutToMe => ErrorCode.ObjectNotCheckedOutToMe,
            EGRCommandResult.cmdObjectCannotBeOverwritten => ErrorCode.ObjectCannotBeOverwritten,
            EGRCommandResult.cmdTemplateInUse => ErrorCode.TemplateInUse,
            EGRCommandResult.cmdObjectIsAContainer => ErrorCode.ObjectIsAContainer,
            EGRCommandResult.cmdObjectHostNotFound => ErrorCode.ObjectHostNotFound,
            EGRCommandResult.cmdInstanceIsHost => ErrorCode.InstanceIsHost,
            EGRCommandResult.cmdObjectIsRequired => ErrorCode.ObjectIsRequired,
            EGRCommandResult.cmdObjectNotAnAutomationObject => ErrorCode.ObjectNotAnAutomationObject,
            EGRCommandResult.cmdObjectInBadState => ErrorCode.ObjectInBadState,
            EGRCommandResult.cmdCustomConfigurationError => ErrorCode.CustomConfigurationError,
            EGRCommandResult.cmdObjectInReadOnlyMode => ErrorCode.ObjectInReadOnlyMode,
            EGRCommandResult.cmdObjectHostNotDeployed => ErrorCode.ObjectHostNotDeployed,
            EGRCommandResult.cmdInstanceIsDeployed => ErrorCode.InstanceIsDeployed,
            EGRCommandResult.cmdInvallidGRLoadMode => ErrorCode.InvalidGalaxyLoadMode,
            EGRCommandResult.cmdOEMVersionIncompatible => ErrorCode.IncompatibleOemVersion,
            EGRCommandResult.cmdRetryClientSync => ErrorCode.RetryClientSync,
            EGRCommandResult.cmdSyncSemaphore => ErrorCode.SyncSemaphore,
            EGRCommandResult.cmdOutOfSync => ErrorCode.OutOfSync,
            EGRCommandResult.cmdInstanceProtectionDenied => ErrorCode.InstanceProtectionDenied,
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
    }

    /// <summary>
    /// Maps a given <see cref="DataType"/> value to its corresponding <see cref="MxDataType"/> type.
    /// </summary>
    /// <param name="dataType">The <see cref="DataType"/> value to be mapped.</param>
    /// <returns>An <see cref="MxDataType"/> that represents the equivalent type for the specified <paramref name="dataType"/> value.</returns>
    public static MxDataType ToMxType(this DataType dataType)
    {
        return dataType switch
        {
            DataType.Boolean => MxDataType.MxBoolean,
            DataType.Integer => MxDataType.MxInteger,
            DataType.Float => MxDataType.MxFloat,
            DataType.Double => MxDataType.MxDouble,
            DataType.String => MxDataType.MxString,
            DataType.Time => MxDataType.MxTime,
            DataType.ElapsedTime => MxDataType.MxElapsedTime,
            DataType.InternationalizedString => MxDataType.MxInternationalizedString,
            DataType.BigString => MxDataType.MxBigString,
            _ => MxDataType.MxDataTypeUnknown
        };
    }

    /// <summary>
    /// Maps the specified <see cref="MxDataType"/> value to its corresponding <see cref="DataType"/> type.
    /// </summary>
    /// <param name="dataType">The <see cref="MxDataType"/> value to be converted.</param>
    /// <returns>A <see cref="DataType"/> that represents the equivalent type for the specified <paramref name="dataType"/> value.</returns>
    public static DataType ToCoreType(this MxDataType dataType)
    {
        return dataType switch
        {
            MxDataType.MxBoolean => DataType.Boolean,
            MxDataType.MxInteger => DataType.Integer,
            MxDataType.MxFloat => DataType.Float,
            MxDataType.MxDouble => DataType.Double,
            MxDataType.MxString => DataType.String,
            MxDataType.MxTime => DataType.Time,
            MxDataType.MxElapsedTime => DataType.ElapsedTime,
            MxDataType.MxInternationalizedString => DataType.InternationalizedString,
            MxDataType.MxBigString => DataType.BigString,

            //todo not sure if we want to do these really. We shouldn't have to
            MxDataType.MxDataTypeUnknown => DataType.String,
            MxDataType.MxNoData => DataType.String,
            MxDataType.MxReferenceType => DataType.String,
            MxDataType.MxStatusType => DataType.String,
            MxDataType.MxDataTypeEnum => DataType.String,
            MxDataType.MxSecurityClassificationEnum => DataType.String,
            MxDataType.MxDataQualityType => DataType.String,
            MxDataType.MxQualifiedEnum => DataType.String,
            MxDataType.MxQualifiedStruct => DataType.String,
            MxDataType.MxDataTypeEND => DataType.String,
            _ => throw new ArgumentOutOfRangeException(nameof(dataType),
                $"MxDataType value '{dataType}' is not defined for '{nameof(DataType)}'.")
        };
    }

    /// <summary>
    /// Maps a given <see cref="Writeability"/> value to its corresponding <see cref="MxAttributeCategory"/> type.
    /// </summary>
    /// <param name="writeability">The <see cref="Writeability"/> value to be mapped.</param>
    /// <returns>An <see cref="MxAttributeCategory"/> that corresponds to the specified <paramref name="writeability"/> value.</returns>
    public static MxAttributeCategory ToMxType(this Writeability? writeability)
    {
        if (!writeability.HasValue)
            return MxAttributeCategory.MxCategoryWriteable_USC_Lockable;

        return writeability.Value switch
        {
            Writeability.UserWriteable => MxAttributeCategory.MxCategoryWriteable_USC_Lockable,
            Writeability.ObjectWriteable => MxAttributeCategory.MxCategoryWriteable_C_Lockable,
            Writeability.Calculated => MxAttributeCategory.MxCategoryCalculated,
            Writeability.CalculatedRetentive => MxAttributeCategory.MxCategoryCalculatedRetentive,
            _ => MxAttributeCategory.MxCategoryWriteable_USC_Lockable
        };
    }

    /// <summary>
    /// Converts a specified <see cref="MxAttributeCategory"/> to its corresponding <see cref="Writeability"/> value.
    /// </summary>
    /// <param name="category">The <see cref="MxAttributeCategory"/> to be converted.</param>
    /// <returns>The corresponding <see cref="Writeability"/> value for the given <paramref name="category"/>.</returns>
    /// <exception cref="category">Thrown when the <paramref name="category"/> value is not supported or recognized.</exception>
    public static Writeability ToCoreType(this MxAttributeCategory category)
    {
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        // Only a subset of categories are configurable via UDA which is what we care about.
        return category switch
        {
            MxAttributeCategory.MxCategoryWriteable_USC_Lockable => Writeability.UserWriteable,
            MxAttributeCategory.MxCategoryWriteable_C_Lockable => Writeability.ObjectWriteable,
            MxAttributeCategory.MxCategoryCalculated => Writeability.Calculated,
            MxAttributeCategory.MxCategoryCalculatedRetentive => Writeability.CalculatedRetentive,
            _ => throw new ArgumentOutOfRangeException(nameof(category), category,
                $"Category {category} does not map back to a core security type.")
        };
    }

    /// <summary>
    /// Converts the specified <paramref name="security"/> value from <see cref="Security"/>
    /// to the corresponding <see cref="MxSecurityClassification"/> enumeration value.
    /// </summary>
    /// <param name="security">
    /// The <see cref="Security"/> value representing the security classification level to convert.
    /// </param>
    /// <returns>
    /// A <see cref="MxSecurityClassification"/> enumeration value that corresponds to the specified
    /// <paramref name="security"/> level. Returns <see cref="MxSecurityClassification.MxSecurityUndefined"/>
    /// if the provided <paramref name="security"/> value does not match any defined classification.
    /// </returns>
    public static MxSecurityClassification ToMxType(this Security security)
    {
        return security switch
        {
            Security.FreeAccess => MxSecurityClassification.MxSecurityFreeAccess,
            Security.Operate => MxSecurityClassification.MxSecurityOperate,
            Security.SecuredWrite => MxSecurityClassification.MxSecuritySecuredWrite,
            Security.VerifiedWrite => MxSecurityClassification.MxSecurityVerifiedWrite,
            Security.Tune => MxSecurityClassification.MxSecurityTune,
            Security.Configure => MxSecurityClassification.MxSecurityConfigure,
            Security.ViewOnly => MxSecurityClassification.MxSecurityViewOnly,
            _ => MxSecurityClassification.MxSecurityUndefined
        };
    }

    /// <summary>
    /// Maps a <see cref="MxSecurityClassification"/> instance to its corresponding <see cref="Security"/> value.
    /// </summary>
    /// <param name="security">The <see cref="MxSecurityClassification"/> to be converted.</param>
    /// <returns>A corresponding <see cref="Security"/> value if the classification is valid; otherwise, null.</returns>
    public static Security? ToSecurity(this MxSecurityClassification security)
    {
        return security switch
        {
            MxSecurityClassification.MxSecurityFreeAccess => Security.FreeAccess,
            MxSecurityClassification.MxSecurityOperate => Security.Operate,
            MxSecurityClassification.MxSecuritySecuredWrite => Security.SecuredWrite,
            MxSecurityClassification.MxSecurityVerifiedWrite => Security.VerifiedWrite,
            MxSecurityClassification.MxSecurityTune => Security.Tune,
            MxSecurityClassification.MxSecurityConfigure => Security.Configure,
            MxSecurityClassification.MxSecurityViewOnly => Security.ViewOnly,
            _ => null
        };
    }
}