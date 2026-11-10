using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Provides custom YAML serialization and deserialization for <see cref="ItemBinding" /> types,
/// supporting compact flow sequences for static items, scalar keywords for enums, and mappings for array references.
/// </summary>
public class ItemBindingConverter : IYamlTypeConverter
{
    /// <inheritdoc />
    public bool Accepts(Type type) => typeof(ItemBinding).IsAssignableFrom(type);

    /// <inheritdoc />
    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        // 1. Scalar keyword (e.g. "enum")
        if (parser.TryConsume<Scalar>(out var scalar))
        {
            if (string.Equals(scalar.Value, "enum", StringComparison.OrdinalIgnoreCase))
                return new EnumItemBinding();

            // Fallback: single scalar treated as single item
            return new StaticItemBinding([new ControlItem { Value = scalar.Value, Caption = scalar.Value }]);
        }

        // 2. Sequence (e.g. [ "Off", "Manual", "Auto" ] or block sequence of items)
        if (parser.Accept<SequenceStart>(out _))
        {
            var items = (List<object>)rootDeserializer(typeof(List<object>))!;
            var controlItems = new List<ControlItem>();

            foreach (var item in items)
                switch (item)
                {
                    case string text:
                        controlItems.Add(new ControlItem { Value = text, Caption = text });
                        break;
                    case IDictionary<object, object> dict:
                        var val = dict.TryGetValue("value", out var v) ? v?.ToString() ?? string.Empty : string.Empty;
                        var cap = dict.TryGetValue("caption", out var c) ? c?.ToString() ?? val : val;
                        controlItems.Add(new ControlItem { Value = val, Caption = cap });
                        break;
                }

            return new StaticItemBinding(controlItems);
        }

        // 3. Mapping (e.g. { array: "Plant.RecipeNames" })
        if (parser.Accept<MappingStart>(out _))
        {
            var dict = (Dictionary<string, string>)rootDeserializer(typeof(Dictionary<string, string>))!;

            if (dict.TryGetValue("array", out var arrayRef) || dict.TryGetValue("Array", out arrayRef))
                return new ArrayItemBinding(arrayRef);
        }

        return null;
    }

    /// <inheritdoc />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        switch (value)
        {
            case EnumItemBinding:
                emitter.Emit("enum");
                break;

            case ArrayItemBinding arrayBinding:
                emitter.EmitMap(e => e.Emit("array", arrayBinding.Reference), MappingStyle.Flow);
                break;

            case StaticItemBinding staticBinding:
                var items = staticBinding.Items.ToList();

                // Compact flow sequence [A, B, C] if values match captions
                if (items.All(i => i.Value == i.Caption))
                {
                    emitter.EmitSequence(e =>
                    {
                        foreach (var item in items)
                            e.Emit(item.Caption);
                    });
                }
                else
                {
                    emitter.EmitSequence(e =>
                    {
                        foreach (var item in items)
                        {
                            e.EmitMap(m =>
                            {
                                m.Emit("value", item.Value);
                                m.Emit("caption", item.Caption);
                            }, MappingStyle.Flow);
                        }
                    }, SequenceStyle.Block);
                }

                break;
        }
    }
}