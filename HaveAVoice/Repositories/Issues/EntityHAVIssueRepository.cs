using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using HaveAVoice.Models.View;

namespace HaveAVoice.Repositories.Issues {
    public class EntityHAVIssueRepository : IHAVIssueRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddHitToIssue(int anIssueId) {
            IssueHit myIssueHit = IssueHit.CreateIssueHit(0, anIssueId, 1, DateTime.UtcNow);
            theEntities.AddToIssueHits(myIssueHit);
            theEntities.SaveChanges();
        }

        public Issue CreateIssue(Issue anIssueToCreate, User aUserCreating) {
            anIssueToCreate = Issue.CreateIssue(0, anIssueToCreate.Title, anIssueToCreate.Description, aUserCreating.City, aUserCreating.State, aUserCreating.Zip, DateTime.UtcNow, aUserCreating.Id, false);

            theEntities.AddToIssues(anIssueToCreate);
            theEntities.SaveChanges();

            return anIssueToCreate;
        }

        public void CreateIssueStance(User aUser, int anIssueId, int aStance) {
            IssueDisposition myIssueDisposition = IssueDisposition.CreateIssueDisposition(0, anIssueId, aUser.Id, aStance);
            theEntities.AddToIssueDispositions(myIssueDisposition);
            theEntities.SaveChanges();
        }

        public void DeleteIssue(User aDeletingUser, Issue anIssue, bool anAdminDelete) {
            anIssue.Deleted = true;
            anIssue.DeletedByUserId = aDeletingUser.Id;
            theEntities.ApplyCurrentValues(anIssue.EntityKey.EntitySetName, anIssue);
            theEntities.SaveChanges();
        }

        public IEnumerable<IssueWithDispositionModel> GetIssues(User aUser) {
            ObjectQuery<Issue> myIssues = theEntities.Issues;
            ObjectQuery<IssueDisposition> myIssueDispositions = theEntities.IssueDispositions;
            int myUserId = aUser == null ? 0 : aUser.Id;

            return (from i in myIssues
                    let d = myIssueDispositions.Where(d2 => d2.Issue.Id == i.Id)
                    .Where(d2 => d2.User.Id == myUserId)
                    .FirstOrDefault()
                    where i.Deleted == false
                    select new IssueWithDispositionModel() {
                        Issue = i,
                        HasDisposition = (d == null && myUserId != 0 ? false : true),
                        TotalAgrees = (from d2 in i.IssueDispositions
                                       where d2.Disposition == (int)Disposition.Like
                                       select d2).Count<IssueDisposition>(),
                        TotalDisagrees = (from d2 in i.IssueDispositions
                                          where d2.Disposition == (int)Disposition.Dislike
                                          select d2).Count<IssueDisposition>(),
                    }).ToList<IssueWithDispositionModel>();
        }

        public IEnumerable<IssueWithDispositionModel> GetIssuesByDescription(User aUser, string aSearchTerm) {
            ObjectQuery<Issue> myIssues = theEntities.Issues;
            ObjectQuery<IssueDisposition> myIssueDispositions = theEntities.IssueDispositions;
            int myUserId = aUser == null ? 0 : aUser.Id;

            return (from i in myIssues
                    let d = myIssueDispositions.Where(d2 => d2.Issue.Id == i.Id)
                    .Where(d2 => d2.User.Id == myUserId)
                    .FirstOrDefault()
                    where i.Deleted == false
                    && i.Description.Contains(aSearchTerm)
                    select new IssueWithDispositionModel() {
                        Issue = i,
                        HasDisposition = (d == null && myUserId != 0 ? false : true),
                        TotalAgrees = (from d2 in i.IssueDispositions
                                       where d2.Disposition == (int)Disposition.Like
                                       select d2).Count<IssueDisposition>(),
                        TotalDisagrees = (from d2 in i.IssueDispositions
                                          where d2.Disposition == (int)Disposition.Dislike
                                          select d2).Count<IssueDisposition>(),
                    }).ToList<IssueWithDispositionModel>();
        }

        public IEnumerable<IssueWithDispositionModel> GetIssuesByTitle(User aUser, string aSearchTerm) {
            ObjectQuery<Issue> myIssues = theEntities.Issues;
            ObjectQuery<IssueDisposition> myIssueDispositions = theEntities.IssueDispositions;
            int myUserId = aUser == null ? 0 : aUser.Id;

            return (from i in myIssues
                    let d = myIssueDispositions.Where(d2 => d2.Issue.Id == i.Id)
                    .Where(d2 => d2.User.Id == myUserId)
                    .FirstOrDefault()
                    where i.Deleted == false
                    && i.Title.Contains(aSearchTerm)
                    select new IssueWithDispositionModel() {
                        Issue = i,
                        HasDisposition = (d == null && myUserId != 0 ? false : true),
                        TotalAgrees = (from d2 in i.IssueDispositions
                                       where d2.Disposition == (int)Disposition.Like
                                       select d2).Count<IssueDisposition>(),
                        TotalDisagrees = (from d2 in i.IssueDispositions
                                          where d2.Disposition == (int)Disposition.Dislike
                                          select d2).Count<IssueDisposition>(),
                    }).ToList<IssueWithDispositionModel>();
        }

        public Issue GetIssue(int anIssueId) {
            return (from i in theEntities.Issues
                    where i.Id == anIssueId
                    select i).FirstOrDefault();
        }

        public Issue GetIssueByTitle(string aSearchTerm) {
            return (from i in theEntities.Issues
                    where i.Title.Contains(aSearchTerm)
                    && i.Deleted == false
                    select i).FirstOrDefault();
        }

        public Issue GetIssueByDescription(string aSearchTerm) {
            return (from i in theEntities.Issues
                    where i.Description.Contains(aSearchTerm)
                    && i.Deleted == false
                    select i).FirstOrDefault();
        }

        public bool HasIssueStance(User aUser, int anIssueId) {
            return (from i in theEntities.IssueDispositions
                    where i.UserId == aUser.Id && i.IssueId == anIssueId
                    select i).Count<IssueDisposition>() > 0 ? true : false;
        }

        public bool HasIssueTitleBeenUsed(string aTitle) {
            return GetIssueByTitle(aTitle) != null ? true : false;
        }

        public IEnumerable<Issue> GetIssuesByTitle(string aTitlePortion) {
            return (from i in theEntities.Issues
                    where i.Title.Contains(aTitlePortion)
                    select i).ToList<Issue>();
        }

        public IEnumerable<Issue> GetLatestIssues() {
            return theEntities.Issues.ToList<Issue>();
        }

        public IEnumerable<Issue> GetMostPopularIssuesByHitCount(int aLimit) {
            IEnumerable<AggregatedIssueHit> myMostPopular = (from h in theEntities.IssueHits
                                                             group h by h.IsssueId into g
                                                             select new AggregatedIssueHit { IssueId = g.Key, HitCountSum = g.Sum(h2 => h2.HitCount) })
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

        public void MarkIssueAsReadForAuthor(Issue anIssue) {
            IssueViewedState myIssueViewedState = GetIssueViewedState(anIssue.Id, anIssue.UserId);
            myIssueViewedState.Viewed = true;

            theEntities.SaveChanges();
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

        private IssueViewedState GetIssueViewedState(int anIssueId, int anUserId) {
            return (from v in theEntities.IssueViewedStates
                    where v.IssueId == anIssueId
                    && v.UserId == anUserId
                    select v).FirstOrDefault<IssueViewedState>();
        }

        private class AggregatedIssueHit {
            public int IssueId = 0;
            public int HitCountSum = 0;
        }
    }
}