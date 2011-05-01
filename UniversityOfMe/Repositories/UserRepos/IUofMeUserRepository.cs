using System.Collections.Generic;
using Social.User.Repositories;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.UserRepos {
    public interface IUofMeUserRepository : IUserRepository<User, Role, UserRole> {
        IEnumerable<User> GetNewestUsersFromUniversity(User aRequestingUser, string aUniversity, int aLimit);
        void UpdateUserEmailAndUniversities(User aUser);
    }
}
