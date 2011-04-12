using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVUserService {
        bool CreateUserAuthority(User aUserToCreate, string aToken, string anAuthorityType, bool anAgreement, string anIpAddress);
        bool EditUser(EditUserModel aUserToEdit, string aHashedPassword);
        EditUserModel GetUserForEdit(User aUser);
    }
}