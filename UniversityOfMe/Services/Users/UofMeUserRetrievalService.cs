using System.Collections.Generic;
using Social.Generic.Constants;
using Social.User.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.UserRepos;

namespace UniversityOfMe.Services.Users {
    public class UofMeUserRetrievalService : UserRetrievalService<User>, IUofMeUserRetrievalService {
        private IUofMeUserRetrievalRepository theUserRetrievalRepository;

        public UofMeUserRetrievalService()
            : this(new EntityUserRetrievalRepository()) { }

        public UofMeUserRetrievalService(IUofMeUserRetrievalRepository aUserRetrievalRepository)
            : base(aUserRetrievalRepository) {
            theUserRetrievalRepository = aUserRetrievalRepository;
        }

        public IEnumerable<User> GetAllFemaleUsers() {
            return theUserRetrievalRepository.GetUsersWithGender(Gender.FEMALE);
        }

        public IEnumerable<User> GetAllMaleUsers() {
            return theUserRetrievalRepository.GetUsersWithGender(Gender.MALE);
        }
    }
}
