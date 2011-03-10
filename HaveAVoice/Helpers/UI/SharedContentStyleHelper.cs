using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Services.Helpers;
using System.Web.Mvc;
using HaveAVoice.Models;

namespace HaveAVoice.Helpers.UI {
    public class SharedContentStyleHelper {
        public static string ProfilePictureDiv(User aUser, bool anIsAnonymous, string aDivCssClass, string anImageCssClass) {
            var myProfilePictureDiv = new TagBuilder("div");
            myProfilePictureDiv.AddCssClass(aDivCssClass);

            var myImage = new TagBuilder("img");
            myImage.AddCssClass(anImageCssClass);

            if (anIsAnonymous) {
                myImage.MergeAttribute("alt", "Anonymous");
                myImage.MergeAttribute("src", PhotoHelper.ConstructUrl(HAVConstants.NO_PROFILE_PICTURE_IMAGE));
            } else {
                myImage.MergeAttribute("src", PhotoHelper.ProfilePicture(aUser));
                myImage.MergeAttribute("alt", NameHelper.FullName(aUser));
            }

            myProfilePictureDiv.InnerHtml = myImage.ToString();
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

    }
}