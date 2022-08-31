using Bonsai.FlexibleLinks.Core;
using Umbraco.Cms.Core.Models.ContentEditing;

namespace Bonsai.FlexibleLinks.Core.Types {
    public abstract class BasePickedLinkType: IFlexibleLinkType {
        public virtual PickerType Picker => PickerType.Custom;
        public bool Manual => false;
        public abstract bool AllowExtra { get; }
        public abstract bool AllowNewTab { get; }
        public virtual string UrlModifier => "#url";
        public abstract string Icon { get; }
        public abstract string Name { get; }
        public abstract Guid Key { get; }
        public virtual string TargetAttribute => "href";
        public virtual string UrlPlaceholder => "Url";
        public virtual string PickerPath => "/app_plugins/flexiblelinks/tree-picker.html";
        public virtual IEnumerable<DataTypeConfigurationFieldDisplay> Settings => null;

        public abstract string GetUrl(FlexibleLink link, string culture);
        public abstract SelectionInfo GetInfo(string id, string culture);

        public IEnumerable<SelectionInfo> GetInfo(IEnumerable<string> ids, string culture) {
            return ids.Select(id => GetInfo(id, culture));
        }

        public virtual IEnumerable<SelectionInfo> GetTree(string id, string culture) {
            return Enumerable.Empty<SelectionInfo>();
        }

        public virtual IEnumerable<SelectionInfo> Search(string searchTerm, string culture) {
            throw new NotImplementedException();
        }
    }
}
