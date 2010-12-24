using HaveAVoice.Validation;
using System.Collections.Generic;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;
using HaveAVoice.Models;

namespace HaveAVoice.Services.AdminFeatures {
    public class HAVRoleService : HAVBaseService, IHAVRoleService {
        private IValidationDictionary theValidationDictionary;
        private IHAVRoleRepository theRoleRepo;

        public HAVRoleService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityHAVRoleRepository(), new HAVBaseRepository()) { }

        public HAVRoleService(IValidationDictionary aValidationDictionary, IHAVRoleRepository aRepository,
                                          IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theRoleRepo = aRepository;
        }

        public Role FindRole(int id) {
            return theRoleRepo.FindRole(id);;
        }

        public IEnumerable<Role> GetAllRoles() {
            return theRoleRepo.GetAllRoles();
        }

        public bool Create(UserInformationModel aCreatedByUser, Role aRoleToCreate, List<int> aSelectedPermissionIds, int aSelectedRestrictionId) {
            if (!ValidateRole(aRoleToCreate) | !ValidateRestriction(aSelectedRestrictionId)) {
                return false;
            }
            if (!AllowedToPerformAction(aCreatedByUser, HAVPermission.Create_Role)) {
                return false;
            }

            theRoleRepo.Create(aCreatedByUser.Details, aRoleToCreate, aSelectedPermissionIds, aSelectedRestrictionId);
            return true;
        }

        public bool Edit(UserInformationModel anEditedByUser, Role aRoleToEdit, List<int> aSelectedPermissions, int selectedRestrictionId) {
            if (!ValidateRole(aRoleToEdit)) {
                return false;
            }
            if (!AllowedToPerformAction(anEditedByUser, HAVPermission.Edit_Role)) {
                return false;
            }
            theRoleRepo.Edit(anEditedByUser.Details, aRoleToEdit, aSelectedPermissions, selectedRestrictionId);
            return true;
            
        }
       
        public bool Delete(UserInformationModel aDeletedByUser, Role aRoleToDelete) {
            if (!AllowedToPerformAction(aDeletedByUser, HAVPermission.Delete_Role)) {
                return false;
            }
            theRoleRepo.Delete(aDeletedByUser.Details, aRoleToDelete);
            return true;
        }

        public IEnumerable<User> UsersInRole(int aRoleId) {
            return theRoleRepo.FindUsersInRole(aRoleId);
        }

        private bool ValidateSwitchingRole(List<int> aUsers, int aFromRoleId, int aToRoleId) {
            if (aUsers.Count == 0) {
                theValidationDictionary.AddError("UsersToMove", string.Empty, "No users selected to move.");
            }
            if (aFromRoleId < 1) {
                theValidationDictionary.AddError("CurrentRole", string.Empty, "Please select a Role to moves the users from.");
            }
            if (aToRoleId < 1) {
                theValidationDictionary.AddError("MoveToRole", string.Empty, "Please select a Role to moves the users to.");
            }
            if (aFromRoleId == aToRoleId) {
                theValidationDictionary.AddError("MoveToRole", string.Empty, "Can't move users to the same role they are from.");
            }

            return theValidationDictionary.isValid;
        }

        public bool MoveUsersToRole(List<int> aUsers, int aFromRoleId, int aToRoleId) {
            if (!ValidateSwitchingRole(aUsers, aFromRoleId, aToRoleId)) {
                return false;
            }
            theRoleRepo.MoveUsersToRole(aUsers, aFromRoleId, aToRoleId);
            return true;
        }

        private bool ValidatePermission(Permission aPermission) {
            if (aPermission.Name.Trim().Length == 0) {
                theValidationDictionary.AddError("Name", aPermission.Name.Trim(), "Permission name is required.");
            }

            if (aPermission.Description.Trim().Length == 0) {
                theValidationDictionary.AddError("Description", aPermission.Description.Trim(), "Permission description is required.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidateRole(Role role) {
            if (role.Name.Trim().Length == 0) {
                theValidationDictionary.AddError("Name", role.Name.Trim(), "Role name is required.");
            }
            if (role.Description.Trim().Length == 0) {
                theValidationDictionary.AddError("Description", role.Description.Trim(), "Role description is required.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidateRestriction(int restrictionId) {
            if (restrictionId == 0) {
                theValidationDictionary.AddError("Restriction", string.Empty, "Please select a restriction.");
            }
            return theValidationDictionary.isValid;
        }

        private bool AllowedToPerformAction(UserInformationModel aUser, HAVPermission aPermission) {
            if (!HAVPermissionHelper.AllowedToPerformAction(aUser, aPermission)) {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to perform that action.");
            }

            return theValidationDictionary.isValid;
        }
    }
}