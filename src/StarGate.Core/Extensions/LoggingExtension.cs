using StarGate.Core.Abstractions;
using StarGate.Core.Attributes;
using StarGate.Core.Objects;

namespace StarGate.Core.Extensions;

/// <summary>
/// Represents a log change extension that configures whether changes to an attribute's
/// value are recorded as data change events in the Galaxy log.
/// </summary>
[GalaxyExtension("LogDataChangeEventExtension", ExtensionType.Attribute)]
public class LoggingExtension : IMergeable<LoggingExtension>, IAttributeExtension
{
    /// <summary>
    /// Gets the configuration that enables logging of a data change event whenever the
    /// attribute's value changes.
    /// </summary>
    [GalaxyAttribute("LogDataChangeEvent", Security.Tune)]
    public AttributeConfig? LogEvent { get; init; }

    /// <inheritdoc />
    public LoggingExtension Merge(LoggingExtension? current)
    {
        if (current is null) return this;

        return new LoggingExtension
        {
            LogEvent = LogEvent?.Merge(current.LogEvent) ?? current.LogEvent
        };
    }

    /// <inheritdoc />
    public bool Supports(DataType dataType) => dataType is DataType.Boolean || dataType.IsNumeric();
}