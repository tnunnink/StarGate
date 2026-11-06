using System;
using System.Collections.Generic;

namespace StarGate.Core;

public class WizardTarget
{
    public required WizardAssociation Kind { get; init; }

    public required string Name { get; init; }

    public bool? Trim { get; init; }

    public IEnumerable<WizardSetting> Settings { get; init; } = [];
}