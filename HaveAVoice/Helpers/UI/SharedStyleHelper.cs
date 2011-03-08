using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaveAVoice.Helpers.UI {
    class SharedStyleHelper {
        public static TagBuilder StyledHtmlDiv(string aStyle, string aLink) {
            var myComplaintDiv = new TagBuilder("div");
            myComplaintDiv.MergeAttribute("class", aStyle);
            myComplaintDiv.InnerHtml = aLink;
            return myComplaintDiv;
        }

        public static string InfoSpeakSpan() {
            var myInfoSpeakSpan = new TagBuilder("span");
            myInfoSpeakSpan.MergeAttribute("class", "speak-lft");
            myInfoSpeakSpan.InnerHtml = "&nbsp;";
            return myInfoSpeakSpan.ToString();
        }

        public static string ClearDiv() {
            var myClearDiv = new TagBuilder("div");
            myClearDiv.MergeAttribute("class", "clear");
            myClearDiv.InnerHtml = "&nbsp;";
            return myClearDiv.ToString();
        }
    }
}