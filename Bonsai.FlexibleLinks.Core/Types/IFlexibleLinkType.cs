using Bonsai.FlexibleLinks.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Models.ContentEditing;

namespace Bonsai.FlexibleLinks.Core.Types {
    public interface IFlexibleLinkType : IDiscoverable {
        PickerType Picker { get; }
        bool Manual { get; }
        bool AllowExtra { get; }
        bool AllowNewTab { get; }
        string Icon { get; }
        string Name { get; }
        Guid Key { get; }
        string UrlModifier { get; }
        string TargetAttribute { get; }
        string UrlPlaceholder { get; }
        string PickerPath { get; }
        IEnumerable<DataTypeConfigurationFieldDisplay> Settings { get; }
        string GetUrl(FlexibleLink link, string culture);
        SelectionInfo GetInfo(string id, string culture);
        IEnumerable<SelectionInfo> GetInfo(IEnumerable<string> ids, string culture);
        IEnumerable<SelectionInfo> GetTree(string id, string culture);
        IEnumerable<SelectionInfo> Search(string searchTerm, string culture);
    }
}
