using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StarGate.Galaxy;

/// <summary>
/// A dispatcher that manages the registration and delegation of translators for handling
/// derived types of a base type and their associated XML representations.
/// 
/// This class extends the functionality of the <see cref="GraphicTranslator{T}"/> class by
/// providing infrastructure for dynamic type translation based on runtime type or XML tag names.
/// </summary>
/// <typeparam name="TBase">The base type for all supported translation types.</typeparam>
public class TranslatorDispatcher<TBase> : GraphicTranslator<TBase>
{
    private readonly Dictionary<string, IGraphicTranslator> _tagMap = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<Type, IGraphicTranslator> _typeMap = new();

    /// <summary>
    /// Registers a translator for a specific derived type and its associated XML representation.
    /// </summary>
    /// <typeparam name="TDerived">The derived type of the object to be translated.</typeparam>
    /// <param name="xName">The XML tag name associated with the derived type.</param>
    /// <param name="translator">The translator responsible for handling the specified derived type.</param>
    public void Register<TDerived>(string xName, GraphicTranslator<TDerived> translator) where TDerived : TBase
    {
        _tagMap[xName] = translator;
        _typeMap[typeof(TDerived)] = translator;
    }

    /// <inheritdoc />
    public override TBase? Read(XElement? element)
    {
        if (element is null) return default;

        if (!_tagMap.TryGetValue(element.Name.LocalName, out var translator))
            return default;

        return (TBase?)translator.Read(element);
    }

    /// <inheritdoc />
    public override XElement? Write(TBase? source)
    {
        if (source is null) return null;

        if (!_typeMap.TryGetValue(source.GetType(), out var translator))
            return null;

        return translator.Write(source);
    }
}