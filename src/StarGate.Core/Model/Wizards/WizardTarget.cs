using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// 
/// </summary>
public class WizardTarget
{
    /// <summary>
    /// 
    /// </summary>
    public WizardAssociation Kind { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public bool Trim { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<WizardSetting> Settings { get; set; } = [];
}