using Microsoft.Extensions.DependencyInjection;
using Bonsai.FlexibleLinks.Core.Types;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Bonsai.FlexibleLinks.Core {
    public class CustomPickerComposer : IComposer {

        public void Compose(IUmbracoBuilder builder) {
            builder.Services.AddSingleton<FlexibleLinkHelper>();
            builder.FlexibleLinkTypes().Add(() => builder.TypeLoader.GetTypes<IFlexibleLinkType>());
        }
    }
}