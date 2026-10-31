using System;
using System.Collections.Generic;

namespace StarGate.Core;

public class WizardRule : IGalaxyMergeable<WizardRule>
{
    public required string When { get; init; }

    public string? Name { get; init; }

    public IEnumerable<WizardTarget> Include { get; init; } = [];

    public WizardRule Merge(WizardRule? current)
    {
        throw new NotImplementedException();
    }
}