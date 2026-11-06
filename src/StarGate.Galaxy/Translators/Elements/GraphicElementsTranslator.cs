using System.Collections.Generic;
using System.Xml.Linq;
using StarGate.Core;

namespace StarGate.Galaxy;

/// <summary>
/// A concrete implementation of <see cref="GraphicTranslator{T}"/> that is responsible for
/// translating collections of <see cref="GraphicElement"/> objects between their in-memory
/// representation and their XML equivalent.
/// </summary>
public class GraphicElementsTranslator : GraphicTranslator<IEnumerable<GraphicElement>>
{
    private static readonly TranslatorDispatcher<GraphicElement> Dispatcher = new();

    static GraphicElementsTranslator()
    {
        Dispatcher.Register("Rectangle", new RectangleTranslator());
    }

    /// <inheritdoc />
    public override IEnumerable<GraphicElement>? Read(XElement? target)
    {
        if (target is null) yield break;

        foreach (var element in target.Elements())
        {
            var graphic = Dispatcher.Read(element);
            
            if (graphic is not null)
                yield return graphic;
        }
    }

    /// <inheritdoc />
    public override XElement? Write(IEnumerable<GraphicElement>? source)
    {
        throw new System.NotImplementedException();
    }
}