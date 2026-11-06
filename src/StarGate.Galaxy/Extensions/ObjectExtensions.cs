using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using ArchestrA.GRAccess;
using ArchestrA.Visualization.GraphicLibrary;
using StarGate.Core;

// ReSharper disable SuspiciousTypeConversion.Global

namespace StarGate.Galaxy;

/// <summary>
/// Provides extension methods for interacting with <see cref="IgObject" /> instances.
/// </summary>
public static class ObjectExtensions
{
    /// <param name="obj">The <see cref="IgObject" /> to search for the attribute.</param>
    extension(IgObject obj)
    {
        /// <summary>
        /// Gets a value indicating whether the object is a template.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the object is of type <see cref="ITemplate" />; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTemplate => obj is ITemplate;

        /// <summary>
        /// Gets a value indicating whether the object is an instance.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the object is of type <see cref="IInstance" />; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInstance => obj is IInstance;

        /// <summary>
        /// Executes the specified action on the provided <see cref="IgObject" /> and returns the resulting
        /// <see cref="GalaxyResult" />.
        /// </summary>
        /// <param name="action">The action to perform on the <see cref="IgObject" />.</param>
        /// <returns>A <see cref="GalaxyResult" /> representing the success or failure of the operation.</returns>
        public GalaxyResult Execute(Action<IgObject> action)
        {
            action.Invoke(obj);
            return obj.CommandResult.ToResult();
        }

        /// <summary>
        /// Saves the current state of the <see cref="IgObject" /> and checks it in with the specified comment.
        /// </summary>
        /// <param name="comment">The comment to associate with the check-in operation.</param>
        /// <returns>
        /// A <see cref="GalaxyResult" /> indicating whether both the save and check-in operations were successful,
        /// along with any errors encountered.
        /// </returns>
        public GalaxyResult Commit(string comment)
        {
            // This chains each call and will short-circuit if save fails.
            return obj.Execute(x => x.Save()).Then(() => obj.Execute(x => x.CheckIn(comment)));
        }

        /// <summary>
        /// Reverts any unsaved changes to the specified <see cref="IgObject" /> by unloading the object and
        /// undoing its checkout status.
        /// </summary>
        /// <returns>
        /// A <see cref="GalaxyResult" /> indicating whether the revert operations were successful,
        /// along with any errors encountered.
        /// </returns>
        public GalaxyResult Rollback()
        {
            // This chains each call but will not short-circuit. We want best-effort for a revert operation
            return obj.Execute(x => x.Unload()).And(() => obj.Execute(x => x.UndoCheckOut()));
        }

        /// <summary>
        /// Deletes the <see cref="IgObject" /> based on its type and returns a <see cref="GalaxyResult" /> indicating the
        /// outcome of the operation.
        /// If the object is a template, it deletes the template, cascading the deletion to undeployed descendants if running.
        /// If the object is an instance, it deletes the instance, undeploying it if deployed.
        /// For unsupported types, the operation succeeds with an <see cref="GalaxyResult.Ok" /> result.
        /// </summary>
        /// <returns>A <see cref="GalaxyResult" /> representing the success or failure of the delete operation.</returns>
        public GalaxyResult Delete()
        {
            switch (obj)
            {
                case ITemplate template:
                    template.DeleteTemplate(EForceDeleteTemplateOption.cascadeDeleteUndeployDescendentsIfRunning);
                    return template.CommandResult.ToResult();
                case IInstance instance:
                    instance.DeleteInstance(EForceDeleteInstanceOption.undeployIfDeployed);
                    return instance.CommandResult.ToResult();
                default:
                    return GalaxyResult.Ok();
            }
        }

        /// <summary>
        /// Creates a new object, either a template or an instance, based on the specified tag name.
        /// </summary>
        /// <param name="tagName">The name of the tag to use when creating the new object. If the tag name starts
        /// with the character '$', a template will be created; otherwise, an instance will be created.</param>
        /// <returns>A <see cref="GalaxyResult{IgObject}" /> representing the newly created object or
        /// an error if the operation fails.</returns>
        public GalaxyResult<IgObject> CreateObject(string tagName)
        {
            if (obj is not ITemplate parent)
                return GalaxyResult.Fail(obj, ErrorCode.UnknownError, "");

            if (tagName.StartsWith("$"))
            {
                var template = parent.CreateTemplate(tagName);
                return parent.CommandResult.ToResult((IgObject)template);
            }

            var instance = parent.CreateInstance(tagName);
            return parent.CommandResult.ToResult((IgObject)instance);
        }

        /// <summary>
        /// Attempts to retrieve an attribute with the specified name from the given <see cref="IgObject" />.
        /// </summary>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <param name="attribute">
        /// When this method returns, contains the <see cref="IAttribute" /> if found, or <c>null</c> if
        /// not found.
        /// </param>
        /// <returns><c>true</c> if the attribute is found; otherwise, <c>false</c>.</returns>
        public bool TryGetAttribute(string? name, out IAttribute attribute)
        {
            attribute = name is not null ? obj.Attributes[name] : null!;
            return attribute is not null;
        }

        /// <summary>
        /// Retrieves the attribute configuration for a specified attribute of a given extension type based on the
        /// provided selector and path.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension object containing the attribute.</typeparam>
        /// <param name="selector">An expression that selects the specific attribute to retrieve the configuration for.</param>
        /// <param name="path">The attribute path used as part of the attribute's fully qualified name.</param>
        /// <returns>
        /// An <see cref="AttributeConfig" /> containing the value, lock status, and security classification
        /// of the attribute, or <c>null</c> if the attribute does not exist or is locked in the parent.
        /// </returns>
        public AttributeConfig? GetConfig<TExtension>(
            Expression<Func<TExtension, AttributeConfig?>> selector,
            string path)
        {
            if (selector.Body is not MemberExpression expression)
                throw new ArgumentException("The provided selector expression must be a valid member expression.");

            var config = expression.Member.GetCustomAttributes<GalaxyAttributeAttribute>().SingleOrDefault();
            var name = config?.Name ?? expression.Member.Name;
            var attributeName = string.IsNullOrEmpty(path) ? name : $"{path}.{name}";

            // Only return existing attribute configurations.
            // We don't want to return parent-locked attribute configs since they can't be configured anyway.
            if (!obj.TryGetAttribute(attributeName, out var attribute) || attribute.InheritsLock)
                return null;

            var value = attribute.GetValue();
            var locked = attribute.Locked == MxPropertyLockedEnum.MxLockedInMe;
            var security = attribute.SecurityClassification.ToSecurity();

            // For instances only return value since lock and security can't be modified at that level.
            // For templates, omit the security setting if it is the default value for the given attribute.
            return new AttributeConfig
            {
                Value = value,
                Locked = obj.IsTemplate ? locked : null,
                Security = obj.IsTemplate ? config?.DefaultSecurity == security ? null : security : null
            };
        }

        /// <summary>
        /// Configures the specified extension by applying a configuration to the attribute identified by the selector
        /// expression.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension being configured.</typeparam>
        /// <param name="extension">The extension object containing the properties to configure.</param>
        /// <param name="selector">An expression that selects the property of the extension to associate with the attribute.</param>
        /// <param name="path">An optional path used to qualify the attribute name.</param>
        /// <exception cref="ArgumentException">Thrown if the provided selector expression is not a valid member expression.</exception>
        public GalaxyResult Configure<TExtension>(
            TExtension extension,
            Expression<Func<TExtension, AttributeConfig?>> selector,
            string path)
        {
            if (selector.Body is not MemberExpression expression)
                throw new ArgumentException("The provided selector expression must be a valid member expression.");

            var definition = expression.Member.GetCustomAttributes<GalaxyAttributeAttribute>().SingleOrDefault();
            var name = definition?.Name ?? expression.Member.Name;
            var attributeName = string.IsNullOrEmpty(path) ? name : $"{path}.{name}";

            if (!obj.TryGetAttribute(attributeName, out var attribute) || attribute.InheritsLock)
                return GalaxyResult.Ok();

            var config = selector.Compile().Invoke(extension);

            // lock state and security can only be set for templates
            if (obj.IsInstance)
                return attribute.SetValue(config?.Value);

            return attribute.SetValue(config?.Value)
                .And(() => attribute.SetLocked(config?.Locked))
                .And(() => attribute.SetSecurity(config?.Security));
        }

        /// <summary>
        /// Retrieves the names of user-defined attributes (UDAs) from the given <see cref="IgObject" />.
        /// </summary>
        /// <returns>A collection of strings containing the names of UDAs that are not inherited from another tag.</returns>
        /// <remarks>
        /// This method parses the XML representation of UDAs and filters out any attributes that are marked as inherited.
        /// </remarks>
        public IEnumerable<string> ListUda()
        {
            var xml = obj.Attributes["UDAs"].value.GetString();

            return XElement.Parse(xml).Descendants("Attribute")
                .Where(e => e.HasAttributes && HasValue(e, "InheritedFromTagName", string.Empty))
                .Select(e => e.Attribute("Name")?.Value)
                .Where(n => n is not null)
                .Cast<string>()
                .ToArray();
        }

        /// <summary>
        /// Retrieves a list of extensions associated with the specified <see cref="IgObject" />.
        /// </summary>
        /// <returns>An enumerable collection of extension names that are not inherited from a tag.</returns>
        public IEnumerable<string> ListExtensions(string primitive)
        {
            var xml = obj.Attributes["Extensions"].value.GetString();

            return XElement.Parse(xml).Descendants()
                .Where(e =>
                    e.HasAttributes
                    && HasValue(e, "InheritedFromTagName", string.Empty)
                    && HasValue(e, "Name", primitive)
                )
                .Select(e => e.Attribute("ExtensionType")?.Value)
                .Where(x => x is not null)
                .Cast<string>()
                .ToArray();
        }

        /// <summary>
        /// Determines whether the specified <see cref="IgObject" /> has an extension of the given type and primitive name.
        /// </summary>
        /// <param name="type">The <see cref="Type" /> of the extension to check for.</param>
        /// <param name="primitive">The name of the primitive associated with the extension.</param>
        /// <returns><c>true</c> if the extension exists; otherwise, <c>false</c>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the specified type does not define any <see cref="GalaxyExtensionAttribute" />.
        /// </exception>
        public bool HasExtension(Type type, string primitive)
        {
            var lookup = new HashSet<string>(
                type.GetCustomAttributes<GalaxyExtensionAttribute>().Select(e => e.Name),
                StringComparer.OrdinalIgnoreCase
            );

            if (lookup.Count == 0)
                throw new InvalidOperationException(
                    $"No extension attribute is defined for object of type {type.Name}");

            var xml = obj.Attributes["Extensions"].value.GetString();

            var configured = XElement.Parse(xml).Descendants()
                .Where(e => e.HasAttributes && HasValue(e, "InheritedFromTagName", string.Empty))
                .Where(e => HasValue(e, "Name", primitive));

            return configured
                .Select(e => e.Attribute("ExtensionType")?.Value)
                .Any(t => t is not null && lookup.Contains(t));
        }

        /// <summary>
        /// Validates whether the specified attribute extension is supported for the given attribute name and returns the
        /// validation result.
        /// </summary>
        /// <typeparam name="TExtension">
        /// The type of the attribute extension, which must implement
        /// <see cref="IAttributeExtension" />.
        /// </typeparam>
        /// <param name="extension">The instance of the attribute extension to validate.</param>
        /// <param name="name">The name of the attribute to validate the extension against.</param>
        /// <param name="result">A <see cref="GalaxyResult" /> that represents the success or failure of the validation.</param>
        /// <returns><c>true</c> if the extension is valid for the given attribute; otherwise, <c>false</c>.</returns>
        public bool ValidateExtension<TExtension>(TExtension extension, string name, out GalaxyResult result)
            where TExtension : IAttributeExtension
        {
            if (!obj.TryGetAttribute(name, out var attribute))
            {
                result = GalaxyResult.Fail(ErrorCode.MissingAttribute,
                    $"Host attribute '{name}' does not exist for object '{obj.Tagname}'");
                return false;
            }

            if (!extension.Supports(attribute.DataType.ToCoreType()))
            {
                result = GalaxyResult.Fail(ErrorCode.InvalidExtension,
                    $"'{extension.GetType().Name}' is not supported for attributes of type '{attribute.DataType.ToCoreType()}'");
                return false;
            }

            result = GalaxyResult.Ok();
            return true;
        }

        /// <summary>
        /// Adds a specified extension, identified by its type, to an IgObject using the provided primitive name.
        /// </summary>
        /// <typeparam name="TExtension">
        /// The type of the extension to be added, which must be decorated with a
        /// <see cref="GalaxyExtensionAttribute" />.
        /// </typeparam>
        /// <param name="primitive">The name of the primitive associated with the extension.</param>
        /// <returns>A <see cref="GalaxyResult" /> representing the outcome of the operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the specified extension type is not decorated with a
        /// <see cref="GalaxyExtensionAttribute" />.
        /// </exception>
        public GalaxyResult<IgObject> AddExtension<TExtension>(string primitive) where TExtension : IAttributeExtension
        {
            var extension = typeof(TExtension).GetCustomAttributes<GalaxyExtensionAttribute>().FirstOrDefault();

            if (extension is null)
                throw new InvalidOperationException(
                    $"No extension attribute is defined for object of type {typeof(TExtension)}");

            if (obj.HasExtension(typeof(TExtension), primitive))
                return GalaxyResult.Ok(obj);

            obj.AddExtensionPrimitive(extension.Name, primitive, extension.Type == ExtensionType.Object);
            return obj.CommandResult.ToResult(obj);
        }

        /// <summary>
        /// Replaces an existing attribute on the specified <see cref="IgObject" /> or adds a new attribute if it does not
        /// exist.
        /// </summary>
        /// <param name="attribute">The <see cref="GalaxyAttribute" /> to replace or add to the <see cref="IgObject" />.</param>
        /// <returns>
        /// A <see cref="GalaxyResult" /> indicating the success or failure of the operation. Returns failure if the
        /// attribute does not have a data type.
        /// </returns>
        public GalaxyResult ReplaceAttribute(GalaxyAttribute attribute)
        {
            if (!attribute.DataType.HasValue)
                return GalaxyResult.Fail(
                    ErrorCode.MissingDataType,
                    $"Attribute '{attribute.Name}' requires a data type to be configured."
                );

            // If it already exists, delete it first; if not, just proceed to AddUDA
            var prepare = obj.TryGetAttribute(attribute.Name, out _)
                ? obj.Execute(x => x.DeleteUDA(attribute.Name))
                : GalaxyResult.Ok();

            return prepare.Then(() => obj.Execute(x =>
            {
                x.AddUDA(attribute.Name,
                    attribute.DataType.Value.ToMxType(),
                    attribute.Writeability.ToMxType(),
                    MxSecurityClassification.MxSecurityOperate,
                    attribute.Length is > 0,
                    attribute.Length ?? 0
                );
            }));
        }

        /// <summary>
        /// Enables the specified attribute on the <see cref="IgObject" /> if it exists and is of a boolean data type.
        /// </summary>
        /// <param name="attributeName">The name of the attribute to enable.</param>
        /// <returns>
        /// A <see cref="GalaxyResult" /> indicating the success or failure of the operation, including error details if
        /// applicable.
        /// </returns>
        public GalaxyResult EnableAttribute(string attributeName)
        {
            if (!obj.TryGetAttribute(attributeName, out var attribute))
                return GalaxyResult.Fail(
                    ErrorCode.MissingAttribute,
                    $"Object {obj.Tagname} does not contain {attributeName}."
                );

            if (attribute.DataType != MxDataType.MxBoolean)
                return GalaxyResult.Fail(
                    ErrorCode.InvalidDataType,
                    $"Attribute '{attribute.Name}' of type '{attribute.DataType}' can not be enabled.");

            attribute.SetValue(true);
            return attribute.CommandResult.ToResult();
        }

        /// <summary>
        /// Reads the symbol definition from the specified <see cref="IgObject" /> and returns it as an <see cref="XDocument" />.
        /// </summary>
        /// <param name="primitiveName">The name of the primitive to retrieve the symbol definition for. If null, the default primitive is used.</param>
        /// <returns>An <see cref="XDocument" /> containing the serialized symbol definition.</returns>
        public XDocument ReadSymbol(string? primitiveName = null)
        {
            var container = new aaSymbolGraphicContainer();
            var blob = obj.GetVisualDefinition(primitiveName);
            container.Load(blob);

            var doc = GenerateDocument(out var root);
            container.Save(doc, root, aaXMLFormat.All);

            using var reader = new XmlNodeReader(doc);
            return XDocument.Load(reader);

            XmlDocument GenerateDocument(out XmlElement xmlElement)
            {
                const string ns = "http://www.wonderware.com/ArchestrA";
                var xmlDocument = new XmlDocument();
                xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null));

                xmlElement = xmlDocument.CreateElement("aa", "Symbol", ns);
                xmlElement.SetAttribute("xmlns:aa", ns);
                xmlElement.SetAttribute("GraphicVersion", aaGraphicVersion.LatestVersion.ToString());
                xmlDocument.AppendChild(xmlElement);
                return xmlDocument;
            }
        }

        /// <summary>
        /// Retrieves the visual definition associated with the specified primitive name or the default visual element definition
        /// if no primitive name is provided.
        /// </summary>
        /// <param name="primitiveName">The name of the primitive whose visual definition is to be retrieved. If null or empty, retrieves the default visual element definition.</param>
        /// <returns>A byte array containing the visual definition data.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the visual definition cannot be retrieved due to an invalid or missing attribute value.</exception>
        private byte[] GetVisualDefinition(string? primitiveName = null)
        {
            var attributeName = string.IsNullOrEmpty(primitiveName)
                ? "_VisualElementDefinition"
                : primitiveName + "._VisualElementDefinition";

            var value = obj.Attributes[attributeName].GetValue();

            if (value is not byte[] binary)
                throw new InvalidOperationException("");

            return binary;
        }
    }

    /// <summary>
    /// Compares the value of a specified attribute in the given <see cref="XElement" /> with a provided value.
    /// </summary>
    /// <param name="element">The <see cref="XElement" /> to inspect.</param>
    /// <param name="name">The name of the attribute to check within the <see cref="XElement" />.</param>
    /// <param name="value">The value to compare with the attribute's value.</param>
    /// <returns><c>true</c> if the attribute exists and its value matches the specified value; otherwise, <c>false</c>.</returns>
    private static bool HasValue(XElement element, string name, string value)
    {
        return StringComparer.OrdinalIgnoreCase.Equals(element.Attribute(name)?.Value, value);
    }
}