﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Helpers {
    public static class LinkHelper {
        public static string ProfilePage(User aUser) {
            return "/Profile/Show/" + aUser.Id;
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

        public static string LikeIssueReply(int anIssueReplyId, int anIssueId) {
            return "/IssueReply/Disposition" + anIssueReplyId +"?issueId=" + anIssueId + "&disposition=" + (int)Disposition.Like;
        }

        public static string DislikeIssueReply(int anIssueReplyId, int anIssueId) {
            return "/IssueReply/Disposition" + anIssueReplyId +"?issueId=" + anIssueId + "&disposition=" + (int)Disposition.Dislike;
        }

        public static string EditIssueReply(int anIssueReplyId) {
            return "/IssueReply/Edit/" + anIssueReplyId;
        }
    }
}