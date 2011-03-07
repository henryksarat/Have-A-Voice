﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using HaveAVoice.Models.View;
using HaveAVoice.Services.Helpers;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers.UserInformation;

namespace HaveAVoice.Helpers.UI {
    public class IssueReplyHelper {
        public static string IssueReplyDisplay(IEnumerable<IssueReply> anIssueReplies) {
            string myList = string.Empty;

            int myCount = 0;

            foreach (IssueReply myIssueReply in anIssueReplies) {
                if (myCount >= 4) {
                    break;
                }
                var myLI = new TagBuilder("li");
                myLI.InnerHtml = myIssueReply.Reply;
                myList += myLI.ToString();

                myCount++;
            }


            return myList;
        }

        public static bool ShouldDisplayEditLink(UserInformationModel aUserInformation, int anIssueReplyAuthorUserId) {
            return (HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Edit_Issue_Reply) && aUserInformation.Details.Id == anIssueReplyAuthorUserId) 
                || HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Edit_Any_Issue_Reply);
        }

        public static bool ShouldDisplayDeleteLink(UserInformationModel aUserInformation, int anIssueReplyAuthorUserId) {
            return (HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Delete_Issue_Reply) && aUserInformation.Details.Id == anIssueReplyAuthorUserId) 
                || HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Delete_Any_Issue_Reply);
        }

        public static string UserIssueReply(IssueReplyModel anIssueReply) {
            var myReplyDiv = new TagBuilder("div");

            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Agree) {
                myReplyDiv.MergeAttribute("class", "agree m-btm10");
            } else {
                myReplyDiv.MergeAttribute("class", "disagree m-btm10");
            }

            var myProfileDiv = new TagBuilder("div");
            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Disagree) {
                myProfileDiv.MergeAttribute("class", "col-2 center push-20");
            } else {
                myProfileDiv.MergeAttribute("class", "push-6 col-2 center");
            }

            var myProfileImage = new TagBuilder("img");
            if (anIssueReply.Anonymous) {
                myProfileImage.MergeAttribute("alt", "Anonymous");
                myProfileImage.MergeAttribute("src", PhotoHelper.ConstructUrl(HAVConstants.NO_PROFILE_PICTURE_IMAGE));
            } else {
                myProfileImage.MergeAttribute("alt", NameHelper.FullName(anIssueReply.User));
                myProfileImage.MergeAttribute("src", PhotoHelper.ProfilePicture(anIssueReply.User));
            }
            myProfileImage.MergeAttribute("class", "profile");

            myProfileDiv.InnerHtml += myProfileImage.ToString();
            myReplyDiv.InnerHtml += myProfileDiv.ToString();

            var myReplyCommentDiv = new TagBuilder("div");
            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Disagree) {
                myReplyCommentDiv.MergeAttribute("class", "push-6 m-lft col-12 m-rgt comment");
            } else {
                myReplyCommentDiv.MergeAttribute("class", "push-6 m-lft col-12 m-rgt comment");
            }

            var mySpeakSpan = new TagBuilder("span");

            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Agree) {
                mySpeakSpan.MergeAttribute("class", "speak-lft");
            } else {
                mySpeakSpan.MergeAttribute("class", "speak-rgt");
            }
            mySpeakSpan.InnerHtml = "&nbsp;";

            myReplyCommentDiv.InnerHtml += mySpeakSpan.ToString();

            var myReplyCommentPad = new TagBuilder("div");
            myReplyCommentPad.MergeAttribute("class", "p-a10");

            var myName = new TagBuilder("a");
            myName.MergeAttribute("class", "name");
            if (anIssueReply.Anonymous) {
                myName.InnerHtml = "Anonymous";
                myName.MergeAttribute("href", "#");
            } else {
                myName.InnerHtml = NameHelper.FullName(anIssueReply.User);
                myName.MergeAttribute("href", LinkHelper.Profile(anIssueReply.User));
            }

            myReplyCommentPad.InnerHtml += myName.ToString();
            myReplyCommentPad.InnerHtml += "&nbsp;";
            myReplyCommentPad.InnerHtml += anIssueReply.Reply;

            myReplyCommentPad.InnerHtml += SharedStyleHelper.ClearDiv();

            UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation();

            var myEditAndStancesDiv = new TagBuilder("div");
            myEditAndStancesDiv.MergeAttribute("class", "col-11 options");

            var myEditAndStancesPad = new TagBuilder("div");
            myEditAndStancesPad.MergeAttribute("class", "p-v10");

            var myDeleteDiv = DeleteDiv(myUserInformationModel, anIssueReply.Id, anIssueReply.User.Id);
            var myEditDiv = EditDiv(myUserInformationModel, anIssueReply.Id, anIssueReply.User.Id);
            var myAgreeDiv = AgreeDiv(anIssueReply.Id, anIssueReply.Issue.Id, anIssueReply.TotalAgrees, anIssueReply.HasDisposition, HAVUserInformationFactory.IsLoggedIn());
            var myDisagreeDiv = DisagreeDiv(anIssueReply.Id, anIssueReply.Issue.Id, anIssueReply.TotalDisagrees, anIssueReply.HasDisposition, HAVUserInformationFactory.IsLoggedIn());

            myEditAndStancesPad.InnerHtml += myDeleteDiv.ToString();
            myEditAndStancesPad.InnerHtml += myEditDiv.ToString();
            myEditAndStancesPad.InnerHtml += myAgreeDiv.ToString();
            myEditAndStancesPad.InnerHtml += myDisagreeDiv.ToString();
            myEditAndStancesPad.InnerHtml += SharedStyleHelper.ClearDiv();

            myEditAndStancesDiv.InnerHtml += myEditAndStancesPad.ToString();

            myReplyCommentPad.InnerHtml += myEditAndStancesDiv.ToString();

            myReplyCommentDiv.InnerHtml += myReplyCommentPad.ToString();

            var divTimeStamp = new TagBuilder("div");

            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Disagree) {
                divTimeStamp.MergeAttribute("class", "col-3 date-tile pull-9 right");
            } else {
                divTimeStamp.MergeAttribute("class", "col-3 date-tile push-6");
            }

            myReplyDiv.InnerHtml += myReplyCommentDiv.ToString();

            var divTimePad = new TagBuilder("div");
            divTimePad.MergeAttribute("class", "p-a10");

            var spanTime = new TagBuilder("span");
            spanTime.InnerHtml = anIssueReply.DateTimeStamp.ToString("MMM").ToUpper();

            divTimePad.InnerHtml += spanTime.ToString();
            divTimePad.InnerHtml += "&nbsp;";
            divTimePad.InnerHtml += anIssueReply.DateTimeStamp.ToString("dd");

            divTimeStamp.InnerHtml += divTimePad.ToString();

            myReplyDiv.InnerHtml += divTimeStamp.ToString();
            myReplyDiv.InnerHtml += SharedStyleHelper.ClearDiv();

            return myReplyDiv.ToString(TagRenderMode.Normal);
        }

        public static string IssueReply(IssueReply anIssueReply) {
            string myIssueDiv = BuildIssueForIssueReplyDisplay(anIssueReply.Issue);
            string myIssueReplyDiv = BuildReplyForIssueReplyDisplay(anIssueReply);

            return myIssueDiv + SharedStyleHelper.ClearDiv() + myIssueReplyDiv;
        }

        private static string BuildIssueForIssueReplyDisplay(Issue anIssue) {
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

            myIssueInfoPadding.InnerHtml += myHeadTitle.ToString();
            myIssueInfoPadding.InnerHtml += anIssue.Description;

            myIssueInfoPadding.InnerHtml += SharedStyleHelper.ClearDiv();
            myIssueInfoDiv.InnerHtml += myIssueInfoPadding.ToString();

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

        private static string BuildReplyForIssueReplyDisplay(IssueReply anIssueReply) {
            var myReplyDiv = new TagBuilder("div");
            myReplyDiv.MergeAttribute("class", "m-btm10 alt");

            var myReplyProfileDiv = new TagBuilder("div");
            myReplyProfileDiv.MergeAttribute("class", "push-3 col-3 center issue-profile");

            string myReplyProfilePictureURL = PhotoHelper.ProfilePicture(anIssueReply.User);
            string myReplyFullName = NameHelper.FullName(anIssueReply.User);

            var myProfileImage = new TagBuilder("img");
            myProfileImage.MergeAttribute("alt", myReplyFullName);
            myProfileImage.MergeAttribute("src", myReplyProfilePictureURL);
            myProfileImage.MergeAttribute("class", "profile");

            myReplyProfileDiv.InnerHtml += myProfileImage.ToString();
            myReplyDiv.InnerHtml += myReplyProfileDiv.ToString();

            var myReplyInfoDiv = new TagBuilder("div");
            myReplyInfoDiv.MergeAttribute("class", "push-3 m-lft col-15 m-rgt comment");

            var myReplyInfoPadding = new TagBuilder("div");
            myReplyInfoPadding.MergeAttribute("class", "p-a10");
            myReplyInfoPadding.InnerHtml += SharedStyleHelper.InfoSpeakSpan();

            var myUserLink = new TagBuilder("a");
            myUserLink.MergeAttribute("class", "name");
            myUserLink.MergeAttribute("href", LinkHelper.Profile(anIssueReply.User));
            myUserLink.InnerHtml = myReplyFullName;

            myReplyInfoPadding.InnerHtml += myUserLink.ToString();
            myReplyInfoPadding.InnerHtml += "&nbsp;";
            myReplyInfoPadding.InnerHtml += anIssueReply.Reply;
            myReplyInfoPadding.InnerHtml += SharedStyleHelper.ClearDiv();

            UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation();

            var myEditAndStancesDiv = new TagBuilder("div");
            myEditAndStancesDiv.MergeAttribute("class", "col-14 options");

            var myEditAndStancesPad = new TagBuilder("div");
            myEditAndStancesPad.MergeAttribute("class", "p-v10 p-h10");

            int myTotalAgrees = GetTotalAgrees(anIssueReply);
            int myTotalDisagrees = GetTotalDisagrees(anIssueReply);
            bool myHasDisposition = GetHasDisposition(anIssueReply, myUserInformationModel);
            var myDeleteDiv = DeleteDiv(myUserInformationModel, anIssueReply.Id, anIssueReply.User.Id);
            var myEditDiv = EditDiv(myUserInformationModel, anIssueReply.Id, anIssueReply.User.Id);
            var myAgreeDiv = AgreeDiv(anIssueReply.Id, anIssueReply.Issue.Id, myTotalAgrees, myHasDisposition, HAVUserInformationFactory.IsLoggedIn());
            var myDisagreeDiv = DisagreeDiv(anIssueReply.Id, anIssueReply.Issue.Id, myTotalDisagrees, myHasDisposition, HAVUserInformationFactory.IsLoggedIn());

            myEditAndStancesPad.InnerHtml += SharedStyleHelper.StyledHtmlDiv("col-2 center", ComplaintHelper.IssueReplyLinkStyled(anIssueReply.Id)).ToString();
            myEditAndStancesPad.InnerHtml += myDeleteDiv.ToString();
            myEditAndStancesPad.InnerHtml += myEditDiv.ToString();
            myEditAndStancesPad.InnerHtml += myAgreeDiv.ToString();
            myEditAndStancesPad.InnerHtml += myDisagreeDiv.ToString();
            myEditAndStancesPad.InnerHtml += SharedStyleHelper.ClearDiv();

            myEditAndStancesDiv.InnerHtml += myEditAndStancesPad.ToString();

            myReplyInfoPadding.InnerHtml += myEditAndStancesDiv.ToString();

            myReplyInfoDiv.InnerHtml += myReplyInfoPadding.ToString();

            myReplyDiv.InnerHtml += myReplyInfoDiv.ToString();

            var myReplyTimeStampDiv = new TagBuilder("div");
            myReplyTimeStampDiv.MergeAttribute("class", "push-3 col-3 date-tile");

            var myReplyTimeStampPad = new TagBuilder("div");
            myReplyTimeStampPad.MergeAttribute("class", "p-a10");

            var rTime = new TagBuilder("span");
            rTime.InnerHtml = anIssueReply.DateTimeStamp.ToString("MMM").ToUpper();

            myReplyTimeStampPad.InnerHtml += rTime.ToString();
            myReplyTimeStampPad.InnerHtml += "&nbsp;";
            myReplyTimeStampPad.InnerHtml += anIssueReply.DateTimeStamp.ToString("dd");

            myReplyTimeStampDiv.InnerHtml += myReplyTimeStampPad.ToString();

            myReplyDiv.InnerHtml += myReplyTimeStampDiv.ToString();
            myReplyDiv.InnerHtml += SharedStyleHelper.ClearDiv();

            return myReplyDiv.ToString();
        }

        private static int GetTotalAgrees(IssueReply anIssueReply) {
            int myTotalAgrees = (from s in anIssueReply.IssueReplyDispositions
                                 where s.Disposition == (int)Disposition.Like
                                 select s).Count<IssueReplyDisposition>();
            return myTotalAgrees;
        }

        private static int GetTotalDisagrees(IssueReply anIssueReply) {
            int myTotalDisagrees = (from s in anIssueReply.IssueReplyDispositions
                                    where s.Disposition == (int)Disposition.Dislike
                                    select s).Count<IssueReplyDisposition>();
            return myTotalDisagrees;
        }

        private static bool GetHasDisposition(IssueReply anIssueReply, UserInformationModel myUserInformationModel) {
            bool myHasDisposition = myUserInformationModel.Details != null &&
                                    (from s in anIssueReply.IssueReplyDispositions
                                     where s.UserId == myUserInformationModel.Details.Id
                                     select s).Count<IssueReplyDisposition>() > 0 ? true : false;
            return myHasDisposition;
        }

        private static TagBuilder EditDiv(UserInformationModel myUserInformationModel, int anIssueReplyId, int anIssueReplyAuthorUserId) {
            var myEditDiv = new TagBuilder("div");
            myEditDiv.MergeAttribute("class", "col-2 center");

            if (IssueReplyHelper.ShouldDisplayEditLink(myUserInformationModel, anIssueReplyAuthorUserId)) {
                var myEditLink = new TagBuilder("a");
                myEditLink.MergeAttribute("href", LinkHelper.EditIssueReply(anIssueReplyId));
                myEditLink.MergeAttribute("class", "edit");
                myEditLink.InnerHtml += "Edit";
                myEditDiv.InnerHtml += myEditLink.ToString();
            } else {
                myEditDiv.InnerHtml += "&nbsp;";
            }
            return myEditDiv;
        }

        private static TagBuilder DeleteDiv(UserInformationModel myUserInformationModel, int anIssueReplyId, int anIssueReplyAuthorUserId) {
            var myDeleteDiv = new TagBuilder("div");
            myDeleteDiv.MergeAttribute("class", "col-3 center");

            if (IssueReplyHelper.ShouldDisplayDeleteLink(myUserInformationModel, anIssueReplyAuthorUserId)) {
                var myDeleteLink = new TagBuilder("a");
                myDeleteLink.MergeAttribute("href", LinkHelper.EditIssueReply(anIssueReplyId));
                myDeleteLink.MergeAttribute("class", "delete");
                myDeleteLink.InnerHtml += "Delete";
                myDeleteDiv.InnerHtml += myDeleteLink.ToString();
            } else {
                myDeleteDiv.InnerHtml += "&nbsp;";
            }
            return myDeleteDiv;
        }

        private static TagBuilder AgreeDiv(int anIssueReplyId, int anIssueId, int aTotalAgrees, bool aHasDisposition, bool aIsLoggedIn) {
            return StanceDiv(anIssueReplyId, anIssueId, aHasDisposition, aIsLoggedIn,
               LinkHelper.DisagreeIssueReply(anIssueReplyId, anIssueId, SiteSection.Issue, anIssueId),
               "like", aTotalAgrees, "Agrees", "Agree");
        }

        private static TagBuilder DisagreeDiv(int anIssueReplyId, int anIssueId, int aTotalDisagrees, bool aHasDisposition, bool aIsLoggedIn) {
            return StanceDiv(anIssueReplyId, anIssueId, aHasDisposition, aIsLoggedIn,
                LinkHelper.DisagreeIssueReply(anIssueReplyId, anIssueId, SiteSection.Issue, anIssueId),
                "dislike", aTotalDisagrees, "Disagrees", "Disagree");
        }

        private static TagBuilder StanceDiv(int anIssueReplyId, int anIssueId, bool aHasDisposition, bool aIsLoggedIn,
                                            string aStanceUrl, string aLinkCssClass, int aTotalForStance, string aSingularDisplayText, string aPluralDisplayText) {
            var myStanceDiv = new TagBuilder("div");
            myStanceDiv.MergeAttribute("class", "col-3 center");

            if (!aHasDisposition && aIsLoggedIn) {
                var myDisagreeLink = new TagBuilder("a");
                myDisagreeLink.MergeAttribute("href", LinkHelper.DisagreeIssueReply(anIssueReplyId, anIssueId, SiteSection.Issue, anIssueId));
                myDisagreeLink.MergeAttribute("class", aLinkCssClass);
                myDisagreeLink.InnerHtml += aPluralDisplayText + " (" + aTotalForStance + ")";
                myStanceDiv.InnerHtml += myDisagreeLink.ToString();
            } else {
                var myDisagreeSpan = new TagBuilder("span");
                myDisagreeSpan.MergeAttribute("class", aLinkCssClass);
                string mySingleOrPlural = aTotalForStance == 1 ? " Person " + aSingularDisplayText : " People " + aPluralDisplayText;
                myDisagreeSpan.InnerHtml = aTotalForStance.ToString() + mySingleOrPlural;
                myStanceDiv.InnerHtml += myDisagreeSpan.ToString();
            }

            return myStanceDiv;
        }
    }
}