using System.Collections.Generic;

namespace StarGate.Core.Graphics.Controls;

/// <summary>
/// A fixed, static list of selectable items configured manually.
/// </summary>
public class StaticItemBinding(IEnumerable<ControlItem> items) : ItemBinding
{
    public IEnumerable<ControlItem> Items { get; init; } = items;
}