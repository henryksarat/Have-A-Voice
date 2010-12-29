using System.Collections.Generic;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;
using System;
using HaveAVoice.Models;


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
            IEnumerable<IssueReply> myIssueReplys = theHomeRepository.FanFeed(aUser);
            return new LoggedInModel(aUser) {
                ProfilePictureURL = theUserPictureService.GetProfilePictureURL(aUser),
                IssueReplys = myIssueReplys,
            };
        }

        public LoggedInModel OfficialReplys(User aUser) {
            IEnumerable<IssueReply> myIssueReplys = theHomeRepository.FanFeed(aUser);
            IEnumerable<IssueReply> myOfficialsReplys = theHomeRepository.OfficialsFeed(RoleHelper.OfficialRoles());
            return new LoggedInModel(aUser) {
                ProfilePictureURL = theUserPictureService.GetProfilePictureURL(aUser),
                IssueReplys = myOfficialsReplys
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
