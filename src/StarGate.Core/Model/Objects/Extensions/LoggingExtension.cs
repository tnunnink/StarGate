namespace StarGate.Core;

/// <summary>
/// Represents a log change extension that configures whether changes to an attribute's
/// value are recorded as data change events in the Galaxy log.
/// </summary>
[GalaxyExtension("LogDataChangeEventExtension", ExtensionType.Attribute)]
public class LoggingExtension : IAttributeExtension
{
    /// <summary>
    /// Gets the configuration that enables logging of a data change event whenever the
    /// attribute's value changes.
    /// </summary>
    [GalaxyAttribute("LogDataChangeEvent", Security.Tune)]
    public AttributeConfig? LogEvent { get; init; }

    /// <inheritdoc />
    public bool Supports(AttributeDataType dataType)
    {
        return dataType is AttributeDataType.Boolean || dataType.IsNumeric();
    }
}