using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;

namespace HaveAVoice.Helpers.UI {
    public class BoardHelper {
        public static string BoardInformationDiv(string anOuterCssClass, string aContentPlacementCssClass, string aContentCssClass, string aLinkCssClass, User aPostedBy, string aMessage) {
            var myOuterDiv = new TagBuilder("div");
            myOuterDiv.AddCssClass(anOuterCssClass);

            var myContentPlacementDiv = new TagBuilder("div");
            myContentPlacementDiv.AddCssClass(aContentPlacementCssClass);

            var myContentDiv = new TagBuilder("div");
            myContentDiv.AddCssClass(aContentCssClass);
            myContentDiv.InnerHtml = SharedContentStyleHelper.LinkToProfile(aPostedBy, aLinkCssClass);
            myContentDiv.InnerHtml += " " + aMessage;
            myContentDiv.InnerHtml += SharedStyleHelper.ClearDiv();
            myContentPlacementDiv.InnerHtml = myContentDiv.ToString();
            myOuterDiv.InnerHtml = myContentPlacementDiv.ToString();

            return myOuterDiv.ToString();
        }
    }
}