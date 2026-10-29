using System;
using StarGate.Core.Abstractions;
using StarGate.Core.Attributes;
using StarGate.Core.Objects;

namespace StarGate.Core.Extensions;

/// <summary>
/// Represents the input/output (I/O) extension configuration for an object attribute,
/// defining data source/destination bindings, scaling, conversion, and communication options.
/// </summary>
[GalaxyExtension("InputExtension", ExtensionType.Attribute)]
[GalaxyExtension("OutputExtension", ExtensionType.Attribute)]
[GalaxyExtension("InputOutputExtension", ExtensionType.Attribute)]
public class IOExtension : IMergeable<IOExtension> , IAttributeExtension
{
    /// <summary>
    /// Gets or sets the binding type configuration specifying the nature of the interaction
    /// between an attribute and its associated input/output operations.
    /// </summary>
    public BindingType? Binding { get; init; }

    /// <summary>
    /// Gets or sets the input source configuration defining where input data is read from.
    /// </summary>
    [GalaxyAttribute("InputSource", Security.Configure)]
    public AttributeConfig? InputSource { get; init; }

    /// <summary>
    /// Gets or sets the primary output destination configuration defining where output data is written.
    /// </summary>
    [GalaxyAttribute("OutputDest", Security.Configure)]
    public AttributeConfig? OutputDest { get; init; }

    /// <summary>
    /// Gets or sets the differentiated output destination configuration.
    /// </summary>
    [GalaxyAttribute("DiffOutputDest", Security.Configure)]
    public AttributeConfig? DifferentOutput { get; init; }

    /// <summary>
    /// Gets or sets the configuration indicating whether communication is buffered.
    /// </summary>
    [GalaxyAttribute("Buffered")]
    public AttributeConfig? Buffered { get; init; }

    /// <summary>
    /// Gets or sets the configuration indicating whether discrete input/output values are inverted.
    /// </summary>
    [GalaxyAttribute("InvertValue", Security.ViewOnly)]
    public AttributeConfig? Invert { get; init; }

    /// <summary>
    /// Gets or sets the configuration indicating whether output values are written on every engine scan.
    /// </summary>
    [GalaxyAttribute("OutputEveryScan", Security.Tune)]
    public AttributeConfig? OutputEveryScan { get; init; }

    /// <summary>
    /// Gets or sets the deadband configuration used to filter out small value fluctuations.
    /// </summary>
    [GalaxyAttribute("Deadband", Security.Configure)]
    public AttributeConfig? Deadband { get; init; }

    /// <summary>
    /// Gets or sets the scaling configuration for the input/output (I/O) extension,
    /// defining parameters used to map raw values to engineering units and vice versa.
    /// Scaling can include properties for value clamping, conversion modes,
    /// and range boundaries.
    /// </summary>
    public ScalingExtension? Scaling { get; init; }

    /// <summary>
    /// Determines the configured extension names based on the current binding type.
    /// </summary>
    /// <returns>
    /// An array of strings representing the names of the configured extensions.
    /// Possible values are "InputExtension", "OutputExtension", or "InputOutputExtension" based on the binding type.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the binding type is not supported or recognized.
    /// </exception>
    public string[] GetConfiguredExtensions()
    {
        string[] extensions = Binding switch
        {
            BindingType.ReadOnly => ["InputExtension"],
            BindingType.WriteOnly => ["OutputExtension"],
            BindingType.ReadWrite => ["InputOutputExtension"],
            BindingType.Reflect => ["InputExtension", "OutputExtension"],
            _ => throw new ArgumentOutOfRangeException(nameof(Binding),
                $"The source binding '{Binding}' is not a supported binding type.")
        };

        return extensions;
    }

    /// <inheritdoc />
    public IOExtension Merge(IOExtension? current)
    {
        if (current is null) return this;

        return new IOExtension
        {
            Binding = Binding ?? current.Binding,
            InputSource = InputSource?.Merge(current.InputSource) ?? current.InputSource,
            OutputDest = OutputDest?.Merge(current.OutputDest) ?? current.OutputDest,
            DifferentOutput = DifferentOutput?.Merge(current.DifferentOutput) ?? current.DifferentOutput,
            Buffered = Buffered?.Merge(current.Buffered) ?? current.Buffered,
            Invert = Invert?.Merge(current.Invert) ?? current.Invert,
            OutputEveryScan = OutputEveryScan?.Merge(current.OutputEveryScan) ?? current.OutputEveryScan,
            Deadband = Deadband?.Merge(current.Deadband) ?? current.Deadband,
            Scaling = Scaling?.Merge(current.Scaling) ?? current.Scaling
        };
    }

    public bool Supports(DataType dataType) => dataType != DataType.InternationalizedString;
}

/// <summary>
/// Specifies the type of binding relationship between an attribute and its input/output operations,
/// determining how data is accessed and manipulated within the system.
/// </summary>
public enum BindingType
{
    /// <summary>
    /// Represents a binding type where data is restricted to read-only access,
    /// preventing any modification of the underlying data during input/output operations.
    /// </summary>
    ReadOnly,

    /// <summary>
    /// Represents a binding type where data is restricted to write-only access,
    /// allowing modification of the underlying data but prohibiting any reading during input/output operations.
    /// </summary>
    WriteOnly,

    /// <summary>
    /// Represents a binding type that allows both reading and writing of data,
    /// enabling full access and modification during input/output operations.
    /// </summary>
    ReadWrite,

    /// <summary>
    /// Represents a binding type where input data is immediately reflected to the output,
    /// ensuring a direct mirroring of values between input and output operations.
    /// </summary>
    Reflect
}