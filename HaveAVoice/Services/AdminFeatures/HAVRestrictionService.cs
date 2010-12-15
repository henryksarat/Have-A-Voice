using System.Collections.Generic;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;
using HaveAVoice.Models;

namespace HaveAVoice.Services.AdminFeatures {
    public class HAVRestrictionService : HAVBaseService, IHAVRestrictionService {
        private IValidationDictionary theValidationDictionary;
        private IHAVRestrictionRepository theRepository;

        public HAVRestrictionService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityHAVRestrictionRepository(), new HAVBaseRepository()) { }

        public HAVRestrictionService(IValidationDictionary aValidationDictionary, IHAVRestrictionRepository aRepository,
                                     IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theRepository = aRepository;
        }

        #region Validation

        public bool ValidateRestriction(Restriction restriction) {
            if (restriction.Name.Trim().Length == 0) {
                theValidationDictionary.AddError("Name", restriction.Name.Trim(), "Restriction name is required.");
            }
            if (restriction.Description.Trim().Length == 0) {
                theValidationDictionary.AddError("Description", restriction.Description.Trim(), "Restriction description is required.");
            }

            return theValidationDictionary.isValid;
        }

        #endregion

        public IEnumerable<Restriction> GetAllRestrictions() {
            return theRepository.GetAllRestrictions();
        }

        public Restriction GetRestriction(int restrictionId) {
            return theRepository.GetRestriction(restrictionId);
        }
        
        public bool CreateRestriction(UserInformationModel aCreatedByUser, Restriction aRestrictionToCreate) {
            if (!ValidateRestriction(aRestrictionToCreate)) {
                return false;
            }
            if (!AllowedToPerformAction(aCreatedByUser, HAVPermission.Create_Restriction)) {
                return false;
            }

            theRepository.CreateRestriction(aCreatedByUser.Details, aRestrictionToCreate);
            return true;
        }

        public bool EditRestriction(UserInformationModel anEditedByUser, Restriction aRestrictionToEdit) {
            if (!ValidateRestriction(aRestrictionToEdit)) {
                return false;
            }
            if (!AllowedToPerformAction(anEditedByUser, HAVPermission.Edit_Restriction)) {
                return false;
            }
            theRepository.EditRestriction(anEditedByUser.Details, aRestrictionToEdit);
            return true;
        }

        public bool DeleteRestriction(UserInformationModel aDeletedByUser, Restriction aRestrictionDelete) {
            if (!AllowedToPerformAction(aDeletedByUser, HAVPermission.Delete_Restriction)) {
                return false;
            }
            theRepository.DeleteRestriction(aDeletedByUser.Details, aRestrictionDelete);
            return true;
        }

        private bool AllowedToPerformAction(UserInformationModel aUser, HAVPermission aPermission) {
            if (!HAVPermissionHelper.AllowedToPerformAction(aUser, aPermission)) {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to perform that action.");
            }

            return theValidationDictionary.isValid;
        }
    }
}