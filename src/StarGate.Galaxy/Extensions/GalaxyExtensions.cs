using System.Collections.Generic;
using System.Linq;
using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

public static class GalaxyExtensions
{
    /// <param name="galaxy">The galaxy instance to search for the template.</param>
    extension(IGalaxy galaxy)
    {
        /// <summary>
        /// Retrieves a collection of Galaxy objects by their specified tag names from the provided galaxy instance,
        /// with an option to filter objects based on whether they are templates or instances.
        /// </summary>
        /// <param name="tagNames">
        /// An array of tag names representing the objects to retrieve. Each tag name is used as a filter to locate
        /// the corresponding objects in the galaxy.
        /// </param>
        /// <param name="option">
        /// Specifies the filter criteria to retrieve either templates, instances, or both from the galaxy.
        /// </param>
        /// <returns>
        /// A <see cref="GalaxyResult" /> containing the collection of retrieved objects.
        /// If no objects are found or the operation fails, the result will include an error indicating the failure reason.
        /// </returns>
        public GalaxyResult<IEnumerable<GalaxyObject>> GetObjectsByName(string[] tagNames,
            EgObjectIsTemplateOrInstance option)
        {
            var handler = new GalaxyObjectHandler();

            if (tagNames.Length == 0)
                return new GalaxyResult<IEnumerable<GalaxyObject>>([]);

            var results = galaxy.QueryObjectsByName(option, tagNames)
                .AsEnumerable()
                .Select(x => handler.Extract(x))
                .ToArray();

            return galaxy.CommandResults.ToResult<IEnumerable<GalaxyObject>>(results);
        }

        /// <summary>
        /// Retrieves a collection of Galaxy symbols by their specified tag names from the provided galaxy instance.
        /// </summary>
        /// <param name="tagNames">
        /// An array of tag names representing the symbols to retrieve. Each tag name is used as a filter to locate
        /// the corresponding symbols in the galaxy.
        /// </param>
        /// <returns>
        /// A <see cref="GalaxyResult" /> containing the collection of retrieved symbols.
        /// If no symbols are found or the operation fails, the result will include an error indicating the failure reason.
        /// </returns>
        public IEnumerable<IgObject> GetSymbols(params string[] tagNames)
        {
            if (tagNames.Length == 0) return [];
            var results = new List<IgObject>();

            const EgObjectIsTemplateOrInstance option = EgObjectIsTemplateOrInstance.gObjectIsInstance;
            var conditions = galaxy.CreateConditionsObject();
            conditions.Add(EConditionType.NameSpaceIdIs, 3);

            foreach (var tagName in tagNames)
            {
                if (conditions.count > 1) conditions.Remove(1);
                conditions.Add(EConditionType.NameEquals, tagName);

                var symbol = galaxy.QueryObjectsMultiCondition(option, conditions)[tagName];
                results.Add(symbol);
            }

            return results;
        }

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
                if (existing.DerivedFrom != config.DerivedFrom)
                    return GalaxyResult.Fail(existing, ErrorCode.ObjectNotFound, "Failed");

                return GalaxyResult.Ok(existing);
            }

            if (!galaxy.TryGetObject(config.DerivedFrom, out var template))
                return GalaxyResult.Fail<IgObject>(
                    null!,
                    ErrorCode.ObjectNotFound,
                    $"Template object {config.DerivedFrom} does not exist."
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

        /// <summary>
        /// Attempts to retrieve a symbol from the galaxy with the specified tag name.
        /// Returns whether the operation was successful and outputs the retrieved symbol if it exists.
        /// </summary>
        /// <param name="tagName">The tag name of the symbol to search for.</param>
        /// <param name="result">
        /// When this method returns, contains the retrieved symbol if found; otherwise, contains <c>null</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the symbol with the specified tag name exists and was successfully retrieved; otherwise, <c>false</c>.
        /// </returns>
        private bool TryGetSymbol(string tagName, out IgObject result)
        {
            const EgObjectIsTemplateOrInstance option = EgObjectIsTemplateOrInstance.gObjectIsInstance;

            //todo I'm not sure if there is another way to get symbols. The main API QueryObjectsByName doesn't return visual elements.
            var conditions = galaxy.CreateConditionsObject();
            conditions.Add(EConditionType.NameSpaceIdIs, 3);
            conditions.Add(EConditionType.NameEquals, tagName);

            var symbol = galaxy.QueryObjectsMultiCondition(option, conditions)[tagName];
            result = symbol;
            return result is not null;
        }
    }
}