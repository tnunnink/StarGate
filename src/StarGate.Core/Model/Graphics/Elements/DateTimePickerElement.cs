using System;
using System.ComponentModel;

namespace StarGate.Core;

/// <summary>
/// Represents a date and time picker control allowing users to select or display date and time values.
/// </summary>
public record DateTimePickerElement : GraphicElement
{
    /// <inheritdoc />

    public override ElementKind Kind => ElementKind.DateTimePicker;

    /// <summary>
    /// Gets the default initial date and time value displayed in the control.
    /// </summary>
    public DateTime? Default { get; init; }

    /// <summary>
    /// Gets the format string applied to the date and time display representation.
    /// </summary>
    public string? Format { get; init; }

    /// <summary>
    /// Gets the two-way data binding linking the selected date and time to a Galaxy tag or attribute reference.
    /// </summary>
    public ValueBinding? Binding { get; init; }

    /// <summary>
    /// Gets or initializes the minimum allowable date value for the DateTimePicker element.
    /// This defines the earliest date that can be selected or displayed.
    /// </summary>
    public GraphicExpression? MinDate { get; init; }

    /// <summary>
    /// Specifies the maximum allowable date for the <see cref="DateTimePickerElement" />.
    /// Represents a graphical reference that can define the maximum date as either a literal text
    /// value or a dynamic reference expression.
    /// </summary>
    public GraphicExpression? MaxDate { get; init; }

    /// <summary>
    /// Gets the background color displayed within the drop-down month calendar.
    /// </summary>
    [DefaultValue("#ffffff")]
    public GraphicColor? MonthBackColor { get; init; }

    /// <summary>
    /// Gets the foreground text color displayed within the drop-down month calendar.
    /// </summary>
    [DefaultValue("#6d6d6d")]
    public GraphicColor? MonthTrailingForeColor { get; init; }

    /// <summary>
    /// Gets the background color of the drop-down calendar title bar.
    /// </summary>
    [DefaultValue("#99b4d1")]
    public GraphicColor? TitleBackColor { get; init; }

    /// <summary>
    /// Gets the foreground text color of the drop-down calendar title bar.
    /// </summary>
    [DefaultValue("#000000")]
    public GraphicColor? TitleForeColor { get; init; }
}