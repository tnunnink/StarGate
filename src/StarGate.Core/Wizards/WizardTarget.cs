using System.Collections.Generic;
using StarGate.Core.Abstractions;

namespace StarGate.Core.Wizards;

public class WizardTarget : IMergeable<WizardTarget>
{
    public required WizardAssociation Kind { get; init; }

    public required string Name { get; init; }

    public bool? Trim { get; init; }

    public IEnumerable<WizardSetting> Settings { get; init; } = [];
    
    public WizardTarget Merge(WizardTarget? current)
    {
        throw new System.NotImplementedException();
    }
}