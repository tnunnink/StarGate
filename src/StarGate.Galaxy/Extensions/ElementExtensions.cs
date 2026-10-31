using System;
using System.Linq;
using System.Xml.Linq;
using StarGate.Core;

namespace StarGate.Galaxy.Extensions;

/// <summary>
/// Provides extension methods for <see cref="XElement" /> instances.
/// </summary>
public static class ElementExtensions
{
    /// <param name="element">The <see cref="XElement" /> to retrieve the attribute value from.</param>
    extension(XElement element)
    {
        /// <summary>
        /// Retrieves a child XML element from the current element based on the specified selector path.
        /// </summary>
        /// <param name="selector">
        /// A dot-separated string specifying the path of child elements to locate. Each segment represents a child element
        /// in the XML hierarchy. If the path is invalid or no matching element is found, null is returned.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/> that matches the specified selector, or null if no matching element exists.
        /// </returns>
        public XElement? GetChild(string selector)
        {
            return element.SelectElement(selector);
        }

        /// <summary>
        /// Retrieves the value from the specified selector, applies a parser, and returns the required parsed value.
        /// Throws an exception if the selector does not yield a valid value.
        /// </summary>
        /// <param name="selector">
        /// Specifies the path or attribute to locate the value within the XML element. If it starts with '@', it represents an attribute;
        /// otherwise, it is treated as a descendant element selector or the current element's value if empty.
        /// </param>
        /// <param name="parser">A function to parse the extracted string into the desired type.</param>
        /// <typeparam name="TValue">The type of the value to be parsed and returned.</typeparam>
        /// <returns>The parsed value of type <typeparamref name="TValue"/> extracted from the selector.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified selector does not yield a valid value or the value is missing.
        /// </exception>
        public TValue GetRequiredValue<TValue>(string selector, Func<string, TValue> parser)
        {
            var value = element.SelectValue(selector);

            if (value is null)
                throw new InvalidOperationException(
                    $"Required attribute '{selector}' was not found on element '{element.Name}'.");

            return parser(value);
        }

        /// <summary>
        /// Retrieves the value from the specified selector, applies a parser to it, and returns the parsed value.
        /// If the value is null or cannot be found, the provided fallback value is returned.
        /// </summary>
        /// <param name="selector">
        /// Specifies the path or attribute to retrieve the value from. If the selector starts with '@', it is treated as an attribute reference.
        /// Otherwise, it is treated as a descendant element selector or the current element's value if empty.
        /// </param>
        /// <param name="parser">A function to parse the retrieved string into the desired type.</param>
        /// <param name="fallback">The fallback value to return when the selector does not yield a valid value.</param>
        /// <typeparam name="TValue">The type of the value to be parsed and returned.</typeparam>
        /// <returns>The parsed value of type <typeparamref name="TValue"/>, or the fallback value if the selector does not match.</returns>
        public TValue? GetValue<TValue>(string selector, Func<string, TValue> parser, TValue? fallback = default)
        {
            var value = element.SelectValue(selector);
            return value is not null ? parser(value) : fallback;
        }

        /// <summary>
        /// Retrieves a value from the specified element using the given selector and parses it into a struct of the specified type.
        /// If the value is not found, the provided fallback value is returned.
        /// </summary>
        /// <typeparam name="TStruct">The type of the value to be returned, constrained to value types.</typeparam>
        /// <param name="selector">
        /// A string specifying how to locate the value. If it starts with '@', it is treated as an attribute of the current element.
        /// Otherwise, it is treated as a descendant element selector. An empty selector assumes the value of the current element.
        /// </param>
        /// <param name="parser">A function to parse the string into the specified struct type.</param>
        /// <param name="fallback">The fallback value to return if the specified value cannot be found or parsed.</param>
        /// <returns>
        /// The parsed value of type <typeparamref name="TStruct"/> if successful; otherwise, the fallback value is returned.
        /// </returns>
        public TStruct GetStruct<TStruct>(string selector, Func<string, TStruct> parser, TStruct fallback = default)
            where TStruct : struct
        {
            var value = element.SelectValue(selector);
            return value is not null ? parser(value) : fallback;
        }

        /// <summary>
        /// Retrieves the value of the specified selector as an enumeration of type <typeparamref name="TEnum"/>.
        /// </summary>
        /// <param name="selector">The name of the attribute or descendant element from which to retrieve the value.
        /// If the selector starts with '@', it is treated as an attribute reference; otherwise,
        /// it is treated as a descendant element.
        /// </param>
        /// <param name="fallback">The fallback value to return if the selector value is not found or cannot be parsed as <typeparamref name="TEnum"/>.</param>
        /// <typeparam name="TEnum">The enumeration type to which the value will be parsed. Must be a struct and an enumeration type.</typeparam>
        /// <returns>The parsed enumeration value of type <typeparamref name="TEnum"/> if successful, or the fallback value if parsing fails.</returns>
        public TEnum GetEnum<TEnum>(string selector, TEnum fallback = default) where TEnum : struct, Enum
        {
            var value = element.SelectValue(selector);

            if (value is not null && Enum.TryParse<TEnum>(value, true, out var result))
                return result;

            return fallback;
        }

        /// <summary>
        /// Retrieves the color information from the specified XML element and
        /// constructs a <see cref="GraphicColor" /> instance.
        /// </summary>
        /// <param name="selector">The name of the XML element containing the color attributes.</param>
        /// <returns>
        /// A <see cref="GraphicColor" /> instance representing the color defined in the XML.
        /// If the element or attributes are missing, returns the default color.
        /// </returns>
        public GraphicColor GetColor(string selector)
        {
            var color = element.Descendants(selector).FirstOrDefault();

            if (color is null) return default;

            var r = color.GetStruct("@R", byte.Parse);
            var g = color.GetStruct("@G", byte.Parse);
            var b = color.GetStruct("@B", byte.Parse);
            var a = color.GetStruct("@A", byte.Parse, (byte)255);

            return new GraphicColor(r, g, b, a);
        }

        /// <summary>
        /// Retrieves the graphical bounds of an element, including its position and dimensions.
        /// </summary>
        /// <returns>
        /// A <see cref="GraphicBounds" /> instance containing the left, top, width, and height
        /// values of the element obtained from the corresponding attributes.
        /// </returns>
        public GraphicBounds GetBounds()
        {
            return new GraphicBounds
            {
                Left = element.GetStruct("@Left", double.Parse),
                Top = element.GetStruct("@Top", double.Parse),
                Width = element.GetStruct("@Width", double.Parse),
                Height = element.GetStruct("@Height", double.Parse)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bounds"></param>
        public void SetBounds(GraphicBounds bounds)
        {
            element.SetAttributeValue("Left", bounds.Left);
            element.SetAttributeValue("Top", bounds.Top);
            element.SetAttributeValue("Width", bounds.Width);
            element.SetAttributeValue("Height", bounds.Height);
        }

        /// <summary>
        /// Gets the origin point of the <see cref="XElement" /> as a <see cref="GraphicPoint" />.
        /// </summary>
        /// <returns>A <see cref="GraphicPoint" /> representing the origin of the element.
        /// If the "OriginX" or "OriginY" attributes are not present or cannot be parsed, their values default to 0.</returns>
        public GraphicPoint GetOrigin()
        {
            return new GraphicPoint
            {
                X = element.GetStruct("@OriginX", double.Parse),
                Y = element.GetStruct("@OriginY", double.Parse)
            };
        }

        /// <summary>
        /// Sets the origin of the graphical element by updating its "OriginX" and "OriginY" attributes.
        /// </summary>
        /// <param name="point">The <see cref="GraphicPoint" /> representing the new origin coordinates.</param>
        public void SetOrigin(GraphicPoint point)
        {
            element.SetAttributeValue("OriginX", point.X);
            element.SetAttributeValue("OriginY", point.Y);
        }

        /// <summary>
        /// Determines the flip state of a graphic element based on its transformation attributes
        /// in the <see cref="XElement" />.
        /// </summary>
        /// <returns>
        /// A <see cref="GraphicFlip" /> value that specifies whether the graphic element is flipped along
        /// the horizontal axis, vertical axis, both axes, or not flipped at all.
        /// </returns>
        public GraphicFlip GetFlip()
        {
            var hasTransform = element.Attribute("HasTransform")?.Value;

            if (!string.Equals(hasTransform, "true", StringComparison.OrdinalIgnoreCase))
                return GraphicFlip.None;

            var t0 = element.Attribute("Transform_0")?.Value;
            var t3 = element.Attribute("Transform_3")?.Value;

            var flipX = string.Equals(t0, "-1", StringComparison.Ordinal);
            var flipY = string.Equals(t3, "-1", StringComparison.Ordinal);

            return (flipX, flipY) switch
            {
                (true, true) => GraphicFlip.Both,
                (true, false) => GraphicFlip.Horizontal,
                (false, true) => GraphicFlip.Vertical,
                _ => GraphicFlip.None
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="flip"></param>
        public void SetFlip(GraphicFlip? flip)
        {
            if (flip is null or GraphicFlip.None)
                return;

            element.SetAttributeValue("HasTransform", "True");

            switch (flip)
            {
                case GraphicFlip.Horizontal:
                    element.SetAttributeValue("Transform_0", "-1");
                    element.SetAttributeValue("Transform_3", "1");
                    break;
                case GraphicFlip.Vertical:
                    element.SetAttributeValue("Transform_0", "1");
                    element.SetAttributeValue("Transform_3", "-1");
                    break;
                case GraphicFlip.Both:
                    element.SetAttributeValue("Transform_0", "-1");
                    element.SetAttributeValue("Transform_3", "-1");
                    break;
                case GraphicFlip.None:
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(flip), flip, null);
            }
        }

        /// <summary>
        /// Retrieves the value of the specified selector from an XML element.
        /// The selector can navigate through element names or attributes to extract the desired value.
        /// If the selector is null or empty, the value of the current element is returned.
        /// </summary>
        /// <param name="selector">
        /// A dot-separated path specifying the hierarchy of elements or an attribute name prefixed with '@'.
        /// If the selector is empty or null, the value of the current element is returned.
        /// </param>
        /// <returns>
        /// The value extracted from the specified selector as a string or null if the selector does
        /// not yield a valid element or attribute.
        /// </returns>
        private string? SelectValue(string? selector)
        {
            if (string.IsNullOrEmpty(selector))
                return element.Value;

            var current = element;
            var span = selector.AsSpan();

            while (!span.IsEmpty)
            {
                var dotIndex = span.IndexOf('.');
                var segment = dotIndex >= 0 ? span[..dotIndex] : span;
                span = dotIndex >= 0 ? span[(dotIndex + 1)..] : default;

                if (segment.IsEmpty)
                    continue;

                if (segment.StartsWith("@"))
                    return current?.Attribute(segment[1..].ToString())?.Value;

                current = current?.Element(segment.ToString());
            }

            return current?.Value;
        }

        /// <summary>
        /// Navigates the XML structure starting from the current element and selects a descendant element
        /// based on the dot-separated selector string.
        /// </summary>
        /// <param name="selector">
        /// A dot-separated string that represents the path of descendant elements to navigate.
        /// Each segment in the string corresponds to a child element. If the selector is null or empty, the current element is returned.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/> corresponding to the specified path, or null if no matching element is found.
        /// </returns>
        private XElement? SelectElement(string? selector)
        {
            if (string.IsNullOrEmpty(selector))
                return element;

            var current = element;
            var span = selector.AsSpan();

            while (!span.IsEmpty)
            {
                var dotIndex = span.IndexOf('.');
                var segment = dotIndex >= 0 ? span[..dotIndex] : span;
                span = dotIndex >= 0 ? span[(dotIndex + 1)..] : default;

                if (segment.IsEmpty)
                    continue;

                current = current?.Element(segment.ToString());
            }

            return current;
        }
    }
}