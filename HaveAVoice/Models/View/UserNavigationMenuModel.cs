using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class UserNavigationMenuModel : NavigationItemModel {
        public SiteSection SiteSection { get; set; }

        public UserNavigationMenuModel(SiteSection aSection, string aCssClass) {
            SiteSection = aSection;
            CssClass = aCssClass;
        }
    }
}