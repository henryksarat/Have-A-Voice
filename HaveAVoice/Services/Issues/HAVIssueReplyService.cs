using System.Collections.Generic;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.Issues;
using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;
using Social.Generic.Constants;
using HaveAVoice.Helpers;

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

        public bool CreateIssueReply(IssueModel anIssueModel) {
            if (!ValidIssueModel(anIssueModel)) {
                return false;
            }

            theIssueReplyRepository.CreateIssueReply(anIssueModel.UserId, anIssueModel.City, anIssueModel.State, 
                                                     anIssueModel.Zip, anIssueModel.IssueId, anIssueModel.Reply, 
                                                     anIssueModel.Anonymous, (int)anIssueModel.Disposition, anIssueModel.FirstName, anIssueModel.LastName);
            theIssueRepository.MarkIssueAsUnreadForAuthor(anIssueModel.IssueId);

            return true;
        }

        public bool CreateIssueReply(UserInformationModel<User> aUserCreating, int anIssueId, string aReply, int aDisposition, bool anAnonymous) {
            if (!ValidateReply(aReply, aDisposition)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, SocialPermission.Post_Issue_Reply)) {
                return false;
            }

            //theIssueReplyRepository.CreateIssueReply(aUserCreating.Details, anIssueId, aReply, anAnonymous, aDisposition);
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

        public IEnumerable<IssueReplyModel> GetReplysToIssue(Issue anIssue, IEnumerable<string> aRoleNames, PersonFilter aFilter) {
            return theIssueReplyRepository.GetReplysToIssue(anIssue, aRoleNames, aFilter);
        }

        public IEnumerable<IssueReplyModel> GetReplysToIssue(User aUser, Issue anIssue, IEnumerable<string> aRoleNames, PersonFilter aFilter) {
            return theIssueReplyRepository.GetReplysToIssue(aUser, anIssue, aRoleNames, aFilter);
        }

        private bool ValidateReply(IssueReply anIssueReply) {
            return ValidateReply(anIssueReply.Reply, anIssueReply.Disposition);
        }

        private bool ValidIssueModel(IssueModel anIssueModel) {
            if (anIssueModel.Reply.Trim().Length == 0) {
                theValidationDictionary.AddError("Reply", anIssueModel.Reply, "Reply is required.");
            }
            if (anIssueModel.Disposition == (int)Disposition.None) {
                theValidationDictionary.AddError("Disposition", anIssueModel.Disposition.ToString(), "Disposition is required.");
            }

            if (anIssueModel.UserId == HAVConstants.PRIVATE_USER_ID) {
                if (anIssueModel.FirstName.Trim().Length == 0) {
                    theValidationDictionary.AddError("FirstName", anIssueModel.FirstName, "First name is required.");
                }

                if (anIssueModel.LastName.Trim().Length == 0) {
                    theValidationDictionary.AddError("LastName", anIssueModel.LastName, "Last name is required.");
                }
            }

            if (anIssueModel.Zip.ToString().Trim().Length != 5) {
                if (anIssueModel.UserId == HAVConstants.PRIVATE_USER_ID) {
                    theValidationDictionary.AddError("Zip", anIssueModel.Zip.ToString(), "The zip code must be 5 digits.");
                } else {
                    theValidationDictionary.AddError("Zip", anIssueModel.Zip.ToString(), "It appears your account isn't associated with a zip code. Please go to Settings, located at the top, and enter a zip code. Then try posting again.");
                }
            }

            if (anIssueModel.City.Trim().Length == 0) {
                theValidationDictionary.AddError("City", anIssueModel.City, "City is required.");
            }

            if (string.IsNullOrEmpty(anIssueModel.State) || anIssueModel.State.Equals(Constants.SELECT)) {
                theValidationDictionary.AddError("State", anIssueModel.State, "State is required.");
            }

            return theValidationDictionary.isValid;
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