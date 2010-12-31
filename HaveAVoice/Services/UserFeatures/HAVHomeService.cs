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
        private IHAVFanService theFanService;
        private IHAVHomeRepository theHomeRepository;

        public HAVHomeService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new HAVFanService(), new HAVUserPictureService(), new EntityHAVHomeRepository(), new HAVBaseRepository()) { }

        public HAVHomeService(IValidationDictionary aValidationDictionary, IHAVFanService aFanService, 
                              IHAVUserPictureService aUserPictureService, IHAVHomeRepository aRepository, 
                              IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theFanService = aFanService;
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

        public LoggedInModel<FeedModel> FanReplys(User aUser) {
            IEnumerable<Issue> myIssues = theHomeRepository.FanIssueFeed(aUser);
            IEnumerable<IssueReply> myIssueReplys = theHomeRepository.FanIssueReplyFeed(aUser);
            IEnumerable<FeedModel> myFeedModel = CreateFeedModel(aUser, myIssues, myIssueReplys, false);

            UserModel myUserModel = new UserModel(aUser) {
                ProfilePictureUrl = theUserPictureService.GetProfilePictureURL(aUser)
            };
            return new LoggedInModel<FeedModel>(myUserModel) {
                Models = myFeedModel
            };
        }

        public LoggedInModel<FeedModel> OfficialsFeed(User aUser) {
            IEnumerable<Issue> myIssues = theHomeRepository.OfficialsIssueFeed(aUser, RoleHelper.OfficialRoles());
            IEnumerable<IssueReply> myIssueReplys = theHomeRepository.OfficialsIssueReplyFeed(aUser, RoleHelper.OfficialRoles());
            IEnumerable<FeedModel> myFeedModel = CreateFeedModel(aUser, myIssues, myIssueReplys, false);

            UserModel myUserModel = new UserModel(aUser) {
                ProfilePictureUrl = theUserPictureService.GetProfilePictureURL(aUser)
            };

            return new LoggedInModel<FeedModel>(myUserModel) {
                Models = myFeedModel
            };
        }

        public LoggedInModel<FeedModel> FilteredFeed(User aUser) {
            IEnumerable<Issue> myIssues = theHomeRepository.FilteredIssuesFeed(aUser);
            IEnumerable<IssueReply> myIssueReplys = theHomeRepository.FilteredIssueReplysFeed(aUser);
            IEnumerable<FeedModel> myFeedModel = CreateFeedModel(aUser, myIssues, myIssueReplys, true);

            UserModel myUserModel = new UserModel(aUser) {
                ProfilePictureUrl = theUserPictureService.GetProfilePictureURL(aUser)
            };

            return new LoggedInModel<FeedModel>(myUserModel) {
                Models = myFeedModel
            };
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
            string myProfilePictureUrl = theUserPictureService.GetProfilePictureURL(myIssue.User);

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
            string myProfilePictureUrl = theUserPictureService.GetProfilePictureURL(myIssueReply.User);

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
