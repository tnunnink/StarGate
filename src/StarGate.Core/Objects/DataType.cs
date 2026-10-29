namespace StarGate.Core.Objects;

/// <summary>
/// Specifies the supported data types for attributes and values within the Galaxy system.
/// </summary>
public enum DataType
{
    /// <summary>
    /// Represents a boolean (true or false) value.
    /// </summary>
    Boolean,

    /// <summary>
    /// Represents a signed integer numeric value.
    /// </summary>
    Integer,

    /// <summary>
    /// Represents a single-precision floating-point numeric value.
    /// </summary>
    Float,

    /// <summary>
    /// Represents a double-precision floating-point numeric value.
    /// </summary>
    Double,

    /// <summary>
    /// Represents a standard character string value.
    /// </summary>
    String,

    /// <summary>
    /// Represents an absolute date and time value.
    /// </summary>
    Time,

    /// <summary>
    /// Represents a span of elapsed time or duration.
    /// </summary>
    ElapsedTime,

    /// <summary>
    /// Represents an internationalized or localized text string value.
    /// </summary>
    InternationalizedString,

    /// <summary>
    /// Represents an extended or large text string value.
    /// </summary>
    BigString
}

/// <summary>
/// Provides extension methods for the <see cref="DataType"/> enumeration, enabling additional functionality
/// and operations based on the nature of data types.
/// </summary>
public static class DataTypeExtensions
{
    /// <summary>
    /// Determines whether the specified <see cref="DataType"/> is numeric.
    /// A numeric data type includes Integer, Float, and Double.
    /// </summary>
    /// <param name="dataType">The <see cref="DataType"/> to check.</param>
    /// <returns>
    /// Returns <c>true</c> if the <paramref name="dataType"/> is numeric; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNumeric(this DataType dataType)
    {
        return dataType is DataType.Integer or DataType.Float or DataType.Double;
    }

    /// <summary>
    /// Determines whether the specified <see cref="DataType"/> is textual.
    /// A textual data type includes String, BigString, and InternationalizedString.
    /// </summary>
    /// <param name="dataType">The <see cref="DataType"/> to check.</param>
    /// <returns>
    /// Returns <c>true</c> if the <paramref name="dataType"/> is textual; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsTextual(this DataType dataType)
    {
        return dataType is DataType.String or DataType.BigString or DataType.InternationalizedString;
    }
}