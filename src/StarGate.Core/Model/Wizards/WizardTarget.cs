using System.Collections.Generic;

namespace StarGate.Core;

/// <summary>
/// 
/// </summary>
public record WizardTarget
{
    /// <summary>
    /// 
    /// </summary>
    public required WizardAssociation Kind { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public bool? Trim { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<WizardSetting> Settings { get; init; } = [];
}