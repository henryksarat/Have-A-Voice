using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View;
using System.Web.Mvc;
using System.Collections;
using HaveAVoice.Models;
using HaveAVoice.Helpers.UserInformation;

namespace HaveAVoice.Helpers.UI {
    public class NavigationHelper {
        public static string UserNavigation(SiteSection aSiteSection, SiteSection[] aSections, string[] aCssClasses, string[] aUrls, string[] aDisplayNames, User aTargetUser) {
            bool myIsMyself = false;

            UserInformationModel myUserInfo = HAVUserInformationFactory.GetUserInformation();
            if (myUserInfo != null && myUserInfo.Details.Id == aTargetUser.Id) {
                myIsMyself = true;
            }

            var myUlTag = new TagBuilder("ul");

            for (int myIndex = 0; myIndex < aSections.Count(); myIndex++) {
                var myLiTag = new TagBuilder("li");

                if (aSections[myIndex] == aSiteSection) {
                    if (myIndex == 0) {
                        myLiTag.MergeAttribute("class", "first active");
                    } else if (myIndex == (aSections.Count() - 1)) {
                        myLiTag.MergeAttribute("class", "last active");
                    } else {
                        myLiTag.MergeAttribute("class", "active");
                    }

                }

                myLiTag.MergeAttribute("name", aDisplayNames[myIndex]); 

                string myUrl = aUrls[myIndex];

                if (!myIsMyself && !aUrls[myIndex].Equals("#")) {
                    if (aSections[myIndex] == SiteSection.Home) {
                        myUrl = LinkHelper.Profile(aTargetUser);
                    } else {
                        myUrl += "/" + aTargetUser.Id;
                    }
                }

                if (aUrls[myIndex].Equals("#")) {
                    myUrl = LinkHelper.Profile(aTargetUser);
                }

                myLiTag.InnerHtml += String.Format("<a class=\"{0}\" href=\"{1}\" title=\"{2}\">{2}</a>", aCssClasses[myIndex], myUrl, aDisplayNames[myIndex]);

                myUlTag.InnerHtml += myLiTag.ToString();
            }

            return myUlTag.ToString();
        }
    }
}