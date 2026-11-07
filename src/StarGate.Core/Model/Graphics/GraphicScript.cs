using System.ComponentModel;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a script associated with a graphic symbol or element that executes custom logic based on specified trigger events.
/// </summary>
public record GraphicScript
{
    /// <summary>
    /// Gets the user-defined name of the script. 
    /// Can be omitted or null for predefined lifecycle scripts (OnShow, OnHide, WhileShowing).
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets the trigger condition that defines when the script executes.
    /// </summary>
    public ScriptTrigger Trigger { get; init; } = ScriptTrigger.WhileTrue;

    /// <summary>
    /// Gets the expression evaluated to trigger the script, or <see langword="null"/> if no condition expression is required.
    /// </summary>
    public GraphicExpression? Expression { get; init; }

    /// <summary>
    /// Gets the execution period in milliseconds for continuous or recurring script triggers.
    /// </summary>
    [DefaultValue(1000)]
    public int Period { get; init; } = 1000;

    /// <summary>
    /// Gets the minimum deadband threshold required before a value change triggers script execution.
    /// </summary>
    public int Deadband { get; init; }

    /// <summary>
    /// Gets a value indicating whether data quality changes trigger script execution.
    /// </summary>
    public bool QualityChange { get; init; }

    /// <summary>
    /// Gets the script source code or expression text to execute when the trigger occurs.
    /// </summary>
    [YamlMember(ScalarStyle = ScalarStyle.Literal)]
    public string Text { get; init; } = string.Empty;

    /// <summary>
    /// Determines whether the <see cref="Period"/> property should be serialized.
    /// </summary>
    /// <returns><see langword="true"/> if the trigger is continuous and the period is not the default value; otherwise, <see langword="false"/>.</returns>
    public bool ShouldSerializePeriod() => Trigger.IsContinuous() && Period != 1000;

    /// <summary>
    /// Determines whether the <see cref="Deadband"/> property should be serialized.
    /// </summary>
    /// <returns><see langword="true"/> if the trigger is a data change and deadband is non-zero; otherwise, <see langword="false"/>.</returns>
    public bool ShouldSerializeDeadband() => Trigger == ScriptTrigger.DataChange && Deadband != 0;

    /// <summary>
    /// Determines whether the <see cref="QualityChange"/> property should be serialized.
    /// </summary>
    /// <returns><see langword="true"/> if the trigger is a data change and quality change is enabled; otherwise, <see langword="false"/>.</returns>
    public bool ShouldSerializeQualityChange() => Trigger == ScriptTrigger.DataChange && QualityChange;
}

/// <summary>
/// Specifies the trigger condition that initiates execution of a graphic script.
/// </summary>
public enum ScriptTrigger
{
    /// <summary>
    /// The script executes once when the graphic element or symbol is initially displayed.
    /// </summary>
    OnShow,

    /// <summary>
    /// The script executes once when the graphic element or symbol is closed or hidden.
    /// </summary>
    OnHide,

    /// <summary>
    /// The script executes periodically while the graphic element or symbol remains displayed.
    /// </summary>
    WhileShowing,

    /// <summary>
    /// The script executes periodically while the trigger expression evaluates to true.
    /// </summary>
    WhileTrue,

    /// <summary>
    /// The script executes periodically while the trigger expression evaluates to false.
    /// </summary>
    WhileFalse,

    /// <summary>
    /// The script executes once when the trigger expression transitions from false to true.
    /// </summary>
    OnTrue,

    /// <summary>
    /// The script executes once when the trigger expression transitions from true to false.
    /// </summary>
    OnFalse,

    /// <summary>
    /// The script executes when the evaluated data value or quality changes.
    /// </summary>
    DataChange
}

/// <summary>
/// Provides extension methods for evaluating <see cref="ScriptTrigger"/> instances.
/// </summary>
public static class ScriptTriggerExtensions
{
    /// <summary>
    /// Determines whether the specified script trigger executes continuously on a periodic interval.
    /// </summary>
    /// <param name="trigger">The script trigger to evaluate.</param>
    /// <returns><see langword="true"/> if the trigger executes on a continuous recurring period; otherwise, <see langword="false"/>.</returns>
    public static bool IsContinuous(this ScriptTrigger trigger) => trigger
        is ScriptTrigger.WhileShowing
        or ScriptTrigger.WhileTrue
        or ScriptTrigger.WhileFalse;
}