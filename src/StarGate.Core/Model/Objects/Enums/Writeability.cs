namespace StarGate.Core;

/// <summary>
/// Specifies the writeability category and modification behavior for an attribute.
/// </summary>
public enum Writeability
{
    /// <summary>
    /// Attribute is writable by users, scripts, and communications, and can be locked (default writeability).
    /// </summary>
    /// <remarks>
    /// This corresponds to MxCategoryWriteable_USC_Lockable
    /// </remarks>
    UserWriteable,

    /// <summary>
    /// Attribute is writable by internal object logic and can be locked, but is restricted from direct user modification.
    /// </summary>
    /// <remarks>
    /// This corresponds to MxCategoryWriteable_C_Lockable
    /// </remarks>
    ObjectWriteable,

    /// <summary>
    /// Attribute value is calculated at runtime and is not retained across restarts.
    /// </summary>
    /// <remarks>
    /// This corresponds to MxCategoryCalculated
    /// </remarks>
    Calculated,

    /// <summary>
    /// Attribute value is calculated at runtime and retained across restarts.
    /// </summary>
    /// <remarks>
    /// This corresponds to MxCategoryCalculatedRetentive
    /// </remarks>
    CalculatedRetentive
}