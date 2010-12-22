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

        public void AddMergeComplaint(User filedBy, User towardUser, string complaint, int mergeRequestId) {
            throw new NotImplementedException();
        }

        public void AddIssueComplaint(User filedBy, string aComplaint, int issueId) {
            IHAVIssueRepository issueRepo = new EntityHAVIssueRepository();
            IssueComplaint issueComplaint = IssueComplaint.CreateIssueComplaint(0, aComplaint);
            issueComplaint.FiledByUser = GetUser(filedBy.Id);
            issueComplaint.Issue = issueRepo.GetIssue(issueId);
            
            GetEntities().AddToIssueComplaints(issueComplaint);
            GetEntities().SaveChanges();
        }

        public void AddIssueReplyComplaint(User filedBy, string aComplaint, int issueReplyId) {
            IHAVIssueRepository issueRepo = new EntityHAVIssueRepository();
            IssueReplyComplaint issueReplyComplaint = IssueReplyComplaint.CreateIssueReplyComplaint(0, aComplaint);
            issueReplyComplaint.FiledByUser = GetUser(filedBy.Id);
            issueReplyComplaint.IssueReply = issueRepo.GetIssueReply(issueReplyId);

            GetEntities().AddToIssueReplyComplaints(issueReplyComplaint);
            GetEntities().SaveChanges();
        }

        public void AddIssueReplyCommentComplaint(User filedBy, string aComplaint, int issueReplyCommentId) {
            IHAVIssueRepository issueRepo = new EntityHAVIssueRepository();
            IssueReplyCommentComplaint issueReplyCommentComplaint = IssueReplyCommentComplaint.CreateIssueReplyCommentComplaint(0, filedBy.Id, issueReplyCommentId, aComplaint);

            GetEntities().AddToIssueReplyCommentComplaints(issueReplyCommentComplaint);
            GetEntities().SaveChanges();
        }

        public void AddProfileComplaint(User filedBy, string aComplaint, int towardUserId) {
            ProfileComplaint profileComplaint = ProfileComplaint.CreateProfileComplaint(0, aComplaint);
            profileComplaint.FiledByUser = GetUser(filedBy.Id);
            profileComplaint.TowardUser = GetUser(towardUserId);

            GetEntities().AddToProfileComplaints(profileComplaint);
            GetEntities().SaveChanges();
        }

        public void AddUserPictureComplaint(User filedBy, string aComplaint, int pictureId) {
            UserPictureComplaint userPicutreComplaint = UserPictureComplaint.CreateUserPictureComplaint(0, filedBy.Id, pictureId, aComplaint);

            GetEntities().AddToUserPictureComplaints(userPicutreComplaint);
            GetEntities().SaveChanges();
        }

        private User GetUser(int anId) {
            IHAVUserRetrievalRepository myUserRetrieval = new EntityHAVUserRetrievalRepository();
            return myUserRetrieval.GetUser(anId);
        }
    }
}