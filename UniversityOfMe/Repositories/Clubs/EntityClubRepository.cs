using System;
using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;
using UniversityOfMe.Helpers;
using UniversityOfMe.Repositories.Helpers;
using UniversityOfMe.Helpers.Badges;

namespace UniversityOfMe.Repositories.Clubs {
    public class EntityClubRepository : IClubRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void ActivateClub(User aUser, int aClubId) {
            Club myClub = GetClub(aClubId);
            myClub.DeactivatedByUserId = null;
            myClub.DeativatedDateTimeStamp = null;
            myClub.Active = true;
            theEntities.SaveChanges();
        }

        public void ApproveClubMember(User anAdminUser, int aClubMemberId, string aTitle, bool anAdministrator) {
            ClubMember myClubMember = GetClubMember(aClubMemberId);
            myClubMember.Administrator = anAdministrator;
            myClubMember.Title = aTitle;
            myClubMember.Approved = UOMConstants.APPROVED;
            myClubMember.ApprovedByUserId = anAdminUser.Id;
            myClubMember.ApprovedDateTimeStamp = DateTime.UtcNow;

            BadgeHelper.AddNecessaryBadgesAndPoints(theEntities, myClubMember.ClubMemberUserId, BadgeAction.JOINED, BadgeSection.ORGANIZATION, aClubMemberId);

            theEntities.SaveChanges();
        }

        public void AddMemberToClub(User anAdminUser, int aNewMemberUserId, int aClubId, string aTitle, bool anAdministrator) {
            IEnumerable<ClubBoard> myClubBoards = GetClubBoards(aClubId);

            bool myViewed = true;

            if(myClubBoards.Count<ClubBoard>() > 0) {
                myViewed = false;
            }

            ClubMember myClubMember = ClubMember.CreateClubMember(0, aNewMemberUserId, aClubId, aTitle, anAdministrator, UOMConstants.APPROVED, DateTime.UtcNow, false, myViewed);
            myClubMember.ApprovedByUserId = anAdminUser.Id;

            theEntities.AddToClubMembers(myClubMember);
            theEntities.SaveChanges();
        }

        public Club CreateClub(User aUser, string aUniversityId, string aClubType, string aName, string aDescription) {
            Club myClub = Club.CreateClub(0, aUniversityId, aClubType, aUser.Id, aName, aDescription, DateTime.UtcNow, true);
            theEntities.AddToClubs(myClub);
            theEntities.SaveChanges();
            return myClub;
        }

        public void DeactivateClub(User aUser, int aClubId) {
            Club myClub = GetClub(aClubId);
            myClub.DeactivatedByUserId = aUser.Id;
            myClub.DeativatedDateTimeStamp = DateTime.UtcNow;
            myClub.Active = false;
            theEntities.SaveChanges();
        }

        public void DeleteClub(int aClubId) {
            Club myClub = GetClub(aClubId);
            theEntities.DeleteObject(myClub);
            theEntities.SaveChanges();
        }

        public void DeleteRequestToJoinClub(User aUser, int aClubId) {
            ClubMember myClubMember = GetClubMember(aUser.Id, aClubId);
            theEntities.DeleteObject(myClubMember);
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

        public void DenyClubMember(User anAdminUser, int aClubMemberId) {
            ClubMember myClubMember = GetClubMember(aClubMemberId);
            myClubMember.Approved = UOMConstants.DENIED;
            myClubMember.DeniedByUserId = anAdminUser.Id;
            myClubMember.DeniedByDateTimeStamp = DateTime.UtcNow;
            theEntities.SaveChanges();
        }

        public Club GetClub(User aUser, int aClubId) {
            if (aUser != null) {
                return (from c in theEntities.Clubs
                        join cm in theEntities.ClubMembers on c.Id equals cm.ClubId
                        where c.Id == aClubId
                        && (c.Active || (cm.ClubMemberUserId == aUser.Id && cm.Administrator))
                        select c).FirstOrDefault<Club>();
            } else {
                return (from c in theEntities.Clubs
                        join cm in theEntities.ClubMembers on c.Id equals cm.ClubId
                        where c.Id == aClubId
                        && c.Active
                        select c).FirstOrDefault<Club>();
            }
        }

        public Club GetClub(User aUser, string aUniversityId, string aName) {
            if (aUser != null) {
                return (from c in theEntities.Clubs
                        join cm in theEntities.ClubMembers on c.Id equals cm.ClubId
                        where c.Name == aName
                        && c.UniversityId == aUniversityId
                        && (c.Active || (cm.ClubMemberUserId == aUser.Id && cm.Administrator))
                        select c).FirstOrDefault<Club>();
            } else {
                return (from c in theEntities.Clubs
                        join cm in theEntities.ClubMembers on c.Id equals cm.ClubId
                        where c.Name == aName
                        && c.UniversityId == aUniversityId
                        && c.Active
                        select c).FirstOrDefault<Club>();
            }
        }

        public IEnumerable<ClubBoard> GetClubBoardPostings(int aClubId) {
            return (from cb in theEntities.ClubBoards
                    where cb.ClubId == aClubId
                    select cb).ToList<ClubBoard>();
        }

        public IEnumerable<ClubMember> GetClubMembers(int aClubId) {
            return (from cm in theEntities.ClubMembers
                    where cm.ClubId == aClubId
                    && cm.Approved == UOMConstants.APPROVED
                    select cm).ToList<ClubMember>();
        }

        public ClubMember GetClubMember(int aClubMemberId) {
            return (from cm in theEntities.ClubMembers
                    where cm.Id == aClubMemberId
                    select cm).FirstOrDefault<ClubMember>();
        }

        public ClubMember GetClubMember(int aUserId, int aClubId) {
            return (from cm in theEntities.ClubMembers
                    where cm.ClubMemberUserId == aUserId
                    && cm.ClubId == aClubId
                    && cm.Deleted == false
                    select cm).FirstOrDefault<ClubMember>();
        }

        public IEnumerable<Club> GetClubs(User aUser, string aUniversityId) {
            IEnumerable<int> myAdminOfClubs = (from cm in theEntities.ClubMembers
                                               where cm.ClubMemberUserId == aUser.Id
                                               && cm.Administrator
                                               select cm.ClubId);

            return (from c in theEntities.Clubs
                    where c.UniversityId == aUniversityId
                    && (c.Active || myAdminOfClubs.Contains(c.Id))
                    select c).ToList<Club>();
        }

        public IEnumerable<ClubType> GetClubTypes() {
            return (from ct in theEntities.ClubTypes
                    select ct).ToList<ClubType>();
        }

        public void MarkClubBoardAsViewed(User aUser, int aClubId) {
            ClubMember myClubMember = GetClubMember(aUser.Id, aClubId);
            if (myClubMember != null) {
                myClubMember.BoardViewed = true;
                theEntities.SaveChanges();
            }
        }

        public void MemberRequestToJoinClub(User aRequestingUser, int aClubId, string aTitle) {
            ClubMember myClubMember = ClubMember.CreateClubMember(0, aRequestingUser.Id, aClubId, aTitle, false, UOMConstants.PENDING, DateTime.UtcNow, false, true);
            theEntities.AddToClubMembers(myClubMember);
            theEntities.SaveChanges();
        }

        public void PostToClubBoard(User aPostingUser, int aClubId, string aMessage) {
            ClubBoard myClubBoard = ClubBoard.CreateClubBoard(0, aPostingUser.Id, aClubId, aMessage, DateTime.UtcNow);
            theEntities.AddToClubBoards(myClubBoard);

            IEnumerable<ClubMember> myClubMembers = GetClubMembers(aClubId);

            DateTime myCurrentTime = DateTime.UtcNow;

            foreach (ClubMember myClubMember in myClubMembers) {
                if (myClubMember.ClubMemberUserId == aPostingUser.Id) {
                    myClubMember.BoardViewed = true;
                } else {
                    myClubMember.BoardViewed = false;
                    myClubMember.LastBoardPost = myCurrentTime;
                }
            }

            theEntities.SaveChanges();
        }

        public void UpdateClub(Club aClub) {
            theEntities.ApplyCurrentValues(aClub.EntityKey.EntitySetName, aClub);
            theEntities.SaveChanges();
        }

        private Club GetClub(int aClubId) {
            return (from c in theEntities.Clubs
                    where c.Id == aClubId
                    select c).FirstOrDefault<Club>();
        }

        private IEnumerable<ClubBoard> GetClubBoards(int aClubId) {
            return (from cb in theEntities.ClubBoards
                    where cb.ClubId == aClubId
                    select cb);
        }
    }
}