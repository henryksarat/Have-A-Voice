using System.Collections.Generic;
using Social.User.Repositories;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.UserRepos {
    public interface IUofMeUserRetrievalRepository : IUserRetrievalRepository<User> {
        IEnumerable<User> GetUsersWithGender(string aGender);
    }
}
