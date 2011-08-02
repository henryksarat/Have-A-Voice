using System.Collections.Generic;
using HaveAVoice.Models;
using Social.Generic.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVAuthorityVerificationService {
        bool AddZipCodesForUser(UserInformationModel<User> anAdminUser, string anEmail, string aZipCodes);
        IEnumerable<AuthorityViewableZipCode> GetAuthorityViewableZipCodes(UserInformationModel<User> anAdminUser, string anEmail);
        bool RequestTokenForAuthority(UserInformationModel<User> aRequestingUser, string anEmail, string anExtraInfo, 
            string anAuthorityType, string anAuthorityPosition);
        bool IsValidToken(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        void VerifyAuthority(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        IEnumerable<UserPosition> GetUserPositions();
        void AddExtraInfoToAuthority(User aUser, string anExtraInfo);
    }
}