using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.Constants;
using HaveAVoice.Helpers.Search;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.Groups;
using Social.Admin.Exceptions;
using Social.Admin.Helpers;
using Social.Generic.Constants;
using Social.Generic.Exceptions;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Services.Groups {
    public class GroupService : IGroupService {
        private const string REQUEST_SUCCESS = "Your request to join the group has been submitted! Now an admin of the group must approve you before you can join.";
        private const string AUTOADD_SUCCESS = "You have joined the group!";
        private const string GROUP_DOESNT_EXIST = "That group doesn't exist.";
        private const string MEMBER_DOESNT_EXIST = "That user doesn't exist.";
        private const string NOT_ADMINISTRATOR = "You are not an administrator of this group.";

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

        public Group CreateGroup(UserInformationModel<User> aUser, EditGroupModel aGroupModel) {
            if (!ValidGroup(aGroupModel, true) 
                | !ValidTags(aGroupModel.KeywordTags, aGroupModel.ZipCodeTags, aGroupModel.CityTag, aGroupModel.StateTag)) {
                return null;
            }

            Group myGroup = theGroupRepository.CreateGroup(aUser.Details, aGroupModel.Name, aGroupModel.Description, aGroupModel.AutoAccept);

            try {
                int myIntHolder;
                List<int> myZipCodes = aGroupModel.ZipCodeTags.Split(',').Where(z => int.TryParse(z.Trim(), out myIntHolder)).Select(z => int.Parse(z)).ToList(); ;
                List<string> myKeyWords = aGroupModel.KeywordTags.Split(',').Select(k => k.Trim()).Where(k => !string.IsNullOrEmpty(k)).ToList();
                theGroupRepository.AddTagsForGroup(aUser.Details, myGroup.Id, myZipCodes, myKeyWords, aGroupModel.CityTag, aGroupModel.StateTag);
                theGroupRepository.AddMemberToGroup(aUser.Details, aUser.Details.Id, myGroup.Id, aGroupModel.CreatorTitle, true);
            } catch (Exception myException) {
                theGroupRepository.RefreshConnection();
                theGroupRepository.DeleteGroup(myGroup.Id);
                throw new Exception("Error adding the creating member of the group as a group member. So we deleted the group.", myException);
            }

            return myGroup;
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

        public bool EditGroup(UserInformationModel<User> aUserEditing, EditGroupModel anEditGroupModel) {
            if (!ValidGroup(anEditGroupModel, false)
                | !ValidTags(anEditGroupModel.KeywordTags, anEditGroupModel.ZipCodeTags, anEditGroupModel.CityTag, anEditGroupModel.StateTag)) {
                return false;
            }

            Group myGroup = theGroupRepository.GetGroup(aUserEditing.Details, anEditGroupModel.Id);
            IEnumerable<int> myCurrentZipCodes = myGroup.GroupZipCodeTags.Select(z => z.ZipCode);
            IEnumerable<string> myCurrentTags = myGroup.GroupTags.Select(t => t.Tag);

            myGroup.Name = anEditGroupModel.Name;
            myGroup.Description = anEditGroupModel.Description;
            myGroup.AutoAccept = anEditGroupModel.AutoAccept;
            myGroup.LastEditedByUserId = aUserEditing.UserId;
            myGroup.LastEditedDateTimeStamp = DateTime.UtcNow;

            theGroupRepository.UpdateGroup(myGroup);

            int myIntHolder;
            List<int> myEditedZipCodes = anEditGroupModel.ZipCodeTags.Split(',').Where(z => int.TryParse(z.Trim(), out myIntHolder)).Select(z => int.Parse(z)).ToList<int>();
            List<string> myEditedKeyWords = anEditGroupModel.KeywordTags.Split(',').Select(k => k.Trim()).Where(k => !string.IsNullOrEmpty(k)).ToList<string>();

            IEnumerable<int> myZipCodesToAdd = myEditedZipCodes.Except(myCurrentZipCodes);
            IEnumerable<string> myKeywordsToAdd = myEditedKeyWords.Except(myCurrentTags);

            List<int> myZipCodesToDelete = myCurrentZipCodes.Except(myEditedZipCodes).ToList<int>();
            List<string> myKeywordsToDelete = myCurrentTags.Except(myEditedKeyWords).ToList<string>();

            bool myDeleteCityStateTag = false;

            if (string.IsNullOrEmpty(anEditGroupModel.CityTag) || anEditGroupModel.StateTag.Equals(Constants.SELECT)) {
                myDeleteCityStateTag = true;
            }

            bool myAddNewCityStateTag = false;
            GroupCityStateTag myCityStateTag = theGroupRepository.GetGroupCityStateTag(anEditGroupModel.Id);

            if ((myCityStateTag == null && !string.IsNullOrEmpty(anEditGroupModel.CityTag) && !string.IsNullOrEmpty(anEditGroupModel.StateTag) && !anEditGroupModel.StateTag.Equals(Constants.SELECT)) 
                || (myCityStateTag != null && (!anEditGroupModel.CityTag.Equals(myCityStateTag.City) || !anEditGroupModel.StateTag.Equals(myCityStateTag.State)))) {
                myDeleteCityStateTag = true;
                myAddNewCityStateTag = true;
            }

            theGroupRepository.UpdateTagsForGroup(aUserEditing.Details, anEditGroupModel.Id,
                                                  myZipCodesToAdd, myZipCodesToDelete, myKeywordsToAdd,
                                                  myKeywordsToDelete, myDeleteCityStateTag, myAddNewCityStateTag,
                                                  anEditGroupModel.CityTag, anEditGroupModel.StateTag);

            return true;
        }

        public bool EditGroupMember(UserInformationModel<User> aUser, int aGroupId, int aGroupMemberId, string aTitle, bool anAdministrator) {
            if (!ValidTitle(aTitle) | !ValidateAdmin(aUser.Details, aGroupId)) {
                return false;
            }

            theGroupRepository.EditGroupMember(aUser.Details, aGroupMemberId, aTitle, anAdministrator);

            return true;    
        }

        public EditGroupModel GetGroupForEdit(UserInformationModel<User> aUser, int aGroupId) {
            if (!IsAdmin(aUser.Details, aGroupId)) {
                if (!PermissionHelper<User>.AllowedToPerformAction(theValidationDictionary, aUser, SocialPermission.Edit_Any_Group)) {
                    throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
                }
            }

            Group myGroup = theGroupRepository.GetGroup(aUser.Details, aGroupId);
            EditGroupModel myEditGroup = new EditGroupModel(myGroup) {
                ViewAction = ViewAction.Edit
            };
            return myEditGroup;
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

        public IEnumerable<Group> GetGroups(UserInformationModel<User> aUser, string aSearchTerm, SearchBy aSearchBy, OrderBy orderBy, bool aMyGroups) {
            IEnumerable<Group> myGroups = new List<Group>();

            if (aSearchBy == SearchBy.All) {
                myGroups = theGroupRepository.GetGroupsByAll(aUser.Details, aMyGroups);
            } else if (aSearchBy == SearchBy.City) {
                myGroups = theGroupRepository.GetGroupsByCity(aUser.Details, aSearchTerm, aMyGroups);
            } else if (aSearchBy == SearchBy.Name) {
                myGroups = theGroupRepository.GetGroupsByName(aUser.Details, aSearchTerm, aMyGroups);
            } else if (aSearchBy == SearchBy.Tags) {
                myGroups = theGroupRepository.GetGroupsByKeywordTags(aUser.Details, aSearchTerm, aMyGroups);
            } else if (aSearchBy == SearchBy.ZipCode) {
                int myParsedZip;
                bool myTryParsed = int.TryParse(aSearchTerm, out myParsedZip);
                if (!myTryParsed) {
                    throw new CustomException("The zip code must be 5 digits long.");
                }
                myGroups = theGroupRepository.GetGroupsByZipCode(aUser.Details, myParsedZip, aMyGroups);
            }

            if (orderBy == OrderBy.City) {
                myGroups = myGroups.OrderBy(g => g.GroupCityStateTags.OrderBy(gc => gc.City));
            } else if (orderBy == OrderBy.Name) {
                myGroups = myGroups.OrderBy(g => g.Name);
            } else if (orderBy == OrderBy.State) {
                myGroups = myGroups.OrderBy(g => g.GroupCityStateTags.OrderBy(gc => gc.State));
            } else if (orderBy == OrderBy.ZipCode) {
                myGroups = myGroups.OrderBy(g => 
                    g.GroupZipCodeTags.Count > 0 ? 
                    g.GroupZipCodeTags.Min(gz => gz.ZipCode) :
                    0);
            }


            return myGroups;
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

        public void RequestToJoinGroup(UserInformationModel<User> aRequestingMember, int aGroupId, out string aMessage) {
            Group myGroup = GetGroup(aRequestingMember, aGroupId);
            if (myGroup.AutoAccept) {
                aMessage = AUTOADD_SUCCESS;
                theGroupRepository.AutoAcceptGroupMember(aRequestingMember.Details, aGroupId, GroupConstants.DEFAULT_NEW_MEMBER_TITLE);
            } else {
                aMessage = REQUEST_SUCCESS;
                theGroupRepository.MemberRequestToJoinGroup(aRequestingMember.Details, aGroupId, GroupConstants.DEFAULT_NEW_MEMBER_TITLE);
            }
        }

        public IDictionary<string, string> SearchByOptions() {
            IDictionary<string, string> mySearchByOptionsDictionary = new Dictionary<string, string>();
            mySearchByOptionsDictionary.Add("Show all groups", SearchBy.All.ToString());
            mySearchByOptionsDictionary.Add(SearchBy.Name.ToString(), SearchBy.Name.ToString());
            mySearchByOptionsDictionary.Add(SearchBy.Tags.ToString(), SearchBy.Tags.ToString());
            mySearchByOptionsDictionary.Add("Zip Code", SearchBy.ZipCode.ToString());
            mySearchByOptionsDictionary.Add(SearchBy.City.ToString(), SearchBy.City.ToString());
            return mySearchByOptionsDictionary;
        }

        public IDictionary<string, string> OrderByOptions() {
            IDictionary<string, string> myOrderByOptionsDictionary = new Dictionary<string, string>();
            myOrderByOptionsDictionary.Add(OrderBy.Name.ToString(), OrderBy.Name.ToString());
            myOrderByOptionsDictionary.Add("Zip Code", OrderBy.ZipCode.ToString());
            myOrderByOptionsDictionary.Add(OrderBy.City.ToString(), OrderBy.City.ToString());
            myOrderByOptionsDictionary.Add(OrderBy.State.ToString(), OrderBy.State.ToString());
            return myOrderByOptionsDictionary;
        }

        private bool ValidateAdmin(User aUser, int aGroupId) {
            if (!IsAdmin(aUser, aGroupId)) {
                theValidationDictionary.AddError("GroupMemberAdmin", string.Empty, "You are not an admin of the group.");
            }

            return theValidationDictionary.isValid;
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
                theValidationDictionary.AddError("GroupMember", string.Empty, "You are not part of the group so you can't post on the board.");
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

        private bool ValidTags(string aKeywordTags, string aZipCodeTags, string aCityTag, string aStateTag) {
            if (!string.IsNullOrEmpty(aZipCodeTags)) {
                IEnumerable<string> myZipCodes = aZipCodeTags.Split(',').Select(z => z.Trim());
                foreach (string myZipCode in myZipCodes) {
                    int myParsedZipCode;
                    bool myResult = int.TryParse(myZipCode, out myParsedZipCode);
                    if (!myResult || myParsedZipCode.ToString().Length != 5 || myParsedZipCode < 0) {
                        theValidationDictionary.AddError("ZipCodeTags", aZipCodeTags, "A zip code is entered incorrectly. Zip codes must be 5 digits long.");
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(aCityTag) || DropDownItemValidation.IsValid(aStateTag)) {
                if (string.IsNullOrEmpty(aCityTag)) {
                    theValidationDictionary.AddError("CityTag", aCityTag, "You must enter a city.");
                }

                if (!DropDownItemValidation.IsValid(aStateTag)) {
                    theValidationDictionary.AddError("StateTag", aCityTag, "You must enter a state.");
                }
            }
            
            return theValidationDictionary.isValid;
        }
        
        private bool ValidGroup(EditGroupModel aGroupModel, bool aIsCreating) {
            if (aIsCreating) {
                ValidTitle(aGroupModel.CreatorTitle);
            }

            if (string.IsNullOrEmpty(aGroupModel.Name)) {
                theValidationDictionary.AddError("Name", aGroupModel.Name, "A group name is required.");
            }

            if (string.IsNullOrEmpty(aGroupModel.Description)) {
                theValidationDictionary.AddError("Description", aGroupModel.Description, "Some description is required.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidTitle(string aTitle) {
            if (string.IsNullOrEmpty(aTitle)) {
                theValidationDictionary.AddError("CreatorTitle", aTitle, "You must give yourself a title for the group. You can use the default title if you'd like: " + GroupConstants.DEFAULT_GROUP_LEADER_TITLE);
            }

            return theValidationDictionary.isValid;
        }
    }
}
