using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class UserNavigationMenuModel {
        public SiteSection SiteSection { get; set; }
        public string CssClass { get; set; }
        public string Url { get; set; }
        public string AltText { get; set; }

        public UserNavigationMenuModel(SiteSection aSection, string aCssClass) {
            SiteSection = aSection;
            CssClass = aCssClass;
            Url = "#";
            AltText = "user privacy settings disallow that";
        }
    }
}