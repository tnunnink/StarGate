using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

/// <summary>
/// Handles the extraction and application of <see cref="LoggingExtension" /> instances to
/// and from Galaxy objects. Provides methods to configure logging behavior for attribute changes
/// within the Galaxy environment.
/// </summary>
public class LoggingExtensionHandler : IConfigurationHandler<LoggingExtension>
{
    /// <inheritdoc />
    public LoggingExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        if (!target.HasExtension(typeof(LoggingExtension), context))
            return null;

        return new LoggingExtension
        {
            LogEvent = target.GetConfig<LoggingExtension>(x => x.LogEvent, context)
        };
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, LoggingExtension? source, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context) || source is null)
            return GalaxyResult.Ok();

        // Ensure the host attribute exists and supports this extension type.
        if (!target.ValidateExtension(source, context, out var invalid))
            return invalid;

        var result = target.AddExtension<LoggingExtension>(context)
            .Then(() => target.Configure(source, x => x.LogEvent, context));

        return result;
    }
}