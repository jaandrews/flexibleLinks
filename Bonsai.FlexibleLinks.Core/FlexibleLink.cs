using Newtonsoft.Json;
using System;

namespace Bonsai.FlexibleLinks.Core {
    public class FlexibleLink {
        public string Id { get; set; }
        public Guid Key { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public Guid Type { get; set; }
        public string Extra { get; set; }
        public bool NewTab { get; set; }
    }
}
