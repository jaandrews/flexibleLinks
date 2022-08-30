using Bonsai.FlexibleLinks.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Models.ContentEditing;

namespace Bonsai.FlexibleLinks.Core.Types {
    public interface IFlexibleLinkType : IDiscoverable {
        /// <summary>
        /// Sets the ui for adding/editing links with this picker type.
        /// </summary>
        PickerType Picker { get; }
        /// <summary>
        /// Url will be manually editable if this is true.
        /// </summary>
        bool Manual { get; }
        /// <summary>
        /// Allows additions to the end of the url like query strings and hash tags if true.
        /// </summary>
        bool AllowExtra { get; }
        /// <summary>
        /// Allows users to make the link open in a new tab if true.
        /// </summary>
        bool AllowNewTab { get; }
        /// <summary>
        /// Icon used to indicate what type of link this is. Easiest way to handle this is to use one of the built in umbraco icons like "icon-link."
        /// </summary>
        string Icon { get; }
        /// <summary>
        /// The name of the picker. Used in combination with the icon when adding new links to differenciate the pickers.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Unique id used to identify the picker. Must be unique for every picker.
        /// </summary>
        Guid Key { get; }
        /// <summary>
        /// Method that modifies the raw url. Can be used to automatically add the "mailto:" to the front of an entered email for example.
        /// </summary>
        string UrlModifier { get; }
        /// <summary>
        /// The attribute the url will be added in. This is mostly for javascript driven links that require a different attribute than "href."
        /// </summary>
        string TargetAttribute { get; }
        /// <summary>
        /// Prompt shown in the url field of each link when it's empty.
        /// </summary>
        string UrlPlaceholder { get; }
        /// <summary>
        /// Path to the html file that controls the selection of content for the picker. Indented mostly for custom picker controls, which are not supported yet.
        /// </summary>
        string PickerPath { get; }
        /// <summary>
        /// These are used to set additional information needed by the picker. The syntax is the same as what is used to configure data type pre values.
        /// </summary>
        IEnumerable<DataTypeConfigurationFieldDisplay> Settings { get; }
        /// <summary>
        /// Gets the resulting url for the provided data
        /// </summary>
        /// <param name="link">The raw data stored by the flexible link control.</param>
        /// <param name="culture">The current culture.</param>
        /// <returns>A url for the link.</returns>
        string GetUrl(FlexibleLink link, string culture);
        /// <summary>
        /// Gets information about the link from its id.
        /// </summary>
        /// <param name="id">The id of the link.</param>
        /// <param name="culture">The current culture code.</param>
        /// <returns>Meta data for the link.</returns>
        SelectionInfo GetInfo(string id, string culture);
        /// <summary>
        /// Gets information about multiple links from there ids. Often works by iterating through the ids and call the singular GetInfo, but this is not an option for some data sources.
        /// </summary>
        /// <param name="ids">The ids of the link to get more information about.</param>
        /// <param name="culture">The current culture code.</param>
        /// <returns>Meta data for the links.</returns>
        IEnumerable<SelectionInfo> GetInfo(IEnumerable<string> ids, string culture);
        /// <summary>
        /// Gets the children for the link with the current id.
        /// </summary>
        /// <param name="id">The id of the curreent link.</param>
        /// <param name="culture">The current culture code.</param>
        /// <returns>Meta data for children of the link.</returns>
        IEnumerable<SelectionInfo> GetTree(string id, string culture);
        /// <summary>
        /// Returns content that matches the provided search term.
        /// </summary>
        /// <param name="searchTerm">The text to search for.</param>
        /// <param name="culture">The current culture code.</param>
        /// <returns>Meta data for links that match the searchTerm.</returns>
        IEnumerable<SelectionInfo> Search(string searchTerm, string culture);
    }
}
