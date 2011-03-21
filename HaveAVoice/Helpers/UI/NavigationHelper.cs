using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View;
using System.Web.Mvc;
using System.Collections;
using HaveAVoice.Models;
using HaveAVoice.Helpers.UserInformation;
using Social.Generic.Models;

namespace HaveAVoice.Helpers.UI {
    public class NavigationHelper {
        public static string UserNavigation(SiteSection aSiteSection, IEnumerable<UserNavigationMenuModel> aMenuItems, User aTargetUser) {
            bool myIsMyself = false;

            IEnumerator<UserNavigationMenuModel> myMenuEnumerator = aMenuItems.GetEnumerator();

            UserInformationModel<User> myUserInfo = HAVUserInformationFactory.GetUserInformation();
            if (myUserInfo != null && myUserInfo.Details.Id == aTargetUser.Id) {
                myIsMyself = true;
            }

            var myUlTag = new TagBuilder("ul");
            int myItemCount = 0;
            while (myMenuEnumerator.MoveNext()) {
                var myLiTag = new TagBuilder("li");
                UserNavigationMenuModel myMenuItem = myMenuEnumerator.Current;

                if (myMenuItem.SiteSection == aSiteSection) {
                    if (myItemCount == 0) {
                        myLiTag.MergeAttribute("class", "first active");
                    } else if (myItemCount == (aMenuItems.Count() - 1)) {
                        myLiTag.MergeAttribute("class", "last active");
                    } else {
                        myLiTag.MergeAttribute("class", "active");
                    }

                }

                myLiTag.MergeAttribute("name", myMenuItem.AltText);
                string myUrl = myMenuItem.Url;
                if (!myIsMyself && !myUrl.Equals("#")) {
                    if (myMenuItem.SiteSection == SiteSection.Home) {
                        myUrl = LinkHelper.Profile(aTargetUser);
                    } else {
                        myUrl += "/" + aTargetUser.Id;
                    }
                }

                if (myUrl.Equals("#")) {
                    myUrl = LinkHelper.Profile(aTargetUser);
                }

                myLiTag.InnerHtml += String.Format("<a class=\"{0}\" href=\"{1}\" title=\"{2}\">{2}</a>", myMenuItem.CssClass, myUrl, myMenuItem.AltText);

                myUlTag.InnerHtml += myLiTag.ToString();
                myItemCount++;
            }

            return myUlTag.ToString();
        }
    }
}