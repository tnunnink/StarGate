namespace StarGate.Core.Wizards;

public class WizardSetting
{
    public required string Name { get; init; }

    public required object Value { get; init; }

    public bool? Visible { get; init; }
}