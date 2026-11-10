using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// 
/// </summary>
public class WizardSetting
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [Required]
    public object Value { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public bool Visible { get; set; }
}