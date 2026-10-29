using StarGate.Core.Abstractions;
using StarGate.Core.Attributes;
using StarGate.Core.Extensions;

namespace StarGate.Core.Objects;

/// <summary>
/// Represents an attribute of an object, including its data type, configuration values, and optional extensions.
/// </summary>
public class ObjectAttribute : IMergeable<ObjectAttribute>
{
    /// <summary>
    /// Gets or sets the name of the attribute.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets or sets the data type of the attribute.
    /// </summary>
    public DataType? DataType { get; init; }

    /// <summary>
    /// Gets or sets the length of the attribute array, where a value greater than 0 indicates an array.
    /// </summary>
    public short? Length { get; init; }

    /// <summary>
    /// Gets the writeability category of the attribute, indicating its modification behavior,
    /// such as whether it is user-writeable, object-writeable, or calculated.
    /// </summary>
    public Writeability? Writeability { get; init; }

    /// <summary>
    /// Gets or sets the configuration value and settings for the attribute.
    /// </summary>
    [GalaxyAttribute("", Security.Operate)]
    public AttributeConfig? Value { get; init; }

    /// <summary>
    /// Gets or sets the description configuration for the attribute.
    /// </summary>
    [GalaxyAttribute("Description", Security.Operate)]
    public AttributeConfig? Description { get; init; }

    /// <summary>
    /// Gets or sets the engineering units configuration for the attribute.
    /// </summary>
    [GalaxyAttribute("EngUnits", Security.Operate)]
    public AttributeConfig? Units { get; init; }

    /// <summary>
    /// Gets or sets the input/output extension configuration for the attribute.
    /// </summary>
    public IOExtension? IO { get; init; }

    /// <summary>
    /// Gets or sets the historical data extension that defines various configuration
    /// attributes and behaviors for managing historical data.
    /// </summary>
    public HistoryExtension? History { get; init; }

    /// <summary>
    /// Gets or sets the configuration for alarms associated with this attribute.
    /// This property allows for defining various types of alarms, including state,
    /// limit, rate of change, deviation, and quality alarms.
    /// </summary>
    public AlarmExtension? Alarms { get; init; }

    /// <summary>
    /// Gets or sets the statistics extension, which provides configurations
    /// and operations for managing statistical data related to the object attribute.
    /// </summary>
    public StatisticsExtension? Stats { get; init; }

    /// <summary>
    /// Gets or sets the log change extension for the object attribute, which defines
    /// whether changes to the attribute's value are recorded as data change events
    /// in the Galaxy log.
    /// </summary>
    public LoggingExtension? Log { get; init; }

    /// <inheritdoc />
    public ObjectAttribute Merge(ObjectAttribute? current)
    {
        if (current is null) return this;

        return new ObjectAttribute
        {
            Name = Name,
            DataType = DataType ?? current.DataType,
            Length = Length ?? current.Length,
            Writeability = Writeability ?? current.Writeability,
            Value = Value?.Merge(current.Value) ?? current.Value,
            Description = Description?.Merge(current.Description) ?? current.Description,
            Units = Units?.Merge(current.Units) ?? current.Units,
            IO = IO?.Merge(current.IO) ?? current.IO,
            History = History?.Merge(current.History) ?? current.History,
            Alarms = Alarms?.Merge(current.Alarms) ?? current.Alarms,
            Log = Log?.Merge(current.Log) ?? current.Log,
            Stats = Stats?.Merge(current.Stats) ?? current.Stats
        };
    }
}