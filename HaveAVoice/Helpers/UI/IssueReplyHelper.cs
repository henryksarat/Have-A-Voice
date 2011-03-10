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

            var mySpeakSpan = new TagBuilder("span");

            if (anIssueReply.IssueStance == (int)IssueStanceFilter.Agree) {
                mySpeakSpan.AddCssClass("speak-lft");
            } else {
                mySpeakSpan.AddCssClass("speak-rgt");
            }
            mySpeakSpan.InnerHtml = "&nbsp;";

            myReplyCommentDiv.InnerHtml += mySpeakSpan.ToString();

            var myReplyCommentPad = new TagBuilder("div");
            myReplyCommentPad.AddCssClass("p-a10");

            var myName = new TagBuilder("a");
            myName.AddCssClass("name");
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
            myReplyInfoPadding.InnerHtml += SharedStyleHelper.InfoSpeakSpan();

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

            int myTotalAgrees = GetTotalAgrees(anIssueReply);
            int myTotalDisagrees = GetTotalDisagrees(anIssueReply);
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
            return StanceDiv(anIssueReplyId, anIssueId, aHasDisposition, aIsLoggedIn,
               LinkHelper.DisagreeIssueReply(anIssueReplyId, anIssueId, SiteSection.Issue, anIssueId),
               "like", aTotalAgrees, "Agrees", "Agree", LinkHelper.AgreeIssueReply(anIssueReplyId, anIssueId, aSource, aSourceId));
        }

        private static TagBuilder DisagreeDiv(int anIssueReplyId, int anIssueId, int aTotalDisagrees, bool aHasDisposition, bool aIsLoggedIn, SiteSection aSource, int aSourceId) {
            return StanceDiv(anIssueReplyId, anIssueId, aHasDisposition, aIsLoggedIn,
                LinkHelper.DisagreeIssueReply(anIssueReplyId, anIssueId, SiteSection.Issue, anIssueId),
                "dislike", aTotalDisagrees, "Disagrees", "Disagree", LinkHelper.DisagreeIssueReply(anIssueReplyId, anIssueId, aSource, aSourceId));
        }

        private static TagBuilder StanceDiv(int anIssueReplyId, int anIssueId, bool aHasDisposition, bool aIsLoggedIn,
                                            string aStanceUrl, string aLinkCssClass, int aTotalForStance, string aSingularDisplayText, string aPluralDisplayText,
                                            string aStanceLink) {
            var myStanceDiv = new TagBuilder("div");
            myStanceDiv.AddCssClass("col-3 center");

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