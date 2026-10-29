using System.Collections.Generic;
using StarGate.Core.Abstractions;

namespace StarGate.Core.Wizards;

/// <summary>
/// 
/// </summary>
public class WizardOption : IMergeable<WizardOption>
{
    public required string Name { get; init; }

    public string? Comment { get; init; }

    public string? Default { get; init; }

    public string? When { get; init; }

    public List<WizardChoice> Choices { get; init; } = [];

    /// <inheritdoc />
    public WizardOption Merge(WizardOption? current)
    {
        throw new System.NotImplementedException();
    }
}