using Bonsai.FlexibleLinks.Core;

namespace Bonsai.FlexibleLinks.Core.Types {
    public class AnchorLinkType: BaseManualLinkType {
        public override string Icon => "icon-arrow-down";
        public override string Name => "Anchor";
        public override Guid Key => new Guid(FlexibleLinkConstants.LinkTypes.Anchor);
        public override bool AllowExtra => false;
        public override bool AllowNewTab => false;
        public override string UrlModifier => "#" + base.UrlModifier;
        public override string UrlPlaceholder => "Anchor";
    }
}
