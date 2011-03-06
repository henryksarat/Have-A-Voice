using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVAuthorityVerificationRepository {
        bool IsValidEmailWithToken(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        bool TokenForEmailExists(string anEmail, string anAuthorityType, string anAuthorityPosition);
        void CreateTokenForEmail(User aCreatingUser, string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        void SetEmailWithTokenToVerified(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        void UpdateTokenForEmail(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        IEnumerable<UserPosition> GetUserPositions();
    }
}