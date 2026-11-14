using System;
using System.ComponentModel;

namespace StarGate.Core;

/// <summary>
/// Represents a month calendar grid control for displaying and selecting dates, supporting multi-month layouts and
/// bolded dates.
/// </summary>
public class CalendarElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.Calendar;

    /// <summary>
    /// Gets the default initial date value displayed on the calendar.
    /// </summary>
    public DateTime? Default { get; set; }

    /// <summary>
    /// Gets the number of month rows displayed simultaneously in the calendar grid.
    /// </summary>
    [DefaultValue(1)]
    public int Rows { get; set; } = 1;

    /// <summary>
    /// Gets the number of month columns displayed simultaneously in the calendar grid.
    /// </summary>
    [DefaultValue(1)]
    public int Columns { get; set; } = 1;

    /// <summary>
    /// Gets a value indicating whether the current date indicator is displayed at the bottom of the control.
    /// </summary>
    [DefaultValue(true)]
    public bool ShowToday { get; set; } = true;

    /// <summary>
    /// Gets the first day of the week displayed in the calendar header, or <see langword="null" /> for system default.
    /// </summary>
    public DayOfWeek? FirstDay { get; set; }

    /// <summary>
    /// Gets the two-way data binding linking the selected date to a Galaxy tag or attribute reference.
    /// </summary>
    public ValueBinding? Binding { get; set; }

    /// <summary>
    /// Gets the minimum selectable date, either as a literal date string or dynamic expression.
    /// </summary>
    public GraphicExpression? MinDate { get; set; }

    /// <summary>
    /// Gets the maximum selectable date, either as a literal date string or dynamic expression.
    /// </summary>
    public GraphicExpression? MaxDate { get; set; }

    /// <summary>
    /// Gets the Galaxy tag reference pointing to an array of dates to render in bold font.
    /// </summary>
    public string? BoldDates { get; set; }

    /// <summary>
    /// Gets the background fill color of the calendar title bar.
    /// </summary>
    public GraphicColor? TitleBackColor { get; set; }

    /// <summary>
    /// Gets the text color of the calendar title bar.
    /// </summary>
    public GraphicColor? TitleForeColor { get; set; }

    /// <summary>
    /// Gets the text color of trailing dates from preceding and succeeding months.
    /// </summary>
    public GraphicColor? TrailingForeColor { get; set; }
}