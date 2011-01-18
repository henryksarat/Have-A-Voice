using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVAuthorityVerificationService {
        bool RequestTokenForAuthority(UserInformationModel aRequestingUser, string anEmail);
        bool IsValidToken(string anEmail, string aToken);
        void VerifyAuthority(string anEmail, string aToken);
    }
}