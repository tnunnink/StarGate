using System;
using System.Collections.Generic;

namespace StarGate.Core;

public class WizardDefinition : IGalaxyMergeable<WizardDefinition>
{
    public IEnumerable<WizardOption> Options { get; init; } = [];

    public IEnumerable<WizardRule> Rules { get; init; } = [];

    public WizardDefinition Merge(WizardDefinition? current)
    {
        throw new NotImplementedException();
    }
}