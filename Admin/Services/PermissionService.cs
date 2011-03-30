using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Admin;
using Social.User.Models;
using Social.Admin.Helpers;
using Social.Validation;
using Social.Generic.Models;
using Social.Generic.Helpers;
using Social.Admin.Exceptions;
using Social.Generic.Constants;

namespace Social.Admin  {
    public class PermissionService<T, U> : IPermissionService<T, U> {
        private IValidationDictionary theValidationDictionary;
        private IPermissionRepository<T, U> thePermissionRepo;

        public PermissionService(IValidationDictionary aValidationDictionary, IPermissionRepository<T, U> aRepository) {
            theValidationDictionary = aValidationDictionary;
            thePermissionRepo = aRepository;
        }

        public IEnumerable<U> GetAllPermissions(UserInformationModel<T> aViewingUser) {
            if (!AllowedToPerformAction(aViewingUser, SocialPermission.View_Permissions)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }
            return thePermissionRepo.GetAllPermissions();
        }

        public U FindPermission(int id) {
            return thePermissionRepo.FindPermission(id);
        }

        public bool Create(UserInformationModel<T> aCreatedByUser, AbstractPermissionModel<U> aPermissionToCreate) {
            if (!ValidatePermission(aPermissionToCreate)) {
                return false;
            }

            if (!AllowedToPerformAction(aCreatedByUser, SocialPermission.Create_Permission)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }

            thePermissionRepo.Create(aCreatedByUser.Details, aPermissionToCreate.FromModel());
            return true;
        }

        public bool Edit(UserInformationModel<T> anEditedByUser, AbstractPermissionModel<U> aPermissionToEdit) {
            if (!ValidatePermission(aPermissionToEdit)) {
                return false;
            }

            if (!AllowedToPerformAction(anEditedByUser, SocialPermission.Edit_Permission)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }

            thePermissionRepo.Edit(anEditedByUser.Details, aPermissionToEdit.FromModel());
            return true;
        }

        public bool Delete(UserInformationModel<T> aDeletedByUser, U aPermissionToDelete) {
            if (!AllowedToPerformAction(aDeletedByUser, SocialPermission.Delete_Permission)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }

            thePermissionRepo.Delete(aDeletedByUser.Details, aPermissionToDelete);
            return true;
        }

        private bool ValidatePermission(AbstractPermissionModel<U> aPermission) {
            if (aPermission.Name.Trim().Length == 0) {
                theValidationDictionary.AddError("Name", aPermission.Name.Trim(), "Permission name is required.");
            }

            if (aPermission.Description.Trim().Length == 0) {
                theValidationDictionary.AddError("Description", aPermission.Description.Trim(), "Permission description is required.");
            }

            if (thePermissionRepo.nameExists(aPermission.Name)) {
                theValidationDictionary.AddError("Name", aPermission.Name.Trim(), "Permission with name " + aPermission.Name + " already exists");
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