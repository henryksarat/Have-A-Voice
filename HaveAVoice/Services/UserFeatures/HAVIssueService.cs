﻿using System.Collections.Generic;
using HaveAVoice.Validation;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Repositories;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using System.Collections;
using System.Linq;

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

        public Issue GetIssue(int aIssueId, UserInformationModel aViewingUser) {
            Issue myIssue = theRepository.GetIssue(aIssueId);
            if (aViewingUser != null && aViewingUser.Details.Id == myIssue.UserId) {
                theRepository.MarkIssueAsReadForAuthor(myIssue);
            }
            return myIssue;
        }

        public bool CreateIssue(UserInformationModel aUserCreating, Issue aIssueToCreate) {
            if (!ValidateIssue(aIssueToCreate) || !IssueDoesntExist(aIssueToCreate.Title)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, HAVPermission.Post_Issue)) {
                return false;
            }

            Issue myIssue = theRepository.CreateIssue(aIssueToCreate, aUserCreating.Details);
            theRepository.MarkIssueAsUnreadForAuthor(myIssue.Id);
            return true;
        }

        public bool CreateIssueReply(UserInformationModel aUserCreating, IssueModel aIssueModel) {
            if (!ValidateReply(aIssueModel.Comment, (int)aIssueModel.Disposition)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, HAVPermission.Post_Issue_Reply)) {
                return false;
            }

            theRepository.CreateIssueReply(aUserCreating.Details, aIssueModel.Issue.Id, aIssueModel.Comment, aIssueModel.Anonymous, (int)aIssueModel.Disposition);
            theRepository.MarkIssueAsUnreadForAuthor(aIssueModel.Issue.Id);

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
            theRepository.MarkIssueAsUnreadForAuthor(anIssueId);

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

        public IssueReply GetIssueReply(User aViewingUser, int anIssueReplyId) {
            IssueReply myIssueReply = GetIssueReply(anIssueReplyId);
            theRepository.MarkIssueReplyAsViewed(aViewingUser.Id, anIssueReplyId);

            return myIssueReply;
        }

        public IEnumerable<IssueReplyComment> GetIssueReplyComments(int aIssueReplyId) {
            return theRepository.GetIssueReplyComments(aIssueReplyId);
        }

        public IssueReplyComment GetIssueReplyComment(int aIssueReplyCommentId) {
            return theRepository.GetIssueReplyComment(aIssueReplyCommentId);
        }

        public bool AddIssueDisposition(User aUser, int anIssueId, int aDisposition) {
            if(theRepository.HasIssueDisposition(aUser, anIssueId)) {
                return false;
            } else {
                theRepository.CreateIssueDisposition(aUser, anIssueId, aDisposition);
                return true;
            }
        }

        public IEnumerable<IssueWithDispositionModel> GetIssues(User aUser) {
            return theRepository.GetIssues(aUser);
        }

        public bool AddIssueReplyDisposition(User aUser, int anIssueReplyId, int aDisposition) {
            if (theRepository.HasIssueReplyDisposition(aUser, anIssueReplyId)) {
                return false;
            } else {
                theRepository.CreateIssueReplyDisposition(aUser, anIssueReplyId, aDisposition);
                return true;
            }
        }

        public bool EditIssue(UserInformationModel aUserEditing, Issue anIssue) {
            if (!ValidateIssue(anIssue)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserEditing, HAVPermission.Edit_Issue, HAVPermission.Edit_Any_Issue)) {
                return false;
            }

            bool myOverride = HAVPermissionHelper.AllowedToPerformAction(aUserEditing, HAVPermission.Edit_Any_Issue);
            Issue myOriginalIssue = GetIssue(anIssue.Id, aUserEditing);

            if (myOriginalIssue.User.Id == aUserEditing.Details.Id || myOverride) {
                if (!myOriginalIssue.Title.Equals(anIssue.Title) && !IssueDoesntExist(anIssue.Title)) {
                    return false;
                }
                theRepository.UpdateIssue(aUserEditing.Details, myOriginalIssue, anIssue, myOverride);
                return true;
            } else {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to edit the issue.");
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
            Issue myIssue = GetIssue(anIssueId, aDeletingUser);
            if (myIssue.User.Id == aDeletingUser.Details.Id || myAdminOverride) {
                theRepository.DeleteIssue(aDeletingUser.Details, myIssue, myAdminOverride);
                return true;
            } else {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to delete the issue.");
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

        private bool IssueDoesntExist(string aTitle) {
            if (theRepository.HasIssueTitleBeenUsed(aTitle)) {
                theValidationDictionary.AddError("Title", aTitle, "An issue with that same exact title exists. Please reply to that issue.");
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


        public IssueModel CreateIssueModel(UserInformationModel myUserInfo, int anIssueId) {
            Issue myIssue = GetIssue(anIssueId, myUserInfo);
            return FillIssueModel(myUserInfo.Details, myIssue);
        }

        public IssueModel CreateIssueModel(User aViewingUser, string aTitle) {
            Issue myIssue = theRepository.GetIssueByTitle(aTitle);
            if (aViewingUser != null && aViewingUser.Id == myIssue.UserId) {
                theRepository.MarkIssueAsReadForAuthor(myIssue);
            }
            theRepository.AddHitToIssue(myIssue.Id);
            return FillIssueModel(aViewingUser, myIssue);
        }

        public IssueModel CreateIssueModel(string aTitle) {
            Issue myIssue = theRepository.GetIssueByTitle(aTitle);
            theRepository.AddHitToIssue(myIssue.Id);
            return FillIssueModel(null, myIssue);
        }

        private IssueModel FillIssueModel(User myViewingUser, Issue anIssue) {
            if (anIssue == null) {
                return null;
            }

            IEnumerable<IssueReplyModel> myPeopleReplys = new List<IssueReplyModel>();
            IEnumerable<IssueReplyModel> myPoliticianReplys = new List<IssueReplyModel>();
            IEnumerable<IssueReplyModel> myPoliticalCandidateReplys = new List<IssueReplyModel>();

            if (myViewingUser == null) {
                myPeopleReplys = theRepository.GetReplysToIssue(anIssue, UserRoleHelper.RegisteredRoles(), PersonFilter.People);
                myPoliticianReplys = theRepository.GetReplysToIssue(anIssue, UserRoleHelper.PoliticianRoles(), PersonFilter.Politicians);
                myPoliticalCandidateReplys = theRepository.GetReplysToIssue(anIssue, UserRoleHelper.PoliticalCandidateRoles(), PersonFilter.PoliticalCandidates);
            } else {
                myPeopleReplys = theRepository.GetReplysToIssue(myViewingUser, anIssue, UserRoleHelper.RegisteredRoles(), PersonFilter.People);
                myPoliticianReplys = theRepository.GetReplysToIssue(myViewingUser, anIssue, UserRoleHelper.PoliticianRoles(), PersonFilter.Politicians);
                myPoliticalCandidateReplys = theRepository.GetReplysToIssue(myViewingUser, anIssue, UserRoleHelper.PoliticalCandidateRoles(), PersonFilter.PoliticalCandidates);
            }

            List<IssueReplyModel> myMerged = new List<IssueReplyModel>();
            myMerged.AddRange(myPeopleReplys);
            myMerged.AddRange(myPoliticianReplys);
            myMerged.AddRange(myPoliticalCandidateReplys);

            myMerged = myMerged.OrderByDescending(i => i.DateTimeStamp).ToList<IssueReplyModel>();

            return new IssueModel(anIssue, myMerged);
        }

        public IEnumerable<Issue> GetIssueByTitleSearch(string aTitlePortion) {
            return theRepository.GetIssuesByTitleContains(aTitlePortion);
        }

        public IEnumerable<Issue> GetMostPopularIssuesByHitCount(int aLimit) {
            return theRepository.GetMostPopularIssuesByHitCount(aLimit);
        }


        public IEnumerable<Issue> GetNewestIssues(int aLimit) {
            return theRepository.GetNewestIssues(aLimit);
        }
    }
}
