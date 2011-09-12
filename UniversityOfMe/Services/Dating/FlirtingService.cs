using System.Web.Mvc;
using UniversityOfMe.Helpers.Configuration;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories.Dating;
using System.Collections.Generic;
using System;
using System.Linq;
using Social.Generic.Models;
using UniversityOfMe.Models;
using Social.Validation;
using UniversityOfMe.Services.Users;

namespace UniversityOfMe.Services.Dating {
    public class FlirtingService : IFlirtingService {
        private IValidationDictionary theValidationDictionary;
        private IFlirtingRepository theFlirtingRepository;
        private IUofMeUserRetrievalService theUserRetrievalService;

        public FlirtingService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new UofMeUserRetrievalService(), new FlirtingRepository()) { }

        public FlirtingService(IValidationDictionary aValidationDictionary, UofMeUserRetrievalService aUserRetService, IFlirtingRepository aFlirtingRepo) {
            theValidationDictionary = aValidationDictionary;
            theFlirtingRepository = aFlirtingRepo;
            theUserRetrievalService = aUserRetService;
        }

        public FlirtModel GetFlirtModel(int aTaggedUserId) {
            FlirtModel myFlirtModel = GetFlirtModel();
            User myUser = theUserRetrievalService.GetUser(aTaggedUserId);
            if (myUser != null) {
                myFlirtModel.TaggedUser = myUser;
                myFlirtModel.TaggedUserId = aTaggedUserId;
            }
            return myFlirtModel;
        }

        public FlirtModel GetFlirtModel() {
            string[] myAdjectives = SiteConfiguration.FlirtAdjectives();
            string[] myDeliciousTreats = SiteConfiguration.FlirtDeliciousTreats();
            string[] myAnimals = SiteConfiguration.FlirtAnimals();

            Random myRandom = new Random();
            string myRandomAdjetive = myAdjectives.OrderBy<string, int>(f => myRandom.Next()).FirstOrDefault<string>();
            string myRandomDeliciousTreats = myDeliciousTreats.OrderBy<string, int>(f => myRandom.Next()).FirstOrDefault<string>();
            string myRandomAnimals = myAnimals.OrderBy<string, int>(f => myRandom.Next()).FirstOrDefault<string>();

            IEnumerable<SelectListItem> myAdjectivesListed = new SelectList(myAdjectives, myRandomAdjetive);
            IEnumerable<SelectListItem> myDeliciousTreatsListed = new SelectList(myDeliciousTreats, myRandomDeliciousTreats);
            IEnumerable<SelectListItem> myAnimalsListed = new SelectList(myAnimals, myRandomAnimals);
            
            return new FlirtModel() {
                Adjectives = myAdjectivesListed,
                DeliciousTreats = myDeliciousTreatsListed,
                Animals = myAnimalsListed
            };
        }


        public bool CreateFlirt(UserInformationModel<User> aUserInfoModel, string aUniversityId, FlirtModel aFlirtModel) {
            if (!ValidFlirt(aFlirtModel)) {
                return false;
            }

            theFlirtingRepository.CreateFlirt(
                aUserInfoModel.Details, 
                aUniversityId,
                aFlirtModel.Adjective, 
                aFlirtModel.DeliciousTreat, 
                aFlirtModel.Animal, 
                aFlirtModel.HairColor, 
                aFlirtModel.Gender, 
                aFlirtModel.Message, 
                aFlirtModel.TaggedUserId,
                aFlirtModel.Where
            );

            return true;
        }

        public IEnumerable<AnonymousFlirt> GetFlirtsWithinUniversity(string aUniversityId, int aLimit) {
            return theFlirtingRepository.GetAnonymousFlirtsWithinUniversity(aUniversityId).Take<AnonymousFlirt>(aLimit);
        }

        private bool ValidFlirt(FlirtModel aFlirtModel) {
            if (string.IsNullOrEmpty(aFlirtModel.Gender)) {
                theValidationDictionary.AddError("Gender", aFlirtModel.Gender, "A gender is required.");
            }

            if (string.IsNullOrEmpty(aFlirtModel.HairColor)) {
                theValidationDictionary.AddError("HairColor", aFlirtModel.HairColor, "A hair color is required.");
            }

            if (string.IsNullOrEmpty(aFlirtModel.Message)) {
                theValidationDictionary.AddError("Message", aFlirtModel.Message, "A message is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}
