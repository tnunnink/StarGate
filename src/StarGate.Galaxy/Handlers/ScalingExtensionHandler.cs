using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Galaxy.Abstractions;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Handlers;

/// <summary>
/// Handles the extraction and application of the <see cref="ScalingExtension" /> configuration
/// on an ArchestrA Galaxy object.
/// </summary>
/// <remarks>
/// This class provides functionality for working with the scaling extension attributes
/// of Galaxy objects. It allows you to extract the configuration of a scaling extension
/// from a target object and apply a given scaling configuration to a target object.
/// </remarks>
public class ScalingExtensionHandler : IConfigurationHandler<ScalingExtension>
{
    /// <inheritdoc />
    public ScalingExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        if (!target.HasExtension(typeof(ScalingExtension), context))
            return null;

        return new ScalingExtension
        {
            RawMax = target.GetConfig<ScalingExtension>(x => x.RawMax, context),
            RawMin = target.GetConfig<ScalingExtension>(x => x.RawMin, context),
            EngMax = target.GetConfig<ScalingExtension>(x => x.EngMax, context),
            EngMin = target.GetConfig<ScalingExtension>(x => x.EngMin, context),
            RangeMax = target.GetConfig<ScalingExtension>(x => x.RangeMax, context),
            RangeMin = target.GetConfig<ScalingExtension>(x => x.RangeMin, context),
            ConversionMode = target.GetConfig<ScalingExtension>(x => x.ConversionMode, context),
            ClampEnabled = target.GetConfig<ScalingExtension>(x => x.ClampEnabled, context)
        };
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, ScalingExtension? source, string? context = null)
    {
        if (context is null || string.IsNullOrEmpty(context) || source is null)
            return GalaxyResult.Ok();

        // Ensure the host attribute exists and supports this extension type.
        if (!target.ValidateExtension(source, context, out var invalid))
            return invalid;

        var result = target.AddExtension<ScalingExtension>(context)
            .Then(() => target.Configure(source, x => x.RawMax, context))
            .And(() => target.Configure(source, x => x.RawMin, context))
            .And(() => target.Configure(source, x => x.EngMax, context))
            .And(() => target.Configure(source, x => x.EngMin, context))
            .And(() => target.Configure(source, x => x.RangeMax, context))
            .And(() => target.Configure(source, x => x.RangeMin, context))
            .And(() => target.Configure(source, x => x.ConversionMode, context))
            .And(() => target.Configure(source, x => x.ClampEnabled, context));

        return result;
    }
}