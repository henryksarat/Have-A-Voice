using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Services.Helpers;
using System.Web.Mvc;
using HaveAVoice.Models;

namespace HaveAVoice.Helpers.UI {
    public class SharedIssueStyleHelper {
        public static string TimeStampDiv(DateTime aDateTime, string aDivCssClass, string aPaddingDivCssClass, string aDateTimeStampMonthFormat, string aDateTimeStampDayFormat) {
            var myIssueTimeStampDiv = new TagBuilder("div");
            myIssueTimeStampDiv.AddCssClass(aDivCssClass);

            var myIssueTimeStampPad = new TagBuilder("div");
            myIssueTimeStampPad.AddCssClass(aPaddingDivCssClass);

            var myTimeStampSpan = new TagBuilder("span");
            myTimeStampSpan.InnerHtml = aDateTime.ToString(aDateTimeStampMonthFormat).ToUpper();

            myIssueTimeStampPad.InnerHtml += myTimeStampSpan.ToString();
            myIssueTimeStampPad.InnerHtml += "&nbsp;";
            myIssueTimeStampPad.InnerHtml += aDateTime.ToString(aDateTimeStampDayFormat);

            myIssueTimeStampDiv.InnerHtml += myIssueTimeStampPad.ToString();
            myIssueTimeStampDiv.InnerHtml += SharedStyleHelper.ClearDiv();

            return myIssueTimeStampDiv.ToString();
        }

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
    }
}