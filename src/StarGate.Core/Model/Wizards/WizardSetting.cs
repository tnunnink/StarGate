namespace StarGate.Core;

/// <summary>
/// 
/// </summary>
public record WizardSetting
{
    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required object Value { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public bool Visible { get; init; }
}