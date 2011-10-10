using System.Collections.Generic;
using System.Linq;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Marketplace;
using UniversityOfMe.Services.TextBooks;

namespace UniversityOfMe.Services {
    public class UniversityService : IUniversityService {
        private ITextBookService theTextBookService;
        private IMarketplaceService theMarketplaceService;
        private IUniversityRepository theUniversityRepository;

        public UniversityService(IValidationDictionary aValidationDictionary) 
            : this(new TextBookService(aValidationDictionary), 
                   new MarketplaceService(aValidationDictionary),
                   new EntityUniversityRepository()) { }

        public UniversityService(ITextBookService aTextBookService, 
                                 IMarketplaceService aMarketplaceService,
                                 IUniversityRepository aUniversityRepository) {
            theTextBookService = aTextBookService;
            theMarketplaceService = aMarketplaceService;
            theUniversityRepository = aUniversityRepository;
        }

        public void AddUserToUniversity(User aUser) {
            string myUniversityEmail = EmailHelper.ExtractEmailExtension(aUser.Email);
            theUniversityRepository.AddUserToUniversity(aUser.Id, myUniversityEmail);
        }

        public IDictionary<string, string> CreateAllUniversitiesDictionaryEntry() {
            IEnumerable<University> myUniversities = theUniversityRepository.Universities();
            IDictionary<string, string> myDictionary = new Dictionary<string, string>();
            myDictionary.Add(Constants.SELECT, Constants.SELECT);
            foreach(University myUniversity in myUniversities) {
                myDictionary.Add(myUniversity.UniversityName, myUniversity.Id);
            }
            return myDictionary;
        }

        public IDictionary<string, string> CreateAcademicTermsDictionaryEntry() {
            IEnumerable<AcademicTerm> myAcademicTerms = theUniversityRepository.GetAcademicTerms();
            IDictionary<string, string> myDictionary = new Dictionary<string, string>();
            myDictionary.Add(Constants.SELECT, Constants.SELECT);
            foreach (AcademicTerm myAcademicTerm in myAcademicTerms) {
                myDictionary.Add(myAcademicTerm.DisplayName, myAcademicTerm.Id);
            }
            return myDictionary;
        }

        public University GetUniversityById(string aUniversityId) {
            return theUniversityRepository.GetUniversity(aUniversityId);
        }

        public UniversityView GetUniversityProfile(UserInformationModel<User> aUserInformation, string aUniversityId) {
            University myUniversity = theUniversityRepository.GetUniversity(aUniversityId);
            IEnumerable<TextBook> myTextBooks = theTextBookService.GetTextBooksForUniversity(aUniversityId);
            IEnumerable<MarketplaceItem> myMarketplaceItems = theMarketplaceService.GetAllLatestItemsSellingInUniversity(aUniversityId);

            return new UniversityView() {
                University = myUniversity,
                TextBooks = myTextBooks,
                MarketplaceItems = myMarketplaceItems
            };
        }

        public IEnumerable<University> GetValidUniversities() {
            return theUniversityRepository.ValidUniversities();
        }

        public bool IsValidUniversityEmailAddress(string anEmailAddress) {
            string anEmailExtension = EmailHelper.ExtractEmailExtension(anEmailAddress);
            IEnumerable<string> myValidEmails = theUniversityRepository.ValidEmails();
            return myValidEmails.Contains(anEmailExtension);
        }

        public IEnumerable<string> ValidEmails() {
            return theUniversityRepository.ValidEmails();
        }

        public bool IsFromUniversity(User aUser, string aUniversityId) {
            string myEmailExtension = EmailHelper.ExtractEmailExtension(aUser.Email);
            IEnumerable<string> myUniversityEmails = theUniversityRepository.UniversityEmails(aUniversityId);
            return myUniversityEmails.Contains(myEmailExtension);
        }
    }
}