using Bonsai.FlexibleLinks.Core;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Bonsai.FlexibleLinks.Core.Types {
    public class ContentLinkType: BasePickedLinkType {
        public IEnumerable<string> ExcludedContentTypes => Enumerable.Empty<string>();
        public override PickerType Picker => PickerType.Content;
        public override bool AllowExtra => true;
        public override bool AllowNewTab => true;
        public override string Icon => "icon-enter";
        public override string Name => "Content";
        public override Guid Key => new Guid(FlexibleLinkConstants.LinkTypes.Content);
        public override IEnumerable<DataTypeConfigurationFieldDisplay> Settings => new List<DataTypeConfigurationFieldDisplay> {
            new DataTypeConfigurationFieldDisplay {
                View = "treepicker",
                Key = "startNode",
                Description = "Chooses the start node for this picker.",
                Name = "Start Node"
            }
        };

        private readonly IUmbracoContextFactory _umbracoContextFactory;
        public ContentLinkType(IUmbracoContextFactory umbracoContextFactory) {
            _umbracoContextFactory = umbracoContextFactory;
        }
        public override string GetUrl(FlexibleLink link, string culture) {
            using (var context = _umbracoContextFactory.EnsureUmbracoContext()) {
                var node = UdiParser.TryParse(link.Id, out Udi udi) ? context.UmbracoContext.Content.GetById(udi) : null;
                if (node == null) {
                    return null;
                }
                if (node.HasValue("umbracoRedirect")) {
                    var redirect = context.UmbracoContext.Content.GetById(node.Value<Udi>("umbracoRedirect"));
                    if (redirect != null) {
                        return redirect.Url(culture) + link.Extra;
                    }

                }
                if (node.HasValue("externalRedirect")) {
                    link.NewTab = true;
                    return node.Value("externalRedirect") + link.Extra;

                }
                return node.Url(culture) + link.Extra;
            }
        }

        public override SelectionInfo GetInfo(string id, string culture) {
            using (var context = _umbracoContextFactory.EnsureUmbracoContext()) {
                var node = context.UmbracoContext.Content.GetById(UdiParser.Parse(id));
                return node != null ? new SelectionInfo {
                    Name = node.Name(culture),
                    Url = node.Url(culture)
                } : null;
            }
        }

        public override IEnumerable<SelectionInfo> GetTree(string id, string culture) {
            using (var context = _umbracoContextFactory.EnsureUmbracoContext()) {
                var node = context.UmbracoContext.Content.GetById(UdiParser.Parse(id));
                if (node.Value<bool>("hideChildrenInNavigation")) {
                    return Enumerable.Empty<SelectionInfo>();
                }
                return node.Children(culture).Where(x => !ExcludedContentTypes.Contains(x.ContentType.Alias)).Select(child => new SelectionInfo {
                    Name = child.Name(culture),
                    Url = child.Url(culture),
                    Icon = child.Value<string>("icon"),
                    HasChildren = !child.Value<bool>("hideChildrenInNavigation") && child.Children(culture).Any(),
                    Id = new GuidUdi(Constants.UdiEntityType.Document, child.Key).ToString(),
                    Hide = child.Value<bool>("hideInNavigation")
                });
            }
        }
    }
}
