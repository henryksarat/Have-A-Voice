using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;

namespace HaveAVoice.Helpers {
    public static class LinkHelper {
        public static string Profile(User aUser) {
            string myUrl = "/Profile/Show/" + aUser.Id;

            if (!string.IsNullOrEmpty(aUser.ShortUrl)) {
                myUrl = "/" + aUser.ShortUrl;
            }

            return myUrl;
        }

        public static string AddFriend(User aUser, string aControllerSource, string anActionSource) {
            return "/Friend/AddViaGet/" + aUser.Id + "?controllerRedirect=" + aControllerSource + "&actionRedirect=" + anActionSource;
        }

        public static string CreateIssue() {
            return "/Issue/Create";
        }

        public static string IgnoreFriendSuggestion(int aUserIdToIgnore, string aController, string anAction) {
            return "/UserProfileQuestions/IgnoreUser?userToIgnore=" + aUserIdToIgnore + "&controllerRedirect=" + aController + "&actionRedirect=" + anAction;
        }

        public static string IssueUrl(string aTitle) {
            return "/Issue/Details/" + aTitle.Replace(' ', '-');
        }

        public static string GroupUrl(Group aGroup) {
            return "/Group/Details/" + aGroup.Id;
        }

        public static string GroupUrl(string anId) {
            return "/Group/Details/" + anId;
        }

        public static string EditIssue(Issue anIssue) {
            return "/Issue/Edit/" + anIssue.Id;
        }

        public static string DeleteIssue(Issue anIssue) {
            return "/Issue/Delete/" + anIssue.Id;
        }

        public static string IssueReplyUrl(int anIssueReplyId) {
            return "/IssueReply/Details/" + anIssueReplyId;
        }

        public static string ReportIssue(Issue anIssue) {
            return "/Complaint/Complaint?sourceId=" + anIssue.Id + "&complaintType=Issue";
        }

        public static string PetitionUrl(Petition aPetition) {
            return "/Petition/Details/" + aPetition.Id;
        }

        public static string AgreeIssueReply(int anIssueReplyId, int anIssueId, SiteSection aSection, int aSourceId) {
            return "/IssueReply/Disposition/" + anIssueReplyId + "?issueId=" + anIssueId + "&disposition=" + Disposition.Like + "&section=" + aSection + "&sourceId=" + aSourceId;
        }

        public static string DisagreeIssueReply(int anIssueReplyId, int anIssueId, SiteSection aSection, int aSourceId) {
            return "/IssueReply/Disposition/" + anIssueReplyId + "?issueId=" + anIssueId + "&disposition=" + Disposition.Dislike + "&section=" + aSection + "&sourceId=" + aSourceId;
        }

        public static string EditIssueReply(int anIssueReplyId) {
            return "/IssueReply/Edit/" + anIssueReplyId;
        }

        public static string AgreeIssue(int anIssueId, SiteSection aSection, int aSourceId) {
            return "/Issue/Disposition?issueId=" + anIssueId + "&disposition=" + Disposition.Like + "&section=" + aSection + "&sourceId=" + aSourceId;
        }

        public static string DisagreeIssue(int anIssueId, SiteSection aSection, int aSourceId) {
            return "/Issue/Disposition?issueId=" + anIssueId + "&disposition=" + Disposition.Dislike + "&section=" + aSection + "&sourceId=" + aSourceId;
        }

        public static string Report(int aSourceId, ComplaintType aComplaintType) {
            return String.Format("/Complaint/Create?sourceId={0}&complaintType={1}", aSourceId, aComplaintType.ToString());
        }

        public static string SendMessage(User aUser) {
            return SendMessage(aUser, string.Empty);
        }

        public static string SendMessage(User aUser, string aSubject) {
            return "/Message/Create/" + aUser.Id + "?subject=" + aSubject;
        }

        public static string UserProfileQuestions() {
            return "/UserProfileQuestions/Edit";
        }
    }
}