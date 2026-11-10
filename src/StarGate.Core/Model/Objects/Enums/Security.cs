namespace StarGate.Core;

/// <summary>
/// Defines the security classification levels and authorization requirements for accessing and modifying attributes.
/// </summary>
public enum Security
{
    /// <summary>
    /// Default security classification, applying the standard authorization behavior when no specific access level is assigned.
    /// </summary>
    /// <remarks>
    /// This does not represent a real ArchestrA security option. It exists to support mapping default security
    /// options for attributes to a common value that we can omit in the YAML serialization model.
    /// </remarks>
    Default,

    /// <summary>
    /// Operational access level, permitting routine operator control actions.
    /// </summary>
    Operate,

    /// <summary>
    /// Unrestricted access level, requiring no special security privileges for reading or writing.
    /// </summary>
    FreeAccess,

    /// <summary>
    /// Secured write access level, requiring standard user authentication before modifications are permitted.
    /// </summary>
    SecuredWrite,

    /// <summary>
    /// Verified write access level, requiring confirmation or dual authentication before modifications are applied.
    /// </summary>
    VerifiedWrite,

    /// <summary>
    /// Tuning access level, permitting engineers and technicians to adjust tuning parameters.
    /// </summary>
    Tune,

    /// <summary>
    /// Configuration access level, requiring administrative or engineering privileges to alter configuration settings.
    /// </summary>
    Configure,

    /// <summary>
    /// Read-only access level, preventing any modifications to the attribute.
    /// </summary>
    ViewOnly
}