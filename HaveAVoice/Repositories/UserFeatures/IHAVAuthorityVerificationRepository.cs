using System.Collections.Generic;
using HaveAVoice.Models;
using Social.Generic;
using HaveAVoice.Helpers.Authority;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVAuthorityVerificationRepository {
        void AddZipCodesForUser(User anAdminUser, string anEmail, IEnumerable<int> aZipCodes);
        IEnumerable<AuthorityViewableZipCode> GetAuthorityViewableZipCodes(string anEmail);
        IEnumerable<RegionSpecific> GetClashingRegions(string aCity, string aState, int aZip);
        UserRegionSpecific GetRegionSpecifcInformationForUser(User aUser, UserPosition aUserPosition);
        bool IsValidEmailWithToken(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        bool TokenForEmailExists(string anEmail, string anAuthorityType, string anAuthorityPosition);
        void CreateTokenForEmail(User aCreatingUser, string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        void SetEmailWithTokenToVerified(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        void UpdateTokenForEmail(string anEmail, string aToken, string anAuthorityType, string anAuthorityPosition);
        IEnumerable<UserPosition> GetUserPositions();
        void SetExtraInfoForAuthority(User aUser, string anExtraInfo);
        void UpdateUserRegionSpecifics(User aUser, IEnumerable<Pair<AuthorityPosition, int>> aToAddAndUpdate, IEnumerable<Pair<AuthorityPosition, int>> aToRemove);
    }
}