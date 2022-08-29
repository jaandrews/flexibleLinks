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

        public IEnumerable<HtmlString> GetLinks(IEnumerable<FlexibleLink> links, string format = "#label#", string classes = "", string culture = null, Dictionary<string, string> mappings = null)
        {
            return links.Select(link => GetLink(link, format, classes, culture, mappings));
        }
        public SelectionInfo GetInfo(FlexibleLink link, string culture = null) {
            var type = _linkTypes.Value.First(x => x.Key == link.Type);
            return type.GetInfo(link.Id, culture);
        }

        public string GetUrl(FlexibleLink link, string culture = null) {
            var type = _linkTypes.Value.First(x => x.Key == link.Type);
            return type.GetUrl(link, culture);
        }

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

        public string GetFullLink(FlexibleLink link, string culture = null)
        {
            var type = _linkTypes.Value.First(x => x.Key == link.Type);
            var result = "";
            var hasExtra = !string.IsNullOrEmpty(link.Extra);
            result = type.GetUrl(link, culture);
            if (!string.IsNullOrEmpty(link.Extra)) {
                result += link.Extra;
            }
            if (link.NewTab) {
                result += " target=\"_blank\"";
            }
            return result;
        }

        public IEnumerable<FlexibleLink> Children(FlexibleLink link, string culture = null) {
            var type = _linkTypes.Value.First(x => x.Key == link.Type);
            return type.GetTree(link.Id, culture).Where(x => !x.Hide).Select(x => new FlexibleLink {
                Id = x.Id,
                Label = x.Name,
                Type = link.Type,
                NewTab = false,
                Url = x.Url
            });
        }
    }
}