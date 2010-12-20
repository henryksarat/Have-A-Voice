using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVAuthenticationService {
        UserInformationModel ActivateNewUser(string activationCode);
        UserInformationModel AuthenticateUser(string anEmail, string aPassword);
        void CreateRememberMeCredentials(User aUserModel);
        User ReadRememberMeCredentials();
    }
}