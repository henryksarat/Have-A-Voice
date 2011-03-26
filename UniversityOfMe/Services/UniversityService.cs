using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Helpers;
using UniversityOfMe.Repositories;

namespace UniversityOfMe.Services {
    public class UniversityService : IUniversityService {
        IUniversityRepository theUniversityRepository;

        public UniversityService(): this(new UniversityRepository()) { }

        public UniversityService(IUniversityRepository aUniversityRepository) {
            theUniversityRepository = aUniversityRepository;
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