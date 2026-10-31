using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy.Extensions;

public static class GalaxyExtensions
{
    /// <param name="galaxy">The galaxy instance to search for the template.</param>
    extension(IGalaxy galaxy)
    {
        /// <summary>
        /// Ensures that an object with the specified definition is created within the galaxy. If the object already exists
        /// and matches the expected template and base configuration, it is returned. Otherwise, attempts to create a new
        /// object.
        /// </summary>
        /// <param name="config">
        /// The definition of the object to be ensured, including the tag name and related template information.
        /// </param>
        /// <param name="wasCreated">
        /// When this method returns, contains a value indicating whether a new object was successfully created.
        /// This will be <c>false</c> if the object already existed and matched the expected criteria.
        /// </param>
        /// <returns>
        /// A <see cref="GalaxyResult{IgObject}" /> representing the result of the operation, including the object if found
        /// or successfully created. If the operation fails, the result will contain an error indicating the cause of the
        /// failure.
        /// </returns>
        public GalaxyResult<IgObject> EnsureObjectCreated(GalaxyObject config, out bool wasCreated)
        {
            wasCreated = false;

            if (galaxy.TryGetObject(config.TagName, out var existing))
            {
                if (existing.DerivedFrom != config.Template)
                    return GalaxyResult.Fail(existing, ErrorCode.ObjectNotFound, "Failed");

                return GalaxyResult.Ok(existing);
            }

            if (!galaxy.TryGetObject(config.Template, out var template))
                return GalaxyResult.Fail<IgObject>(
                    null!,
                    ErrorCode.ObjectNotFound,
                    $"Template object {config.Template} does not exist."
                );

            var created = template.CreateObject(config.TagName);
            wasCreated = created.Success;
            return created;
        }

        /// <summary>
        /// Attempts to retrieve an object from the galaxy based on the provided tag name.
        /// </summary>
        /// <param name="tagName">The name of the tag representing the object to be retrieved.</param>
        /// <param name="result">
        /// When this method returns, contains an <see cref="IgObject" /> representing
        /// the object retrieved if the operation is successful; otherwise, it contains
        /// <c>null</c> if the object could not be found.
        /// </param>
        /// <returns>
        /// <c>true</c> if the object is successfully retrieved; otherwise, <c>false</c>.
        /// </returns>
        private bool TryGetObject(string tagName, out IgObject result)
        {
            var option = tagName.StartsWith("$")
                ? EgObjectIsTemplateOrInstance.gObjectIsTemplate
                : EgObjectIsTemplateOrInstance.gObjectIsInstance;

            var obj = galaxy.QueryObjectsByName(option, [tagName])[tagName];
            result = obj;
            return result is not null;
        }
    }
}