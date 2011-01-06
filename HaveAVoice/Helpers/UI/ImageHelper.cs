using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HaveAVoice.Helpers.UI {
    public class ImageHelper {
        public static string ImageLink(string controller, string action, string strOthers, string strImageURL, string alternateText, string strStyle, object htmlAttributes) {
            // Create tag builder
            var builder = new TagBuilder("a");

            // Add attributes
            builder.MergeAttribute("href", "/" + controller + "/" + action + strOthers); //form target URL
            builder.InnerHtml = "<img src='" + strImageURL + "' alt='" + alternateText + "' style=\"" + strStyle + "\">"; //set the image as inner html
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            // Render tag
            return builder.ToString(TagRenderMode.Normal); //to add </a> as end tag
        }

        public static string Image(string anImageUrl) {
            var myBuilder = new TagBuilder("img");
            myBuilder.MergeAttribute("src", anImageUrl);
            myBuilder.MergeAttribute("alt", anImageUrl);
            return myBuilder.ToString();
        }
    }
}