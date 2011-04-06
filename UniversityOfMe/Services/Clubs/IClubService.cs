using System.Collections.Generic;
using System.Web;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Services.Professors {
    public interface IClubService {
        bool AddMemberToClub(UserInformationModel<User> anAdminUserAdding, int aClubMemberUserId, int aClubId, string aTitle, bool anAdministrator);
        bool CreateClub(UserInformationModel<User> aUser, CreateClubModel aCreateClubModel);
        IDictionary<string, string> CreateAllClubTypesDictionaryEntry();
        Club GetClub(int aClubId);
        IEnumerable<ClubBoard> GetClubBoardPostings(int aClubId);
        IEnumerable<ClubMember> GetClubMembers(int aClubId);
        IEnumerable<Club> GetClubs(string aUniversityId);
        bool IsAdmin(User aUser, int aClubId);
        bool PostToClubBoard(UserInformationModel<User> aPostingUser, int aClubId, string aMessage);
        bool RemoveClubMember(UserInformationModel<User> aClubAdmin, int aCurrentUserId, int aClubId);
    }
}