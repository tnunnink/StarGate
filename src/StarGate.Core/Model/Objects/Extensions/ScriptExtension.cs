using System;

namespace StarGate.Core;

/// <summary>
/// Represents a script extension that encapsulates configuration attributes and logic for
/// object-level scripts, including trigger behaviors, execution parameters, timeouts,
/// alarms, and script body definitions.
/// </summary>
[GalaxyExtension("ScriptExtension", ExtensionType.Object)]
public class ScriptExtension
{
    /// <summary>
    /// Gets the unique name identifier of the script extension.
    /// This property is required and serves as the distinguishing identifier
    /// for this specific extension within the StarGate framework.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets or sets the trigger expression configuration that determines when the script executes.
    /// </summary>
    [GalaxyAttribute("Expression")]
    public AttributeConfig? Expression { get; init; }

    /// <summary>
    /// Gets or sets the trigger type configuration specifying when and how the script is executed
    /// (such as periodic, on true, on false, while true, while false, or on data change).
    /// </summary>
    [GalaxyAttribute("TriggerType")]
    public AttributeConfig? TriggerType { get; init; }

    /// <summary>
    /// Gets or sets the trigger period configuration defining the execution interval for periodic
    /// or repeating scripts.
    /// </summary>
    [GalaxyAttribute("TriggerPeriod")]
    public AttributeConfig? Period { get; init; }

    /// <summary>
    /// Gets or sets the data change deadband configuration specifying the minimum value change required
    /// to trigger execution on data change.
    /// </summary>
    [GalaxyAttribute("DataChangeDeadband")]
    public AttributeConfig? Deadband { get; init; }

    /// <summary>
    /// Gets or sets the configuration indicating whether the script is triggered when the quality
    /// of referenced data changes.
    /// </summary>
    [GalaxyAttribute("TriggerOnQualityChange")]
    public AttributeConfig? RunsOnQualityChange { get; init; }

    /// <summary>
    /// Gets or sets the configuration indicating whether the script executes asynchronously in the background.
    /// </summary>
    [GalaxyAttribute("RunsAsync")]
    public AttributeConfig? RunsAsync { get; init; }

    /// <summary>
    /// Gets or sets the execution timeout configuration defining the maximum time allowed for script
    /// execution before timing out.
    /// </summary>
    [GalaxyAttribute("ExecuteTimeout.Limit")]
    public AttributeConfig? Timeout { get; init; }

    /// <summary>
    /// Gets or sets the configuration indicating whether an alarm is raised upon a script execution error.
    /// </summary>
    [GalaxyAttribute("ExecutionError.Alarmed")]
    public AttributeConfig? AlarmEnabled { get; init; }

    /// <summary>
    /// Gets or sets the alarm priority configuration assigned when an execution error occurs.
    /// </summary>
    [GalaxyAttribute("ExecutionError.Priority")]
    public AttributeConfig? AlarmPriority { get; init; }

    /// <summary>
    /// Gets or sets the configuration indicating whether script execution state information is historized.
    /// </summary>
    [GalaxyAttribute("State.Historized")]
    public AttributeConfig? Historize { get; init; }

    /// <summary>
    /// Gets or sets the alias definitions configuration used within the script.
    /// </summary>
    [GalaxyAttribute("Aliases")]
    public AttributeConfig? Aliases { get; init; }

    /// <summary>
    /// Gets or sets the external attribute reference mappings configuration for script aliases.
    /// </summary>
    [GalaxyAttribute("AliasReferences")]
    public AttributeConfig? AliasReferences { get; init; }

    /// <summary>
    /// Gets or sets the script declarations block containing local variable definitions and declarations text.
    /// </summary>
    [GalaxyAttribute("DeclarationsText")]
    public AttributeConfig? Declarations { get; init; }

    /// <summary>
    /// Gets or sets the primary execution script body text that runs when the script is triggered.
    /// </summary>
    [GalaxyAttribute("ExecuteText")]
    public AttributeConfig? ExecuteText { get; init; }

    /// <summary>
    /// Gets or sets the startup script body text that runs during object initialization or engine startup.
    /// </summary>
    [GalaxyAttribute("StartupText")]
    public AttributeConfig? StartupText { get; init; }

    /// <summary>
    /// Gets or sets the shutdown script body text that runs during object shutdown or engine stop.
    /// </summary>
    [GalaxyAttribute("ShutdownText")]
    public AttributeConfig? ShutdownText { get; init; }

    /// <summary>
    /// Gets or sets the script body text that runs when the object transitions on-scan.
    /// </summary>
    [GalaxyAttribute("OnScanText")]
    public AttributeConfig? OnScanText { get; init; }

    /// <summary>
    /// Gets or sets the script body text that runs when the object transitions off-scan.
    /// </summary>
    [GalaxyAttribute("OffScanText")]
    public AttributeConfig? OffScanText { get; init; }
}