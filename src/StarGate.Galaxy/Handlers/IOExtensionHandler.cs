using System.Collections.Generic;
using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Core.Extensions;
using StarGate.Galaxy.Abstractions;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Handlers;

// ReSharper disable StringLiteralTypo
/// <summary>
/// Handles operations related to the IOExtension type, specifically for extracting and applying
/// its configuration on IgObject instances within the Galaxy framework. It ensures fidelity of the
/// object configuration and manages extensions such as Input, Output, and InputOutput.
/// </summary>
public class IOExtensionHandler : IConfigurationHandler<IOExtension>
{
    private readonly IConfigurationHandler<ScalingExtension> _scalingHandler = new ScalingExtensionHandler();

    /// <inheritdoc />
    public IOExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context)) 
            return null;

        if (!target.HasExtension(typeof(IOExtension), context))
            return null;

        var extensions = new HashSet<string>(target.ListExtensions(context));
        var hasInput = extensions.Contains("inputextension");
        var hasOutput = extensions.Contains("inputextension");
        var hasInputOutput = extensions.Contains("inputoutputextension");

        // We have to preserve which extensions are configured to preserve object fidelity.
        // We will do this using the custom binding type that reflects the options you can select from the IDE.
        var binding = (hasInput, hasOutput, hasInputOutput) switch
        {
            (_, _, true) => BindingType.ReadWrite,
            (true, true, _) => BindingType.Reflect,
            (true, false, _) => BindingType.ReadOnly,
            (false, true, _) => BindingType.WriteOnly,
            _ => BindingType.ReadWrite
        };

        return new IOExtension
        {
            Binding = binding,
            InputSource = target.GetConfig<IOExtension>(x => x.InputSource, context),
            OutputDest = target.GetConfig<IOExtension>(x => x.OutputDest, context),
            DifferentOutput = target.GetConfig<IOExtension>(x => x.DifferentOutput, context),
            Buffered = target.GetConfig<IOExtension>(x => x.Buffered, context),
            Invert = target.GetConfig<IOExtension>(x => x.Invert, context),
            OutputEveryScan = target.GetConfig<IOExtension>(x => x.OutputEveryScan, context),
            Deadband = target.GetConfig<IOExtension>(x => x.Deadband, context),
            Scaling = _scalingHandler.Extract(target, context)
        };
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, IOExtension? source, string? context = null)
    {
        if (context is null || string.IsNullOrEmpty(context) || source is null)
            return GalaxyResult.Ok();
        
        // Ensure the host attribute exists and supports this extension type.
        if (!target.ValidateExtension(source, context, out var invalid))
            return invalid;

        var result = ConfigureExtension(target, source, context)
            .Then(() => target.Configure(source, x => x.InputSource, context))
            .And(() => target.Configure(source, x => x.OutputDest, context))
            .And(() => target.Configure(source, x => x.DifferentOutput, context))
            .And(() => target.Configure(source, x => x.Buffered, context))
            .And(() => target.Configure(source, x => x.Invert, context))
            .And(() => target.Configure(source, x => x.OutputEveryScan, context))
            .And(() => target.Configure(source, x => x.Deadband, context))
            .And(() => _scalingHandler.Apply(target, source.Scaling, context));

        return result;
    }

    private static GalaxyResult ConfigureExtension(IgObject target, IOExtension extension, string primitive)
    {
        var names = extension.GetConfiguredExtensions();
        var result = GalaxyResult.Ok();

        foreach (var name in names)
        {
            target.AddExtensionPrimitive(name, primitive, false);
            result = result.Merge(target.CommandResult.ToResult());
        }

        return result;
    }
}