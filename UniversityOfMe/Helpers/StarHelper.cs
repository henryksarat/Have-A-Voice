using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityOfMe.Helpers {
    public static class StarHelper {
        public static string FiveStarReadOnly(int aUniqueId, int aSelectedRating) {
            string myOutputHtml = string.Empty;
            for (int i = 1; i <= 5; i++) {

                bool myChecked = i == aSelectedRating ? true : false;
                var myRadio = new TagBuilder("input");
                myRadio.MergeAttribute("id", "rating" + aUniqueId);
                myRadio.MergeAttribute("name", "rating" + aUniqueId);
                myRadio.MergeAttribute("type", "radio");
                myRadio.MergeAttribute("value", i.ToString());

                if (myChecked) {
                    myRadio.MergeAttribute("checked", "checked");
                }

                myOutputHtml += myRadio.ToString();
            }

            return myOutputHtml;
        }
    }
}