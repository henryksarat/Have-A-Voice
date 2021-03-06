﻿using System.Collections.Generic;
using HaveAVoice.Models;
using Social.Generic.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Search;

namespace HaveAVoice.Services.Groups {
    public interface IGroupService {
        void AcceptGroupInvitation(UserInformationModel<User> aUserInfo, int aGroupInvitationId, out string aMessage);
        bool ActivateGroup(UserInformationModel<User> aUser, int aGroupId);
        bool ApproveGroupMember(UserInformationModel<User> aUser, int aGroupMemberId, string aTitle, bool anAdministrator);
        void CancelRequestToJoin(UserInformationModel<User> aUser, int aGroupId);
        Group CreateGroup(UserInformationModel<User> aUser, EditGroupModel aGroupModel);
        void DeclineGroupInvitation(UserInformationModel<User> aUserInfo, int aGroupInvitationId);
        void DenyGroupMember(UserInformationModel<User> aUser, int aGroupMemberId);
        bool DeactivateGroup(UserInformationModel<User> aUser, int aGroupId);
        bool EditGroup(UserInformationModel<User> aUserEditing, EditGroupModel aGroupModel);
        bool EditGroupMember(UserInformationModel<User> aUser, int aGroupId, int aGroupMemberId, string aTitle, bool anAdministrator);
        GroupInviteModel GetFriendsToInvite(UserInformationModel<User> aUser, int aGroupId);
        Group GetGroup(UserInformationModel<User> aUser, int aGroupId);
        EditGroupModel GetGroupForEdit(UserInformationModel<User> aUser, int aGroupId);
        GroupMember GetGroupMember(UserInformationModel<User> aUser, int aGroupMemberId);
        IEnumerable<GroupMember> GetActiveGroupMembers(int aGroupId);
        IEnumerable<Group> GetGroups(UserInformationModel<User> aUser, string aSearchTerm, SearchBy aSearchBy, OrderBy orderBy, bool aMyGroups);
        void InviteMembers(UserInformationModel<User> aUser, int[] aMembers, int aGroupId);
        bool IsAdmin(UserInformationModel<User> aUser, int aGroupId);
        bool IsApartOfGroup(UserInformationModel<User> aUser, int aGroupId);
        bool IsPendingApproval(UserInformationModel<User> aUser, int aGroupId);
        IDictionary<string, string> OrderByOptions();
        bool PostToGroupBoard(UserInformationModel<User> aPostingUser, int aGroupId, string aMessage);
        bool RemoveGroupMember(UserInformationModel<User> aGroupAdmin, int aCurrentUserId, int aGroupId);
        void RequestToJoinGroup(UserInformationModel<User> aRequestingMember, int aGroupId, out string aMessage);
        IDictionary<string, string> SearchByOptions();
    }
}