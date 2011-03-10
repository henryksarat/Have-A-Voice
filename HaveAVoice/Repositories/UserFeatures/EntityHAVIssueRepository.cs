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

        public void AddHitToIssue(int anIssueId) {
            IssueHit myIssueHit = IssueHit.CreateIssueHit(0, anIssueId, 1, DateTime.UtcNow);
            theEntities.AddToIssueHits(myIssueHit);
            theEntities.SaveChanges();
        }

        public Issue GetIssue(int anIssueId) {
            return (from i in theEntities.Issues
                    where i.Id == anIssueId
                    select i).FirstOrDefault();
        }

        public Issue GetIssueByTitle(string aTitle) {
            return (from i in theEntities.Issues
                    where i.Title == aTitle
                    && i.Deleted == false
                    select i).FirstOrDefault();
        }

        public void MarkIssueAsReadForAuthor(Issue anIssue) {
            IssueViewedState myIssueViewedState = GetIssueViewedState(anIssue.Id, anIssue.UserId);
            myIssueViewedState.Viewed = true;

            theEntities.SaveChanges();
        }

        public bool HasIssueTitleBeenUsed(string aTitle) {
            return GetIssueByTitle(aTitle) != null ? true : false;
        }

        public IssueReply CreateIssueReply(User aUserCreating, int anIssueId, string aReply, bool anAnonymous, int aDisposition) {
            IssueReply myIssueReply = IssueReply.CreateIssueReply(0, anIssueId, aUserCreating.Id, aReply, aUserCreating.City, aUserCreating.State, aDisposition, anAnonymous, DateTime.UtcNow, false);
            myIssueReply.Zip = aUserCreating.Zip;

            theEntities.AddToIssueReplys(myIssueReply);
            theEntities.SaveChanges();

            CreateIssueReplyViewedState(aUserCreating.Id, myIssueReply.Id, true);

            return myIssueReply;
        }

        public IEnumerable<IssueReplyModel> GetReplysToIssue(User aUser, Issue anIssue, IEnumerable<string> aSelectedRoles, PersonFilter aFilter) {
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
                        IssueStance = (ir.Disposition == 1) ? (int)IssueStanceFilter.Agree : (int)IssueStanceFilter.Disagree,
                        User = u,
                        Reply = ir.Reply,
                        DateTimeStamp = ir.DateTimeStamp,
                        CommentCount = ir.IssueReplyComments.Where(cc => cc.Deleted == false).Count(),
                        Anonymous = ir.Anonymous,
                        HasDisposition = (i == null && ir.UserId != aUser.Id) ? false : true,
                        TempDispositionHolder = ir.Disposition,
                        TempPersonFilterHolder = (int)aFilter,
                        TotalAgrees = (from d in ir.IssueReplyDispositions where ir.Disposition == (int)IssueStanceFilter.Agree select d).Count<IssueReplyDisposition>(),
                        TotalDisagrees = (from d in ir.IssueReplyDispositions where ir.Disposition == (int)IssueStanceFilter.Disagree select d).Count<IssueReplyDisposition>()

                    }).ToList<IssueReplyModel>();
        }

        public IEnumerable<IssueReplyModel> GetReplysToIssue(Issue anIssue, IEnumerable<string> aSelectedRoles, PersonFilter aFilter) {
            IEnumerable<IssueReplyDisposition> myIssueReplyDispositions = theEntities.IssueReplyDispositions;

            return (from ir in theEntities.IssueReplys.Include("IssueReplyComments")
                    join u in theEntities.Users on ir.User.Id equals u.Id
                    join ur in theEntities.UserRoles on u.Id equals ur.User.Id
                    join r in theEntities.Roles on ur.Role.Id equals r.Id
                    where ir.Issue.Id == anIssue.Id
                    && aSelectedRoles.Contains(r.Name)
                    && ir.Deleted == false
                    select new IssueReplyModel {
                        Id = ir.Id,
                        Issue = ir.Issue,
                        IssueStance = (ir.Disposition == 1) ? (int)IssueStanceFilter.Agree : (int)IssueStanceFilter.Disagree,
                        User = u,
                        Reply = ir.Reply,
                        DateTimeStamp = ir.DateTimeStamp,
                        CommentCount = ir.IssueReplyComments.Where(cc => cc.Deleted == false).Count(),
                        Anonymous = ir.Anonymous,
                        HasDisposition = false,
                        TempDispositionHolder = ir.Disposition,
                        TempPersonFilterHolder = (int)aFilter,
                        TotalAgrees = (from d in ir.IssueReplyDispositions where ir.Disposition == (int)IssueStanceFilter.Agree select d).Count<IssueReplyDisposition>(),
                        TotalDisagrees = (from d in ir.IssueReplyDispositions where ir.Disposition == (int)IssueStanceFilter.Disagree select d).Count<IssueReplyDisposition>()
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

        public void CreateCommentToIssueReply(User aUserCreating, int anIssueReplyId, string aComment) {
            IssueReplyComment myIssueReplyComment = IssueReplyComment.CreateIssueReplyComment(0, anIssueReplyId, aComment, DateTime.UtcNow, aUserCreating.Id, false);
            theEntities.AddToIssueReplyComments(myIssueReplyComment);

            IssueReply myIssueReply = GetIssueReply(anIssueReplyId);

            UpdateCurrentIssueReplyViewedStateAndAddIfNecessaryWithoutSave(aUserCreating.Id, anIssueReplyId, myIssueReply.UserId);

            theEntities.SaveChanges();
        }

        public void MarkIssueReplyAsViewed(int aUserId, int anIssueReplyId) {
            IssueReplyViewedState myViewedState = GetIssueReplyViewedState(aUserId, anIssueReplyId);
            if (myViewedState != null) {
                myViewedState.Viewed = true;
                myViewedState.LastUpdated = DateTime.UtcNow;
                theEntities.ApplyCurrentValues(myViewedState.EntityKey.EntitySetName, myViewedState);
                theEntities.SaveChanges();
            }
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
                    select new IssueWithDispositionModel() {
                        Issue = i,
                        HasDisposition = (d == null ? false : true),
                        TotalAgrees = (from d2 in i.IssueDispositions
                                       where d2.Disposition == (int)Disposition.Like
                                       select d2).Count<IssueDisposition>(),
                        TotalDisagrees = (from d2 in i.IssueDispositions
                                       where d2.Disposition == (int)Disposition.Dislike
                                       select d2).Count<IssueDisposition>(), 
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

        public bool HasIssueDisposition(User aUser, int anIssueId) {
            return (from i in theEntities.IssueDispositions
                    where i.UserId == aUser.Id && i.IssueId == anIssueId
                    select i).Count<IssueDisposition>() > 0 ? true : false;
        }

        public bool HasIssueReplyDisposition(User aUser, int anIssueReplyId) {
            return (from ir in theEntities.IssueReplyDispositions
                    where ir.UserId == aUser.Id && ir.IssueReplyId == anIssueReplyId
                    select ir).Count<IssueReplyDisposition>() > 0 ? true : false;
        }


        public void MarkIssueAsUnreadForAuthor(int anIssueId) {
            Issue myIssue = GetIssue(anIssueId);
            IssueViewedState myIssueViewedState = GetIssueViewedState(myIssue.Id, myIssue.UserId);

            if (myIssueViewedState == null) {
                myIssueViewedState = IssueViewedState.CreateIssueViewedState(0, myIssue.Id, myIssue.UserId, false, DateTime.UtcNow);
                theEntities.AddToIssueViewedStates(myIssueViewedState);
            } else {
                myIssueViewedState.Viewed = false;
                myIssueViewedState.LastUpdated = DateTime.UtcNow;
                theEntities.ApplyCurrentValues(myIssueViewedState.EntityKey.EntitySetName, myIssueViewedState);
            }

            theEntities.SaveChanges();
        }

        private void UpdateCurrentIssueReplyViewedStateAndAddIfNecessaryWithoutSave(int aUserId, int anIssueReplyId, int anIssueReplyAuthorId) {
            bool myHasViewedState = false;
            bool myAuthorHasViewedState = false;

            IEnumerable<IssueReplyViewedState> myViewedStates = GetIssueReplyViewedStates(anIssueReplyId);

            foreach (IssueReplyViewedState myViewedState in myViewedStates) {
                if (myViewedState.UserId == aUserId) {
                    myHasViewedState = true;
                    myViewedState.Viewed = true;
                } else {
                    myViewedState.Viewed = false;
                }

                if (myViewedState.UserId == anIssueReplyAuthorId) {
                    myAuthorHasViewedState = true;
                }

                myViewedState.LastUpdated = DateTime.UtcNow;
                theEntities.ApplyCurrentValues(myViewedState.EntityKey.EntitySetName, myViewedState);
            }

            if (!myHasViewedState) {
                CreateIssueReplyViewedStateWithoutSave(aUserId, anIssueReplyId, true);
            }

            if (!myAuthorHasViewedState) {
                CreateIssueReplyViewedStateWithoutSave(anIssueReplyAuthorId, anIssueReplyId, false);
            }
        }

        private void CreateIssueReplyViewedState(int aUserId, int anIssueReplyId, bool aViewed) {
            CreateIssueReplyViewedStateWithoutSave(aUserId, anIssueReplyId, aViewed);
            theEntities.SaveChanges();
        }

        private void CreateIssueReplyViewedStateWithoutSave(int aUserId, int anIssueReplyId, bool aViewed) {
            IssueReplyViewedState myViewedState = IssueReplyViewedState.CreateIssueReplyViewedState(0, anIssueReplyId, aUserId, aViewed, DateTime.UtcNow);
            theEntities.AddToIssueReplyViewedStates(myViewedState);
        }

        private IssueViewedState GetIssueViewedState(int anIssueId, int anUserId) {
            return (from v in theEntities.IssueViewedStates
                    where v.IssueId == anIssueId
                    && v.UserId == anUserId
                    select v).FirstOrDefault<IssueViewedState>();
        }

        private IssueReplyViewedState GetIssueReplyViewedState(int aUserId, int anIssueReplyId) {
            return (from v in theEntities.IssueReplyViewedStates
                    where v.UserId == aUserId
                    && v.IssueReplyId == anIssueReplyId
                    select v).FirstOrDefault<IssueReplyViewedState>();
        }

        private IEnumerable<IssueReplyViewedState> GetIssueReplyViewedStates(int anIssueReplyId) {
            return (from v in theEntities.IssueReplyViewedStates
                    where v.IssueReplyId == anIssueReplyId
                    select v).ToList<IssueReplyViewedState>();
        }

        public IEnumerable<Issue> GetIssuesByTitleContains(string aTitlePortion) {
            return (from i in theEntities.Issues
                    where i.Title.Contains(aTitlePortion)
                    select i).ToList<Issue>();
        }


        public IEnumerable<Issue> GetMostPopularIssuesByHitCount(int aLimit) {
            IEnumerable<AggregatedIssueHit> myMostPopular = (from h in theEntities.IssueHits
                                              group h by h.IsssueId into g
                                              select new AggregatedIssueHit { IssueId = g.Key, HitCountSum = g.Sum(h2 => h2.HitCount) } )
                                              .OrderByDescending(p => p.HitCountSum)
                                              .Take<AggregatedIssueHit>(aLimit);

            List<Issue> myMostPopularIssues = (from p in myMostPopular
                                           join i in theEntities.Issues on p.IssueId equals i.Id
                                           select i).ToList<Issue>();

            if (myMostPopularIssues.Count<Issue>() < aLimit) {
                int myCountNeeded = aLimit - myMostPopularIssues.Count;
                IEnumerable<Issue> myNewestIssues = GetNewestIssues(aLimit).Except(myMostPopularIssues);

                myMostPopularIssues.AddRange(myNewestIssues);
            }


            return myMostPopularIssues;
        }

        public IEnumerable<Issue> GetNewestIssues(int aLimit) {
            return (from i in theEntities.Issues
                    select i)
                    .OrderByDescending(i2 => i2.DateTimeStamp)
                    .Take<Issue>(aLimit)
                    .ToList<Issue>();
        }

        private class AggregatedIssueHit {
            public int IssueId;
            public int HitCountSum;
        }
    }
}