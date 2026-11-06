using System;
using System.Xml.Linq;
using StarGate.Core;

namespace StarGate.Galaxy;

/// <summary>
/// Translates instances of <see cref="GalaxySymbol"/> between domain objects and their XML representations.
/// </summary>
/// <remarks>
/// This implementation provides the logic to transform a <see cref="GalaxySymbol"/> object into its
/// XML representation and vice versa, enabling integration between the system's internal structures
/// and external configurations or data exchange formats.
/// </remarks>
public class SymbolTranslator : GraphicTranslator<GalaxySymbol>
{
    private static readonly GraphicElementsTranslator ElementsTranslator = new();

    /// <inheritdoc />
    public override GalaxySymbol Read(XElement? target)
    {
        if (target is null)
            throw new ArgumentNullException(nameof(target));

        return new GalaxySymbol
        {
            TagName = target.GetRequiredValue("@Name"),
            Folder = target.GetValue("@Folder"),
            Elements = ElementsTranslator.Read(target.Element("GraphicElements")) ?? [],
        };
    }

    /// <inheritdoc />
    public override XElement? Write(GalaxySymbol? source)
    {
        throw new System.NotImplementedException();
    }
}