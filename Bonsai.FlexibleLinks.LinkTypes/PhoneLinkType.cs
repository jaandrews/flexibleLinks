using Bonsai.FlexibleLinks.Core;

namespace Bonsai.FlexibleLinks.Core.Types {
    public class PhoneLinkType: BaseManualLinkType {
        public override string Icon => "icon-phone";
        public override string Name => "Phone Number";
        public override Guid Key => new Guid(FlexibleLinkConstants.LinkTypes.Phone);
        public override bool AllowExtra => false;
        public override bool AllowNewTab => false;
        public override string UrlModifier => "tel:" + base.UrlModifier;
        public override string UrlPlaceholder => "Phone Number";
    }
}
