using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.Helpers;
using HaveAVoice.Models;

namespace HaveAVoice.Helpers.UI {
    class SharedStyleHelper {
        public static TagBuilder StyledHtmlDiv(string aStyle, string aLink) {
            var myComplaintDiv = new TagBuilder("div");
            myComplaintDiv.MergeAttribute("class", aStyle);
            myComplaintDiv.InnerHtml = aLink;
            return myComplaintDiv;
        }

        public static string InfoSpeakSpan(string aCssClass) {
            var myInfoSpeakSpan = new TagBuilder("span");
            myInfoSpeakSpan.AddCssClass(aCssClass);
            myInfoSpeakSpan.InnerHtml = "&nbsp;";
            return myInfoSpeakSpan.ToString();
        }

        public static string ClearDiv() {
            var myClearDiv = new TagBuilder("div");
            myClearDiv.MergeAttribute("class", "clear");
            myClearDiv.InnerHtml = "&nbsp;";
            return myClearDiv.ToString();
        }

        public static string Link(string aCssClass, string aUrl, string aDisplay) {
            var myLink = new TagBuilder("a");
            myLink.AddCssClass(aCssClass);
            myLink.MergeAttribute("href", aUrl);
            myLink.InnerHtml = aDisplay;
            return myLink.ToString();
        }
    }
}