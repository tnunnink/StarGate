using System.Collections.Generic;

namespace StarGate.Core;

public class WizardDefinition
{
    public IEnumerable<WizardOption> Options { get; set; } = [];

    public IEnumerable<WizardRule> Rules { get; set; } = [];
}