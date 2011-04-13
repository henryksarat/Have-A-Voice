using System.Collections.Generic;
using Social.Admin.Helpers;
using Social.Admin.Repositories;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;
using Social.Generic.Constants;
using Social.Admin.Exceptions;
using System.Linq;
using System;

namespace Social.Admin.Services {
    public class RoleService<T, U, V> : IRoleService<T, U, V> {
        private IValidationDictionary theValidationDictionary;
        private IRoleRepository<T, U, V> theRoleRepo;


        public RoleService(IValidationDictionary aValidationDictionary, IRoleRepository<T, U, V> aRepository) {
            theValidationDictionary = aValidationDictionary;
            theRoleRepo = aRepository;
        }

        public U FindRole(UserInformationModel<T> aViewingUser, int aRoleId, SocialPermission aSocialPermission) {
            if (!PermissionHelper<T>.AllowedToPerformAction(theValidationDictionary, aViewingUser, aSocialPermission)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }
            return theRoleRepo.FindRole(aRoleId);
        }

        public IEnumerable<U> GetAllRoles(UserInformationModel<T> aViewingUser) {
            if (!PermissionHelper<T>.AllowedToPerformAction(theValidationDictionary, aViewingUser, SocialPermission.View_Roles)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }
            return theRoleRepo.GetAllRoles();
        }

        public bool Create(UserInformationModel<T> aCreatedByUser, AbstractRoleModel<U> aRoleToCreate, List<int> aSelectedPermissionIds) {
            if (!ValidateRole(aRoleToCreate)) {
                return false;
            }
            if (!PermissionHelper<T>.AllowedToPerformAction(theValidationDictionary, aCreatedByUser, SocialPermission.Create_Role)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }

            theRoleRepo.Create(aCreatedByUser.Details, aRoleToCreate.FromModel(), aSelectedPermissionIds);
            return true;
        }

        public bool Edit(UserInformationModel<T> anEditedByUser, AbstractRoleModel<U> aRoleToEdit, List<int> aSelectedPermissions) {
            if (!ValidateRole(aRoleToEdit)) {
                return false;
            }
            if (!PermissionHelper<T>.AllowedToPerformAction(theValidationDictionary, anEditedByUser, SocialPermission.Edit_Role)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }

            IEnumerable<int> myCurrentPermissionsAssociatedWithRole = theRoleRepo.GetAbstractRolePermissionsByRole(aRoleToEdit.FromModel()).Select(r => r.PermissionId);
            List<int> myPermissionsToCreateAssociateWith = new List<int>();
            List<int> myPermissionToDeleteAssociationWith = new List<int>();

            if (myCurrentPermissionsAssociatedWithRole.Count() == 0) {
                myPermissionsToCreateAssociateWith.AddRange(aSelectedPermissions);
            } else if (aSelectedPermissions == null || aSelectedPermissions.Count == 0) {
                myPermissionToDeleteAssociationWith.AddRange(myCurrentPermissionsAssociatedWithRole);
            } else {
                myPermissionToDeleteAssociationWith.AddRange(myCurrentPermissionsAssociatedWithRole.Except(aSelectedPermissions).ToList<int>());
                myPermissionsToCreateAssociateWith.AddRange(aSelectedPermissions.Except(myCurrentPermissionsAssociatedWithRole).ToList<int>());
            }

            theRoleRepo.Edit(anEditedByUser.Details, aRoleToEdit.FromModel(), myPermissionsToCreateAssociateWith, myPermissionToDeleteAssociationWith);
            return true;
            
        }
       
        public bool Delete(UserInformationModel<T> aDeletedByUser, U aRoleToDelete) {
            if (!PermissionHelper<T>.AllowedToPerformAction(theValidationDictionary, aDeletedByUser, SocialPermission.Delete_Role)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }
            theRoleRepo.Delete(aDeletedByUser.Details, aRoleToDelete);
            return true;
        }

        public bool MoveUsersToRole(UserInformationModel<T> aUserDoingMove, List<int> aUsers, int aFromRoleId, int aToRoleId) {
            if (!ValidateSwitchingRole(aUsers, aFromRoleId, aToRoleId)) {
                return false;
            }
            if (!PermissionHelper<T>.AllowedToPerformAction(theValidationDictionary, aUserDoingMove, SocialPermission.Switch_Users_Role)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }

            theRoleRepo.MoveUsersToRole(aUsers, aFromRoleId, aToRoleId);
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

        private bool ValidateRole(AbstractRoleModel<U> role) {
            if (role.Name.Trim().Length == 0) {
                theValidationDictionary.AddError("Name", role.Name.Trim(), "Role name is required.");
            }
            if (role.Description.Trim().Length == 0) {
                theValidationDictionary.AddError("Description", role.Description.Trim(), "Role description is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}