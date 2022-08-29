using Bonsai.FlexibleLinks.Core;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Bonsai.FlexibleLinks.Core.Types {
    public class ModalLinkType: BasePickedLinkType {
        public override PickerType Picker => PickerType.Content;
        public override string Icon => "icon-application-window-alt";
        public override string Name => "Modal";
        public override Guid Key => new Guid(FlexibleLinkConstants.LinkTypes.Model);
        public override bool AllowExtra => false;
        public override bool AllowNewTab => false;
        public override string TargetAttribute => "data-open";
        public override IEnumerable<DataTypeConfigurationFieldDisplay> Settings => new List<DataTypeConfigurationFieldDisplay> {
            new DataTypeConfigurationFieldDisplay {
                View = "treepicker",
                Key = "startNode",
                Description = "Chooses the start node modals.",
                Name = "Start Node"
            }
        };

        private readonly IUmbracoContextFactory _umbracoContextFactory;
        public ModalLinkType(IUmbracoContextFactory umbracoContextFactory) {
            _umbracoContextFactory = umbracoContextFactory;
        }
        public override string GetUrl(FlexibleLink link, string culture) {
            using (var context = _umbracoContextFactory.EnsureUmbracoContext()) {
                var node = UdiParser.TryParse(link.Id, out Udi udi) ? context.UmbracoContext.Content.GetById(udi) : null;
                return node != null ? node.UrlSegment(culture) : null;
            }
        }

        public override SelectionInfo GetInfo(string id, string culture) {
            using (var context = _umbracoContextFactory.EnsureUmbracoContext()) {
                var node = context.UmbracoContext.Content.GetById(UdiParser.Parse(id));
                return node != null ? new SelectionInfo {
                    Name = node.Name,
                    Url = node.UrlSegment(culture)
                } : null;
            }
        }
    }
}
