namespace StarGate.Core.Serialization;

/// <summary>
/// Provides configuration options for customizing object serialization behavior.
/// </summary>
public class SerializationOptions
{
    /// <summary>
    /// Omits attribute-specific security configuration enum from all attribute values.
    /// </summary>
    public bool OmitSecurity { get; init; } = true;

    /// <summary>
    /// Omits attribute-specific locked configuration from all attribute serialization.
    /// </summary>
    public bool OmitLocked { get; init; }
}