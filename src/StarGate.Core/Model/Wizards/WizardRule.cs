using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// 
/// </summary>
public class WizardRule
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string When { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<WizardTarget> Include { get; set; } = [];
}