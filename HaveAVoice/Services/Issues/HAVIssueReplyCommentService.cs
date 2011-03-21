using System.Collections.Generic;
using HaveAVoice.Models;
using HaveAVoice.Repositories.Issues;
using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Services.Issues {
    public class HAVIssueReplyCommentService : IHAVIssueReplyCommentService {
        private IValidationDictionary theValidationDictionary;
        private IHAVIssueReplyCommentRepository theIssueReplyCommentRepository;

        public HAVIssueReplyCommentService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityHAVIssueReplyCommentRepository()) { }

        public HAVIssueReplyCommentService(IValidationDictionary aValidationDictionary, IHAVIssueReplyCommentRepository anIssueReplyCommentRepo) {
            theValidationDictionary = aValidationDictionary;
            theIssueReplyCommentRepository = anIssueReplyCommentRepo;
        }

        public bool CreateCommentToIssueReply(UserInformationModel<User> aUserCreating, int anIssueReplyId, string aComment) {
            if (!ValidateComment(aComment)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, SocialPermission.Post_Issue_Reply_Comment)) {
                return false;
            }

            theIssueReplyCommentRepository.CreateCommentToIssueReply(aUserCreating.Details, anIssueReplyId, aComment);
            return true;
        }

        public bool DeleteIssueReplyComment(UserInformationModel<User> aDeletingUser, int anIssueReplyCommentId) {
            bool myAdminOverride = PermissionHelper<User>.AllowedToPerformAction(aDeletingUser, SocialPermission.Delete_Any_Issue_Reply_Comment);
            IssueReplyComment myComment = GetIssueReplyComment(anIssueReplyCommentId);

            if (myComment.User.Id == aDeletingUser.Details.Id || myAdminOverride) {
                theIssueReplyCommentRepository.DeleteIssueReplyComment(aDeletingUser.Details, myComment, myAdminOverride);
                return true;
            }
            return false;
        }

        public bool EditIssueReplyComment(UserInformationModel<User> aUserEditing, IssueReplyComment aComment) {
            if (!ValidateComment(aComment)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserEditing, SocialPermission.Edit_Issue_Reply_Comment, SocialPermission.Edit_Any_Issue_Reply_Comment)) {
                return false;
            }

            bool myOverrideDelete = PermissionHelper<User>.AllowedToPerformAction(aUserEditing, SocialPermission.Edit_Any_Issue_Reply_Comment);
            IssueReplyComment myOriginalComment = GetIssueReplyComment(aComment.Id);
            if (myOriginalComment.UserId == aUserEditing.Details.Id || myOverrideDelete) {
                theIssueReplyCommentRepository.UpdateIssueReplyComment(aUserEditing.Details, myOriginalComment, aComment, myOverrideDelete);
                return true;
            }

            return false;
        }

        public IssueReplyComment GetIssueReplyComment(int anIssueReplyCommentId) {
            return theIssueReplyCommentRepository.GetIssueReplyComment(anIssueReplyCommentId);
        }

        public IEnumerable<IssueReplyComment> GetIssueReplyComments(int anIssueReplyId) {
            return theIssueReplyCommentRepository.GetIssueReplyComments(anIssueReplyId);
        }

        private bool ValidateComment(IssueReplyComment aComment) {
            return ValidateComment(aComment.Comment);
        }

        private bool ValidateComment(string aComment) {
            if (aComment.Trim().Length == 0) {
                theValidationDictionary.AddError("Comment", aComment, "Comment is required.");
            }

            return theValidationDictionary.isValid;
        }

        private bool AllowedToPerformAction(UserInformationModel<User> aUser, params SocialPermission[] aPermissions) {
            if (!PermissionHelper<User>.AllowedToPerformAction(aUser, aPermissions)) {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to post. Please try again.");
            }

            return theValidationDictionary.isValid;
        }
    }
}