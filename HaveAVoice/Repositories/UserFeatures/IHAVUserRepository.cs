using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVUserRepository {
        IEnumerable<UserDetailsModel> GetUserList(User user);
        User CreateUser(User userToCreate);
        void UpdateUser(User userToEdit);
        void DeleteUser(User userToDelete);
        User DeleteUserFromRole(int userId, int roleId);
        bool EmailRegistered(string email);
        bool UsernameRegistered(string username);

        void AddFan(User aUser, int aSourceUserId);
        
        void RemoveUserFromRole(User aUser, Role aRole);
        UserRole AddUserToRole(User user, Role role);
    }
}