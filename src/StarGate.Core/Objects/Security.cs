namespace StarGate.Core.Objects;

/// <summary>
/// Defines the security classification levels and authorization requirements for accessing and modifying attributes.
/// </summary>
public enum Security
{
    /// <summary>
    /// Represents an undefined security classification, indicating that no specific security level has been assigned.
    /// </summary>
    Undefined = -1,

    /// <summary>
    /// Operational access level, permitting routine operator control actions.
    /// </summary>
    Operate = 0,

    /// <summary>
    /// Unrestricted access level, requiring no special security privileges for reading or writing.
    /// </summary>
    FreeAccess = 1,

    /// <summary>
    /// Secured write access level, requiring standard user authentication before modifications are permitted.
    /// </summary>
    SecuredWrite = 2,

    /// <summary>
    /// Verified write access level, requiring confirmation or dual authentication before modifications are applied.
    /// </summary>
    VerifiedWrite = 3,

    /// <summary>
    /// Tuning access level, permitting engineers and technicians to adjust tuning parameters.
    /// </summary>
    Tune = 4,

    /// <summary>
    /// Configuration access level, requiring administrative or engineering privileges to alter configuration settings.
    /// </summary>
    Configure = 5,

    /// <summary>
    /// Read-only access level, preventing any modifications to the attribute.
    /// </summary>
    ViewOnly = 6
}