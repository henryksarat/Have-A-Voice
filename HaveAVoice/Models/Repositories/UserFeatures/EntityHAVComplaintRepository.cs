using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Models.Repositories.UserFeatures {
    public class EntityHAVComplaintRepository : HAVBaseRepository, IHAVComplaintRepository {
        IHAVUserRepository theUserRepo;
        
        public EntityHAVComplaintRepository() {
            theUserRepo = new EntityHAVUserRepository();
        }

        public void AddMergeComplaint(User filedBy, User towardUser, string complaint, int mergeRequestId) {
            throw new NotImplementedException();
        }

        public void AddIssueComplaint(User filedBy, string complaint, int issueId) {
            IHAVIssueRepository issueRepo = new EntityHAVIssueRepository();
            IssueComplaint issueComplaint = IssueComplaint.CreateIssueComplaint(0, complaint);
            issueComplaint.FiledByUser = theUserRepo.GetUser(filedBy.Id);
            issueComplaint.Issue = issueRepo.GetIssue(issueId);
            
            GetEntities().AddToIssueComplaints(issueComplaint);
            GetEntities().SaveChanges();
        }

        public void AddIssueReplyComplaint(User filedBy, string complaint, int issueReplyId) {
            IHAVIssueRepository issueRepo = new EntityHAVIssueRepository();
            IssueReplyComplaint issueReplyComplaint = IssueReplyComplaint.CreateIssueReplyComplaint(0, complaint);
            issueReplyComplaint.FiledByUser = theUserRepo.GetUser(filedBy.Id);
            issueReplyComplaint.IssueReply = issueRepo.GetIssueReply(issueReplyId);

            GetEntities().AddToIssueReplyComplaints(issueReplyComplaint);
            GetEntities().SaveChanges();
        }

        public void AddIssueReplyCommentComplaint(User filedBy, string complaint, int issueReplyCommentId) {
            IHAVIssueRepository issueRepo = new EntityHAVIssueRepository();
            IssueReplyCommentComplaint issueReplyCommentComplaint = IssueReplyCommentComplaint.CreateIssueReplyCommentComplaint(0, filedBy.Id, issueReplyCommentId, complaint);

            GetEntities().AddToIssueReplyCommentComplaints(issueReplyCommentComplaint);
            GetEntities().SaveChanges();
        }

        public void AddProfileComplaint(User filedBy, string complaint, int towardUserId) {
            ProfileComplaint profileComplaint = ProfileComplaint.CreateProfileComplaint(0, complaint);
            profileComplaint.FiledByUser = theUserRepo.GetUser(filedBy.Id);
            profileComplaint.TowardUser = theUserRepo.GetUser(towardUserId);

            GetEntities().AddToProfileComplaints(profileComplaint);
            GetEntities().SaveChanges();
        }

        public void AddUserPictureComplaint(User filedBy, string complaint, int pictureId) {
            UserPictureComplaint userPicutreComplaint = UserPictureComplaint.CreateUserPictureComplaint(0, complaint);
            userPicutreComplaint.FiledByUser = theUserRepo.GetUser(filedBy.Id);
            userPicutreComplaint.UserPicture = theUserRepo.GetUserPicture(pictureId);

            GetEntities().AddToUserPictureComplaints(userPicutreComplaint);
            GetEntities().SaveChanges();
        }

        public void AddMergeComplaint(User filedBy, string complaint, int mergeRequestId) {
            throw new NotImplementedException();
        }
    }
}