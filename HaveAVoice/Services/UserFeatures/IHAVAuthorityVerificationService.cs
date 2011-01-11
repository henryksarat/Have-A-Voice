using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVAuthorityVerificationService {
        void RequestNewTokenForAuthority(User aRequestingUser, string anEmail);
        bool IsValidToken(string anEmail, string aToken);
        void VerifyAuthority(string anEmail, string aToken);
    }
}