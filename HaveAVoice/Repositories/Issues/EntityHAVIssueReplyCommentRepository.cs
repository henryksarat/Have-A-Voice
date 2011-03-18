using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Repositories.Issues.Helpers;

namespace HaveAVoice.Repositories.Issues {
    public class EntityHAVIssueReplyCommentRepository : IHAVIssueReplyCommentRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void CreateCommentToIssueReply(User aUserCreating, int anIssueReplyId, string aComment) {
            IssueReplyComment myIssueReplyComment = IssueReplyComment.CreateIssueReplyComment(0, anIssueReplyId, aComment, DateTime.UtcNow, aUserCreating.Id, false);
            theEntities.AddToIssueReplyComments(myIssueReplyComment);

            IssueReplyViewedHelper.UpdateCurrentIssueReplyViewedStateAndAddIfNecessaryWithoutSave(theEntities, aUserCreating.Id, anIssueReplyId);

            theEntities.SaveChanges();
        }

        public void DeleteIssueReplyComment(User aDeletingUser, IssueReplyComment anIssueReplyComment, bool anAdminDelete) {
            anIssueReplyComment.Deleted = true;
            anIssueReplyComment.DeletedByUserId = aDeletingUser.Id;
            theEntities.ApplyCurrentValues(anIssueReplyComment.EntityKey.EntitySetName, anIssueReplyComment);
            theEntities.SaveChanges();
        }

        public IssueReplyComment GetIssueReplyComment(int anIssueReplyCommentId) {
            return (from irc in theEntities.IssueReplyComments
                    where irc.Id == anIssueReplyCommentId
                    && irc.Deleted == false
                    select irc).FirstOrDefault();
        }

        public IEnumerable<IssueReplyComment> GetIssueReplyComments(int anIssueReplyId) {
            return (from irc in theEntities.IssueReplyComments
                    where irc.IssueReply.Id == anIssueReplyId
                    && irc.Deleted == false
                    orderby irc.DateTimeStamp descending
                    select irc).ToList<IssueReplyComment>();
        }

        public void UpdateIssueReplyComment(User aUser, IssueReplyComment anOriginal, IssueReplyComment aNew, bool anOverride) {
            string myOldComment = anOriginal.Comment;
            AuditIssueReplyComment myAUdit = AuditIssueReplyComment.CreateAuditIssueReplyComment(0, anOriginal.Id, myOldComment, DateTime.UtcNow, aUser.Id);
            anOriginal.Comment = aNew.Comment;
            anOriginal.UpdatedDateTimeStamp = DateTime.UtcNow;
            anOriginal.UpdatedByUserId = aUser.Id;
            theEntities.AddToAuditIssueReplyComments(myAUdit);
            theEntities.ApplyCurrentValues(anOriginal.EntityKey.EntitySetName, anOriginal);
            theEntities.SaveChanges();
        }
    }
}