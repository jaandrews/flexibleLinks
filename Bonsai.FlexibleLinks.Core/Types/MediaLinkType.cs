using Bonsai.FlexibleLinks.Core;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Bonsai.FlexibleLinks.Core.Types {
    public class MediaLinkType: BasePickedLinkType {
        public override PickerType Picker => PickerType.Media;
        public override string Icon => "icon-picture";
        public override string Name => "Media";
        public override Guid Key => new Guid(FlexibleLinkConstants.LinkTypes.Media);
        public override bool AllowExtra => true;
        public override bool AllowNewTab => true;
        public override IEnumerable<DataTypeConfigurationFieldDisplay> Settings => new List<DataTypeConfigurationFieldDisplay> {
            new DataTypeConfigurationFieldDisplay {
                View = "mediapicker",
                Key = "startNode",
                Description = "Chooses the start node for the media picker.",
                Name = "Start Node"
            }
        };

        private readonly IUmbracoContextFactory _umbracoContextFactory;
        public MediaLinkType(IUmbracoContextFactory umbracoContextFactory) {
            _umbracoContextFactory = umbracoContextFactory;
        }
        public override string GetUrl(FlexibleLink link, string culture) {
            using (var context = _umbracoContextFactory.EnsureUmbracoContext()) {
                var media = UdiParser.TryParse(link.Id, out Udi udi) ? context.UmbracoContext.Media.GetById(udi) : null;
                return media != null ? media.Url(culture) + link.Extra : null;
            }
        }

        public override SelectionInfo GetInfo(string id, string culture) {
            using (var context = _umbracoContextFactory.EnsureUmbracoContext()) {
                var media = context.UmbracoContext.Media.GetById(UdiParser.Parse(id));
                return media != null ? new SelectionInfo {
                    Name = media.Name,
                    Url = media.Url(culture)
                } : null;
            }
        }

        public override IEnumerable<SelectionInfo> GetTree(string id, string culture) {
            using (var context = _umbracoContextFactory.EnsureUmbracoContext()) {
                var media = context.UmbracoContext.Media.GetById(UdiParser.Parse(id));
                return media.Children.Select(child => new SelectionInfo {
                    Name = child.Name,
                    Url = child.Url(culture),
                    HasChildren = child.Children(culture).Any(),
                    Id = new GuidUdi(Constants.UdiEntityType.Media, child.Key).ToString()
                });
            }
        }
    }
}
