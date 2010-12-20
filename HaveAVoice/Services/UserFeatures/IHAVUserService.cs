using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVUserService {
        User GetUser(int userId);
        bool CreateUser(User aUserToCreate, bool aCaptchaValid, bool anAgreement, string anIpAddress);
        UserInformationModel AuthenticateUser(string anEmail, string aPassword);
        void CreateRememberMeCredentials(User aUserModel);
        User ReadRememberMeCredentials();
        UserInformationModel ActivateNewUser(string anActivationCode);
        IEnumerable<UserDetailsModel> GetUserList(User anExcludedUser);
        IEnumerable<Timezone> GetTimezones();
        bool EditUser(EditUserModel aUserToEdit);
        EditUserModel GetUserForEdit(User aUser);

        UserPrivacySetting GetUserPrivacySettings(User aUser);
        void UpdatePrivacySettings(User aUser, UserPrivacySetting aUserPrivacySetting);

        UserPicture GetProfilePicture(int aUserId);
        IEnumerable<UserPicture> GetUserPictures(int aUserId);
        UserPicture GetUserPicture(int aUserPictureId);
        void SetToProfilePicture(int aUserPictureId, User aUser);
        void DeleteUserPictures(List<int> aUserPictureIds);

        IEnumerable<Permission> GetPermissionsForUser(User aUser);

        bool ForgotPasswordRequest(string anEmail);
        bool ForgotPasswordProcess(string anEmail, string aForgotPasswordHash);
        bool ChangePassword(string anEmail, string aForgotPasswordHash, string aPassword, string aRetypedPassword);

        void AddFan(User aUser, int aSourceUserId);
    }
}