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
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVHomeService : HAVBaseService, IHAVHomeService {
        private IValidationDictionary theValidationDictionary;
        private IHAVFriendService theFriendService;
        private IHAVHomeRepository theHomeRepository;

        public HAVHomeService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new HAVFriendService(), new EntityHAVHomeRepository(), new HAVBaseRepository()) { }

        public HAVHomeService(IValidationDictionary aValidationDictionary, IHAVFriendService aFriendService, 
                              IHAVHomeRepository aRepository, IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theFriendService = aFriendService;
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
