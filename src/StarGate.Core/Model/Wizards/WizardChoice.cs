namespace StarGate.Core;

public class WizardChoice
{
    public required string Name { get; init; }

    public string? Comment { get; set; }

    public string? When { get; set; }
}