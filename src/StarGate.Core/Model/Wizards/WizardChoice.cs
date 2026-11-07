namespace StarGate.Core;

/// <summary>
/// 
/// </summary>
public record WizardChoice
{
    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? Comment { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? When { get; init; }
}