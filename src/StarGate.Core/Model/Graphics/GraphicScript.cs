using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents a script associated with a graphic symbol or element that executes custom logic based on specified trigger events.
/// </summary>
public class GraphicScript
{
    /// <summary>
    /// Gets the user-defined name of the script. 
    /// Can be omitted or null for predefined lifecycle scripts (OnShow, OnHide, WhileShowing).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets the trigger condition that defines when the script executes.
    /// </summary>
    [Required]
    public ScriptTrigger Trigger { get; set; } = ScriptTrigger.WhileTrue;

    /// <summary>
    /// Gets the expression evaluated to trigger the script, or <see langword="null"/> if no condition expression is required.
    /// </summary>
    public string? Expression { get; set; }

    /// <summary>
    /// Gets the execution period in milliseconds for continuous or recurring script triggers.
    /// </summary>
    [DefaultValue(1000)]
    public int Period { get; set; } = 1000;

    /// <summary>
    /// Gets the minimum deadband threshold required before a value change triggers script execution.
    /// </summary>
    public int Deadband { get; set; }

    /// <summary>
    /// Gets a value indicating whether data quality changes trigger script execution.
    /// </summary>
    public bool QualityChange { get; set; }

    /// <summary>
    /// Gets the script source code or expression text to execute when the trigger occurs.
    /// </summary>
    [YamlMember(ScalarStyle = ScalarStyle.Literal)]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Determines whether the <see cref="Expression"/> property should be serialized.
    /// </summary>
    /// <returns><see langword="true"/> if the trigger is not predefined and the expression contains non-whitespace text; otherwise, <see langword="false"/>.</returns>
    public bool ShouldSerializeExpression() => !Trigger.IsPredefined && !string.IsNullOrWhiteSpace(Expression);

    /// <summary>
    /// Determines whether the <see cref="Period"/> property should be serialized.
    /// </summary>
    /// <returns><see langword="true"/> if the trigger is continuous and the period is not the default value; otherwise, <see langword="false"/>.</returns>
    public bool ShouldSerializePeriod() => Trigger.IsContinuous && Period != 1000;

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
    /// <param name="trigger"></param>
    extension(ScriptTrigger trigger)
    {
        /// <summary>
        /// Gets a value indicating whether the script trigger represents one of the predefined lifecycle triggers.
        /// </summary>
        public bool IsPredefined => trigger
            is ScriptTrigger.OnShow
            or ScriptTrigger.WhileShowing
            or ScriptTrigger.OnHide;

        /// <summary>
        /// Determines whether the specified script trigger executes continuously on a periodic interval.
        /// </summary>
        public bool IsContinuous => trigger
            is ScriptTrigger.WhileShowing
            or ScriptTrigger.WhileTrue
            or ScriptTrigger.WhileFalse;
    }
}