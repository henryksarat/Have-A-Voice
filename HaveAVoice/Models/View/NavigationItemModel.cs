using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class NavigationItemModel {
        public string CssClass { get; set; }
        public string Url { get; set; }
        public string AltText { get; set; }
        public bool Display { get; set; }
        public string DisplayText { get; set; }

        public NavigationItemModel() {
            CssClass = string.Empty;
            Url = "#";
            AltText = "user privacy settings disallow that";
            Display = true;
            DisplayText = string.Empty;
        }
    }
}