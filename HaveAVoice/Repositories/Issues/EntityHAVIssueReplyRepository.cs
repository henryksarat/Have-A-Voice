using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.Issues.Helpers;
using HaveAVoice.Helpers;

namespace HaveAVoice.Repositories.Issues {
    public class EntityHAVIssueReplyRepository : IHAVIssueReplyRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IssueReply CreateIssueReply(int aUserId, string aUserCity, string aUserState, int aUserZipCode, int anIssueId, string aReply, bool anAnonymous, int aDisposition, string aFirstName, string aLastName) {
            IssueReply myIssueReply = IssueReply.CreateIssueReply(0, anIssueId, aUserId, aReply, aUserCity, aUserState, aUserZipCode, aDisposition, anAnonymous, DateTime.UtcNow, false);
            if (aUserId == HAVConstants.PRIVATE_USER_ID) {
                myIssueReply.TempFirstName = aFirstName;
                myIssueReply.TempLastName = aLastName;
            }

            theEntities.AddToIssueReplys(myIssueReply);
            theEntities.SaveChanges();

            if (aUserId != HAVConstants.PRIVATE_USER_ID) {
                IssueReplyViewedHelper.CreateIssueReplyViewedState(theEntities, aUserId, myIssueReply.Id, true);
            }

            return myIssueReply;
        }

        public void CreateIssueReplyStance(User aUser, int anIssueReplyId, int aDisposition) {
            IssueReplyDisposition myIssueReplyDisposition = IssueReplyDisposition.CreateIssueReplyDisposition(0, anIssueReplyId, aUser.Id, aDisposition);
            theEntities.AddToIssueReplyDispositions(myIssueReplyDisposition);
            theEntities.SaveChanges();
        }

        public void DeleteIssueReply(User aDeletingUser, IssueReply anIssueReply, bool anAdminDelete) {
            anIssueReply.Deleted = true;
            anIssueReply.DeletedByUserId = aDeletingUser.Id;
            theEntities.ApplyCurrentValues(anIssueReply.EntityKey.EntitySetName, anIssueReply);
            theEntities.SaveChanges();
        }

        public IssueReply GetIssueReply(int anIssueReplyId) {
            return (from ir in theEntities.IssueReplys.Include("Issue")
                    where ir.Id == anIssueReplyId
                    && ir.Deleted == false
                    select ir).FirstOrDefault();
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
                        IssueStance = ir.Disposition,
                        User = u,
                        Reply = ir.Reply,
                        FirstName = ir.TempFirstName,
                        LastName = ir.TempLastName,
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
                        IssueStance = ir.Disposition,
                        User = u,
                        Reply = ir.Reply,
                        FirstName = ir.TempFirstName,
                        LastName = ir.TempLastName,
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

        public bool HasIssueReplyStance(User aUser, int anIssueReplyId) {
            return (from ir in theEntities.IssueReplyDispositions
                    where ir.UserId == aUser.Id && ir.IssueReplyId == anIssueReplyId
                    select ir).Count<IssueReplyDisposition>() > 0 ? true : false;
        }

        public void MarkIssueReplyAsViewed(int aUserId, int anIssueReplyId) {
            IssueReplyViewedState myViewedState = IssueReplyViewedHelper.GetIssueReplyViewedState(theEntities, aUserId, anIssueReplyId);
            if (myViewedState != null) {
                myViewedState.Viewed = true;
                myViewedState.LastUpdated = DateTime.UtcNow;
                theEntities.ApplyCurrentValues(myViewedState.EntityKey.EntitySetName, myViewedState);
                theEntities.SaveChanges();
            }
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
    }
}