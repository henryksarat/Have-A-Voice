using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVAuthorityVerificationRepository {
        bool IsValidEmailWithToken(string anEmail, string aToken);
        bool TokenForEmailExists(string anEmail);
        void CreateTokenForEmail(User aCreatingUser, string anEmail, string aToken);
        void SetEmailWithTokenToVerified(string anEmail, string aToken);
        void UpdateTokenForEmail(string anEmail, string aToken);
    }
}