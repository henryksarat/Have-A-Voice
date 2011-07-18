using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Helpers;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Groups {
    public class EntityGroupRepository : IGroupRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void ActivateGroup(User aUser, int aGroupId) {
            Group myClub = GetGroup(aGroupId);
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

        public void AutoAcceptGroupMember(User aUser, int aGroupId, string aTitle) {
            GroupMember myGroupMember = GroupMember.CreateGroupMember(0, aUser.Id, aGroupId, aTitle, false, HAVConstants.APPROVED, DateTime.UtcNow, false, false);
            theEntities.AddToGroupMembers(myGroupMember);
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

        public void AddTagsForGroup(User aUser, int aGroupId, IEnumerable<int> aZipCodeTags, IEnumerable<string> aKeywordTags, string aCityTag, string aStateTag) {
            foreach (int myZipCode in aZipCodeTags) {
                GroupZipCodeTag myZipCodeTag = GroupZipCodeTag.CreateGroupZipCodeTag(0, aUser.Id, aGroupId, myZipCode);
                theEntities.AddToGroupZipCodeTags(myZipCodeTag);
            }

            foreach (string myKeyword in aKeywordTags) {
                GroupTag myTag = GroupTag.CreateGroupTag(0, aUser.Id, aGroupId, myKeyword);
                theEntities.AddToGroupTags(myTag);
            }

            if (!string.IsNullOrEmpty(aCityTag) && !string.IsNullOrEmpty(aStateTag)) {
                GroupCityStateTag myCityStateTag = GroupCityStateTag.CreateGroupCityStateTag(0, aUser.Id, aGroupId, aCityTag, aStateTag);
                theEntities.AddToGroupCityStateTags(myCityStateTag);
            }

            theEntities.SaveChanges();
        }

        public Group CreateGroup(User aUser, string aName, string aDescription, bool anAutoAccept) {
            Group myGroup = Group.CreateGroup(0, aUser.Id, aName, aDescription, DateTime.UtcNow, true, anAutoAccept);
            theEntities.AddToGroups(myGroup);
            theEntities.SaveChanges();
            return myGroup;
        }

        public void DeactivateGroup(User aUser, int aGroupId) {
            Group myClub = GetGroup(aGroupId);
            myClub.DeactivatedByUserId = aUser.Id;
            myClub.DeactivatedDateTimeStamp = DateTime.UtcNow;
            myClub.Active = false;
            theEntities.SaveChanges();
        }

        public void DeleteGroup(int aGroupId) {
            Group myGroup = GetGroup(aGroupId);
            theEntities.DeleteObject(myGroup);
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

        public void DenyGroupMember(User anAdminUser, int aGroupMemberId) {
            GroupMember myClubMember = GetGroupMember(aGroupMemberId);
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

        public IEnumerable<GroupMember> GetGroupMembers(int aGroupId) {
            return (from cm in theEntities.GroupMembers
                    where cm.GroupId == aGroupId
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

        public IEnumerable<Group> GetGroups(User aUser) {
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

        public void MemberRequestToJoinGroup(User aRequestingUser, int aGroupId, string aTitle) {
            GroupMember myGroupMember = GroupMember.CreateGroupMember(0, aRequestingUser.Id, aGroupId, aTitle, false, HAVConstants.PENDING, DateTime.UtcNow, false, true);
            theEntities.AddToGroupMembers(myGroupMember);
            theEntities.SaveChanges();
        }

        public void PostToGroupBoard(User aPostingUser, int aGroupId, string aMessage) {
            GroupBoard myGroupBoard = GroupBoard.CreateGroupBoard(0, aPostingUser.Id, aGroupId, aMessage, DateTime.UtcNow);
            theEntities.AddToGroupBoards(myGroupBoard);

            IEnumerable<GroupMember> myClubMembers = GetGroupMembers(aGroupId);

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

        public void RefreshConnection() {
            theEntities = new HaveAVoiceEntities();
        }

        public void UpdateGroup(Group aGroup) {
            theEntities.ApplyCurrentValues(aGroup.EntityKey.EntitySetName, aGroup);
            theEntities.SaveChanges();
        }

        public void UpdateTagsForGroup(User aUser, int aGroupId, IEnumerable<int> aZipCodesToAdd, List<int> aZipCodesToDelete, 
            IEnumerable<string> aKeywordsTagsToAdd, List<string> aKeywordsTagsToDelete, 
            bool aDeleteCityStateTag, bool aNewCityStatetag, string aCityTag, string aStateTag) {

            foreach (int myZipCode in aZipCodesToAdd) {
                GroupZipCodeTag myZipCodeTag = GroupZipCodeTag.CreateGroupZipCodeTag(0, aUser.Id, aGroupId, myZipCode);
                theEntities.AddToGroupZipCodeTags(myZipCodeTag);
            }

            foreach (int myZipCode in aZipCodesToDelete) {
                GroupZipCodeTag myZipCodeTag = GetGroupZipCodeTag(aGroupId, myZipCode);
                if (myZipCodeTag != null) {
                    theEntities.DeleteObject(myZipCodeTag);
                }
            }

            foreach (string myKeyword in aKeywordsTagsToAdd) {
                GroupTag myKeywordTag = GroupTag.CreateGroupTag(0, aUser.Id, aGroupId, myKeyword);
                theEntities.AddToGroupTags(myKeywordTag);
            }

            foreach (string myKeyword in aKeywordsTagsToDelete) {
                GroupTag myKeywordTag = GetGroupKeywordTag(aGroupId, myKeyword);
                if (myKeywordTag != null) {
                    theEntities.DeleteObject(myKeywordTag);
                }
            }

            if (aDeleteCityStateTag) {
                GroupCityStateTag myGroupCityStateTag = GetGroupCityStateTag(aGroupId);
                theEntities.DeleteObject(myGroupCityStateTag);
            }

            if (aNewCityStatetag) {
                GroupCityStateTag myGroupCityStatetag = GroupCityStateTag.CreateGroupCityStateTag(0, aUser.Id, aGroupId, aCityTag, aStateTag);
                theEntities.AddToGroupCityStateTags(myGroupCityStatetag);
            }

            theEntities.SaveChanges();

        }

        private Group GetGroup(int aGroupId) {
            return (from c in theEntities.Groups
                    where c.Id == aGroupId
                    select c).FirstOrDefault<Group>();
        }

        public GroupCityStateTag GetGroupCityStateTag(int aGroupId) {
            return (from c in theEntities.GroupCityStateTags
                    where c.GroupId == aGroupId
                    select c).FirstOrDefault<GroupCityStateTag>();
        }

        private IEnumerable<GroupBoard> GetGroupBoards(int aGroupId) {
            return (from cb in theEntities.GroupBoards
                    where cb.GroupId == aGroupId
                    select cb);
        }

        private GroupTag GetGroupKeywordTag(int aGroupId, string aKeyword) {
            return (from k in theEntities.GroupTags
                    where k.GroupId == aGroupId
                    && k.Tag == aKeyword
                    select k).FirstOrDefault<GroupTag>();
        }

        private GroupZipCodeTag GetGroupZipCodeTag(int aGroupId, int aZipCode) {
            return (from z in theEntities.GroupZipCodeTags
                    where z.GroupId == aGroupId
                    && z.ZipCode == aZipCode
                    select z).FirstOrDefault<GroupZipCodeTag>();
        }
    }
}