using System.Collections.Generic;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;
using System;
using HaveAVoice.Models;
using System.Linq;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVHomeService : HAVBaseService, IHAVHomeService {
        private IValidationDictionary theValidationDictionary;
        private IHAVUserPictureService theUserPictureService;
        private IHAVHomeRepository theHomeRepository;

        public HAVHomeService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new HAVUserPictureService(), new EntityHAVHomeRepository(), new HAVBaseRepository()) { }

        public HAVHomeService(IValidationDictionary aValidationDictionary, IHAVUserPictureService aUserPictureService, 
                                            IHAVHomeRepository aRepository, IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theUserPictureService = aUserPictureService;
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

        public LoggedInModel FanReplys(User aUser) {
            IEnumerable<Issue> myIssues = theHomeRepository.FanIssueFeed(aUser);
            IEnumerable<IssueReply> myIssueReplys = theHomeRepository.FanIssueReplyFeed(aUser);
            IEnumerable<FeedModel> myFeedModel = CreateFeedModel(aUser, myIssues, myIssueReplys);

            return new LoggedInModel(aUser) {
                ProfilePictureURL = theUserPictureService.GetProfilePictureURL(aUser),
                FeedModels = myFeedModel,
            };
        }

        public LoggedInModel OfficialReplys(User aUser) {
            IEnumerable<Issue> myIssues = theHomeRepository.OfficialsIssueFeed(aUser, RoleHelper.OfficialRoles());
            IEnumerable<IssueReply> myIssueReplys = theHomeRepository.OfficialsIssueReplyFeed(aUser, RoleHelper.OfficialRoles());
            IEnumerable<FeedModel> myFeedModel = CreateFeedModel(aUser, myIssues, myIssueReplys);

            return new LoggedInModel(aUser) {
                ProfilePictureURL = theUserPictureService.GetProfilePictureURL(aUser),
                FeedModels = myFeedModel,
            };
        }

        private IEnumerable<FeedModel> CreateFeedModel(User aUser, IEnumerable<Issue> anIssues, IEnumerable<IssueReply> anIssueReplys) {
            IEnumerator<Issue> myIssueEnumerator = anIssues.GetEnumerator();
            IEnumerator<IssueReply> myReplyEnumerator = anIssueReplys.GetEnumerator();
            List<FeedModel> myFeedModel = new List<FeedModel>();

            while (myReplyEnumerator.MoveNext() | myIssueEnumerator.MoveNext()) {
                IssueReply myIssueReply = myReplyEnumerator.Current;
                Issue myIssue = myIssueEnumerator.Current;

                if (myIssueReply != null) {
                    myFeedModel.Add(CreateIssueReplyFeedModel(aUser, myIssueReply));
                }

                if (myIssue != null) {
                    myFeedModel.Add(CreateIssueFeedModel(aUser, myIssue));
                }
            }

            return myFeedModel;
        }

        private FeedModel CreateIssueFeedModel(User aUser, Issue myIssue) {
            IEnumerable<IssueDisposition> myIssueDisposition = myIssue.IssueDispositions;

            return new FeedModel(myIssue.User) {
                ProfilePictureUrl = theUserPictureService.GetProfilePictureURL(myIssue.User),
                IssueType = IssueType.Issue,
                Title = myIssue.Title,
                Body = myIssue.Description,
                TotalLikes = (from d in myIssueDisposition where d.Disposition == (int)Disposition.LIKE select d).Count<IssueDisposition>(),
                TotalDislikes = (from d in myIssueDisposition where d.Disposition == (int)Disposition.DISLIKE select d).Count<IssueDisposition>(),
                HasDisposition = (from d in myIssueDisposition where d.UserId == aUser.Id select d).Count<IssueDisposition>() > 1 ? true : false,
                TotalReplys = myIssue.IssueReplys.Count
            };
        }

        private FeedModel CreateIssueReplyFeedModel(User aUser, IssueReply myIssueReply) {
            IEnumerable<IssueReplyDisposition> myReplyDisposition = myIssueReply.IssueReplyDispositions;

            return new FeedModel(myIssueReply.User) {
                ProfilePictureUrl = theUserPictureService.GetProfilePictureURL(myIssueReply.User),
                IssueType = IssueType.Reply,
                Title = myIssueReply.Issue.Title,
                Body = myIssueReply.Reply,
                TotalLikes = (from d in myReplyDisposition where d.Disposition == (int)Disposition.LIKE select d).Count<IssueReplyDisposition>(),
                TotalDislikes = (from d in myReplyDisposition where d.Disposition == (int)Disposition.DISLIKE select d).Count<IssueReplyDisposition>(),
                HasDisposition = (from d in myReplyDisposition where d.UserId == aUser.Id select d).Count<IssueReplyDisposition>() > 1 ? true : false,
                TotalReplys = myIssueReply.IssueReplyComments.Count
            };
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

        private bool ValidateCityStateCode(User aUser, string aCity, string aState) {
            if (aCity.Length == 0) {
                theValidationDictionary.AddError("City", aCity, "Invalid city.");
            }
            if (aState.Length == 0) {
                theValidationDictionary.AddError("State", aState, "Invalid state.");
            }
            if(theValidationDictionary.isValid && theHomeRepository.CityStateFilterExists(aUser, aCity, aState)) {
                theValidationDictionary.AddError("City", aCity, "That city/state filter already exists.");
            }

            return theValidationDictionary.isValid;
        }

        public bool AddZipCodeFilter(User aUser, string aZipCode) {
            if (!ValidateZipCode(aUser, aZipCode)) {
                return false;
            }
            int myZipCode = Convert.ToInt32(aZipCode);
            theHomeRepository.AddZipCodeFilter(aUser, myZipCode);
            return true;
        }

        public bool AddCityStateFilter(User aUser, string aCity, string aState) {
            if (!ValidateCityStateCode(aUser, aCity, aState)) {
                return false;
            }

            theHomeRepository.AddCityStateFilter(aUser, aCity, aState);
            return true;

        }
    }
}
