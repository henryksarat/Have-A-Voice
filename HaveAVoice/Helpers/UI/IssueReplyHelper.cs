using System;
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
        public static string UserIssueReply(IssueReplyModel anIssueReply) {
            var myReplyDiv = new TagBuilder("div");

            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Agree) {
                myReplyDiv.AddCssClass("agree m-btm10");
            } else {
                myReplyDiv.AddCssClass("disagree m-btm10");
            }

            string myTimeStampDviCssClass = string.Empty;
            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Disagree) {
                myTimeStampDviCssClass = "col-3 date-tile pull-9 right";
            } else {
                myTimeStampDviCssClass = "col-3 date-tile push-6";
            }

            myReplyDiv.InnerHtml += ProfilePictureDiv(anIssueReply);
            myReplyDiv.InnerHtml += ReplyInfoDiv(anIssueReply);
            myReplyDiv.InnerHtml += SharedContentStyleHelper.TimeStampDiv(anIssueReply.DateTimeStamp, myTimeStampDviCssClass, "p-a10", string.Empty, "MMM", "dd");
            myReplyDiv.InnerHtml += SharedStyleHelper.ClearDiv();

            return myReplyDiv.ToString(TagRenderMode.Normal);
        }

        private static string ProfilePictureDiv(IssueReplyModel anIssueReply) {
            string myDivCssClass = string.Empty;
            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Disagree) {
                myDivCssClass = "col-2 center push-20";
            } else {
                myDivCssClass = "push-6 col-2 center";
            }

            return SharedContentStyleHelper.ProfilePictureDiv(anIssueReply.User, anIssueReply.Anonymous, myDivCssClass, "profile");
        }

        private static string ReplyInfoDiv(IssueReplyModel anIssueReply) {
            
            var myReplyCommentDiv = new TagBuilder("div");
            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Disagree) {
                myReplyCommentDiv.AddCssClass("push-6 m-lft col-12 m-rgt comment");
            } else {
                myReplyCommentDiv.AddCssClass("push-6 m-lft col-12 m-rgt comment");
            }

            string mySpeakSpan = string.Empty;
            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Agree) {
                mySpeakSpan = SharedStyleHelper.InfoSpeakSpan("speak-lft");
            } else {
                mySpeakSpan = SharedStyleHelper.InfoSpeakSpan("speak-rgt");
            }

            myReplyCommentDiv.InnerHtml += mySpeakSpan.ToString();

            var myReplyCommentPad = new TagBuilder("div");
            myReplyCommentPad.AddCssClass("p-a10");

            string myName = string.Empty;
            if (anIssueReply.Anonymous) {
                myName = SharedStyleHelper.Link("name", "#", HAVConstants.ANONYMOUS);
            } else {
                myName = SharedStyleHelper.Link("name", LinkHelper.Profile(anIssueReply.User), NameHelper.FullName(anIssueReply.User));
            }

            myReplyCommentPad.InnerHtml += myName.ToString();
            myReplyCommentPad.InnerHtml += "&nbsp;";
            myReplyCommentPad.InnerHtml += anIssueReply.Reply;
            myReplyCommentPad.InnerHtml += SharedStyleHelper.Link("read-more", LinkHelper.IssueReplyUrl(anIssueReply.Id), " &raquo;&raquo;");

            myReplyCommentPad.InnerHtml += SharedStyleHelper.ClearDiv();

            UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation();

            var myEditAndStancesDiv = new TagBuilder("div");
            myEditAndStancesDiv.AddCssClass("col-11 options");

            var myEditAndStancesPad = new TagBuilder("div");
            myEditAndStancesPad.AddCssClass("p-v10");

            var myDeleteDiv = DeleteDiv(myUserInformationModel, anIssueReply.Id, anIssueReply.User.Id);
            var myEditDiv = EditDiv(myUserInformationModel, anIssueReply.Id, anIssueReply.User.Id);
            var myAgreeDiv = AgreeDiv(anIssueReply.Id, anIssueReply.Issue.Id, anIssueReply.TotalAgrees, anIssueReply.HasDisposition, HAVUserInformationFactory.IsLoggedIn(), SiteSection.Issue, anIssueReply.Issue.Id);
            var myDisagreeDiv = DisagreeDiv(anIssueReply.Id, anIssueReply.Issue.Id, anIssueReply.TotalDisagrees, anIssueReply.HasDisposition, HAVUserInformationFactory.IsLoggedIn(), SiteSection.Issue, anIssueReply.Issue.Id);

            myEditAndStancesPad.InnerHtml += myDeleteDiv.ToString();
            myEditAndStancesPad.InnerHtml += myEditDiv.ToString();
            myEditAndStancesPad.InnerHtml += myAgreeDiv.ToString();
            myEditAndStancesPad.InnerHtml += myDisagreeDiv.ToString();
            myEditAndStancesPad.InnerHtml += SharedStyleHelper.ClearDiv();

            myEditAndStancesDiv.InnerHtml += myEditAndStancesPad.ToString();

            myReplyCommentPad.InnerHtml += myEditAndStancesDiv.ToString();

            myReplyCommentDiv.InnerHtml += myReplyCommentPad.ToString();

            return myReplyCommentDiv.ToString();
        }

        public static string IssueReply(IssueReply anIssueReply) {
            var myReplyDiv = new TagBuilder("div");
            myReplyDiv.AddCssClass("m-btm10 alt");
            myReplyDiv.InnerHtml += SharedContentStyleHelper.ProfilePictureDiv(anIssueReply.User, anIssueReply.Anonymous, "push-3 col-3 center issue-profile", "profile");

            var myReplyInfoDiv = new TagBuilder("div");
            myReplyInfoDiv.AddCssClass("push-3 m-lft col-15 m-rgt comment");

            var myReplyInfoPadding = new TagBuilder("div");
            myReplyInfoPadding.AddCssClass("p-a10");
            myReplyInfoPadding.InnerHtml += SharedStyleHelper.InfoSpeakSpan("speak-lft");

            string myReplyProfilePictureURL = PhotoHelper.ProfilePicture(anIssueReply.User);
            string myReplyFullName = NameHelper.FullName(anIssueReply.User);

            var myUserLink = new TagBuilder("a");
            myUserLink.AddCssClass("name");
            myUserLink.MergeAttribute("href", LinkHelper.Profile(anIssueReply.User));
            myUserLink.InnerHtml = myReplyFullName;

            myReplyInfoPadding.InnerHtml += myUserLink.ToString();
            myReplyInfoPadding.InnerHtml += "&nbsp;";
            myReplyInfoPadding.InnerHtml += anIssueReply.Reply;
            myReplyInfoPadding.InnerHtml += SharedStyleHelper.ClearDiv();

            UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation();

            var myEditAndStancesDiv = new TagBuilder("div");
            myEditAndStancesDiv.AddCssClass("col-14 options");

            var myEditAndStancesPad = new TagBuilder("div");
            myEditAndStancesPad.AddCssClass("p-v10 p-h10");

            int myTotalAgrees = GetTotalStance(anIssueReply, Disposition.Like);
            int myTotalDisagrees = GetTotalStance(anIssueReply, Disposition.Dislike);
            bool myHasDisposition = GetHasDisposition(anIssueReply, myUserInformationModel);
            var myDeleteDiv = DeleteDiv(myUserInformationModel, anIssueReply.Id, anIssueReply.User.Id);
            var myEditDiv = EditDiv(myUserInformationModel, anIssueReply.Id, anIssueReply.User.Id);
            var myAgreeDiv = AgreeDiv(anIssueReply.Id, anIssueReply.Issue.Id, myTotalAgrees, myHasDisposition, HAVUserInformationFactory.IsLoggedIn(), SiteSection.IssueReply, anIssueReply.Id);
            var myDisagreeDiv = DisagreeDiv(anIssueReply.Id, anIssueReply.Issue.Id, myTotalDisagrees, myHasDisposition, HAVUserInformationFactory.IsLoggedIn(), SiteSection.IssueReply, anIssueReply.Id);

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
            myReplyTimeStampDiv.AddCssClass("push-3 col-3 date-tile");

            var myReplyTimeStampPad = new TagBuilder("div");
            myReplyTimeStampPad.AddCssClass("p-a10");

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

        private static int GetTotalStance(IssueReply anIssueReply, Disposition aDisposition) {
            int myTotalAgrees = (from s in anIssueReply.IssueReplyDispositions
                                 where s.Disposition == (int)aDisposition
                                 select s).Count<IssueReplyDisposition>();
            return myTotalAgrees;
        }

        private static bool GetHasDisposition(IssueReply anIssueReply, UserInformationModel aUserInformationModel) {
            bool myHasDisposition = aUserInformationModel != null &&
                                    (from s in anIssueReply.IssueReplyDispositions
                                     where s.UserId == aUserInformationModel.Details.Id
                                     select s).Count<IssueReplyDisposition>() > 0 ? true : false;
            return myHasDisposition;
        }

        private static TagBuilder EditDiv(UserInformationModel myUserInformationModel, int anIssueReplyId, int anIssueReplyAuthorUserId) {
            var myEditDiv = new TagBuilder("div");
            myEditDiv.AddCssClass("col-2 center");

            if (IssueReplyHelper.ShouldDisplayEditLink(myUserInformationModel, anIssueReplyAuthorUserId)) {
                var myEditLink = new TagBuilder("a");
                myEditLink.MergeAttribute("href", LinkHelper.EditIssueReply(anIssueReplyId));
                myEditLink.AddCssClass("edit");
                myEditLink.InnerHtml += "Edit";
                myEditDiv.InnerHtml += myEditLink.ToString();
            } else {
                myEditDiv.InnerHtml += "&nbsp;";
            }
            return myEditDiv;
        }

        private static TagBuilder DeleteDiv(UserInformationModel myUserInformationModel, int anIssueReplyId, int anIssueReplyAuthorUserId) {
            var myDeleteDiv = new TagBuilder("div");
            myDeleteDiv.AddCssClass("col-3 center");

            if (IssueReplyHelper.ShouldDisplayDeleteLink(myUserInformationModel, anIssueReplyAuthorUserId)) {
                var myDeleteLink = new TagBuilder("a");
                myDeleteLink.MergeAttribute("href", LinkHelper.EditIssueReply(anIssueReplyId));
                myDeleteLink.AddCssClass("delete");
                myDeleteLink.InnerHtml += "Delete";
                myDeleteDiv.InnerHtml += myDeleteLink.ToString();
            } else {
                myDeleteDiv.InnerHtml += "&nbsp;";
            }
            return myDeleteDiv;
        }

        private static TagBuilder AgreeDiv(int anIssueReplyId, int anIssueId, int aTotalAgrees, bool aHasDisposition, bool aIsLoggedIn, SiteSection aSource, int aSourceId) {
            return SharedContentStyleHelper.AgreeStanceDiv("col-3 center", aHasDisposition, aIsLoggedIn, aTotalAgrees, LinkHelper.AgreeIssueReply(anIssueReplyId, anIssueId, aSource, aSourceId));
        }

        private static TagBuilder DisagreeDiv(int anIssueReplyId, int anIssueId, int aTotalDisagrees, bool aHasDisposition, bool aIsLoggedIn, SiteSection aSource, int aSourceId) {
            return SharedContentStyleHelper.DisagreeStanceDiv("col-3 center", aHasDisposition, aIsLoggedIn, aTotalDisagrees, LinkHelper.DisagreeIssueReply(anIssueReplyId, anIssueId, aSource, aSourceId));
        }

        private static bool ShouldDisplayEditLink(UserInformationModel aUserInformation, int anIssueReplyAuthorUserId) {
            return (HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Edit_Issue_Reply) && aUserInformation.Details.Id == anIssueReplyAuthorUserId)
                || HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Edit_Any_Issue_Reply);
        }

        private static bool ShouldDisplayDeleteLink(UserInformationModel aUserInformation, int anIssueReplyAuthorUserId) {
            return (HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Delete_Issue_Reply) && aUserInformation.Details.Id == anIssueReplyAuthorUserId)
                || HAVPermissionHelper.AllowedToPerformAction(aUserInformation, HAVPermission.Delete_Any_Issue_Reply);
        }
    }
}