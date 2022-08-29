using Bonsai.FlexibleLinks.Core;

namespace Bonsai.FlexibleLinks.Core.Types {
    public class ExternalLinkType: BaseManualLinkType {
        public override string Icon => "icon-out";
        public override string Name => "External";
        public override Guid Key => new Guid(FlexibleLinkConstants.LinkTypes.External);
        public override bool AllowExtra => true;
        public override bool AllowNewTab => true;
    }
}
