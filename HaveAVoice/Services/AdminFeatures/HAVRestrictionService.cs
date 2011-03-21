using System.Collections.Generic;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.AdminFeatures;
using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

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
        
        public bool CreateRestriction(UserInformationModel<User> aCreatedByUser, Restriction aRestrictionToCreate) {
            if (!ValidateRestriction(aRestrictionToCreate)) {
                return false;
            }
            if (!AllowedToPerformAction(aCreatedByUser, SocialPermission.Create_Restriction)) {
                return false;
            }

            theRepository.CreateRestriction(aCreatedByUser.Details, aRestrictionToCreate);
            return true;
        }

        public bool EditRestriction(UserInformationModel<User> anEditedByUser, Restriction aRestrictionToEdit) {
            if (!ValidateRestriction(aRestrictionToEdit)) {
                return false;
            }
            if (!AllowedToPerformAction(anEditedByUser, SocialPermission.Edit_Restriction)) {
                return false;
            }
            theRepository.EditRestriction(anEditedByUser.Details, aRestrictionToEdit);
            return true;
        }

        public bool DeleteRestriction(UserInformationModel<User> aDeletedByUser, Restriction aRestrictionDelete) {
            if (!AllowedToPerformAction(aDeletedByUser, SocialPermission.Delete_Restriction)) {
                return false;
            }
            theRepository.DeleteRestriction(aDeletedByUser.Details, aRestrictionDelete);
            return true;
        }

        private bool AllowedToPerformAction(UserInformationModel<User> aUser, SocialPermission aPermission) {
            if (!PermissionHelper<User>.AllowedToPerformAction(aUser, aPermission)) {
                theValidationDictionary.AddError("PerformAction", string.Empty, "You are not allowed to perform that action.");
            }

            return theValidationDictionary.isValid;
        }
    }
}