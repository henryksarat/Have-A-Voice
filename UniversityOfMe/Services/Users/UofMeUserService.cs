using System.Collections.Generic;
using Social.Email;
using Social.User.Services;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.UserRepos;

namespace UniversityOfMe.Services.Users {
    public class UofMeUserService : UserService<User, Role, UserRole>, IUofMeUserService {
        private IUofMeUserRepository theUserRepository;

        public UofMeUserService(IValidationDictionary aValidationDictionary)
            : base(aValidationDictionary, new EntityUserRepository(), new SocialEmail()) {
            theUserRepository = new EntityUserRepository();
        }
        
        public IEnumerable<User> GetNewestUsers(string aUniversityId, int aLimit) {
            return theUserRepository.GetNewestUserFromUniversity(aUniversityId, aLimit);
        }
    }
}
