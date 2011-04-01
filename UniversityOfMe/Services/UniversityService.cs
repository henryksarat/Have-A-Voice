using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Helpers;
using UniversityOfMe.Repositories;
using UniversityOfMe.Models;

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

        public bool IsValidUniversityEmailAddress(string anEmailAddress) {
            string anEmailExtension = EmailHelper.ExtractEmailExtension(anEmailAddress);
            IEnumerable<string> myValidEmails = theUniversityRepository.ValidEmails();
            return myValidEmails.Contains(anEmailExtension);
        }

        public IEnumerable<string> ValidEmails() {
            return theUniversityRepository.ValidEmails();
        }
    }
}