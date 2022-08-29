using Bonsai.FlexibleLinks.Core;
using Umbraco.Cms.Core.Models.ContentEditing;

namespace Bonsai.FlexibleLinks.Core.Types {
    public abstract class BaseManualLinkType: IFlexibleLinkType {
        public PickerType Picker => PickerType.None;
        public bool Manual => true;
        public abstract bool AllowExtra { get; }
        public abstract bool AllowNewTab { get; }
        public virtual string UrlModifier => "#url";
        public virtual string Icon => "icon-link";
        public abstract string Name { get; }
        public abstract Guid Key { get; }
        public virtual string TargetAttribute => "href";
        public virtual string UrlPlaceholder => "Url";
        public virtual string PickerPath => null;
        public virtual IEnumerable<DataTypeConfigurationFieldDisplay> Settings => null;

        public string GetUrl(FlexibleLink link, string culture) {
            return UrlModifier.Replace("#url", link.Url);
        }
        public SelectionInfo GetInfo(string id, string culture) {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectionInfo> GetInfo(IEnumerable<string> ids, string culture) {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectionInfo> GetTree(string id, string culture) {
            return Enumerable.Empty<SelectionInfo>();
        }

        public IEnumerable<SelectionInfo> Search(string searchTerm, string culture) {
            throw new NotImplementedException();
        }
    }
}
