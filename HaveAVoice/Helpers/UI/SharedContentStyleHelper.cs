using System;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Services.Helpers;
using Social.Generic.Constants;

namespace HaveAVoice.Helpers.UI {
    public class SharedContentStyleHelper {
        public static string ProfilePictureDiv(User aUser, string aDivCssClass, string anImageCssClass) {
            var myProfilePictureDiv = new TagBuilder("div");
            myProfilePictureDiv.AddCssClass(aDivCssClass);
            
            var myImage = new TagBuilder("img");
            myImage.AddCssClass(anImageCssClass);

            var myLink = new TagBuilder("a");
            myLink.MergeAttribute("href", LinkHelper.Profile(aUser));

            myImage.MergeAttribute("src", PhotoHelper.ProfilePicture(aUser));
            myImage.MergeAttribute("alt", NameHelper.FullName(aUser));

            myLink.InnerHtml = myImage.ToString();
            myProfilePictureDiv.InnerHtml = myLink.ToString();

            myProfilePictureDiv.InnerHtml += SharedStyleHelper.ClearDiv();

            return myProfilePictureDiv.ToString();
        }
        
        public static string TimeStampDiv(DateTime aDateTime, string aDivCssClass, string aPaddingDivCssClass, string aContentDivCssClass, string aDateTimeStampMonthFormat, string aDateTimeStampDayFormat) {
            var myTimeStampDiv = new TagBuilder("div");
            myTimeStampDiv.AddCssClass(aDivCssClass);

            var myTimeStampPad = new TagBuilder("div");
            myTimeStampPad.AddCssClass(aPaddingDivCssClass);

            var myTimeStampContentDiv = new TagBuilder("div");
            myTimeStampContentDiv.AddCssClass(aContentDivCssClass);

            var myTimeStampSpan = new TagBuilder("span");
            myTimeStampSpan.InnerHtml = aDateTime.ToString(aDateTimeStampMonthFormat).ToUpper();

            myTimeStampContentDiv.InnerHtml = myTimeStampSpan.ToString();
            myTimeStampContentDiv.InnerHtml += "&nbsp;";
            myTimeStampContentDiv.InnerHtml += aDateTime.ToString(aDateTimeStampDayFormat);

            myTimeStampPad.InnerHtml = myTimeStampContentDiv.ToString();

            myTimeStampDiv.InnerHtml += myTimeStampPad.ToString();
            myTimeStampDiv.InnerHtml += SharedStyleHelper.ClearDiv();

            return myTimeStampDiv.ToString();
        }

        public static string LinkToProfile(User aUser, string aCssClass) {
            return SharedStyleHelper.Link(aCssClass, LinkHelper.Profile(aUser), NameHelper.FullName(aUser));
        }

        public static TagBuilder AgreeStanceDiv(string aCssClass, bool aHasDisposition, bool anIsLoggedIn, int aTotalAgrees, string aStanceUrl) {
            return StanceDiv(aCssClass, aHasDisposition, anIsLoggedIn, "like", aTotalAgrees, "Agrees", "Agree", aStanceUrl);
        }

        public static TagBuilder DisagreeStanceDiv(string aCssClass, bool aHasDisposition, bool anIsLoggedIn, int aTotalDisagrees, string aStanceUrl) {
            return StanceDiv(aCssClass, aHasDisposition, anIsLoggedIn, "dislike", aTotalDisagrees, "Disagrees", "Disagree", aStanceUrl);
        }

        private static TagBuilder StanceDiv(string aCssClass, bool aHasDisposition, bool aIsLoggedIn, string aLinkCssClass, int aTotalForStance,
                                            string aSingularDisplayText, string aPluralDisplayText, string aStanceLink) {
            var myStanceDiv = new TagBuilder("div");
            myStanceDiv.AddCssClass(aCssClass);

            if (!aHasDisposition && aIsLoggedIn) {
                var myDisagreeLink = new TagBuilder("a");
                myDisagreeLink.MergeAttribute("href", aStanceLink);
                myDisagreeLink.AddCssClass(aLinkCssClass);
                myDisagreeLink.InnerHtml += aPluralDisplayText + " (" + aTotalForStance + ")";
                myStanceDiv.InnerHtml += myDisagreeLink.ToString();
            } else {
                var myDisagreeSpan = new TagBuilder("span");
                myDisagreeSpan.AddCssClass(aLinkCssClass);
                string mySingleOrPlural = aTotalForStance == 1 ? " Person " + aSingularDisplayText : " People " + aPluralDisplayText;
                myDisagreeSpan.InnerHtml = aTotalForStance.ToString() + mySingleOrPlural;
                myStanceDiv.InnerHtml += myDisagreeSpan.ToString();
            }

            return myStanceDiv;
        }

    }
}