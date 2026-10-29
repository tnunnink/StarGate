using StarGate.Core.Abstractions;
using StarGate.Core.Attributes;
using StarGate.Core.Objects;

namespace StarGate.Core.Extensions;

/// <summary>
/// Represents an extension for handling "bad value" configurations, allowing
/// merging capabilities using the <see cref="IMergeable{T}"/> interface.
/// This class provides support for managing descriptive messages and priority attributes
/// associated with "bad values."
/// </summary>
[GalaxyExtension("BadValueAlarmExtension", ExtensionType.Attribute)]
public class QualityAlarmExtension : IMergeable<QualityAlarmExtension>, IAttributeExtension
{
    /// <summary>
    /// Gets the configuration for the name of the attribute whose value is used as the
    /// alarm message or description.
    /// </summary>
    [GalaxyAttribute("Bad.DescAttrName", Security.FreeAccess)]
    public AttributeConfig? Message { get; init; }

    /// <summary>
    /// Gets the configuration for the alarm priority, which indicates its severity or importance.
    /// </summary>
    [GalaxyAttribute("Bad.Priority", Security.Tune)]
    public AttributeConfig? Priority { get; init; }

    /// <inheritdoc />
    public QualityAlarmExtension Merge(QualityAlarmExtension? current)
    {
        if (current is null) return this;

        return new QualityAlarmExtension
        {
            Priority = Priority?.Merge(current.Priority) ?? current.Priority,
            Message = Message?.Merge(current.Message) ?? current.Message
        };
    }

    /// <inheritdoc />
    public bool Supports(DataType dataType) => dataType is DataType.Boolean || dataType.IsNumeric();
}