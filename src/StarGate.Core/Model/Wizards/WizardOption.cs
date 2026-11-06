using System;
using System.Collections.Generic;

namespace StarGate.Core;

/// <summary>
/// </summary>
public class WizardOption
{
    public required string Name { get; init; }

    public string? Comment { get; init; }

    public string? Default { get; init; }

    public string? When { get; init; }

    public List<WizardChoice> Choices { get; init; } = [];
}