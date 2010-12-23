using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVComplaintRepository : HAVBaseRepository, IHAVComplaintRepository {
        IHAVUserRepository theUserRepo;
        
        public EntityHAVComplaintRepository() {
            theUserRepo = new EntityHAVUserRepository();
        }

        public void AddMergeComplaint(User aFiledBy, User towardUser, string complaint, int mergeRequestId) {
            throw new NotImplementedException();
        }

        public void AddIssueComplaint(User aFiledBy, string aComplaint, int anIssueId) {
            IssueComplaint myIssueComplaint = IssueComplaint.CreateIssueComplaint(0, aFiledBy.Id, anIssueId, aComplaint);
            
            GetEntities().AddToIssueComplaints(myIssueComplaint);
            GetEntities().SaveChanges();
        }

        public void AddIssueReplyComplaint(User aFiledBy, string aComplaint, int anIssueReplyId) {
            IssueReplyComplaint myIssueReplyComplaint = IssueReplyComplaint.CreateIssueReplyComplaint(0, aFiledBy.Id, anIssueReplyId, aComplaint);

            GetEntities().AddToIssueReplyComplaints(myIssueReplyComplaint);
            GetEntities().SaveChanges();
        }

        public void AddIssueReplyCommentComplaint(User aFiledBy, string aComplaint, int anIssueReplyCommentId) {
            IHAVIssueRepository myIssueRepo = new EntityHAVIssueRepository();
            IssueReplyCommentComplaint issueReplyCommentComplaint = IssueReplyCommentComplaint.CreateIssueReplyCommentComplaint(0, aFiledBy.Id, anIssueReplyCommentId, aComplaint);

            GetEntities().AddToIssueReplyCommentComplaints(issueReplyCommentComplaint);
            GetEntities().SaveChanges();
        }

        public void AddProfileComplaint(User aFiledBy, string aComplaint, int aTowardUserId) {
            ProfileComplaint myProfileComplaint = ProfileComplaint.CreateProfileComplaint(0, aFiledBy.Id, aTowardUserId, aComplaint);

            GetEntities().AddToProfileComplaints(myProfileComplaint);
            GetEntities().SaveChanges();
        }

        public void AddUserPictureComplaint(User aFiledBy, string aComplaint, int aPictureId) {
            UserPictureComplaint myUserPicutreComplaint = UserPictureComplaint.CreateUserPictureComplaint(0, aFiledBy.Id, aPictureId, aComplaint);

            GetEntities().AddToUserPictureComplaints(myUserPicutreComplaint);
            GetEntities().SaveChanges();
        }
    }
}