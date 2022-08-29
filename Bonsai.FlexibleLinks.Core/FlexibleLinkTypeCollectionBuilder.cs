using Bonsai.FlexibleLinks.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Composing;

namespace Bonsai.FlexibleLinks.Core {
    public class FlexibleLinkTypeCollectionBuilder : LazyCollectionBuilderBase<FlexibleLinkTypeCollectionBuilder, FlexibleLinkTypeCollection, IFlexibleLinkType> {
        protected override FlexibleLinkTypeCollectionBuilder This => this;
    }
}
