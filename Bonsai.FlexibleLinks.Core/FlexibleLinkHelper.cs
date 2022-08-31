using Bonsai.FlexibleLinks.Core;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;

namespace Bonsai.FlexibleLinks.Core
{
    public class FlexibleLinkHelper {
        private readonly Lazy<FlexibleLinkTypeCollection> _linkTypes;

        public FlexibleLinkHelper(Lazy<FlexibleLinkTypeCollection> linkTypes) {
            _linkTypes = linkTypes;
        }

        /// <summary>
        /// Gets html for a single link.
        /// </summary>
        /// <param name="link">Data for the link.</param>
        /// <param name="format">Used to modify how the label is output. The text "#label#" will be replaced with the value of the label data in the "link" property.</param>
        /// <param name="classes">Classes to add to the link.</param>
        /// <param name="culture">The target culture. Used to resolve the link to the correct url when culture is a factor.</param>
        /// <param name="mappings">Used to replace text in the label that has the form "[key]" with the value in the dictionary.</param>
        /// <param name="attributes">Used to add additional attributes to the "a" tag in the form "key=value."</param>
        /// <returns>The html for the link.</returns>
        public HtmlString GetLink(FlexibleLink link, string format = "#label#", string classes = "", string culture = null, Dictionary<string, string> mappings = null, Dictionary<string, string> attributes = null)
        {
            var label = link.Label ?? "";
            var emphasizer = new Regex("\\*\\*(.*)\\*\\*");
            var breaker = new Regex("(\\s{2,})");
            var result = GetAttributes(link, culture: culture);
            var text = format.Replace("#label#", breaker.Replace(emphasizer.Replace(label, "<span class=\"emphasize\">$1</span>"), "<br/>"));
            var classAttr = string.IsNullOrEmpty(classes) ? "" : "class=\"" + classes + "\"";
            if (mappings != null) {
                foreach (var mapping in mappings) {
                    result = result.Replace($"[{mapping.Key}]", mapping.Value);
                }
            }
            if (attributes != null) {
                foreach(var pair in attributes) {
                    result += $" {pair.Key}=\"{pair.Value}\"";
                }
            }
            return new HtmlString($"<a {classAttr} {result}>{text}</a>");
        }

        /// <summary>
        /// Used to get html for multiple links.
        /// </summary>
        /// <param name="links">Data for the links.</param>
        /// <param name="format">Used to modify how the label is output. The text "#label#" will be replaced with the value of the label data in the "link" property.</param>
        /// <param name="classes">Classes to add to the link.</param>
        /// <param name="culture">The target culture. Used to resolve the link to the correct url when culture is a factor.</param>
        /// <param name="mappings">Used to replace text in the label that has the form "[key]" with the value in the dictionary.</param>
        /// <param name="attributes">Used to add additional attributes to the "a" tag in the form "key=value."</param>
        /// <returns>The html for the links.</returns>
        public IEnumerable<HtmlString> GetLinks(IEnumerable<FlexibleLink> links, string format = "#label#", string classes = "", string culture = null, Dictionary<string, string> mappings = null, Dictionary<string, string> attributes = null)
        {
            return links.Select(link => GetLink(link, format, classes, culture, mappings, attributes));
        }

        /// <summary>
        /// Retrieves meta data for the current link. Can be used to determine if the link has children for example.
        /// </summary>
        /// <param name="link">The raw link data.</param>
        /// <param name="culture">The target culture.</param>
        /// <returns>More information about the link.</returns>
        public SelectionInfo GetInfo(FlexibleLink link, string culture = null) {
            var type = _linkTypes.Value.First(x => x.Key == link.Type);
            return type.GetInfo(link.Id, culture);
        }

        /// <summary>
        /// Retrieves the url for the provided link.
        /// </summary>
        /// <param name="link">The raw link data.</param>
        /// <param name="culture">The target culture.</param>
        /// <returns>Url for the link.</returns>
        public string GetUrl(FlexibleLink link, string culture = null) {
            var type = _linkTypes.Value.First(x => x.Key == link.Type);
            return type.GetUrl(link, culture);
        }

        /// <summary>
        /// Gets the attributes that would be applied to the link when rendering like "target" and "href."
        /// </summary>
        /// <param name="link">The raw link data.</param>
        /// <param name="culture">The target culture code.</param>
        /// <returns>Stringified attributes for the link.</returns>
        public string GetAttributes(FlexibleLink link, string culture = null)
        {
            var type = _linkTypes.Value.First(x => x.Key == link.Type);
            var hasExtra = !string.IsNullOrEmpty(link.Extra);
            var result = type.TargetAttribute + "=\"" + type.GetUrl(link, culture) + "\"";
            if (!string.IsNullOrEmpty(link.Extra)) {
                result += link.Extra;
            }
            if (link.NewTab) {
                result += " target=\"_blank\"";
            }
            return result;
        }

        /// <summary>
        /// Gets children of the targeted link.
        /// </summary>
        /// <param name="link">The raw link data.</param>
        /// <param name="culture">The targeted culture.</param>
        /// <returns>A list of child links.</returns>
        public IEnumerable<FlexibleLink> Children(FlexibleLink link, string culture = null) {
            var type = _linkTypes.Value.First(x => x.Key == link.Type);
            return type.GetTree(link.Id, culture).Where(x => !x.Hide).Select(x => new FlexibleLink {
                Id = x.Id,
                Label = x.Name,
                Type = link.Type,
                NewTab = link.NewTab,
                Url = x.Url
            });
        }
    }
}