using System.Collections.Generic;
using System.Linq;

namespace StarGate.Core.Graphics.Controls;

/// <summary>
/// Abstract base class defining the source of selectable items for selector controls (Static, Enum, or Array).
/// </summary>
public abstract class ItemBinding
{
    /// <summary>
    /// Implicitly converts a list of strings or <see cref="ControlItem"/> objects into a static item source.
    /// </summary>
    public static implicit operator ItemBinding(ControlItem[] items)
    {
        return new StaticItemBinding(items);
    }

    /// <summary>
    /// Implicitly converts a list of strings into a static item source.
    /// </summary>
    public static implicit operator ItemBinding(List<string> items)
    {
        return new StaticItemBinding(items.Select(x => (ControlItem)x).ToArray());
    }
}