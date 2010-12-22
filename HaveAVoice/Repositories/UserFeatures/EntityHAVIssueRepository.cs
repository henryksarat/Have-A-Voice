using System;
using System.Linq;
using System.Collections.Generic;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using System.Data.Objects;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVIssueRepository : HAVBaseRepository, IHAVIssueRepository {
         public IEnumerable<Issue> GetLatestIssues() {
            return GetEntities().Issues.ToList<Issue>();
        }

        public Issue CreateIssue(Issue anIssueToCreate, User aUserCreating) {
            IHAVUserRepository myUserRepository = new EntityHAVUserRepository();
            anIssueToCreate = Issue.CreateIssue(0, anIssueToCreate.Title, anIssueToCreate.Description, DateTime.UtcNow, false);
            anIssueToCreate.User = GetUser(aUserCreating.Id);

             GetEntities().AddToIssues(anIssueToCreate);
             GetEntities().SaveChanges();

             return anIssueToCreate;
         }

        public Issue GetIssue(int anIssueId) {
            return (from i in GetEntities().Issues
                    where i.Id == anIssueId
                    select i).FirstOrDefault();
        }

        public IssueReply CreateIssueReply(Issue anIssue, User aUserCreating, string aReply, bool anAnonymous, Disposition aDisposition) {
            IHAVUserRepository userRepository = new EntityHAVUserRepository();
            IssueReply issueReply = new IssueReply();
            User user = GetUser(aUserCreating.Id);
            Issue issue = GetIssue(anIssue.Id);

            issueReply.Issue = issue;
            issueReply.User = user;
            issueReply.DateTimeStamp = DateTime.UtcNow;
            issueReply.Reply = aReply;
            issueReply.Anonymous = anAnonymous;
            issueReply.Disposition = (int)aDisposition;

            GetEntities().AddToIssueReplys(issueReply);
            GetEntities().SaveChanges();

            return issueReply;
        }

        public IEnumerable<IssueReplyModel> GetReplysToIssue(User aUser, Issue anIssue, IEnumerable<string> aSelectedRoles) {
            IEnumerable<IssueReplyDisposition> myIssueReplyDispositions = GetEntities().IssueReplyDispositions;

            return (from ir in GetEntities().IssueReplys.Include("IssueReplyComments")
                    join u in GetEntities().Users on ir.User.Id equals u.Id
                    join ur in GetEntities().UserRoles on u.Id equals ur.User.Id
                    join r in GetEntities().Roles on ur.Role.Id equals r.Id
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
            return (from ir in GetEntities().IssueReplys.Include("Issue")
                    where ir.Id == anIssueReplyId
                    && ir.Deleted == false
                    select ir).FirstOrDefault();
        }

        public IssueReplyComment GetIssueReplyComment(int anIssueReplyCommentId) {
            return (from irc in GetEntities().IssueReplyComments
                    where irc.Id == anIssueReplyCommentId
                    && irc.Deleted == false
                    select irc).FirstOrDefault();
        }

        public IEnumerable<IssueReplyComment> GetIssueReplyComments(int anIssueReplyId) {
            return (from irc in GetEntities().IssueReplyComments
                      where irc.IssueReply.Id == anIssueReplyId
                      && irc.Deleted == false
                      orderby irc.DateTimeStamp descending
                      select irc).ToList<IssueReplyComment>();
        }

        public IssueReplyComment CreateCommentToIssueReply(IssueReply anIssueReply, User aUserCreating, string aComment) {
            IHAVUserRepository userRepository = new EntityHAVUserRepository();
            IssueReplyComment issueReplyComment = new IssueReplyComment();

            issueReplyComment.User = GetUser(aUserCreating.Id);
            issueReplyComment.IssueReply = GetIssueReply(anIssueReply.Id);
            issueReplyComment.Comment = aComment;
            issueReplyComment.DateTimeStamp = DateTime.UtcNow;

            GetEntities().AddToIssueReplyComments(issueReplyComment);
            GetEntities().SaveChanges();
            return issueReplyComment;
        }

        public void CreateIssueDisposition(User aUser, int anIssueId, int aDisposition) {
            IHAVUserRepository myUserRepository = new EntityHAVUserRepository();
            IssueDisposition myIssueDisposition = IssueDisposition.CreateIssueDisposition(0, aDisposition);
            myIssueDisposition.Issue = GetIssue(anIssueId);
            myIssueDisposition.User = GetUser(aUser.Id);

            GetEntities().AddToIssueDispositions(myIssueDisposition);
            GetEntities().SaveChanges();
        }

        public IEnumerable<IssueWithDispositionModel> GetIssues(User aUser) {
            ObjectQuery<Issue> myIssues = GetEntities().Issues;
            ObjectQuery<IssueDisposition> myIssueDispositions = GetEntities().IssueDispositions;

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
            IHAVUserRepository myUserRepository = new EntityHAVUserRepository();
            IssueReplyDisposition myIssueReplyDisposition = IssueReplyDisposition.CreateIssueReplyDisposition(0, aDisposition);
            myIssueReplyDisposition.IssueReply = GetIssueReply(anIssueReplyId);
            myIssueReplyDisposition.User = GetUser(aUser.Id);

            GetEntities().AddToIssueReplyDispositions(myIssueReplyDisposition);
            GetEntities().SaveChanges();
        }


        public void DeleteIssue(User aDeletingUser, Issue anIssue, bool anAdminDelete) {
            anIssue.Deleted = true;
            anIssue.DeletedByUserId = aDeletingUser.Id;
            GetEntities().ApplyCurrentValues(anIssue.EntityKey.EntitySetName, anIssue);
            GetEntities().SaveChanges();
        }

        public void DeleteIssueReply(User aDeletingUser, IssueReply anIssueReply, bool anAdminDelete) {
            anIssueReply.Deleted = true;
            anIssueReply.DeletedByUserId = aDeletingUser.Id;
            GetEntities().ApplyCurrentValues(anIssueReply.EntityKey.EntitySetName, anIssueReply);
            GetEntities().SaveChanges();
        }

        public void DeleteIssueReplyComment(User aDeletingUser, IssueReplyComment anIssueReplyComment, bool anAdminDelete) {
            anIssueReplyComment.Deleted = true;
            anIssueReplyComment.DeletedByUserId = aDeletingUser.Id;
            GetEntities().ApplyCurrentValues(anIssueReplyComment.EntityKey.EntitySetName, anIssueReplyComment);
            GetEntities().SaveChanges();
        }

        public void UpdateIssue(User aUser, Issue anOriginal, Issue aNew, bool anOverride) {
            string myOldTitle = anOriginal.Title;
            string myOldDescription = anOriginal.Description;
            AuditIssue myAUdit = AuditIssue.CreateAuditIssue(0, anOriginal.Id, myOldTitle, myOldDescription, DateTime.UtcNow, aUser.Id);
            anOriginal.Title = aNew.Title;
            anOriginal.Description = aNew.Description;
            anOriginal.UpdatedDateTimeStamp = DateTime.UtcNow;
            anOriginal.UpdatedByUserId = aUser.Id;
            GetEntities().AddToAuditIssues(myAUdit);
            GetEntities().ApplyCurrentValues(anOriginal.EntityKey.EntitySetName, anOriginal);
            GetEntities().SaveChanges();
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
            GetEntities().AddToAuditIssueReplys(myAudit);
            GetEntities().ApplyCurrentValues(anOriginal.EntityKey.EntitySetName, anOriginal);
            GetEntities().SaveChanges();
        }

        public void UpdateIssueReplyComment(User aUser, IssueReplyComment anOriginal, IssueReplyComment aNew, bool anOverride) {
            string myOldComment = anOriginal.Comment;
            AuditIssueReplyComment myAUdit = AuditIssueReplyComment.CreateAuditIssueReplyComment(0, anOriginal.Id, myOldComment, DateTime.UtcNow, aUser.Id);
            anOriginal.Comment = aNew.Comment;
            anOriginal.UpdatedDateTimeStamp = DateTime.UtcNow;
            anOriginal.UpdatedByUserId = aUser.Id;
            GetEntities().AddToAuditIssueReplyComments(myAUdit);
            GetEntities().ApplyCurrentValues(anOriginal.EntityKey.EntitySetName, anOriginal);
            GetEntities().SaveChanges();
        }

        private User GetUser(int anId) {
            IHAVUserRetrievalRepository myUserRetrieval = new EntityHAVUserRetrievalRepository();
            return myUserRetrieval.GetUser(anId);
        }
    }
}