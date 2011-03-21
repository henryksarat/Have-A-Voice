using System.Collections.Generic;
using Social.Admin.Helpers;
using Social.Admin.Repositories;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace Social.Admin.Services {
    public class RoleService<T, U> : IRoleService<T, U> {
        private IValidationDictionary theValidationDictionary;
        private IRoleRepository<T, U> theRoleRepo;


        public RoleService(IValidationDictionary aValidationDictionary, IRoleRepository<T, U> aRepository) {
            theValidationDictionary = aValidationDictionary;
            theRoleRepo = aRepository;
        }

        public U FindRole(int id) {
            return theRoleRepo.FindRole(id);;
        }

        public IEnumerable<U> GetAllRoles() {
            return theRoleRepo.GetAllRoles();
        }

        public bool Create(UserInformationModel<T> aCreatedByUser, AbstractRoleModel<U> aRoleToCreate, List<int> aSelectedPermissionIds, int aSelectedRestrictionId) {
            if (!ValidateRole(aRoleToCreate) | !ValidateRestriction(aSelectedRestrictionId)) {
                return false;
            }
            if (!AllowedToPerformAction(aCreatedByUser, SocialPermission.Create_Role)) {
                return false;
            }

            theRoleRepo.Create(aCreatedByUser.Details, aRoleToCreate.FromModel(), aSelectedPermissionIds, aSelectedRestrictionId);
            return true;
        }

        public bool Edit(UserInformationModel<T> anEditedByUser, AbstractRoleModel<U> aRoleToEdit, List<int> aSelectedPermissions, int selectedRestrictionId) {
            if (!ValidateRole(aRoleToEdit)) {
                return false;
            }
            if (!AllowedToPerformAction(anEditedByUser, SocialPermission.Edit_Role)) {
                return false;
            }
            theRoleRepo.Edit(anEditedByUser.Details, aRoleToEdit.FromModel(), aSelectedPermissions, selectedRestrictionId);
            return true;
            
        }
       
        public bool Delete(UserInformationModel<T> aDeletedByUser, U aRoleToDelete) {
            if (!AllowedToPerformAction(aDeletedByUser, SocialPermission.Delete_Role)) {
                return false;
            }
            theRoleRepo.Delete(aDeletedByUser.Details, aRoleToDelete);
            return true;
        }

        public IEnumerable<T> UsersInRole(int aRoleId) {
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

        private bool ValidateRole(AbstractRoleModel<U> role) {
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

        private bool AllowedToPerformAction(UserInformationModel<T> aUser, SocialPermission aPermission) {
            if (!PermissionHelper<T>.AllowedToPerformAction(aUser, aPermission)) {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to perform that action.");
            }

            return theValidationDictionary.isValid;
        }
    }
}