using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace Bonsai.FlexibleLinks.Core {
    public class FlexibleLinksConverter : PropertyValueConverterBase {
        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias.Equals("Bonsai.FlexibleLinks");
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Snapshot;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
            var config = propertyType.DataType.ConfigurationAs<Dictionary<string, object>>();

            return (string)config["allowMultiple"] == "1" ? typeof(IEnumerable<FlexibleLink>) : typeof(FlexibleLink);
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview) {
            if (source == null) {
                return null;
            }
            return source.ToString();
        }

        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview) {
            var config = propertyType.DataType.ConfigurationAs<Dictionary<string, object>>();
            if (string.IsNullOrEmpty((string)inter)) {
                return null;
            }
            var links = JsonConvert.DeserializeObject<IEnumerable<FlexibleLink>>((string)inter);
            if ((string)config["allowMultiple"] == "1") {
                return links;
            }
            else {
                return links.FirstOrDefault();
            }
        }
    }
}