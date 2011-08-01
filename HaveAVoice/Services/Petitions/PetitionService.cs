using System.Collections.Generic;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.Petitions;
using Social.Admin.Exceptions;
using Social.Admin.Helpers;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Services.Petitions {
    public class PetitionService : IPetitionService {
        private IValidationDictionary theValidationDictionary;
        private IPetitionRepository theGroupRepository;

        public PetitionService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityPetitionRepository()) { }

        public PetitionService(IValidationDictionary aValidationDictionary, IPetitionRepository aPetitionRepo) {
            theValidationDictionary = aValidationDictionary;
            theGroupRepository = aPetitionRepo;
        }

        public Petition CreatePetition(UserInformationModel<User> aUserInformation, CreatePetitionModel aCreatePetitionModel) {
            if (!ValidatePetition(aCreatePetitionModel)) {
                return null;
            }

            Petition myPetition = theGroupRepository.CreatePetition(aUserInformation.Details, aCreatePetitionModel.Title, 
                aCreatePetitionModel.Description, aCreatePetitionModel.City, 
                aCreatePetitionModel.State, aCreatePetitionModel.ZipCode);

            return myPetition;
        }

        public bool SignPetition(UserInformationModel<User> aUserInformation, CreatePetitionSignatureModel aCreatePetitionSignatureModel) {
            if (!ValidatePetitionSignature(aCreatePetitionSignatureModel)) {
                return false;
            }

            theGroupRepository.AddSignatureToPetition(aUserInformation.Details,  
                aCreatePetitionSignatureModel.PetitionId,
                aCreatePetitionSignatureModel.Comment, 
                aCreatePetitionSignatureModel.Address, 
                aCreatePetitionSignatureModel.City, 
                aCreatePetitionSignatureModel.State, 
                aCreatePetitionSignatureModel.ZipCode, 
                aCreatePetitionSignatureModel.Email);

            return true;
        }

        public bool SetPetitionAsInactive(UserInformationModel<User> aUserInformation, int aPetitionId) {
            Petition myPetition = GetPetition(aUserInformation, aPetitionId).Petition;

            if (myPetition.UserId != aUserInformation.UserId) {
                if (!PermissionHelper<User>.AllowedToPerformAction(theValidationDictionary, aUserInformation, SocialPermission.Deactivate_Any_Petition)) {
                    throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
                }
            }

            theGroupRepository.SetPetitionAsInactive(aUserInformation.Details, aPetitionId);

            return true;
        }

        public IEnumerable<Petition> GetPetitions() {
            return theGroupRepository.GetPetitions();
        }

        public DisplayPetitionModel GetPetition(UserInformationModel<User> aUser, int aPetitionId) {
            DisplayPetitionModel myDisplayPetitionModel = new DisplayPetitionModel(aUser.Details.State);
            Petition myPetition = theGroupRepository.GetPetition(aPetitionId);
            myDisplayPetitionModel.ViewSignatureDetails = CanView(aUser, myPetition);
            myDisplayPetitionModel.Petition = myPetition;
            return myDisplayPetitionModel;
        }

        public bool HasSignedPetition(UserInformationModel<User> aUser, int aPetitionId) {
            bool myHasSigned = true;
            //Must be logged in to sign
            if (aUser != null) {
                PetitionSignature myPetitionSignature = theGroupRepository.GetPetitionSignature(aUser.Details, aPetitionId);
                myHasSigned = myPetitionSignature != null;
            }
            return myHasSigned;
        }

        public bool CanView(UserInformationModel<User> aUser, Petition aPetition) {
            bool myIsAdmin = false;
            if (aUser != null && aPetition.UserId == aUser.UserId
                || PermissionHelper<User>.AllowedToPerformAction(aUser, SocialPermission.View_Any_Petition_Signature_Details)) {
                myIsAdmin = true;
            }

            return myIsAdmin;
        }

        private bool ValidatePetition(CreatePetitionModel aPetitionModel) {
            if (string.IsNullOrEmpty(aPetitionModel.Title)) {
                theValidationDictionary.AddError("Title", aPetitionModel.Title, "A title for the petition is required.");
            } else if (!VarCharValidation.Valid50Length(aPetitionModel.Title)) {
                theValidationDictionary.AddError("Title", aPetitionModel.Title, "Your petition title is too long.");
            }

            if (string.IsNullOrEmpty(aPetitionModel.Description)) {
                theValidationDictionary.AddError("Description", aPetitionModel.Description, "A description of the petition is required.");
            }

            ValidateBasicLocationInformation(aPetitionModel);

            return theValidationDictionary.isValid;
        }

        private bool ValidatePetitionSignature(CreatePetitionSignatureModel aPetitionSignatureModel) {
            if (string.IsNullOrEmpty(aPetitionSignatureModel.Address)) {
                theValidationDictionary.AddError("Address", aPetitionSignatureModel.Address, "An address is required.");
            }

            if (!string.IsNullOrEmpty(aPetitionSignatureModel.Email) && !EmailValidation.IsValidEmail(aPetitionSignatureModel.Email)) {
                theValidationDictionary.AddError("Email", aPetitionSignatureModel.Email, "Email not valid.");
            }

            ValidateBasicLocationInformation(aPetitionSignatureModel);

            return theValidationDictionary.isValid;
        }

        private bool ValidateBasicLocationInformation(BasicLocationModel aBasicLocationModel) {
            if (string.IsNullOrEmpty(aBasicLocationModel.City)) {
                theValidationDictionary.AddError("City", aBasicLocationModel.City, "City is required.");
            }

            if (!ZipCodeValidation.IsValid(aBasicLocationModel.ZipCode)) {
                theValidationDictionary.AddError("ZipCode", aBasicLocationModel.ZipCode, "A valid 5 digit zip code is required.");
            }

            if (!DropDownItemValidation.IsValid(aBasicLocationModel.State)) {
                theValidationDictionary.AddError("State", aBasicLocationModel.State, "A state is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}
