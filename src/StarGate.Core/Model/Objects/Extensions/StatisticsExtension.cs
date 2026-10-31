namespace StarGate.Core;

/// <summary>
/// Represents an extension that provides statistical configurations and operations
/// for managing and interacting with Galaxy-specific data.
/// </summary>
public class StatisticsExtension : IAttributeExtension, IGalaxyMergeable<StatisticsExtension>
{
    /// <summary>
    /// Gets the configuration for automatic reset behavior within the statistical operations
    /// and attribute management of a galaxy-specific extension.
    /// </summary>
    public AttributeConfig? AutoReset { get; init; }

    /// <summary>
    /// Represents the configuration for the size of data samples used in statistical computations
    /// and analysis within the context of Galaxy-specific extension operations.
    /// </summary>
    public AttributeConfig? SampleSize { get; init; }

    /// <inheritdoc />
    public bool Supports(DataType dataType)
    {
        return dataType is DataType.Boolean || dataType.IsNumeric();
    }

    /// <inheritdoc />
    public StatisticsExtension Merge(StatisticsExtension? current)
    {
        if (current is null) return this;

        return new StatisticsExtension
        {
            AutoReset = AutoReset?.Merge(current.AutoReset) ?? current.AutoReset,
            SampleSize = SampleSize?.Merge(current.SampleSize) ?? current.SampleSize
        };
    }
}