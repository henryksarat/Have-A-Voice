using System.Collections.Generic;
using HaveAVoice.Models;
using Social.Generic.Models;
using HaveAVoice.Helpers.Authority;
using Social.Generic;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVAuthorityVerificationService {
        bool AddZipCodesForUser(UserInformationModel<User> anAdminUser, string anEmail, string aZipCodes);
        IEnumerable<AuthorityViewableZipCode> GetAuthorityViewableZipCodes(UserInformationModel<User> anAdminUser, string anEmail);
        IEnumerable<RegionSpecific> GetClashingRegions(string aCity, string aState, int aZip);
        UserRegionSpecific GetRegionSpecifcInformationForUser(User aUser, UserPosition aUserPosition);
        bool RequestTokenForAuthority(UserInformationModel<User> aRequestingUser, string anEmail, string anExtraInfo, 
            string anAuthorityType, string anAuthorityPosition);
        bool IsValidToken(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        void VerifyAuthority(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        IEnumerable<UserPosition> GetUserPositions();
        void AddExtraInfoToAuthority(User aUser, string anExtraInfo);
        void UpdateUserRegionSpecifics(UserInformationModel<User> aUserInfo, IEnumerable<Pair<AuthorityPosition, int>> aToAddAndUpdate, IEnumerable<Pair<AuthorityPosition, int>> aToRemove);
    }
}