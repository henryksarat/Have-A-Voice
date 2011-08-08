using System.Collections.Generic;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Groups {
    public interface IGroupRepository {
        void ActivateGroup(User aUser, int aGroupId);
        void AcceptGroupInvitation(User aUserAccepting, int aGroupInvitationId, out string aMessage);
        void ApproveGroupMember(User anAdminUser, int aGroupMemberId, string aTitle, bool anAdministrator);
        void AutoAcceptGroupMember(User aUser, int aGroupId, string aTitle);
        void AddMemberToGroup(User anAdminUser, int aNewMemberUserId, int aGroupId, string aTitle, bool anAdministrator);
        void AddTagsForGroup(User aUser, int aGroupId, IEnumerable<int> aZipCodeTags, IEnumerable<string> aKeywordTags, string aCityTag, string aStateTag);
        Group CreateGroup(User aUser, string aName, string aDescription, bool anAutoAccept, bool aMakePublic);
        void CreateGroupInvitations(User anInvitingUser, int aGroupId, int[] aUsersToInvite);
        void DeleteRequestToJoinGroup(User aUser, int aGroupId);
        void DeleteGroup(int aGroupId);
        void DeactivateGroup(User aUser, int aGroupId);
        void DeclineGroupInvitation(User aUserAccepting, int aGroupInvitationId);
        void DeleteUserFromGroup(User aDeletingUser, int aFormerMemberUserId, int aGroupId);
        void DenyGroupMember(User anAdminUser, int aGroupMemberId);
        void EditGroupMember(User aUser, int aGroupMemberId, string aTitle, bool anAdministrator);
        Group GetActiveGroupOnly(int aGroupId);
        Group GetGroup(User aUser, int aGroupId);
        GroupCityStateTag GetGroupCityStateTag(int aGroupId);
        GroupInvitation GetGroupInvitation(int aGroupInvitationId);
        GroupMember GetGroupMember(int aGroupMemberId);
        GroupMember GetGroupMember(int aUserId, int aGroupId);
        IEnumerable<GroupMember> GetGroupMembers(int aGroupId);
        IEnumerable<GroupMember> GetGroupMembersPending(int aGroupId);
        IEnumerable<Group> GetGroupsByAll(User aUser, bool anIncludeAdmin);
        IEnumerable<Group> GetGroupsByName(User aUser, string aSearchTerm, bool anIncludeAdmin);
        IEnumerable<Group> GetGroupsByKeywordTags(User aUser, string aSearchTerm, bool anIncludeAdmin);
        IEnumerable<Group> GetGroupsByZipCode(User aUser, int aSearchTerm, bool anIncludeAdmin);
        IEnumerable<Group> GetGroupsByCity(User aUser, string aSearchTerm, bool anIncludeAdmin);
        IEnumerable<Group> GetMyGroupsByAll(User aUser);
        IEnumerable<Group> GetMyGroupsByName(User aUser, string aSearchTerm);
        IEnumerable<Group> GetMyGroupsByKeywordTags(User aUser, string aSearchTerm);
        IEnumerable<Group> GetMyGroupsByZipCode(User aUser, int aSearchTerm);
        IEnumerable<Group> GetMyGroupsByCity(User aUser, string aSearchTerm);
        IEnumerable<GroupInvitation> GetPendingGroupInvites(int aGroupId);
        void MarkGroupBoardAsViewed(User aUser, int aGroupId);
        void MemberRequestToJoinGroup(User aRequestingUser, int aGroupId, string aTitle);
        void PostToGroupBoard(User aPostingUser, int aGroupId, string aMessage);
        void RefreshConnection();
        void UpdateGroup(Group aGroup);
        void UpdateTagsForGroup(User aUser, int aGroupId, IEnumerable<int> aZipCodesToAdd, List<int> aZipCodesToDelete, 
                                IEnumerable<string> aKeywordsTagsToAdd, List<string> aKeywordsTagsToDelete,
                                bool aDeleteCityStateTag, bool aNewCityStatetag, string aCityTag, string aStateTag);
    }
}
