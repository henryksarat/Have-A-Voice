﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.Issues;
using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Services.Issues {
    public class HAVIssueService : IHAVIssueService {
        private IValidationDictionary theValidationDictionary;
        private IHAVIssueRepository theIssueRepository;
        private IHAVIssueReplyRepository theIssueReplyRepository;
        private IHAVIssueReplyCommentRepository theIssueReplyCommentRepository;

        public HAVIssueService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityHAVIssueRepository(), new EntityHAVIssueReplyRepository(), new EntityHAVIssueReplyCommentRepository()) { }

        public HAVIssueService(IValidationDictionary aValidationDictionary, IHAVIssueRepository aRepository, IHAVIssueReplyRepository anIssueReplyRepo,
                               IHAVIssueReplyCommentRepository anIssueReplyCommentRepo) {
            theValidationDictionary = aValidationDictionary;
            theIssueRepository = aRepository;
            theIssueReplyRepository = anIssueReplyRepo;
            theIssueReplyCommentRepository = anIssueReplyCommentRepo;
        }

        public bool AddIssueStance(User aUser, int anIssueId, int aDisposition) {
            if (theIssueRepository.HasIssueStance(aUser, anIssueId)) {
                return false;
            } else {
                theIssueRepository.CreateIssueStance(aUser, anIssueId, aDisposition);
                return true;
            }
        }

        public bool CreateIssue(UserInformationModel<User> aUserCreating, Issue aIssueToCreate) {
            if (!ValidateIssue(aIssueToCreate) || !IssueDoesntExist(aIssueToCreate.Title)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserCreating, SocialPermission.Post_Issue)) {
                return false;
            }

            Issue myIssue = theIssueRepository.CreateIssue(aIssueToCreate, aUserCreating.Details);
            theIssueRepository.MarkIssueAsUnreadForAuthor(myIssue.Id);
            return true;
        }

        public IssueModel CreateIssueModel(UserInformationModel<User> myUserInfo, int anIssueId) {
            Issue myIssue = GetIssue(anIssueId, myUserInfo);
            return FillIssueModel(myUserInfo.Details, myIssue);
        }

        public IssueModel CreateIssueModel(User aViewingUser, string aTitle) {
            Issue myIssue = theIssueRepository.GetIssueByTitle(aTitle);
            if (aViewingUser != null && aViewingUser.Id == myIssue.UserId) {
                theIssueRepository.MarkIssueAsReadForAuthor(myIssue);
            }
            theIssueRepository.AddHitToIssue(myIssue.Id);
            return FillIssueModel(aViewingUser, myIssue);
        }

        public IssueModel CreateIssueModel(string aTitle) {
            Issue myIssue = theIssueRepository.GetIssueByTitle(aTitle);
            theIssueRepository.AddHitToIssue(myIssue.Id);
            return FillIssueModel(null, myIssue);
        }

        public bool DeleteIssue(UserInformationModel<User> aDeletingUser, int anIssueId) {
            bool myAdminOverride = PermissionHelper<User>.AllowedToPerformAction(aDeletingUser, SocialPermission.Delete_Any_Issue);
            Issue myIssue = GetIssue(anIssueId, aDeletingUser);
            if (myIssue.User.Id == aDeletingUser.Details.Id || myAdminOverride) {
                theIssueRepository.DeleteIssue(aDeletingUser.Details, myIssue, myAdminOverride);
                return true;
            } else {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to delete the issue.");
            }
            return false;
        }

        public bool EditIssue(UserInformationModel<User> aUserEditing, Issue anIssue) {
            if (!ValidateIssue(anIssue)) {
                return false;
            }

            if (!AllowedToPerformAction(aUserEditing, SocialPermission.Edit_Issue, SocialPermission.Edit_Any_Issue)) {
                return false;
            }

            bool myOverride = PermissionHelper<User>.AllowedToPerformAction(aUserEditing, SocialPermission.Edit_Any_Issue);
            Issue myOriginalIssue = GetIssue(anIssue.Id, aUserEditing);

            if (myOriginalIssue.User.Id == aUserEditing.Details.Id || myOverride) {
                if (!myOriginalIssue.Title.Equals(anIssue.Title) && !IssueDoesntExist(anIssue.Title)) {
                    return false;
                }
                theIssueRepository.UpdateIssue(aUserEditing.Details, myOriginalIssue, anIssue, myOverride);
                return true;
            } else {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to edit the issue.");
            }

            return false;
        }

        public Issue GetIssue(int aIssueId, UserInformationModel<User> aViewingUser) {
            Issue myIssue = theIssueRepository.GetIssue(aIssueId);
            if (aViewingUser != null && aViewingUser.Details.Id == myIssue.UserId) {
                theIssueRepository.MarkIssueAsReadForAuthor(myIssue);
            }
            return myIssue;
        }

        public IEnumerable<Issue> GetLatestIssues() {
            return theIssueRepository.GetLatestIssues();
        }

        public IEnumerable<IssueWithDispositionModel> GetIssues(User aUser) {
            return theIssueRepository.GetIssues(aUser);
        }

        public IEnumerable<Issue> GetIssueByTitleSearch(string aTitlePortion) {
            return theIssueRepository.GetIssuesByTitleContains(aTitlePortion);
        }

        public IEnumerable<Issue> GetMostPopularIssuesByHitCount(int aLimit) {
            return theIssueRepository.GetMostPopularIssuesByHitCount(aLimit);
        }


        public IEnumerable<Issue> GetNewestIssues(int aLimit) {
            return theIssueRepository.GetNewestIssues(aLimit);
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
            if (theIssueRepository.HasIssueTitleBeenUsed(aTitle)) {
                theValidationDictionary.AddError("Title", aTitle, "An issue with that same exact title exists. Please reply to that issue.");
            }

            return theValidationDictionary.isValid;
        }


        private bool AllowedToPerformAction(UserInformationModel<User> aUser, params SocialPermission[] aPermissions) {
            if (!PermissionHelper<User>.AllowedToPerformAction(aUser, aPermissions)) {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to post. Please try again.");
            }

            return theValidationDictionary.isValid;
        }

        private IssueModel FillIssueModel(User myViewingUser, Issue anIssue) {
            if (anIssue == null) {
                return null;
            }

            IEnumerable<IssueReplyModel> myPeopleReplys = new List<IssueReplyModel>();
            IEnumerable<IssueReplyModel> myPoliticianReplys = new List<IssueReplyModel>();
            IEnumerable<IssueReplyModel> myPoliticalCandidateReplys = new List<IssueReplyModel>();

            if (myViewingUser == null) {
                myPeopleReplys = theIssueReplyRepository.GetReplysToIssue(anIssue, UserRoleHelper.RegisteredRoles(), PersonFilter.People);
                myPoliticianReplys = theIssueReplyRepository.GetReplysToIssue(anIssue, UserRoleHelper.PoliticianRoles(), PersonFilter.Politicians);
                myPoliticalCandidateReplys = theIssueReplyRepository.GetReplysToIssue(anIssue, UserRoleHelper.PoliticalCandidateRoles(), PersonFilter.PoliticalCandidates);
            } else {
                myPeopleReplys = theIssueReplyRepository.GetReplysToIssue(myViewingUser, anIssue, UserRoleHelper.RegisteredRoles(), PersonFilter.People);
                myPoliticianReplys = theIssueReplyRepository.GetReplysToIssue(myViewingUser, anIssue, UserRoleHelper.PoliticianRoles(), PersonFilter.Politicians);
                myPoliticalCandidateReplys = theIssueReplyRepository.GetReplysToIssue(myViewingUser, anIssue, UserRoleHelper.PoliticalCandidateRoles(), PersonFilter.PoliticalCandidates);
            }

            List<IssueReplyModel> myMerged = new List<IssueReplyModel>();
            myMerged.AddRange(myPeopleReplys);
            myMerged.AddRange(myPoliticianReplys);
            myMerged.AddRange(myPoliticalCandidateReplys);

            myMerged = myMerged.OrderByDescending(i => i.DateTimeStamp).ToList<IssueReplyModel>();

            return new IssueModel(anIssue, myMerged);
        }
    }
}