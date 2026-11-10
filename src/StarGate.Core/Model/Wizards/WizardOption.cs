using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// </summary>
public class WizardOption
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
    public string? Default { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? When { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<WizardChoice> Choices { get; set; } = [];
}