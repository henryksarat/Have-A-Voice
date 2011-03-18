using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories.Issues;
using HaveAVoice.Validation;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;

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

        public bool CreateCommentToIssueReply(UserInformationModel aUserCreating, int anIssueReplyId, string aComment) {
            if (!ValidateComment(aComment)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, HAVPermission.Post_Issue_Reply_Comment)) {
                return false;
            }

            theIssueReplyCommentRepository.CreateCommentToIssueReply(aUserCreating.Details, anIssueReplyId, aComment);
            return true;
        }

        public bool DeleteIssueReplyComment(UserInformationModel aDeletingUser, int anIssueReplyCommentId) {
            bool myAdminOverride = HAVPermissionHelper.AllowedToPerformAction(aDeletingUser, HAVPermission.Delete_Any_Issue_Reply_Comment);
            IssueReplyComment myComment = GetIssueReplyComment(anIssueReplyCommentId);

            if (myComment.User.Id == aDeletingUser.Details.Id || myAdminOverride) {
                theIssueReplyCommentRepository.DeleteIssueReplyComment(aDeletingUser.Details, myComment, myAdminOverride);
                return true;
            }
            return false;
        }

        public bool EditIssueReplyComment(UserInformationModel aUserEditing, IssueReplyComment aComment) {
            if (!ValidateComment(aComment)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserEditing, HAVPermission.Edit_Issue_Reply_Comment, HAVPermission.Edit_Any_Issue_Reply_Comment)) {
                return false;
            }

            bool myOverrideDelete = HAVPermissionHelper.AllowedToPerformAction(aUserEditing, HAVPermission.Edit_Any_Issue_Reply_Comment);
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

        private bool AllowedToPerformAction(UserInformationModel aUser, params HAVPermission[] aPermissions) {
            if (!HAVPermissionHelper.AllowedToPerformAction(aUser, aPermissions)) {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to post. Please try again.");
            }

            return theValidationDictionary.isValid;
        }
    }
}