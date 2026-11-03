using System;
using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

/// <summary>
/// 
/// </summary>
public class GalaxySymbolHandler : IConfigurationHandler<GalaxySymbol>
{
    private static readonly SymbolTranslator SymbolTranslator = new();

    public GalaxySymbol Extract(IgObject target, string? context = null)
    {
        var xml = target.ReadSymbol();

        var root = xml.Root ?? throw new InvalidOperationException("");

        // For some reason ArchestrA does not set the symbol name, but we will so that we can
        // just use the symbol translators. Also, embed any additional content as needed since
        // the XML here is transitory.
        root.SetAttributeValue("Name", target.Tagname);

        return SymbolTranslator.Read(xml.Root);
    }

    public GalaxyResult Apply(IgObject target, GalaxySymbol? source, string? context = null)
    {
        throw new System.NotImplementedException();
    }
}