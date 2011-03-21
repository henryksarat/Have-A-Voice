using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View;
using HaveAVoice.Models;
using Social.Generic.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVAuthenticationService {
        void ActivateNewUser(string activationCode);
        void ActivateAuthority(string anActivationCode, string anAuthorityType);
        UserInformationModel<User> RefreshUserInformationModel(UserInformationModel<User> aUserInformationModel);
        UserInformationModel<User> AuthenticateUser(string anEmail, string aPassword);
        UserInformationModel<User> CreateUserInformationModel(User aUser);
        void CreateRememberMeCredentials(User aUserModel);
        User ReadRememberMeCredentials();
    }
}