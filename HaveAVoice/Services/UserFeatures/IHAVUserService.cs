using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVUserService {
        User GetUser(int userId);
        IEnumerable<UserDetailsModel> GetUserList(User anExcludedUser);
        bool CreateUser(User aUserToCreate, bool aCaptchaValid, bool anAgreement, string anIpAddress);
        bool EditUser(EditUserModel aUserToEdit);
        EditUserModel GetUserForEdit(User aUser);

        void AddFan(User aUser, int aSourceUserId);
    }
}