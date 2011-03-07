using System;
using System.Text;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace HaveAVoice.Helpers.UI {
    public class IssueHelper {
        public static string PersonFilterButton(PersonFilter aFilter, Dictionary<string, string> aSelectedFilters, string aDisplayName, string aNonSelectedCssClass, string aSelectedCssClass) {
            return Filter("PersonFilter", "FilterIssueByPersonFilter", aFilter.ToString(), aSelectedFilters, aDisplayName, aNonSelectedCssClass, aSelectedCssClass);
        }

        public static string IssueStanceFilterButton(IssueStanceFilter aFilter, Dictionary<string, string> aSelectedFilters, string aNonSelectedCssClass, string aSelectedCssClass) {
            return Filter("IssueStanceFilter", "FilterIssueByIssueStanceFilter", aFilter.ToString(), aSelectedFilters, aFilter.ToString(), aNonSelectedCssClass, aSelectedCssClass);
        }

        private static string Filter(string aFilterType, string anActionMethodName, string aFilterText, Dictionary<string, string> aSelectedFilters, string aDisplayName, string aNonSelectedCssClass, string aSelectedCssClass) {
            var linkTag = new TagBuilder("a");
            linkTag.InnerHtml = aDisplayName;
            linkTag.MergeAttribute("href", "/Issue/" + anActionMethodName + "?filterValue=" + aFilterText);
            string mySelectedFilter = aSelectedFilters[aFilterType];
            if (mySelectedFilter == aFilterText) {
                linkTag.MergeAttribute("class", aSelectedCssClass);
            } else {
                linkTag.MergeAttribute("class", aNonSelectedCssClass);
            }

            return linkTag.ToString();
        }

        public static string PersonFilterButton(PersonFilter aFilter, PersonFilter aSelectedFilter, string aDisplayName, string aNonSelectedCssClass, string aSelectedCssClass) {
            var linkTag = new TagBuilder("a");
            linkTag.InnerHtml = aDisplayName;
            linkTag.MergeAttribute("href", "/Profile/FilterFeed?filterValue=" + aFilter);
            if (aSelectedFilter == aFilter) {
                linkTag.MergeAttribute("class", aSelectedCssClass);
            } else {
                linkTag.MergeAttribute("class", aNonSelectedCssClass);
            }

            return linkTag.ToString();
        }

        public static string BuildIssueDisplay(IEnumerable<IssueWithDispositionModel> anIssues, bool anIsLike) {
            string myIssueDisplay = string.Empty;

            foreach (IssueWithDispositionModel myIssue in anIssues) {
                string myAvatarURL = PhotoHelper.ConstructUrl(HAVConstants.NO_PROFILE_PICTURE_IMAGE);
                string myName = "Anonymous";
                string myProfile = "/Authentication/Login";
                //if (PrivacyHelper.IsAllowed(myIssue.Issue.User, PrivacyAction.DisplayProfile)) {
                    myAvatarURL = PhotoHelper.ProfilePicture(myIssue.Issue.User);
                    myName = NameHelper.FullName(myIssue.Issue.User);
                    myProfile = LinkHelper.Profile(myIssue.Issue.User);
                //}
                
                var myClearDiv = new TagBuilder("div");
                myClearDiv.MergeAttribute("class", "clear");
                myClearDiv.InnerHtml = "&nbsp;";
                
                var myOuterDiv = new TagBuilder("div");
                myOuterDiv.MergeAttribute("class", "m-btm30");

                var myDivImageWrapper = new TagBuilder("div");
                myDivImageWrapper.MergeAttribute("class", "col-2 center m-rgt10");
                myDivImageWrapper.InnerHtml = "<img src=\"" + myAvatarURL + "\" alt=\"" + myName + "\" class=\"profile\"  />";
                myDivImageWrapper.InnerHtml += myClearDiv.ToString();
                myOuterDiv.InnerHtml = myDivImageWrapper.ToString();

                var myContextDiv = new TagBuilder("div");
                myContextDiv.MergeAttribute("class", "col-9");

                var myUserlink = new TagBuilder("a");
                myUserlink.MergeAttribute("class", "profile");
                myUserlink.MergeAttribute("href", myProfile);
                myUserlink.InnerHtml = myName;
                myContextDiv.InnerHtml += myUserlink.ToString();

				var issueLink = new TagBuilder("a");
				issueLink.MergeAttribute("class", "iss-fnt");
				issueLink.MergeAttribute("href", LinkHelper.IssueUrl(myIssue.Issue.Title));
				issueLink.InnerHtml = myIssue.Issue.Title;
                myContextDiv.InnerHtml += issueLink.ToString();
                myContextDiv.InnerHtml += "<br />";
                myContextDiv.InnerHtml += myIssue.Issue.Description + "  ";
                
                var readMore = new TagBuilder("a");
                readMore.MergeAttribute("class", "read-more");
                readMore.MergeAttribute("href", LinkHelper.IssueUrl(myIssue.Issue.Title));
                readMore.InnerHtml += "Read More and particiapte &raquo;";
                myContextDiv.InnerHtml += readMore.ToString();

                var mySpan = new TagBuilder("span");
                mySpan.MergeAttribute("class", "profile");
                mySpan.InnerHtml = String.Format("On {0} by {1} - {2}, {3}", myIssue.Issue.DateTimeStamp, myName, myIssue.Issue.City, myIssue.Issue.State);
                myContextDiv.InnerHtml += mySpan.ToString();
                myContextDiv.InnerHtml += myClearDiv.ToString();

                myOuterDiv.InnerHtml += myContextDiv.ToString();

                myOuterDiv.InnerHtml += myClearDiv.ToString();

                myIssueDisplay += myOuterDiv.ToString();
            }

            return myIssueDisplay;
        }

        public static string BuildIssueForIssueReplyDisplay(Issue anIssue) {
            var myIssueDiv = new TagBuilder("div");
            myIssueDiv.MergeAttribute("class", "m-btm10");

            var myIssueProfileDiv = new TagBuilder("div");
            myIssueProfileDiv.MergeAttribute("class", "col-3 center issue-profile");

            string myIssueProfilePictureURL = PhotoHelper.ProfilePicture(anIssue.User);
            string myIssueFullName = NameHelper.FullName(anIssue.User);

            var myProfileImg = new TagBuilder("img");
            myProfileImg.MergeAttribute("alt", myIssueFullName);
            myProfileImg.MergeAttribute("src", myIssueProfilePictureURL);
            myProfileImg.MergeAttribute("class", "profile");

            myIssueProfileDiv.InnerHtml += myProfileImg.ToString();
            myIssueDiv.InnerHtml += myIssueProfileDiv.ToString();

            var myIssueInfoDiv = IssueInformationDiv(anIssue);
            myIssueDiv.InnerHtml += myIssueInfoDiv.ToString();

            var myIssueTimeStamp = new TagBuilder("div");
            myIssueTimeStamp.MergeAttribute("class", "col-3 date-tile");

            var myIssueTimeStampPad = new TagBuilder("div");
            myIssueTimeStampPad.MergeAttribute("class", "p-a10");

            var myTimeStampSpan = new TagBuilder("span");
            myTimeStampSpan.InnerHtml = anIssue.DateTimeStamp.ToString("MMM").ToUpper();

            myIssueTimeStampPad.InnerHtml += myTimeStampSpan.ToString();
            myIssueTimeStampPad.InnerHtml += "&nbsp;";
            myIssueTimeStampPad.InnerHtml += anIssue.DateTimeStamp.ToString("dd");

            myIssueTimeStamp.InnerHtml += myIssueTimeStampPad.ToString();

            myIssueDiv.InnerHtml += myIssueTimeStamp.ToString();
            myIssueDiv.InnerHtml += SharedStyleHelper.ClearDiv();

            return myIssueDiv.ToString();
        }

        private static TagBuilder IssueInformationDiv(Issue anIssue) {
            UserInformationModel myUserInfo = HAVUserInformationFactory.GetUserInformation();

            var myIssueInfoDiv = new TagBuilder("div");
            myIssueInfoDiv.MergeAttribute("class", "m-lft col-18 m-rgt comment");

            var myIssueInfoPadding = new TagBuilder("div");
            myIssueInfoPadding.MergeAttribute("class", "p-a10");
            myIssueInfoPadding.InnerHtml += SharedStyleHelper.InfoSpeakSpan();

            var myHeadTitle = new TagBuilder("h1");
            var myIssueLink = new TagBuilder("a");
            myIssueLink.MergeAttribute("href", LinkHelper.IssueUrl(anIssue.Title));
            myIssueLink.InnerHtml = anIssue.Title;
            myHeadTitle.InnerHtml += myIssueLink.ToString();

            var myNameLink = new TagBuilder("a");
            myNameLink.MergeAttribute("class", "name-2");
            myNameLink.MergeAttribute("href", LinkHelper.Profile(anIssue.User));
            myNameLink.InnerHtml = NameHelper.FullName(anIssue.User);

            var myLocationSpan = new TagBuilder("span");
            myLocationSpan.MergeAttribute("class", "loc c-white");
            myLocationSpan.InnerHtml = anIssue.City + ", " + anIssue.State;

            myIssueInfoPadding.InnerHtml += myHeadTitle.ToString();
            myIssueInfoPadding.InnerHtml += myNameLink.ToString();
            myIssueInfoPadding.InnerHtml += "&nbsp;";
            myIssueInfoPadding.InnerHtml += myLocationSpan.ToString();
            myIssueInfoPadding.InnerHtml += new TagBuilder("br").ToString();
            myIssueInfoPadding.InnerHtml += anIssue.Description;

            myIssueInfoPadding.InnerHtml += SharedStyleHelper.ClearDiv();

            var myEditAndStanceDiv = new TagBuilder("div");
            myEditAndStanceDiv.MergeAttribute("class", "col-17 p-v10 options");
            myEditAndStanceDiv.InnerHtml += DeleteDiv(myUserInfo, anIssue);
            myEditAndStanceDiv.InnerHtml += EditDiv(myUserInfo, anIssue);
            myEditAndStanceDiv.InnerHtml += ComplaintDiv(myUserInfo, anIssue);
            myEditAndStanceDiv.InnerHtml += SharedStyleHelper.ClearDiv();

            myIssueInfoPadding.InnerHtml += myEditAndStanceDiv.ToString();

            myIssueInfoDiv.InnerHtml += myIssueInfoPadding.ToString();
            return myIssueInfoDiv;
        }

        private static TagBuilder DeleteDiv(UserInformationModel myUserInformationModel, Issue anIssue) {
            var myDeleteDiv = new TagBuilder("div");
            myDeleteDiv.MergeAttribute("class", "push-10 col-2 center");

            if (IssueHelper.ShouldDisplayDeleteLink(myUserInformationModel, anIssue)) {
                var myDeleteLink = new TagBuilder("a");
                myDeleteLink.MergeAttribute("href", LinkHelper.DeleteIssue(anIssue));
                myDeleteLink.MergeAttribute("class", "delete");
                myDeleteLink.InnerHtml += "Delete";
                myDeleteDiv.InnerHtml += myDeleteLink.ToString();
            } else {
                myDeleteDiv.InnerHtml += "&nbsp;";
            }

            myDeleteDiv.InnerHtml += SharedStyleHelper.ClearDiv();
            return myDeleteDiv;
        }

        private static TagBuilder EditDiv(UserInformationModel myUserInformationModel, Issue anIssue) {
            var myEditDiv = new TagBuilder("div");
            myEditDiv.MergeAttribute("class", "push-10 col-2 center");

            if (IssueHelper.ShouldDisplayEditLink(myUserInformationModel, anIssue)) {
                var myEditLink = new TagBuilder("a");
                myEditLink.MergeAttribute("href", LinkHelper.EditIssue(anIssue));
                myEditLink.MergeAttribute("class", "edit");
                myEditLink.InnerHtml += "Edit";
                myEditDiv.InnerHtml += myEditLink.ToString();
            } else {
                myEditDiv.InnerHtml += "&nbsp;";
            }

            myEditDiv.InnerHtml += SharedStyleHelper.ClearDiv();
            return myEditDiv;
        }

        private static TagBuilder ComplaintDiv(UserInformationModel myUserInformationModel, Issue anIssue) {
            var myComplaintDiv = new TagBuilder("div");
            myComplaintDiv.MergeAttribute("class", "push-10 col-3 center");

            if (myUserInformationModel != null) {
                myComplaintDiv.InnerHtml += ComplaintHelper.IssueLink(anIssue.Id);
            }

            myComplaintDiv.InnerHtml += SharedStyleHelper.ClearDiv();
            return myComplaintDiv;
        }

        public static bool ShouldDisplayEditLink(UserInformationModel aUserInformation, Issue anIssue) {
            return (HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Edit_Issue) && aUserInformation.Details.Id == anIssue.UserId) 
                || HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Edit_Any_Issue);
        }

        public static bool ShouldDisplayDeleteLink(UserInformationModel aUserInformation, Issue anIssue) {
            return (HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Delete_Issue) && aUserInformation.Details.Id == anIssue.UserId) 
                || HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Delete_Any_Issue);
        }
    }
}
