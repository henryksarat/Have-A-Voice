using System.Collections.Generic;
using HaveAVoice.Models;
using Social.User.Repositories;
namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVUserRepository : IUserRepository<User, Role, UserRole> {
        bool IsUserNameTaken(string aUsername);
    }
}