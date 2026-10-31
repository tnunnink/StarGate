namespace StarGate.Core;

/// <summary>
/// Represents a log change extension that configures whether changes to an attribute's
/// value are recorded as data change events in the Galaxy log.
/// </summary>
[GalaxyExtension("LogDataChangeEventExtension", ExtensionType.Attribute)]
public class LoggingExtension : IGalaxyMergeable<LoggingExtension>, IAttributeExtension
{
    /// <summary>
    /// Gets the configuration that enables logging of a data change event whenever the
    /// attribute's value changes.
    /// </summary>
    [GalaxyAttribute("LogDataChangeEvent", Security.Tune)]
    public AttributeConfig? LogEvent { get; init; }

    /// <inheritdoc />
    public bool Supports(DataType dataType)
    {
        return dataType is DataType.Boolean || dataType.IsNumeric();
    }

    /// <inheritdoc />
    public LoggingExtension Merge(LoggingExtension? current)
    {
        if (current is null) return this;

        return new LoggingExtension
        {
            LogEvent = LogEvent?.Merge(current.LogEvent) ?? current.LogEvent
        };
    }
}