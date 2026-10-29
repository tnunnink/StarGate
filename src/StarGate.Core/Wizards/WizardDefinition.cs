using System.Collections.Generic;
using StarGate.Core.Abstractions;

namespace StarGate.Core.Wizards;

public class WizardDefinition : IMergeable<WizardDefinition>
{
    public IEnumerable<WizardOption> Options { get; init; } = [];
    
    public IEnumerable<WizardRule> Rules { get; init; } = [];
    
    public WizardDefinition Merge(WizardDefinition? current)
    {
        throw new System.NotImplementedException();
    }
}