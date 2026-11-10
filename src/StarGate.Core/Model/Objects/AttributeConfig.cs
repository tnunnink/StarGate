namespace StarGate.Core;

/// <summary>
/// Represents configuration settings for an object attribute, including its value, lock state, and security
/// permissions.
/// </summary>
public class AttributeConfig
{
    /// <summary>
    /// Gets or sets the configured value of the attribute.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the attribute is locked against modification.
    /// </summary>
    public bool Locked { get; set; }

    /// <summary>
    /// Gets or sets the security access level required for the attribute.
    /// </summary>
    public Security Security { get; set; }
}