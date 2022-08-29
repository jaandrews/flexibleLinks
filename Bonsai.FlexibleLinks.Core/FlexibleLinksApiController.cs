using Microsoft.AspNetCore.Mvc;
using Bonsai.FlexibleLinks.Core.Types;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.BackOffice.Filters;
using Umbraco.Cms.Web.Common.Attributes;

namespace Bonsai.FlexibleLinks.Core {
    [PluginController("FlexibleLinks")]
    [JsonCamelCaseFormatter]
    public class FlexibleLinksApiController: UmbracoAuthorizedApiController {
        private FlexibleLinkTypeCollection _flexibleLinkTypes;
        public FlexibleLinksApiController(FlexibleLinkTypeCollection flexibleLinkTypes) {
            _flexibleLinkTypes = flexibleLinkTypes;
        }
        [HttpGet]
        public SelectionInfo GetInfo(string id, Guid type, string culture = null) {
            var linkType = _flexibleLinkTypes.First(x => x.Key == type);
            return linkType.GetInfo(id, culture);
        }

        [HttpGet]
        public IEnumerable<IFlexibleLinkType> GetTypes() {
            return _flexibleLinkTypes.OrderBy(x => x.Name);
        }

        [HttpGet]
        public IEnumerable<SelectionInfo> GetTree(string id, Guid type, string culture = null) {
            var linkType = _flexibleLinkTypes.First(x => x.Key == type);
            return linkType.GetTree(id, culture);
        }

        [HttpGet]
        public IEnumerable<SelectionInfo> Search(string searchTerm, Guid type, string culture = null) {
            var linkType = _flexibleLinkTypes.First(x => x.Key == type);
            return linkType.Search(searchTerm, culture);
        }
    }
}