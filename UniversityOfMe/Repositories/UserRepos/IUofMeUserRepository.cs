using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.UserRepos {
    public interface IUofMeUserRepository : Social.User.Repositories.IUserRepository<User, Role, UserRole> {
        IEnumerable<User> GetNewestUserFromUniversity(string aUniversity, int aLimit);
    }
}
