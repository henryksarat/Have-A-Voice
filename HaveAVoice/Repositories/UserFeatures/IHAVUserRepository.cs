using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using Social.User;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVUserRepository : IUserRepository<User> {
        void UpdateUser(User userToEdit);
        User DeleteUserFromRole(int userId, int roleId);
        void RemoveUserFromRole(User aUser, Role aRole);
        UserRole AddUserToRole(User user, Role role);
    }
}