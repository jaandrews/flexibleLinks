using Bonsai.FlexibleLinks.Core;
using Umbraco.Cms.Core.DependencyInjection;

namespace Bonsai.FlexibleLinks.Core {
    public static class WebCompositionExtensions {
        public static FlexibleLinkTypeCollectionBuilder FlexibleLinkTypes(this IUmbracoBuilder builder) => builder.WithCollectionBuilder<FlexibleLinkTypeCollectionBuilder>();
    }
}
