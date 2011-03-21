using System.Collections.Generic;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.Issues;
using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Services.Issues {
    public class HAVIssueReplyService : IHAVIssueReplyService {
        private IValidationDictionary theValidationDictionary;
        private IHAVIssueReplyRepository theIssueReplyRepository;
        private IHAVIssueRepository theIssueRepository;

        public HAVIssueReplyService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityHAVIssueReplyRepository(), new EntityHAVIssueRepository()) { }

        public HAVIssueReplyService(IValidationDictionary aValidationDictionary, IHAVIssueReplyRepository anIssueReplyRepo, IHAVIssueRepository anIssueRepository) {
            theValidationDictionary = aValidationDictionary;
            theIssueRepository = anIssueRepository;
            theIssueReplyRepository = anIssueReplyRepo;
        }

        public bool AddIssueReplyStance(User aUser, int anIssueReplyId, int aStance) {
            if (theIssueReplyRepository.HasIssueReplyStance(aUser, anIssueReplyId)) {
                return false;
            } else {
                theIssueReplyRepository.CreateIssueReplyStance(aUser, anIssueReplyId, aStance);
                return true;
            }
        }

        public bool CreateIssueReply(UserInformationModel<User> aUserCreating, IssueModel anIssueModel) {
            if (!ValidateReply(anIssueModel.Comment, (int)anIssueModel.Disposition)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, SocialPermission.Post_Issue_Reply)) {
                return false;
            }

            theIssueReplyRepository.CreateIssueReply(aUserCreating.Details, anIssueModel.Issue.Id, anIssueModel.Comment, anIssueModel.Anonymous, (int)anIssueModel.Disposition);
            theIssueRepository.MarkIssueAsUnreadForAuthor(anIssueModel.Issue.Id);

            return true;
        }

        public bool CreateIssueReply(UserInformationModel<User> aUserCreating, int anIssueId, string aReply, int aDisposition, bool anAnonymous) {
            if (!ValidateReply(aReply, aDisposition)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, SocialPermission.Post_Issue_Reply)) {
                return false;
            }

            theIssueReplyRepository.CreateIssueReply(aUserCreating.Details, anIssueId, aReply, anAnonymous, aDisposition);
            theIssueRepository.MarkIssueAsUnreadForAuthor(anIssueId);

            return true;
        }

        public bool EditIssueReply(UserInformationModel<User> aUserEditing, IssueReply anIssueReply) {
            if (!ValidateReply(anIssueReply)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserEditing, SocialPermission.Edit_Issue_Reply, SocialPermission.Edit_Any_Issue_Reply)) {
                return false;
            }

            bool myOverride = PermissionHelper<User>.AllowedToPerformAction(aUserEditing, SocialPermission.Edit_Any_Issue_Reply);
            IssueReply myOriginalReply = GetIssueReply(anIssueReply.Id);
            if (myOriginalReply.User.Id == aUserEditing.Details.Id || myOverride) {
                theIssueReplyRepository.UpdateIssueReply(aUserEditing.Details, myOriginalReply, anIssueReply, myOverride);
                return true;
            }

            return false;
        }

        public bool DeleteIssueReply(UserInformationModel<User> aDeletingUser, int anIssueReplyId) {
            bool myAdminOverride = PermissionHelper<User>.AllowedToPerformAction(aDeletingUser, SocialPermission.Delete_Any_Issue_Reply);
            IssueReply myReply = GetIssueReply(anIssueReplyId);
            if (myReply.User.Id == aDeletingUser.Details.Id || myAdminOverride) {
                theIssueReplyRepository.DeleteIssueReply(aDeletingUser.Details, myReply, myAdminOverride);
                return true;
            }
            return false;
        }

        public IssueReply GetIssueReply(int anIssueReplyId) {
            return theIssueReplyRepository.GetIssueReply(anIssueReplyId);
        }

        public IssueReply GetIssueReply(User aViewingUser, int anIssueReplyId) {
            IssueReply myIssueReply = GetIssueReply(anIssueReplyId);
            theIssueReplyRepository.MarkIssueReplyAsViewed(aViewingUser.Id, anIssueReplyId);

            return myIssueReply;
        }

        public IEnumerable<IssueReplyModel> GetReplysToIssue(User aUser, Issue anIssue, IEnumerable<string> aRoleNames, PersonFilter aFilter) {
            return theIssueReplyRepository.GetReplysToIssue(aUser, anIssue, aRoleNames, aFilter);
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

        private bool AllowedToPerformAction(UserInformationModel<User> aUser, params SocialPermission[] aPermissions) {
            if (!PermissionHelper<User>.AllowedToPerformAction(aUser, aPermissions)) {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to post. Please try again.");
            }

            return theValidationDictionary.isValid;
        }
    }
}