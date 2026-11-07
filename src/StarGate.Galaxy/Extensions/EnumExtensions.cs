using System;
using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

public static class EnumExtensions
{
    /// <summary>
    /// Converts a given <see cref="EGRCommandResult" /> value to its corresponding <see cref="ErrorCode" />
    /// representation.
    /// </summary>
    /// <param name="result">The <see cref="EGRCommandResult" /> value to be converted.</param>
    /// <returns>
    /// An <see cref="ErrorCode" /> that represents the equivalent type for the specified <paramref name="result" />
    /// value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="result" /> value is not recognized.</exception>
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
    /// Maps a given <see cref="AttributeDataType" /> value to its corresponding <see cref="MxDataType" /> type.
    /// </summary>
    /// <param name="dataType">The <see cref="AttributeDataType" /> value to be mapped.</param>
    /// <returns>
    /// An <see cref="MxDataType" /> that represents the equivalent type for the specified
    /// <paramref name="dataType" /> value.
    /// </returns>
    public static MxDataType ToMxType(this AttributeDataType dataType)
    {
        return dataType switch
        {
            AttributeDataType.Boolean => MxDataType.MxBoolean,
            AttributeDataType.Integer => MxDataType.MxInteger,
            AttributeDataType.Float => MxDataType.MxFloat,
            AttributeDataType.Double => MxDataType.MxDouble,
            AttributeDataType.String => MxDataType.MxString,
            AttributeDataType.Time => MxDataType.MxTime,
            AttributeDataType.ElapsedTime => MxDataType.MxElapsedTime,
            AttributeDataType.InternationalizedString => MxDataType.MxInternationalizedString,
            AttributeDataType.BigString => MxDataType.MxBigString,
            _ => MxDataType.MxDataTypeUnknown
        };
    }

    /// <summary>
    /// Maps the specified <see cref="MxDataType" /> value to its corresponding <see cref="AttributeDataType" /> type.
    /// </summary>
    /// <param name="dataType">The <see cref="MxDataType" /> value to be converted.</param>
    /// <returns>
    /// A <see cref="AttributeDataType" /> that represents the equivalent type for the specified <paramref name="dataType" />
    /// value.
    /// </returns>
    public static AttributeDataType ToCoreType(this MxDataType dataType)
    {
        return dataType switch
        {
            MxDataType.MxBoolean => AttributeDataType.Boolean,
            MxDataType.MxInteger => AttributeDataType.Integer,
            MxDataType.MxFloat => AttributeDataType.Float,
            MxDataType.MxDouble => AttributeDataType.Double,
            MxDataType.MxString => AttributeDataType.String,
            MxDataType.MxTime => AttributeDataType.Time,
            MxDataType.MxElapsedTime => AttributeDataType.ElapsedTime,
            MxDataType.MxInternationalizedString => AttributeDataType.InternationalizedString,
            MxDataType.MxBigString => AttributeDataType.BigString,

            //todo not sure if we want to do these really. We shouldn't have to
            MxDataType.MxDataTypeUnknown => AttributeDataType.String,
            MxDataType.MxNoData => AttributeDataType.String,
            MxDataType.MxReferenceType => AttributeDataType.String,
            MxDataType.MxStatusType => AttributeDataType.String,
            MxDataType.MxDataTypeEnum => AttributeDataType.String,
            MxDataType.MxSecurityClassificationEnum => AttributeDataType.String,
            MxDataType.MxDataQualityType => AttributeDataType.String,
            MxDataType.MxQualifiedEnum => AttributeDataType.String,
            MxDataType.MxQualifiedStruct => AttributeDataType.String,
            MxDataType.MxDataTypeEND => AttributeDataType.String,
            _ => throw new ArgumentOutOfRangeException(nameof(dataType),
                $"MxDataType value '{dataType}' is not defined for '{nameof(AttributeDataType)}'.")
        };
    }

    /// <summary>
    /// Maps a given <see cref="Writeability" /> value to its corresponding <see cref="MxAttributeCategory" /> type.
    /// </summary>
    /// <param name="writeability">The <see cref="Writeability" /> value to be mapped.</param>
    /// <returns>
    /// An <see cref="MxAttributeCategory" /> that corresponds to the specified <paramref name="writeability" />
    /// value.
    /// </returns>
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
    /// Converts a specified <see cref="MxAttributeCategory" /> to its corresponding <see cref="Writeability" /> value.
    /// </summary>
    /// <param name="category">The <see cref="MxAttributeCategory" /> to be converted.</param>
    /// <returns>The corresponding <see cref="Writeability" /> value for the given <paramref name="category" />.</returns>
    /// <exception cref="category">Thrown when the <paramref name="category" /> value is not supported or recognized.</exception>
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
    /// Converts the specified <paramref name="security" /> value from <see cref="Security" />
    /// to the corresponding <see cref="MxSecurityClassification" /> enumeration value.
    /// </summary>
    /// <param name="security">
    /// The <see cref="Security" /> value representing the security classification level to convert.
    /// </param>
    /// <returns>
    /// A <see cref="MxSecurityClassification" /> enumeration value that corresponds to the specified
    /// <paramref name="security" /> level. Returns <see cref="MxSecurityClassification.MxSecurityUndefined" />
    /// if the provided <paramref name="security" /> value does not match any defined classification.
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
    /// Maps a <see cref="MxSecurityClassification" /> instance to its corresponding <see cref="Security" /> value.
    /// </summary>
    /// <param name="security">The <see cref="MxSecurityClassification" /> to be converted.</param>
    /// <returns>A corresponding <see cref="Security" /> value if the classification is valid; otherwise, null.</returns>
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