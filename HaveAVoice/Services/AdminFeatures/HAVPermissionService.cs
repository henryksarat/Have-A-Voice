using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Validation;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;

namespace HaveAVoice.Services.AdminFeatures  {
    public class HAVPermissionService : HAVBaseService, IHAVPermissionService {
        private IValidationDictionary theValidationDictionary;
        private IHAVPermissionRepository thePermissionRepo;

        public HAVPermissionService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new HAVBaseRepository(), new EntityHAVPermissionRepository()) { }

        public HAVPermissionService(IValidationDictionary aValidationDictionary, IHAVBaseRepository aBaseRepository, IHAVPermissionRepository aRepository)
            : base(aBaseRepository) {
            theValidationDictionary = aValidationDictionary;
            thePermissionRepo = aRepository;
        }

        public IEnumerable<Permission> GetAllPermissions() {
            return thePermissionRepo.GetAllPermissions();
        }

        public Permission FindPermission(int id) {
            return thePermissionRepo.FindPermission(id);
        }

        public bool Create(UserInformationModel aCreatedByUser, Permission aPermissionToCreate) {
            if (!ValidatePermission(aPermissionToCreate)) {
                return false;
            }

            if (!AllowedToPerformAction(aCreatedByUser, HAVPermission.Create_Permission)) {
                return false;
            }

            thePermissionRepo.Create(aCreatedByUser.Details, aPermissionToCreate);
            return true;
        }

        public bool Edit(UserInformationModel anEditedByUser, Permission aPermissionToEdit) {
            if (!ValidatePermission(aPermissionToEdit)) {
                return false;
            }

            if (!AllowedToPerformAction(anEditedByUser, HAVPermission.Edit_Permission)) {
                return false;
            }

            thePermissionRepo.Edit(anEditedByUser.Details, aPermissionToEdit);
            return true;
        }

        public bool Delete(UserInformationModel aDeletedByUser, Permission aPermissionToDelete) {
            if (!AllowedToPerformAction(aDeletedByUser, HAVPermission.Delete_Permission)) {
                return false;
            }

            thePermissionRepo.Delete(aDeletedByUser.Details, aPermissionToDelete);
            return true;
        }

        private bool ValidatePermission(Permission aPermission) {
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

        private bool AllowedToPerformAction(UserInformationModel aUser, HAVPermission aPermission) {
            if (!HAVPermissionHelper.AllowedToPerformAction(aUser, aPermission)) {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to perform that action.");
            }

            return theValidationDictionary.isValid;
        }
    }
}