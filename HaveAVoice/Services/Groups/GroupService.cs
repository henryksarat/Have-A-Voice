using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.Constants;
using HaveAVoice.Models;
using HaveAVoice.Repositories.Groups;
using Social.Admin.Exceptions;
using Social.Admin.Helpers;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Services.Groups {
    public class GroupService : IGroupService {
        private const string GROUP_DOESNT_EXIST = "That club doesn't exist.";
        private const string MEMBER_DOESNT_EXIST = "That user doesn't exist.";
        private const string NOT_ADMINISTRATOR = "You are not an administrator of this club.";

        private IValidationDictionary theValidationDictionary;
        private IGroupRepository theGroupRepository;

        public GroupService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityGroupRepository()) { }

        public GroupService(IValidationDictionary aValidationDictionary, IGroupRepository aProfessorRepo) {
            theValidationDictionary = aValidationDictionary;
            theGroupRepository = aProfessorRepo;
        }

        public bool ActivateGroup(UserInformationModel<User> aUser, int aGroupId) {
            if (!ValidateAdmin(aUser.Details, aGroupId)) {
                return false;
            }

            theGroupRepository.ActivateGroup(aUser.Details, aGroupId);

            return true;
        }

        public bool ApproveGroupMember(UserInformationModel<User> aUser, int aGroupMemberId, string aTitle, bool anAdministrator) {
            if (!ValidTitle(aTitle)) {
                return false;
            }

            theGroupRepository.ApproveGroupMember(aUser.Details, aGroupMemberId, aTitle, anAdministrator);

            return true;
        }

        public void CancelRequestToJoin(UserInformationModel<User> aUser, int aGroupId) {
            theGroupRepository.DeleteRequestToJoinGroup(aUser.Details, aGroupId);
        }

        public bool CreateGroup(UserInformationModel<User> aUser, string aTitle, string aName, string aDescription, bool anAutoAccept) {
            if (!ValidGroup(aName, aDescription, aTitle, true)) {
                return false;
            }

            Group myGroup = theGroupRepository.CreateGroup(aUser.Details, aName, aDescription, anAutoAccept);

            try {
                theGroupRepository.AddMemberToGroup(aUser.Details, aUser.Details.Id, myGroup.Id, aTitle, true);
            } catch (Exception myException) {
                theGroupRepository.DeleteGroup(myGroup.Id);
                throw new Exception("Error adding the creating member of the group as a group member.", myException);
            }

            return true;
        }

        public bool DeactivateGroup(UserInformationModel<User> aUser, int aGroupId) {
            if (!ValidateAdmin(aUser.Details, aGroupId)) {
                return false;
            }

            theGroupRepository.DeactivateGroup(aUser.Details, aGroupId);

            return true;
        }

        public void DenyGroupMember(UserInformationModel<User> aUser, int aGroupMemberId) {
            theGroupRepository.DenyGroupMember(aUser.Details, aGroupMemberId);
        }

        public Group GetGroup(UserInformationModel<User> aUser, int aGroupId) {
            theGroupRepository.MarkGroupBoardAsViewed(aUser.Details, aGroupId);
            return theGroupRepository.GetGroup(aUser.Details, aGroupId);
        }

        public bool EditGroup(UserInformationModel<User> aUserEditing, int aGroupId, string aName, string aDescription) {
            if (!ValidGroup(aName, aDescription, aName, false)) {
                return false;
            }

            Group myGroup = theGroupRepository.GetGroup(aUserEditing.Details, aGroupId);

            myGroup.Name = aName;
            myGroup.Description = aDescription;
            myGroup.LastEditedByUserId = aUserEditing.UserId;
            myGroup.LastEditedDateTimeStamp = DateTime.UtcNow;

            theGroupRepository.UpdateGroup(myGroup);

            return true;
        }

        public Group GetGroupForEdit(UserInformationModel<User> aUser, int aGroupId) {
            if (!IsAdmin(aUser.Details, aGroupId)) {
                if (!PermissionHelper<User>.AllowedToPerformAction(theValidationDictionary, aUser, SocialPermission.Edit_Any_Group)) {
                    throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
                }
            }
            return theGroupRepository.GetGroup(aUser.Details, aGroupId);
        }

        public IEnumerable<GroupBoard> GetGroupBoardPostings(int aGroupId) {
            return theGroupRepository.GetGroupBoardPostings(aGroupId);
        }

        public GroupMember GetGroupMember(UserInformationModel<User> aUser, int aGroupMemberId) {
            GroupMember myGroupMember = theGroupRepository.GetGroupMember(aGroupMemberId);
            if (myGroupMember != null) {
                bool myIsAdmin = IsAdmin(aUser.Details, myGroupMember.GroupId);
                if (!myIsAdmin) {
                    myGroupMember = null;
                }
            }
            return myGroupMember;
        }

        public IEnumerable<GroupMember> GetActiveGroupMembers(int aGroupId) {
            return theGroupRepository.GetGroupMembers(aGroupId).Where(cm => cm.Approved == HAVConstants.APPROVED);
        }

        public IEnumerable<Group> GetGroups(UserInformationModel<User> aUser, string aUniversityId) {
            return theGroupRepository.GetGroups(aUser.Details, aUniversityId);
        }

        public bool IsAdmin(User aUser, int aGroupId) {
            GroupMember myGroupMember = theGroupRepository.GetGroupMember(aUser.Id, aGroupId);
            return myGroupMember != null && myGroupMember.Administrator;
        }

        public bool IsApartOfGroup(int aUserId, int aGroupId) {
            GroupMember myGroupMember = theGroupRepository.GetGroupMember(aUserId, aGroupId);
            return myGroupMember != null && !myGroupMember.Deleted && myGroupMember.Approved == HAVConstants.APPROVED;
        }

        public bool IsPendingApproval(int aUserId, int aGroupId) {
            GroupMember myGroupMember = theGroupRepository.GetGroupMember(aUserId, aGroupId);
            return myGroupMember != null && !myGroupMember.Deleted && myGroupMember.Approved == HAVConstants.PENDING;
        }

        public bool PostToGroupBoard(UserInformationModel<User> aPostingUser, int aGroupId, string aMessage) {
            if(!ValidatePostToBoard(aPostingUser.Details, aGroupId, aMessage)) {
                return false;
            }

            theGroupRepository.PostToGroupBoard(aPostingUser.Details, aGroupId, aMessage);

            return true;
        }

        public bool RemoveGroupMember(UserInformationModel<User> aGroupAdmin, int aCurrentUserId, int aGroupId) {
            if (!ValidateRemovingGroupMember(aGroupAdmin.Details, aCurrentUserId, aGroupId)) {
                return false;
            }

            theGroupRepository.DeleteUserFromGroup(aGroupAdmin.Details, aCurrentUserId, aGroupId);

            return true;
        }

        public void RequestToJoinGroup(UserInformationModel<User> aRequestingMember, int aGroupId) {
            theGroupRepository.MemberRequestToJoinGroup(aRequestingMember.Details, aGroupId, GroupConstants.DEFAULT_NEW_MEMBER_TITLE);
        }

        private bool ValidateAdmin(User aUser, int aGroupId) {
            if (!IsAdmin(aUser, aGroupId)) {
                theValidationDictionary.AddError("GroupMemberAdmin", string.Empty, "You are not an admin of the club.");
                return false;
            }

            return true;
        }

        private bool ValidateRemovingGroupMember(User aUserDoingRemoving, int aCurrentUserId, int aGroupId) {
            ValidateGroupExists(aUserDoingRemoving, aGroupId);
            GroupMember myGroupMember = theGroupRepository.GetGroupMember(aCurrentUserId, aGroupId);
            if (myGroupMember == null) {
                theValidationDictionary.AddError("GroupMember", string.Empty, "That club member doesn't exist.");
            }
            GroupMember myGroupMemberDoingRemoving = theGroupRepository.GetGroupMember(aUserDoingRemoving.Id, aGroupId);
            if (aUserDoingRemoving.Id != aCurrentUserId) {
                if (!myGroupMemberDoingRemoving.Administrator) {
                    theValidationDictionary.AddError("GroupMemberAdmin", string.Empty, NOT_ADMINISTRATOR);
                }
            } 

            return theValidationDictionary.isValid;
        }

        private bool ValidatePostToBoard(User aPostingUser, int aGroupId, string aMessage) {
            ValidateGroupExists(aPostingUser, aGroupId);
            
            GroupMember myPostingUserGroupMember = theGroupRepository.GetGroupMember(aPostingUser.Id, aGroupId);

            if (myPostingUserGroupMember == null) {
                theValidationDictionary.AddError("GroupMember", string.Empty, "You are not part of the club so you can't post on the board.");
            }

            if (string.IsNullOrEmpty(aMessage)) {
                theValidationDictionary.AddError("BoardMessage", aMessage, "To post to the board you need to provide a message.");
            }

            return theValidationDictionary.isValid;
        }

        private void ValidateGroupExists(User aUser, int aGroupId) {
            Group myGroup = theGroupRepository.GetGroup(aUser, aGroupId);
            if (myGroup == null) {
                theValidationDictionary.AddError("Group", aGroupId.ToString(), GROUP_DOESNT_EXIST);
            }
        }

        
        private bool ValidGroup(string aName, string aDescription, string aTitle, bool aIsCreating) {
            if (aIsCreating) {
                ValidTitle(aTitle);
            }

            if (string.IsNullOrEmpty(aName)) {
                theValidationDictionary.AddError("Group", aName, "A group name is required.");
            }

            if (string.IsNullOrEmpty(aDescription)) {
                theValidationDictionary.AddError("Description", aDescription, "Some description is required.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidTitle(string aTitle) {
            if (string.IsNullOrEmpty(aTitle)) {
                theValidationDictionary.AddError("Title", aTitle, "You must give yourself a title for the group. You can use the default title if you'd like: " + GroupConstants.DEFAULT_GROUP_LEADER_TITLE);
            }

            return theValidationDictionary.isValid;
        }
    }
}
