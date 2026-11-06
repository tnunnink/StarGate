using System;
using System.Collections.Generic;

namespace StarGate.Core;

public class WizardRule
{
    public required string When { get; init; }

    public string? Name { get; init; }

    public IEnumerable<WizardTarget> Include { get; init; } = [];
}