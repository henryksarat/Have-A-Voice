using System.Collections.Generic;
using HaveAVoice.Models;
using Social.Generic.Models;

namespace HaveAVoice.Services.Groups {
    public interface IGroupService {
        bool ActivateGroup(UserInformationModel<User> aUser, int aGroupId);
        bool ApproveGroupMember(UserInformationModel<User> aUser, int aGroupMemberId, string aTitle, bool anAdministrator);
        void CancelRequestToJoin(UserInformationModel<User> aUser, int aGroupId);
        bool CreateGroup(UserInformationModel<User> aUser, string aTitle, string aName, string aDescription, bool anAutoAccept);
        void DenyGroupMember(UserInformationModel<User> aUser, int aGroupMemberId);
        bool DeactivateGroup(UserInformationModel<User> aUser, int aGroupId);
        bool EditGroup(UserInformationModel<User> aUserEditing, int aGroupId, string aName, string aDescription);
        Group GetGroup(UserInformationModel<User> aUser, int aGroupId);
        Group GetGroupForEdit(UserInformationModel<User> aUser, int aGroupId);
        IEnumerable<GroupBoard> GetGroupBoardPostings(int aGroupId);
        GroupMember GetGroupMember(UserInformationModel<User> aUser, int aGroupMemberId);
        IEnumerable<GroupMember> GetActiveGroupMembers(int aGroupId);
        IEnumerable<Group> GetGroups(UserInformationModel<User> aUser, string aUniversityId);
        bool IsAdmin(User aUser, int aGroupId);
        bool IsApartOfGroup(int aUserId, int aGroupId);
        bool IsPendingApproval(int aUserId, int aGroupId);
        bool PostToGroupBoard(UserInformationModel<User> aPostingUser, int aGroupId, string aMessage);
        bool RemoveGroupMember(UserInformationModel<User> aGroupAdmin, int aCurrentUserId, int aGroupId);
        void RequestToJoinGroup(UserInformationModel<User> aRequestingMember, int aGroupId);
    }
}