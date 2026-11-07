using System.Collections.Generic;

namespace StarGate.Core;

public record WizardDefinition
{
    public IEnumerable<WizardOption> Options { get; init; } = [];

    public IEnumerable<WizardRule> Rules { get; init; } = [];
}