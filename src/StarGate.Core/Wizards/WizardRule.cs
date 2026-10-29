using System.Collections.Generic;
using StarGate.Core.Abstractions;

namespace StarGate.Core.Wizards;

public class WizardRule : IMergeable<WizardRule>
{
    public required string When { get; init; }
    
    public string? Name { get; init; }

    public IEnumerable<WizardTarget> Include { get; init; } = [];
    
    public WizardRule Merge(WizardRule? current)
    {
        throw new System.NotImplementedException();
    }
}