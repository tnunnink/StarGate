namespace StarGate.Core;

/// <summary>
/// Represents an error related to galaxy-level operations within the StarGate system.
/// Encapsulates an error code and associated human-readable message for diagnostics.
/// </summary>
public class GalaxyError(ErrorCode code, string message)
{
    /// <summary>
    /// Gets the error code associated with the galaxy operation failure.
    /// </summary>
    public ErrorCode Code { get; } = code;

    /// <summary>
    /// Gets the human-readable diagnostic message describing the failure.
    /// </summary>
    public string Message { get; } = message;
}