using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Helpers;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Groups {
    public class EntityGroupRepository : IGroupRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void ActivateGroup(User aUser, int aGroupId) {
            Group myClub = GetClub(aGroupId);
            myClub.DeactivatedByUserId = null;
            myClub.DeactivatedDateTimeStamp = null;
            myClub.Active = true;
            theEntities.SaveChanges();
        }

        public void ApproveGroupMember(User anAdminUser, int aGroupMemberId, string aTitle, bool anAdministrator) {
            GroupMember myGroupMember = GetGroupMember(aGroupMemberId);
            myGroupMember.Administrator = anAdministrator;
            myGroupMember.Title = aTitle;
            myGroupMember.Approved = HAVConstants.APPROVED;
            myGroupMember.ApprovedByUserId = anAdminUser.Id;
            myGroupMember.ApprovedDateTimeStamp = DateTime.UtcNow;
            theEntities.SaveChanges();
        }

        public void AddMemberToGroup(User anAdminUser, int aNewMemberUserId, int aGroupId, string aTitle, bool anAdministrator) {
            IEnumerable<GroupBoard> myClubBoards = GetGroupBoards(aGroupId);

            bool myViewed = true;

            if(myClubBoards.Count<GroupBoard>() > 0) {
                myViewed = false;
            }

            GroupMember myGroupMember = GroupMember.CreateGroupMember(0, aNewMemberUserId, aGroupId, aTitle, anAdministrator, HAVConstants.APPROVED, DateTime.UtcNow, false, myViewed);
            myGroupMember.ApprovedByUserId = anAdminUser.Id;

            theEntities.AddToGroupMembers(myGroupMember);
            theEntities.SaveChanges();
        }

        public Group CreateGroup(User aUser, string aName, string aDescription, bool anAutoAccept) {
            Group myGroup = Group.CreateGroup(0, aUser.Id, aName, aDescription, DateTime.UtcNow, true, anAutoAccept);
            theEntities.AddToGroups(myGroup);
            theEntities.SaveChanges();
            return myGroup;
        }

        public void DeactivateClub(User aUser, int aClubId) {
            Group myClub = GetClub(aClubId);
            myClub.DeactivatedByUserId = aUser.Id;
            myClub.DeactivatedDateTimeStamp = DateTime.UtcNow;
            myClub.Active = false;
            theEntities.SaveChanges();
        }

        public void DeleteGroup(int aGroupId) {
            Group myClub = GetClub(aGroupId);
            theEntities.DeleteObject(myClub);
            theEntities.SaveChanges();
        }

        public void DeleteRequestToJoinGroup(User aUser, int aGroupId) {
            GroupMember myGroupMember = GetGroupMember(aUser.Id, aGroupId);
            theEntities.DeleteObject(myGroupMember);
            theEntities.SaveChanges();
        }

        public void DeleteUserFromGroup(User aDeletingUser, int aFormerMemberUserId, int aGroupId) {
            GroupMember myGroupMember = GetGroupMember(aFormerMemberUserId, aGroupId);
            myGroupMember.DeletedByUserId = aDeletingUser.Id;
            myGroupMember.DeletedByDateTimeStamp = DateTime.UtcNow;
            myGroupMember.Deleted = true;
            theEntities.ApplyCurrentValues(myGroupMember.EntityKey.EntitySetName, myGroupMember);
            theEntities.SaveChanges();
        }

        public void DenyClubMember(User anAdminUser, int aClubMemberId) {
            GroupMember myClubMember = GetGroupMember(aClubMemberId);
            myClubMember.Approved = HAVConstants.DENIED;
            myClubMember.DeniedByUserId = anAdminUser.Id;
            myClubMember.DeniedByDateTimeStamp = DateTime.UtcNow;
            theEntities.SaveChanges();
        }

        public Group GetGroup(User aUser, int aClubId) {
            return (from c in theEntities.Groups
                    join cm in theEntities.GroupMembers on c.Id equals cm.GroupId
                    where c.Id == aClubId
                    && (c.Active || (cm.UserId == aUser.Id && cm.Administrator))
                    select c).FirstOrDefault<Group>();
        }

        public IEnumerable<GroupBoard> GetGroupBoardPostings(int aGroupId) {
            return (from cb in theEntities.GroupBoards
                    where cb.GroupId == aGroupId
                    select cb).ToList<GroupBoard>();
        }

        public IEnumerable<GroupMember> GetClubMembers(int aClubId) {
            return (from cm in theEntities.GroupMembers
                    where cm.GroupId == aClubId
                    && cm.Approved == HAVConstants.APPROVED
                    select cm).ToList<GroupMember>();
        }

        public GroupMember GetGroupMember(int aGroupMemberId) {
            return (from cm in theEntities.GroupMembers
                    where cm.Id == aGroupMemberId
                    select cm).FirstOrDefault<GroupMember>();
        }

        public GroupMember GetGroupMember(int aUserId, int aGroupId) {
            return (from cm in theEntities.GroupMembers
                    where cm.UserId == aUserId
                    && cm.GroupId == aGroupId
                    && cm.Deleted == false
                    select cm).FirstOrDefault<GroupMember>();
        }

        public IEnumerable<Group> GetClubs(User aUser) {
            IEnumerable<int> myAdminOfClubs = (from cm in theEntities.GroupMembers
                                               where cm.UserId == aUser.Id
                                               && cm.Administrator
                                               select cm.GroupId);

            return (from c in theEntities.Groups
                    where (c.Active || myAdminOfClubs.Contains(c.Id))
                    select c).ToList<Group>();
        }

        public void MarkGroupBoardAsViewed(User aUser, int aGroupId) {
            GroupMember myClubMember = GetGroupMember(aUser.Id, aGroupId);
            if (myClubMember != null) {
                myClubMember.BoardViewed = true;
                theEntities.SaveChanges();
            }
        }

        public void MemberRequestToJoinClub(User aRequestingUser, int aGroupId, string aTitle) {
            GroupMember myGroupMember = GroupMember.CreateGroupMember(0, aRequestingUser.Id, aGroupId, aTitle, false, HAVConstants.PENDING, DateTime.UtcNow, false, true);
            theEntities.AddToGroupMembers(myGroupMember);
            theEntities.SaveChanges();
        }

        public void PostToGroupBoard(User aPostingUser, int aGroupId, string aMessage) {
            GroupBoard myGroupBoard = GroupBoard.CreateGroupBoard(0, aPostingUser.Id, aGroupId, aMessage, DateTime.UtcNow);
            theEntities.AddToGroupBoards(myGroupBoard);

            IEnumerable<GroupMember> myClubMembers = GetClubMembers(aGroupId);

            DateTime myCurrentTime = DateTime.UtcNow;

            foreach (GroupMember myClubMember in myClubMembers) {
                if (myClubMember.UserId == aPostingUser.Id) {
                    myClubMember.BoardViewed = true;
                } else {
                    myClubMember.BoardViewed = false;
                    myClubMember.LastBoardPost = myCurrentTime;
                }
            }

            theEntities.SaveChanges();
        }

        public void UpdateClub(Group aGroup) {
            theEntities.ApplyCurrentValues(aGroup.EntityKey.EntitySetName, aGroup);
            theEntities.SaveChanges();
        }

        private Group GetClub(int aClubId) {
            return (from c in theEntities.Groups
                    where c.Id == aClubId
                    select c).FirstOrDefault<Group>();
        }

        private IEnumerable<GroupBoard> GetGroupBoards(int aGroupId) {
            return (from cb in theEntities.GroupBoards
                    where cb.GroupId == aGroupId
                    select cb);
        }
    }
}