using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Repositories.Issues {
    public interface IHAVIssueReplyRepository {
        IssueReply CreateIssueReply(User aUserCreating, int anIssueId, string aReply, bool anAnonymous, int aDisposition);
        void CreateIssueReplyStance(User aUser, int anIssueReplyId, int aDisposition);
        void DeleteIssueReply(User aDeletingUser, IssueReply anIssueReply, bool anAdminDelete);
        IssueReply GetIssueReply(int anIssueReplyId);
        IEnumerable<IssueReplyModel> GetReplysToIssue(Issue anIssue, IEnumerable<string> aSelectedRoles, PersonFilter aFilter);
        IEnumerable<IssueReplyModel> GetReplysToIssue(User aUser, Issue anIssue, IEnumerable<string> aSelectedRoles, PersonFilter aFilter);
        bool HasIssueReplyStance(User aUser, int anIssueReplyId);
        void MarkIssueReplyAsViewed(int aUserId, int anIssueReplyId);
        void UpdateIssueReply(User aUser, IssueReply anOriginal, IssueReply aNew, bool anOverride);
    }
}