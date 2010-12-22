using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVUserService {
        User GetUser(int userId);
        bool CreateUser(User aUserToCreate, bool aCaptchaValid, bool anAgreement, string anIpAddress);
        IEnumerable<UserDetailsModel> GetUserList(User anExcludedUser);
        IEnumerable<Timezone> GetTimezones();
        bool EditUser(EditUserModel aUserToEdit);
        EditUserModel GetUserForEdit(User aUser);

        void AddFan(User aUser, int aSourceUserId);
    }
}