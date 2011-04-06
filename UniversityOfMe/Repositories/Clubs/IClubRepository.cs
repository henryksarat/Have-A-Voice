using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Clubs {
    public interface IClubRepository {
        void AddMemberToClub(User anAdminUser, int aNewMemberUserId, int aClubId, string aTitle, bool anAdministrator);
        void DeleteClub(int aClubId);
        void DeleteUserFromClub(User aDeletingUser, int aFormerMemberUserId, int aClubId);
        Club GetClub(int aClubId);
        IEnumerable<ClubType> GetClubTypes();
        IEnumerable<ClubBoard> GetClubBoardPostings(int aClubId);
        ClubMember GetClubMember(int aUserId, int aClubId);
        IEnumerable<ClubMember> GetClubMembers(int aClubId);
        IEnumerable<Club> GetClubs(string aUniversityId);
        Club CreateClub(User aUser, string aUniversityId, string aClubType, string aName, string aDescription);
        void PostToClubBoard(User aPostingUser, int aClubId, string aMessage);
        void UpdateClub(Club aClub);
    }
}
