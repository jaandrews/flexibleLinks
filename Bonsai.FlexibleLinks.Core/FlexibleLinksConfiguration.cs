using Umbraco.Cms.Core.Models.PublishedContent;

namespace Bonsai.FlexibleLinks.Core {
    public class FlexibleLinksConfiguration {
        public IPublishedContent ContentRoot;
        public IPublishedContent MediaRoot;
        public IPublishedContent ModalRoot;
        public string ModalAttribute;
        public bool AllowMultiple;
        public bool DisableContentPicker;
        public bool DisableExternalLink;
        public bool DisableMediaPicker;
        public bool DisableModalPicker;
        public bool DisableLabels;
    }
}