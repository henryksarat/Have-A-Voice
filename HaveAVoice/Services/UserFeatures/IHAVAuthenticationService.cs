using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View;
using HaveAVoice.Models;
using Social.Generic.Models;
using Social.Authentication.Services;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVAuthenticationService : IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting> {
        void ActivateAuthority(string anActivationCode, string anAuthorityType);
    }
}