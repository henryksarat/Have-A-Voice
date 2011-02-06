using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVAuthorityVerificationRepository {
        bool IsValidEmailWithToken(string anEmail, string aToken, string anAuthorityType);
        bool TokenForEmailExists(string anEmail, string anAuthorityType);
        void CreateTokenForEmail(User aCreatingUser, string anEmail, string aToken, string anAuthorityType);
        void SetEmailWithTokenToVerified(string anEmail, string aToken, string anAuthorityType);
        void UpdateTokenForEmail(string anEmail, string aToken, string anAuthorityType);
    }
}