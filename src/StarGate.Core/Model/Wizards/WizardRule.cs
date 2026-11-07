using System.Collections.Generic;

namespace StarGate.Core;

/// <summary>
/// 
/// </summary>
public record WizardRule
{
    /// <summary>
    /// 
    /// </summary>
    public required string When { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<WizardTarget> Include { get; init; } = [];
}