using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVComplaintRepository : IHAVComplaintRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddMergeComplaint(User aFiledBy, User towardUser, string complaint, int mergeRequestId) {
            throw new NotImplementedException();
        }

        public void AddIssueComplaint(User aFiledBy, string aComplaint, int anIssueId) {
            IssueComplaint myIssueComplaint = IssueComplaint.CreateIssueComplaint(0, aFiledBy.Id, anIssueId, aComplaint);
            
            theEntities.AddToIssueComplaints(myIssueComplaint);
            theEntities.SaveChanges();
        }

        public void AddIssueReplyComplaint(User aFiledBy, string aComplaint, int anIssueReplyId) {
            IssueReplyComplaint myIssueReplyComplaint = IssueReplyComplaint.CreateIssueReplyComplaint(0, aFiledBy.Id, anIssueReplyId, aComplaint);

            theEntities.AddToIssueReplyComplaints(myIssueReplyComplaint);
            theEntities.SaveChanges();
        }

        public void AddIssueReplyCommentComplaint(User aFiledBy, string aComplaint, int anIssueReplyCommentId) {
            IHAVIssueRepository myIssueRepo = new EntityHAVIssueRepository();
            IssueReplyCommentComplaint issueReplyCommentComplaint = IssueReplyCommentComplaint.CreateIssueReplyCommentComplaint(0, aFiledBy.Id, anIssueReplyCommentId, aComplaint);

            theEntities.AddToIssueReplyCommentComplaints(issueReplyCommentComplaint);
            theEntities.SaveChanges();
        }

        public void AddProfileComplaint(User aFiledBy, string aComplaint, int aTowardUserId) {
            ProfileComplaint myProfileComplaint = ProfileComplaint.CreateProfileComplaint(0, aFiledBy.Id, aTowardUserId, aComplaint);

            theEntities.AddToProfileComplaints(myProfileComplaint);
            theEntities.SaveChanges();
        }

        public void AddUserPictureComplaint(User aFiledBy, string aComplaint, int aPictureId) {
            UserPictureComplaint myUserPicutreComplaint = UserPictureComplaint.CreateUserPictureComplaint(0, aFiledBy.Id, aPictureId, aComplaint);

            theEntities.AddToUserPictureComplaints(myUserPicutreComplaint);
            theEntities.SaveChanges();
        }
    }
}