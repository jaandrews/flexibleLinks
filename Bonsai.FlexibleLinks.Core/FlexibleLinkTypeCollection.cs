using Bonsai.FlexibleLinks.Core.Types;
using Umbraco.Cms.Core.Composing;

namespace Bonsai.FlexibleLinks.Core {
    public class FlexibleLinkTypeCollection : BuilderCollectionBase<IFlexibleLinkType> {
        public FlexibleLinkTypeCollection(Func<IEnumerable<IFlexibleLinkType>> items) : base(items) {
        }
    }
}