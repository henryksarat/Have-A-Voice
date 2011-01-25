using System.Collections.Generic;
using HaveAVoice.Validation;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Repositories;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Helpers;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVIssueService : HAVBaseService, IHAVIssueService {
        private IValidationDictionary theValidationDictionary;
        private IHAVIssueRepository theRepository;

        public HAVIssueService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityHAVIssueRepository(), new HAVBaseRepository()) { }

        public HAVIssueService(IValidationDictionary aValidationDictionary, IHAVIssueRepository aRepository,
                                            IHAVBaseRepository baseRepository)
            : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theRepository = aRepository;
        }

        public IEnumerable<Issue> GetLatestIssues() {
            return theRepository.GetLatestIssues();
        }

        public Issue GetIssue(int aIssueId) {
            return theRepository.GetIssue(aIssueId);
        }

        public bool CreateIssue(UserInformationModel aUserCreating, Issue aIssueToCreate) {
            if (!ValidateIssue(aIssueToCreate)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, HAVPermission.Post_Issue)) {
                return false;
            }

            theRepository.CreateIssue(aIssueToCreate, aUserCreating.Details);
            return true;
        }

        public bool CreateIssueReply(UserInformationModel aUserCreating, IssueModel aIssueModel) {
            if (!ValidateReply(aIssueModel.Comment, (int)aIssueModel.Disposition)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, HAVPermission.Post_Issue_Reply)) {
                return false;
            }

            theRepository.CreateIssueReply(aIssueModel.Issue, aUserCreating.Details, aIssueModel.Comment, 
                aIssueModel.Anonymous, aIssueModel.Disposition);
            return true;
        }

        public bool CreateIssueReply(UserInformationModel aUserCreating, int anIssueId, string aReply, int aDisposition, bool anAnonymous) {
            if (!ValidateReply(aReply, aDisposition)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, HAVPermission.Post_Issue_Reply)) {
                return false;
            }

            theRepository.CreateIssueReply(aUserCreating.Details, anIssueId, aReply, anAnonymous, aDisposition);
            return true;
        }

        public bool CreateCommentToIssueReply(UserInformationModel aUserCreating, IssueReplyDetailsModel aIssueReply) {
            if (!ValidateComment(aIssueReply.Comment)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, HAVPermission.Post_Issue_Reply_Comment)) {
                return false;
            }

            theRepository.CreateCommentToIssueReply(aIssueReply.IssueReply, aUserCreating.Details, aIssueReply.Comment);
            return true;
        }

        public bool CreateCommentToIssueReply(UserInformationModel aUserCreating, int aIssueReplyId, string aComment) {
            if (!ValidateComment(aComment)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, HAVPermission.Post_Issue_Reply_Comment)) {
                return false;
            }

            theRepository.CreateCommentToIssueReply(aUserCreating.Details, aIssueReplyId, aComment);
            return true;
        }

        public IEnumerable<IssueReplyModel> GetReplysToIssue(User aUser, Issue aIssue, IEnumerable<string> aSelectedRoles, PersonFilter aFilter) {
            return theRepository.GetReplysToIssue(aUser, aIssue, aSelectedRoles, aFilter);
        }

        public IssueReply GetIssueReply(int anIssueReplyId) {
            return theRepository.GetIssueReply(anIssueReplyId);
        }

        public IEnumerable<IssueReplyComment> GetIssueReplyComments(int aIssueReplyId) {
            return theRepository.GetIssueReplyComments(aIssueReplyId);
        }

        public IssueReplyComment GetIssueReplyComment(int aIssueReplyCommentId) {
            return theRepository.GetIssueReplyComment(aIssueReplyCommentId);
        }

        public void AddIssueDisposition(User aUser, int anIssueId, int aDisposition) {
            theRepository.CreateIssueDisposition(aUser, anIssueId, aDisposition);
        }

        public IEnumerable<IssueWithDispositionModel> GetIssues(User aUser) {
            return theRepository.GetIssues(aUser);
        }

        public void AddIssueReplyDisposition(User aUser, int anIssueReplyId, int aDisposition) {
            theRepository.CreateIssueReplyDisposition(aUser, anIssueReplyId, aDisposition);
        }

        public bool EditIssue(UserInformationModel aUserEditing, Issue anIssue) {
            if (!ValidateIssue(anIssue)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserEditing, HAVPermission.Edit_Issue, HAVPermission.Edit_Any_Issue)) {
                return false;
            }

            bool myOverride = HAVPermissionHelper.AllowedToPerformAction(aUserEditing, HAVPermission.Edit_Any_Issue);
            Issue myOriginalIssue = GetIssue(anIssue.Id);
            if (myOriginalIssue.User.Id == aUserEditing.Details.Id || myOverride) {
                theRepository.UpdateIssue(aUserEditing.Details, myOriginalIssue, anIssue, myOverride);
                return true;
            }

            return false;
        }

        public bool EditIssueReply(UserInformationModel aUserEditing, IssueReply anIssueReply) {
            if (!ValidateReply(anIssueReply)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserEditing, HAVPermission.Edit_Issue_Reply, HAVPermission.Edit_Any_Issue_Reply)) {
                return false;
            }

            bool myOverride = HAVPermissionHelper.AllowedToPerformAction(aUserEditing, HAVPermission.Edit_Any_Issue_Reply);
            IssueReply myOriginalReply = GetIssueReply(anIssueReply.Id);
            if (myOriginalReply.User.Id == aUserEditing.Details.Id || myOverride) {
                theRepository.UpdateIssueReply(aUserEditing.Details, myOriginalReply, anIssueReply, myOverride);
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
                theRepository.UpdateIssueReplyComment(aUserEditing.Details, myOriginalComment, aComment, myOverrideDelete);
                return true;
            }

            return false;
        }

        public bool DeleteIssue(UserInformationModel aDeletingUser, int anIssueId) {
            bool myAdminOverride = HAVPermissionHelper.AllowedToPerformAction(aDeletingUser, HAVPermission.Delete_Any_Issue);
            Issue myIssue = GetIssue(anIssueId);
            if (myIssue.User.Id == aDeletingUser.Details.Id || myAdminOverride) {
                theRepository.DeleteIssue(aDeletingUser.Details, myIssue, myAdminOverride);
                return true;
            }
            return false;
        }

        public bool DeleteIssueReply(UserInformationModel aDeletingUser, int anIssueReplyId) {
            bool myAdminOverride = HAVPermissionHelper.AllowedToPerformAction(aDeletingUser, HAVPermission.Delete_Any_Issue_Reply);
            IssueReply myReply = GetIssueReply(anIssueReplyId);
            if (myReply.User.Id == aDeletingUser.Details.Id || myAdminOverride) {
                theRepository.DeleteIssueReply(aDeletingUser.Details, myReply, myAdminOverride);
                return true;
            }
            return false;
        }

        public bool DeleteIssueReplyComment(UserInformationModel aDeletingUser, int anIssueReplyCommentId) {
            bool myAdminOverride = HAVPermissionHelper.AllowedToPerformAction(aDeletingUser, HAVPermission.Delete_Any_Issue_Reply_Comment);
            IssueReplyComment myComment = GetIssueReplyComment(anIssueReplyCommentId);

            if (myComment.User.Id == aDeletingUser.Details.Id || myAdminOverride) {
                theRepository.DeleteIssueReplyComment(aDeletingUser.Details, myComment, myAdminOverride);
                return true;
            }
            return false;
        }

        private bool ValidateIssue(Issue aIssueToValidate) {
            if (aIssueToValidate.Title.Trim().Length == 0) {
                theValidationDictionary.AddError("Title", aIssueToValidate.Title, "Title is required.");
            }
            if (aIssueToValidate.Description.Trim().Length == 0) {
                theValidationDictionary.AddError("Description", aIssueToValidate.Description, "Description is required.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidateReply(IssueReply anIssueReply) {
            return ValidateReply(anIssueReply.Reply, anIssueReply.Disposition);
        }

        private bool ValidateReply(string aReply, int aDisposition) {
            if (aReply.Trim().Length == 0) {
                theValidationDictionary.AddError("Reply", aReply, "Reply is required.");
            }
            if (aDisposition == (int)Disposition.None) {
                theValidationDictionary.AddError("Disposition", aDisposition.ToString(), "Disposition is required.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidateComment(IssueReplyComment aComment) {
            return ValidateComment(aComment.Comment);
        }

        private bool ValidateComment(string aComment) {
            if (aComment.Trim().Length == 0) {
                theValidationDictionary.AddError("Body", aComment, "Comment is required.");
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
