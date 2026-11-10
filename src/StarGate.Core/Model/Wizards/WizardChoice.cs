using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// 
/// </summary>
public class WizardChoice
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? When { get; set; }
}