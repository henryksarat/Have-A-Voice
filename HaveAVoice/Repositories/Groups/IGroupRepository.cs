using System.Collections.Generic;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Groups {
    public interface IGroupRepository {
        void ActivateGroup(User aUser, int aGroupId);
        void ApproveGroupMember(User anAdminUser, int aGroupMemberId, string aTitle, bool anAdministrator);
        void AddMemberToGroup(User anAdminUser, int aNewMemberUserId, int aGroupId, string aTitle, bool anAdministrator);
        void DeleteRequestToJoinGroup(User aUser, int aGroupId);
        void DeleteGroup(int aGroupId);
        void DeactivateGroup(User aUser, int aGroupId);
        void DeleteUserFromGroup(User aDeletingUser, int aFormerMemberUserId, int aGroupId);
        void DenyGroupMember(User anAdminUser, int aGroupMemberId);
        Group GetGroup(User aUser, int aGroupId);
        IEnumerable<GroupBoard> GetGroupBoardPostings(int aGroupId);
        GroupMember GetGroupMember(int aGroupMemberId);
        GroupMember GetGroupMember(int aUserId, int aGroupId);
        IEnumerable<GroupMember> GetGroupMembers(int aGroupId);
        IEnumerable<Group> GetGroups(User aUser, string aUniversityId);
        Group CreateGroup(User aUser, string aName, string aDescription, bool anAutoAccept);
        void MarkGroupBoardAsViewed(User aUser, int aGroupId);
        void MemberRequestToJoinGroup(User aRequestingUser, int aGroupId, string aTitle);
        void PostToGroupBoard(User aPostingUser, int aGroupId, string aMessage);
        void UpdateGroup(Group aGroup);
    }
}
