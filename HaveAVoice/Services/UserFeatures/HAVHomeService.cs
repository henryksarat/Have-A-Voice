﻿using System.Collections.Generic;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;
using System;
using HaveAVoice.Models;
using System.Linq;
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVHomeService : HAVBaseService, IHAVHomeService {
        private IValidationDictionary theValidationDictionary;
        private IHAVFanService theFanService;
        private IHAVHomeRepository theHomeRepository;

        public HAVHomeService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new HAVFanService(), new EntityHAVHomeRepository(), new HAVBaseRepository()) { }

        public HAVHomeService(IValidationDictionary aValidationDictionary, IHAVFanService aFanService, 
                              IHAVHomeRepository aRepository, IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theFanService = aFanService;
            theHomeRepository = aRepository;
        }

        public NotLoggedInModel NotLoggedIn() {
                return new NotLoggedInModel() {
                    LikedIssues = theHomeRepository.GetMostPopularIssues(Disposition.LIKE),
                    DislikedIssues = theHomeRepository.GetMostPopularIssues(Disposition.DISLIKE),
                    NewestIssueReplys = theHomeRepository.NewestIssueReplys(),
                    MostPopularIssueReplys = theHomeRepository.GetMostPopularIssueReplys()
            };
        }

        public IEnumerable<FeedModel> FanFeed(User aUser) {
            IEnumerable<Issue> myIssues = theHomeRepository.FanIssueFeed(aUser);
            IEnumerable<IssueReply> myIssueReplys = theHomeRepository.FanIssueReplyFeed(aUser);
            return CreateFeedModel(aUser, myIssues, myIssueReplys, false);
        }

        public IEnumerable<FeedModel> OfficialsFeed(User aUser) {
            IEnumerable<Issue> myIssues = theHomeRepository.OfficialsIssueFeed(aUser, RoleHelper.OfficialRoles());
            IEnumerable<IssueReply> myIssueReplys = theHomeRepository.OfficialsIssueReplyFeed(aUser, RoleHelper.OfficialRoles());
            return CreateFeedModel(aUser, myIssues, myIssueReplys, false);
        }

        public FilteredFeedModel FilteredFeed(User aUser) {
            IEnumerable<Issue> myIssues = theHomeRepository.FilteredIssuesFeed(aUser);
            IEnumerable<IssueReply> myIssueReplys = theHomeRepository.FilteredIssueReplysFeed(aUser);
            IEnumerable<FeedModel> myFeedModel = CreateFeedModel(aUser, myIssues, myIssueReplys, true);

            return new FilteredFeedModel(aUser) {
                FeedModels = myFeedModel
            };
        }

        public bool AddFilter(User aUser, string aCity, string aState, string aZipCode) {
            if (!ValidateCityStateZipCode(aUser, aCity, aState, aZipCode)) {
                return false;
            }

            if (aZipCode.Length > 0) {
                int myZipCode = Convert.ToInt32(aZipCode);
                theHomeRepository.AddZipCodeFilter(aUser, myZipCode);
            }

            if (!aState.ToUpper().Equals("SELECT") && aCity.Length > 0) {
                theHomeRepository.AddCityStateFilter(aUser, aCity, aState);
            }

            return true;

        }

        public IEnumerable<FeedModel> UserFeedModel(User aViewingUser, int aTargetUser) {
            IEnumerable<Issue> myIssues = theHomeRepository.UserIssueFeed(aTargetUser);
            IEnumerable<IssueReply> myIssueReplys = theHomeRepository.UserIssueReplyFeed(aTargetUser);
            return CreateFeedModel(aViewingUser, myIssues, myIssueReplys, false);
        }

        private IEnumerable<FeedModel> CreateFeedModel(User aUser, IEnumerable<Issue> anIssues, IEnumerable<IssueReply> anIssueReplys, bool aUseFanService) {
            IEnumerator<Issue> myIssueEnumerator = anIssues.GetEnumerator();
            IEnumerator<IssueReply> myReplyEnumerator = anIssueReplys.GetEnumerator();
            List<FeedModel> myFeedModel = new List<FeedModel>();

            while (myReplyEnumerator.MoveNext() | myIssueEnumerator.MoveNext()) {
                IssueReply myIssueReply = myReplyEnumerator.Current;
                Issue myIssue = myIssueEnumerator.Current;

                if (myIssueReply != null) {
                    myFeedModel.Add(CreateIssueReplyFeedModel(aUser, myIssueReply, aUseFanService));
                }

                if (myIssue != null) {
                    myFeedModel.Add(CreateIssueFeedModel(aUser, myIssue, aUseFanService));
                }
            }

            return myFeedModel;
        }

        private FeedModel CreateIssueFeedModel(User aUser, Issue myIssue, bool aUseFanService) {
            string myUsername = myIssue.User.Username;
            string myProfilePictureUrl = ProfilePictureHelper.ProfilePicture(myIssue.User);

            if (aUseFanService && !theFanService.IsFan(myIssue.UserId, aUser)) {
                myUsername = "Anonymous";
                myProfilePictureUrl = HAVConstants.ANONYMOUS_PICTURE_URL;
            }

            IEnumerable<IssueDisposition> myIssueDisposition = myIssue.IssueDispositions;

            return new FeedModel() {
                Username = myUsername,
                ProfilePictureUrl = myProfilePictureUrl,
                IssueType = IssueType.Issue,
                Title = myIssue.Title,
                Body = myIssue.Description,
                DateTimeStamp = myIssue.DateTimeStamp,
                TotalLikes = (from d in myIssueDisposition where d.Disposition == (int)Disposition.LIKE select d).Count<IssueDisposition>(),
                TotalDislikes = (from d in myIssueDisposition where d.Disposition == (int)Disposition.DISLIKE select d).Count<IssueDisposition>(),
                HasDisposition = (from d in myIssueDisposition where d.UserId == aUser.Id select d).Count<IssueDisposition>() > 1 ? true : false,
                TotalReplys = myIssue.IssueReplys.Count,
            };
        }

        private FeedModel CreateIssueReplyFeedModel(User aUser, IssueReply myIssueReply, bool aUseFanService) {
            string myUsername = myIssueReply.User.Username;
            string myProfilePictureUrl = ProfilePictureHelper.ProfilePicture(myIssueReply.User);

            if(aUseFanService && !theFanService.IsFan(myIssueReply.UserId, aUser)) {
                myUsername = "Anonymous";
                myProfilePictureUrl = HAVConstants.ANONYMOUS_PICTURE_URL;
            }

            IEnumerable<IssueReplyDisposition> myReplyDisposition = myIssueReply.IssueReplyDispositions;

            return new FeedModel() {
                Username = myUsername,
                ProfilePictureUrl = myProfilePictureUrl,
                IssueType = IssueType.Reply,
                Title = myIssueReply.Issue.Title,
                Body = myIssueReply.Reply,
                DateTimeStamp = myIssueReply.DateTimeStamp,
                TotalLikes = (from d in myReplyDisposition where d.Disposition == (int)Disposition.LIKE select d).Count<IssueReplyDisposition>(),
                TotalDislikes = (from d in myReplyDisposition where d.Disposition == (int)Disposition.DISLIKE select d).Count<IssueReplyDisposition>(),
                HasDisposition = (from d in myReplyDisposition where d.UserId == aUser.Id select d).Count<IssueReplyDisposition>() > 1 ? true : false,
                TotalReplys = myIssueReply.IssueReplyComments.Count
            };
        }

        private bool ValidateCityStateZipCode(User aUser, string aCity, string aState, string aZipCode) {
            if (aZipCode.Length > 0) {
                ValidateZipCode(aUser, aZipCode);
            }
            if (IsStateSelected(aState) || aCity.Length > 0) {
                ValidateCityState(aUser, aCity, aState);
            }

            if (aCity.Length == 0 && !IsStateSelected(aState) && aZipCode.Length == 0) {
                theValidationDictionary.AddError("Filtered", aCity, "You need to specify something to filter on.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidateZipCode(User aUser, string aZipCode) {
            int aResult;
            if (aZipCode.Length != 5 || !int.TryParse(aZipCode, out aResult)) {
                theValidationDictionary.AddError("ZipCode", aZipCode.ToString(), "Invalid zip code.");
            }

            if (theValidationDictionary.isValid && theHomeRepository.ZipCodeFilterExists(aUser, Int32.Parse(aZipCode))) {
                theValidationDictionary.AddError("ZipCode", aZipCode.ToString(), "This zip code filter already exists.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidateCityState(User aUser, string aCity, string aState) {
            if (aCity.Length == 0) {
                theValidationDictionary.AddError("City", aCity, "Invalid city.");
            }
            if (!IsStateSelected(aState)) {
                theValidationDictionary.AddError("State", aState, "Select a state.");
            }
            if (theValidationDictionary.isValid && theHomeRepository.CityStateFilterExists(aUser, aCity, aState)) {
                theValidationDictionary.AddError("City", aCity, "That city/state filter already exists.");
            }

            return theValidationDictionary.isValid;
        }

        private bool IsStateSelected(string aState) {
            return !aState.ToUpper().Equals("SELECT");
        }
    }
}
