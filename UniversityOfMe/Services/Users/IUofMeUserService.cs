using System.Collections.Generic;
using Social.User.Services;
using UniversityOfMe.Models;

namespace UniversityOfMe.Services.Users {
    public interface IUofMeUserService : IUserService<User, Role, UserRole> {
        IEnumerable<User> GetNewestUsers(User aRequestingUser, string aUniversityId, int aLimit);
    }
}
