using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVUserRepository {
        User CreateUser(User userToCreate);
        User GetUser(int id);
        void UpdateUser(User userToEdit);
        User GetUser(string email, string password);
        User GetUser(string email);
        User GetUserFromCookieHash(int userId, string cookieHash);
        User UpdateCookieHashCreationDate(User user);
        void DeleteUser(User userToDelete);
        User DeleteUserFromRole(int userId, int roleId);
        bool EmailRegistered(string email);
        bool UsernameRegistered(string username);
        IEnumerable<UserDetailsModel> GetUserList(User user);
        IEnumerable<Timezone> GetTimezones();

        void AddProfilePicture(string anImageURL, User aUser);
        void SetToProfilePicture(int aUserPictureId, User aUser);
        UserPicture GetProfilePicture(int aUserId);
        IEnumerable<UserPicture> GetUserPictures(int aUserId);
        UserPicture GetUserPicture(int userPictureId);
        void DeleteUserPicture(int aUserPictureId);

        IEnumerable<Permission> GetPermissionsForUser(User aUser);
        Restriction GetRestrictionsForUser(User aUser);

        UserPrivacySetting GetUserPrivacySettingsForUser(User aUser);
        void UpdatePrivacySettings(User aUser, UserPrivacySetting aUserPrivacySetting);


        void UpdateUserForgotPasswordHash(string anEmail, string aHashCode);
        User GetUserByEmailAndForgotPasswordHash(string anEmail, string aHashCode);
        void ChangePassword(int aUserId, string aPassword);

        void AddFan(User aUser, int aSourceUserId);
        User FindUserByActivationCode(string activationCode);
        void RemoveUserFromRole(User aUser, Role aRole);
        UserRole AddUserToRole(User user, Role role);
        void AddDefaultUserPrivacySettings(User aUser);
    }
}