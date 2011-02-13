﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;

namespace HaveAVoice.Helpers {
    public static class LinkHelper {
        public static string Profile(User aUser) {
            return "/" + aUser.ShortUrl;
        }

        public static string IssueUrl(string aTitle) {
            return "/Issue/Details/" + aTitle.Replace(' ', '-');
        }

        public static string EditIssue(Issue anIssue) {
            return "/Issue/Edit/" + anIssue;
        }

        public static string DeleteIssue(Issue anIssue) {
            return "/Issue/Delete/" + anIssue;
        }

        public static string ReportIssue(Issue anIssue) {
            return "/Complaint/Complaint?sourceId=" + anIssue.Id + "&complaintType=Issue";
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
    }
}