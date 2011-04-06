using System;
using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;

namespace UniversityOfMe.Repositories.Clubs {
    public class EntityClubRepository : IClubRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void AddMemberToClub(User anAdminUser, int aNewMemberUserId, int aClubId, string aTitle, bool anAdministrator) {
            ClubMember myClubMember = ClubMember.CreateClubMember(0, anAdminUser.Id, aNewMemberUserId, aClubId, aTitle, anAdministrator, DateTime.UtcNow, false);
            theEntities.AddToClubMembers(myClubMember);
            theEntities.SaveChanges();
        }

        public Club CreateClub(User aUser, string aUniversityId, string aClubType, string aName, string aDescription) {
            Club myClub = Club.CreateClub(0, aUniversityId, aClubType, aUser.Id, aName, aDescription, DateTime.UtcNow);
            theEntities.AddToClubs(myClub);
            theEntities.SaveChanges();
            return myClub;
        }

        public void DeleteClub(int aClubId) {
            Club myClub = GetClub(aClubId);
            theEntities.DeleteObject(myClub);
            theEntities.SaveChanges();
        }

        public void DeleteUserFromClub(User aDeletingUser, int aFormerMemberUserId, int aClubId) {
            ClubMember myClubMember = GetClubMember(aFormerMemberUserId, aClubId);
            myClubMember.DeletedByUserId = aDeletingUser.Id;
            myClubMember.DeletedByDateTimeStamp = DateTime.UtcNow;
            myClubMember.Deleted = true;
            theEntities.ApplyCurrentValues(myClubMember.EntityKey.EntitySetName, myClubMember);
            theEntities.SaveChanges();
        }

        public Club GetClub(int aClubId) {
            return (from c in theEntities.Clubs
                    where c.Id == aClubId
                    select c).FirstOrDefault<Club>();
        }

        public IEnumerable<ClubBoard> GetClubBoardPostings(int aClubId) {
            return (from cb in theEntities.ClubBoards
                    where cb.ClubId == aClubId
                    select cb).ToList<ClubBoard>();
        }

        public IEnumerable<ClubMember> GetClubMembers(int aClubId) {
            return (from cm in theEntities.ClubMembers
                    where cm.ClubId == aClubId
                    select cm).ToList<ClubMember>();
        }

        public ClubMember GetClubMember(int aUserId, int aClubId) {
            return (from cm in theEntities.ClubMembers
                    where cm.ClubMemberUserId == aUserId
                    && cm.ClubId == aClubId
                    && cm.Deleted == false
                    select cm).FirstOrDefault<ClubMember>();
        }

        public IEnumerable<Club> GetClubs(string aUniversityId) {
            return (from c in theEntities.Clubs
                    where c.UniversityId == aUniversityId
                    select c).ToList<Club>();
        }

        public IEnumerable<ClubType> GetClubTypes() {
            return (from ct in theEntities.ClubTypes
                    select ct).ToList<ClubType>();
        }

        public void PostToClubBoard(User aPostingUser, int aClubId, string aMessage) {
            ClubBoard myClubBoard = ClubBoard.CreateClubBoard(0, aPostingUser.Id, aClubId, aMessage, DateTime.UtcNow);
            theEntities.AddToClubBoards(myClubBoard);
            theEntities.SaveChanges();
        }

        public void UpdateClub(Club aClub) {
            theEntities.ApplyCurrentValues(aClub.EntityKey.EntitySetName, aClub);
            theEntities.SaveChanges();
        }
    }
}