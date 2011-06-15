using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Clubs {
    public interface IClubRepository {
        void ActivateClub(User aUser, int aClubId);
        void ApproveClubMember(User anAdminUser, int aClubMemberId, string aTitle, bool anAdministrator);
        void AddMemberToClub(User anAdminUser, int aNewMemberUserId, int aClubId, string aTitle, bool anAdministrator);
        void DeleteRequestToJoinClub(User aUser, int aClubId);
        void DeleteClub(int aClubId);
        void DeactivateClub(User aUser, int aClubId);
        void DeleteUserFromClub(User aDeletingUser, int aFormerMemberUserId, int aClubId);
        void DenyClubMember(User anAdminUser, int aClubMemberId);
        Club GetClub(User aUser, int aClubId);
        IEnumerable<ClubType> GetClubTypes();
        IEnumerable<ClubBoard> GetClubBoardPostings(int aClubId);
        ClubMember GetClubMember(int aClubMemberId);
        ClubMember GetClubMember(int aUserId, int aClubId);
        IEnumerable<ClubMember> GetClubMembers(int aClubId);
        IEnumerable<Club> GetClubs(User aUser, string aUniversityId);
        Club CreateClub(User aUser, string aUniversityId, string aClubType, string aName, string aDescription);
        void MemberRequestToJoinClub(User aRequestingUser, int aClubId, string aTitle);
        void PostToClubBoard(User aPostingUser, int aClubId, string aMessage);
        void UpdateClub(Club aClub);
    }
}
