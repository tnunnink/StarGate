using System;
using System.Collections.Generic;
using System.Linq;
using StarGate.Core.Graphics.Controls;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace StarGate.Core.Serialization.Converters;

/// <summary>
/// Provides custom YAML serialization and deserialization for <see cref="ItemBinding"/> types,
/// supporting compact flow sequences for static items, scalar keywords for enums, and mappings for array references.
/// </summary>
public class ItemBindingConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => typeof(ItemBinding).IsAssignableFrom(type);

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        // 1. Scalar keyword (e.g. "enum")
        if (parser.TryConsume<Scalar>(out var scalar))
        {
            if (string.Equals(scalar.Value, "enum", StringComparison.OrdinalIgnoreCase))
            {
                return new EnumItemBinding();
            }

            // Fallback: single scalar treated as single item
            return new StaticItemBinding([new ControlItem { Value = scalar.Value, Caption = scalar.Value }]);
        }

        // 2. Sequence (e.g. [ "Off", "Manual", "Auto" ] or block sequence of items)
        if (parser.Accept<SequenceStart>(out _))
        {
            var items = (List<object>)rootDeserializer(typeof(List<object>))!;
            var controlItems = new List<ControlItem>();

            foreach (var item in items)
            {
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
            }

            return new StaticItemBinding(controlItems);
        }

        // 3. Mapping (e.g. { array: "Plant.RecipeNames" })
        if (parser.Accept<MappingStart>(out _))
        {
            var dict = (Dictionary<string, string>)rootDeserializer(typeof(Dictionary<string, string>))!;

            if (dict.TryGetValue("array", out var arrayRef) || dict.TryGetValue("Array", out arrayRef))
            {
                return new ArrayItemBinding(arrayRef);
            }
        }

        return null;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        switch (value)
        {
            case EnumItemBinding:
                emitter.Emit(new Scalar("enum"));
                break;

            case ArrayItemBinding arrayBinding:
                emitter.Emit(new MappingStart(null, null, isImplicit: true, MappingStyle.Flow));
                emitter.Emit(new Scalar("array"));
                emitter.Emit(new Scalar(arrayBinding.Reference));
                emitter.Emit(new MappingEnd());
                break;

            case StaticItemBinding staticBinding:
                var items = staticBinding.Items.ToList();

                // If all items have identical Value and Caption, format as flow sequence: [A, B, C]
                if (items.All(i => i.Value == i.Caption))
                {
                    emitter.Emit(new SequenceStart(null, null, isImplicit: true, SequenceStyle.Flow));
                    foreach (var item in items)
                    {
                        emitter.Emit(new Scalar(item.Caption));
                    }
                    emitter.Emit(new SequenceEnd());
                }
                else
                {
                    // Full mapping sequence when value differs from caption:
                    emitter.Emit(new SequenceStart(null, null, isImplicit: true, SequenceStyle.Block));
                    foreach (var item in items)
                    {
                        emitter.Emit(new MappingStart(null, null, isImplicit: true, MappingStyle.Flow));
                        emitter.Emit(new Scalar("value"));
                        emitter.Emit(new Scalar(item.Value));
                        emitter.Emit(new Scalar("caption"));
                        emitter.Emit(new Scalar(item.Caption));
                        emitter.Emit(new MappingEnd());
                    }
                    emitter.Emit(new SequenceEnd());
                }
                break;
        }
    }
}
