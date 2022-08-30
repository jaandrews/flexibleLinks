using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.FlexibleLinks.Core {
    /// <summary>
    /// Sets the ui behavior of a link.
    /// </summary>
    public enum PickerType {
        /// <summary>
        /// Cotnent will be selected via umbraco's built in content picker.
        /// </summary>
        Content,
        /// <summary>
        /// Content will be selected via umbraco's built in media picker.
        /// </summary>
        Media,
        /// <summary>
        /// Content will be pulled in via a custom ui (not implemented)
        /// </summary>
        Custom,
        /// <summary>
        /// Content will be manually entered.
        /// </summary>
        None
    }
}
