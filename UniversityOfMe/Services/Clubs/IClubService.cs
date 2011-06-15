using System.Collections.Generic;
using System.Web;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Services.Professors {
    public interface IClubService {
        bool ActivateClub(UserInformationModel<User> aUser, int aClubId);
        bool ApproveClubMember(UserInformationModel<User> aUser, int aClubMemberId, string aTitle, bool anAdministrator);
        void CancelRequestToJoin(UserInformationModel<User> aUser, int aClubId);
        bool CreateClub(UserInformationModel<User> aUser, CreateClubModel aCreateClubModel);
        IDictionary<string, string> CreateAllClubTypesDictionaryEntry();
        void DenyClubMember(UserInformationModel<User> aUser, int aClubMemberId);
        bool DeactivateClub(UserInformationModel<User> aUser, int aClubId);
        Club GetClub(UserInformationModel<User> aUser, int aClubId);
        IEnumerable<ClubBoard> GetClubBoardPostings(int aClubId);
        ClubMember GetClubMember(UserInformationModel<User> aUser, int aClubMemberId);
        IEnumerable<ClubMember> GetActiveClubMembers(int aClubId);
        IEnumerable<Club> GetClubs(UserInformationModel<User> aUser, string aUniversityId);
        bool IsAdmin(User aUser, int aClubId);
        bool IsApartOfClub(int aUserId, int aClubId);
        bool IsPendingApproval(int aUserId, int aClubId);
        bool PostToClubBoard(UserInformationModel<User> aPostingUser, int aClubId, string aMessage);
        bool RemoveClubMember(UserInformationModel<User> aClubAdmin, int aCurrentUserId, int aClubId);
        void RequestToJoinClub(UserInformationModel<User> aRequestingMember, int aClubId);
    }
}