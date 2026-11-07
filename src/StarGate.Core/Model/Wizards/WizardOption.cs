using System.Collections.Generic;

namespace StarGate.Core;

/// <summary>
/// </summary>
public record WizardOption
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
    public string? Default { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? When { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<WizardChoice> Choices { get; init; } = [];
}