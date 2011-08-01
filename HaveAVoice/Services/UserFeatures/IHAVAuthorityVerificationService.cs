using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using Social.Generic.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVAuthorityVerificationService {
        bool RequestTokenForAuthority(UserInformationModel<User> aRequestingUser, string anEmail, string anExtraInfo, 
            string anAuthorityType, string anAuthorityPosition);
        bool IsValidToken(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        void VerifyAuthority(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        IEnumerable<UserPosition> GetUserPositions();
        void AddExtraInfoToAuthority(User aUser, string anExtraInfo);
    }
}