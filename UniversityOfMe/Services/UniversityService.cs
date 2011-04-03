using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Helpers;
using UniversityOfMe.Repositories;
using UniversityOfMe.Models;
using Social.Generic.Constants;

namespace UniversityOfMe.Services {
    public class UniversityService : IUniversityService {
        IUniversityRepository theUniversityRepository;

        public UniversityService(): this(new EntityUniversityRepository()) { }

        public UniversityService(IUniversityRepository aUniversityRepository) {
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