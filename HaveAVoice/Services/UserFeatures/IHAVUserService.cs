using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVUserService {
        IEnumerable<UserDetailsModel> GetUserList(User anExcludedUser);
        bool CreateUser(User aUserToCreate, bool aCaptchaValid, bool anAgreement, string anIpAddress);
        bool CreateUserAuthority(User aUserToCreate, string aToken, bool anAgreement, string anIpAddress);
        bool EditUser(EditUserModel aUserToEdit);
        EditUserModel GetUserForEdit(User aUser);
    }
}