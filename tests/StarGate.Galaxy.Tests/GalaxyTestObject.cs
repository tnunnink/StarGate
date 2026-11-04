using ArchestrA.GRAccess;

// ReSharper disable SuspiciousTypeConversion.Global

namespace StarGate.Galaxy.Tests;

/// <summary>
///     Represents a temporary object that interacts with the ArchestrA Galaxy system.
///     This class facilitates the creation and management of temporary instances of
///     templates for testing and other short-lived operations.
/// </summary>
public class GalaxyTestObject : IDisposable
{
    /// <summary>
    ///     Represents a temporary object that interacts with the ArchestrA Galaxy system.
    ///     It is primarily used for creating and managing temporary instances of templates
    ///     for testing and other short-lived operations.
    /// </summary>
    public GalaxyTestObject(IGalaxy galaxy, string baseTemplate = "$UserDefined")
    {
        TagName = $"Temp_{Guid.NewGuid():N}";

        // Create instance from template
        var results = galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate, [baseTemplate]);
        ((ITemplate)results[baseTemplate]).CreateInstance(TagName);

        // Get the created instance and store locally for access
        var query = galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, [TagName]);
        Instance = query[TagName];
    }

    /// <summary>
    ///     Represents the IgObject instance created from a specified base template within the ArchestrA Galaxy system.
    ///     This property provides access to the instantiated object, allowing operations like configuration and interaction
    ///     with the temporary object created for testing or short-lived purposes.
    /// </summary>
    public IgObject Instance { get; }

    /// <summary>
    ///     Gets the unique name assigned to the temporary instance within the ArchestrA Galaxy system.
    ///     This name is automatically generated in the format "Temp_{GUID}" to ensure uniqueness, and is
    ///     used as an identifier for operations such as querying, creation, or deletion of the instance.
    /// </summary>
    public string TagName { get; }

    public void Dispose()
    {
        try
        {
            if (Instance.CheckoutStatus == ECheckoutStatus.notCheckedOut) Instance.UndoCheckOut();

            ((IInstance)Instance).DeleteInstance();
        }
        catch
        {
            // Suppress or log teardown cleanup errors so they don't mask test assertion failures
        }
    }
}