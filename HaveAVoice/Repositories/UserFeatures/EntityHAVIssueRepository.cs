using System;
using System.Linq;
using System.Collections.Generic;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using System.Data.Objects;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVIssueRepository : IHAVIssueRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

         public IEnumerable<Issue> GetLatestIssues() {
            return theEntities.Issues.ToList<Issue>();
        }

        public Issue CreateIssue(Issue anIssueToCreate, User aUserCreating) {
            anIssueToCreate = Issue.CreateIssue(0, anIssueToCreate.Title, anIssueToCreate.Description, aUserCreating.City, aUserCreating.State, DateTime.UtcNow, aUserCreating.Id, false);
            anIssueToCreate.Zip = aUserCreating.Zip;

            theEntities.AddToIssues(anIssueToCreate);
            theEntities.SaveChanges();

             return anIssueToCreate;
         }

        public Issue GetIssue(int anIssueId) {
            return (from i in theEntities.Issues
                    where i.Id == anIssueId
                    select i).FirstOrDefault();
        }

        public IssueReply CreateIssueReply(Issue anIssue, User aUserCreating, string aReply, bool anAnonymous, Disposition aDisposition) {
            IssueReply issueReply = IssueReply.CreateIssueReply(0, anIssue.Id, aUserCreating.Id, aReply, aUserCreating.City, aUserCreating.State, (int)aDisposition, anAnonymous, DateTime.UtcNow, false);
            issueReply.Zip = aUserCreating.Zip;

            theEntities.AddToIssueReplys(issueReply);
            theEntities.SaveChanges();

            return issueReply;
        }

        public IssueReply CreateIssueReply(User aUserCreating, int anIssueId, string aReply, bool anAnonymous, int aDisposition) {
            IssueReply issueReply = IssueReply.CreateIssueReply(0, anIssueId, aUserCreating.Id, aReply, aUserCreating.City, aUserCreating.State, aDisposition, anAnonymous, DateTime.UtcNow, false);
            issueReply.Zip = aUserCreating.Zip;

            theEntities.AddToIssueReplys(issueReply);
            theEntities.SaveChanges();

            return issueReply;
        }

        public IEnumerable<IssueReplyModel> GetReplysToIssue(User aUser, Issue anIssue, IEnumerable<string> aSelectedRoles) {
            IEnumerable<IssueReplyDisposition> myIssueReplyDispositions = theEntities.IssueReplyDispositions;

            return (from ir in theEntities.IssueReplys.Include("IssueReplyComments")
                    join u in theEntities.Users on ir.User.Id equals u.Id
                    join ur in theEntities.UserRoles on u.Id equals ur.User.Id
                    join r in theEntities.Roles on ur.Role.Id equals r.Id
                    let i = myIssueReplyDispositions.Where(i2 => i2.IssueReply.Id == ir.Id).Where(i2 => i2.User.Id == aUser.Id).FirstOrDefault()
                    where ir.Issue.Id == anIssue.Id
                    && aSelectedRoles.Contains(r.Name)
                    && ir.Deleted == false
                    select new IssueReplyModel { 
                        Id = ir.Id,
                        Issue = ir.Issue,
                        User = u,
                        Reply = ir.Reply,
                        DateTimeStamp = ir.DateTimeStamp,
                        CommentCount = ir.IssueReplyComments.Where(cc => cc.Deleted == false).Count(),
                        Anonymous = ir.Anonymous,
                        HasDisposition = (i == null) ? false : true
                    }).ToList<IssueReplyModel>();
        }

        public IssueReply GetIssueReply(int anIssueReplyId) {
            return (from ir in theEntities.IssueReplys.Include("Issue")
                    where ir.Id == anIssueReplyId
                    && ir.Deleted == false
                    select ir).FirstOrDefault();
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

        public IssueReplyComment CreateCommentToIssueReply(IssueReply anIssueReply, User aUserCreating, string aComment) {
            IssueReplyComment myIssueReplyComment = IssueReplyComment.CreateIssueReplyComment(0, anIssueReply.Id, aComment, DateTime.UtcNow, aUserCreating.Id, false);
            theEntities.AddToIssueReplyComments(myIssueReplyComment);
            theEntities.SaveChanges();
            return myIssueReplyComment;
        }

        public void CreateCommentToIssueReply(User aUserCreating, int anIssueReplyId, string aComment) {
            IssueReplyComment myIssueReplyComment = IssueReplyComment.CreateIssueReplyComment(0, anIssueReplyId, aComment, DateTime.UtcNow, aUserCreating.Id, false);
            theEntities.AddToIssueReplyComments(myIssueReplyComment);
            theEntities.SaveChanges();
        }

        public void CreateIssueDisposition(User aUser, int anIssueId, int aDisposition) {
            IssueDisposition myIssueDisposition = IssueDisposition.CreateIssueDisposition(0, anIssueId, aUser.Id, aDisposition);
            theEntities.AddToIssueDispositions(myIssueDisposition);
            theEntities.SaveChanges();
        }

        public IEnumerable<IssueWithDispositionModel> GetIssues(User aUser) {
            ObjectQuery<Issue> myIssues = theEntities.Issues;
            ObjectQuery<IssueDisposition> myIssueDispositions = theEntities.IssueDispositions;

            return (from i in myIssues
                    let d = myIssueDispositions.Where(d2 => d2.Issue.Id == i.Id)
                    .Where(d2 => d2.User.Id == aUser.Id)
                    .FirstOrDefault()
                    where i.Deleted == false
                    select new IssueWithDispositionModel {
                        Issue = i,
                        HasDisposition = (d == null ? false : true)
                    }).ToList<IssueWithDispositionModel>();
        }

        public void CreateIssueReplyDisposition(User aUser, int anIssueReplyId, int aDisposition) {
            IssueReplyDisposition myIssueReplyDisposition = IssueReplyDisposition.CreateIssueReplyDisposition(0, anIssueReplyId, aUser.Id, aDisposition);
            theEntities.AddToIssueReplyDispositions(myIssueReplyDisposition);
            theEntities.SaveChanges();
        }


        public void DeleteIssue(User aDeletingUser, Issue anIssue, bool anAdminDelete) {
            anIssue.Deleted = true;
            anIssue.DeletedByUserId = aDeletingUser.Id;
            theEntities.ApplyCurrentValues(anIssue.EntityKey.EntitySetName, anIssue);
            theEntities.SaveChanges();
        }

        public void DeleteIssueReply(User aDeletingUser, IssueReply anIssueReply, bool anAdminDelete) {
            anIssueReply.Deleted = true;
            anIssueReply.DeletedByUserId = aDeletingUser.Id;
            theEntities.ApplyCurrentValues(anIssueReply.EntityKey.EntitySetName, anIssueReply);
            theEntities.SaveChanges();
        }

        public void DeleteIssueReplyComment(User aDeletingUser, IssueReplyComment anIssueReplyComment, bool anAdminDelete) {
            anIssueReplyComment.Deleted = true;
            anIssueReplyComment.DeletedByUserId = aDeletingUser.Id;
            theEntities.ApplyCurrentValues(anIssueReplyComment.EntityKey.EntitySetName, anIssueReplyComment);
            theEntities.SaveChanges();
        }

        public void UpdateIssue(User aUser, Issue anOriginal, Issue aNew, bool anOverride) {
            string myOldTitle = anOriginal.Title;
            string myOldDescription = anOriginal.Description;
            AuditIssue myAUdit = AuditIssue.CreateAuditIssue(0, anOriginal.Id, myOldTitle, myOldDescription, DateTime.UtcNow, aUser.Id);
            anOriginal.Title = aNew.Title;
            anOriginal.Description = aNew.Description;
            anOriginal.UpdatedDateTimeStamp = DateTime.UtcNow;
            anOriginal.UpdatedByUserId = aUser.Id;
            theEntities.AddToAuditIssues(myAUdit);
            theEntities.ApplyCurrentValues(anOriginal.EntityKey.EntitySetName, anOriginal);
            theEntities.SaveChanges();
        }

        public void UpdateIssueReply(User aUser, IssueReply anOriginal, IssueReply aNew, bool anOverride) {
            string myOldReply = anOriginal.Reply;
            int myOldDisposition = anOriginal.Disposition;
            AuditIssueReply myAudit =
                AuditIssueReply.CreateAuditIssueReply(0, anOriginal.Id, myOldReply, myOldDisposition, DateTime.UtcNow, aUser.Id);
            anOriginal.Disposition = aNew.Disposition;
            anOriginal.Reply = aNew.Reply;
            anOriginal.UpdatedDateTimeStamp = DateTime.UtcNow;
            anOriginal.UpdatedUserId = aUser.Id;
            theEntities.AddToAuditIssueReplys(myAudit);
            theEntities.ApplyCurrentValues(anOriginal.EntityKey.EntitySetName, anOriginal);
            theEntities.SaveChanges();
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