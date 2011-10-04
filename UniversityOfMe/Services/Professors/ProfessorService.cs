using System.Collections.Generic;
using System.Web;
using Social.Generic.Constants;
using Social.Generic.Exceptions;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Professors;
using UniversityOfMe.Helpers;
using Social.Photo.Helpers;
using UniversityOfMe.Helpers.Constants;
using System;
using Social.Photo.Exceptions;
using UniversityOfMe.Helpers.AWS;
using UniversityOfMe.Helpers.Configuration;

namespace UniversityOfMe.Services.Professors {
    public class ProfessorService : IProfessorService {
        private IValidationDictionary theValidationDictionary;
        private IProfessorRepository theProfessorRepository;

        public ProfessorService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityProfessorRepository()) { }

        public ProfessorService(IValidationDictionary aValidationDictionary, IProfessorRepository aProfessorRepo) {
            theValidationDictionary = aValidationDictionary;
            theProfessorRepository = aProfessorRepo;
        }

        public IEnumerable<Professor> GetProfessorsAssociatedWithClass(int aClassId) {
            return theProfessorRepository.GetProfessorsAssociatedWithClass(aClassId);
        }

        public IEnumerable<Professor> GetProfessorsForUniversity(string aUniversityId) {
            return theProfessorRepository.GetProfessorsByUniversity(aUniversityId);
        }

        public bool CreateProfessor(UserInformationModel<User> aCreatingUser, string aUniversityId, string aFirstName, string aLastName, HttpPostedFileBase aProfessorImage) {
            if (!ValidProfessor(aUniversityId, aFirstName, aLastName, aProfessorImage)) {
                return false;
            }

            string myProfessorImage = string.Empty;

            if (aProfessorImage != null) {
                try {
                    myProfessorImage = AWSPhotoHelper.TakeImageAndResizeAndUpload(aProfessorImage,
                        AWSHelper.GetClient(),
                        SiteConfiguration.ProfessorPhotosBucket(),
                        aUniversityId + "_" + aFirstName.Trim().Replace(" ", "") + "_" + aLastName.Trim().Replace(" ", ""),
                        ProfessorConstants.PROFESSOR_MAX_SIZE);
                } catch (Exception myException) {
                    throw new PhotoException("Unable to upload the professor image.", myException);
                }
            }


            theProfessorRepository.CreateProfessor(aCreatingUser.Details, aUniversityId, aFirstName.Trim(), aLastName.Trim(), myProfessorImage);
            return true;
        }


        public Professor GetProfessor(int aProfessorId) {
            return theProfessorRepository.GetProfessor(aProfessorId);
        }

        public bool CreateProfessorImageSuggestion(UserInformationModel<User> aSuggestingUser, int aProfessorId, HttpPostedFileBase aProfessorImage) {
            if (!ValidProfessorSuggestedImage(aProfessorImage)) {
                return false;
            }

            string myProfessorImage = string.Empty;

            try {
                myProfessorImage = AWSPhotoHelper.TakeImageAndResizeAndUpload(aProfessorImage,
                        AWSHelper.GetClient(),
                        SiteConfiguration.ProfessorPhotosBucket(),
                        "Suggested_" + aProfessorId,
                        ProfessorConstants.PROFESSOR_MAX_SIZE);
            } catch (Exception myException) {
                throw new PhotoException("Unable to upload the professor image.", myException);
            }

            try {
                theProfessorRepository.CreateProfessorSuggestedPicture(aSuggestingUser.Details, aProfessorId, myProfessorImage);
            } catch (Exception myException) {
                AWSPhotoHelper.PhysicallyDeletePhoto(AWSHelper.GetClient(), SiteConfiguration.ProfessorPhotosBucket(), myProfessorImage);
                throw new Exception("Unable to create the professor suggested picture database entry so we removed the image.", myException);
            }

            return true;
        }

        public Professor GetProfessor(string aUniversityId, string aProfessorName) {
            string myName = URLHelper.FromUrlFriendlyToNormalString(aProfessorName);
            string[] mySplitName = SplitIpProfessorName(myName);

            if (mySplitName.Length != 2) {
                throw new CustomException("The professors name doesn't have a first and last name.");
            }
            string myFirstname = mySplitName[0];
            string myLastname = mySplitName[1];


            return theProfessorRepository.GetProfessor(aUniversityId, myFirstname, myLastname);
        }

        public bool IsProfessorExists(string aUniversityId, string aFullname) {
            string myName = URLHelper.FromUrlFriendlyToNormalString(aFullname);
            string[] mySplitName = SplitIpProfessorName(myName);

            if (mySplitName.Length != 2) {
                return false;
            }
            string myFirstname = mySplitName[0];
            string myLastname = mySplitName[1];

            Professor myProfessor = theProfessorRepository.GetProfessor(aUniversityId, myFirstname, myLastname);

            return myProfessor == null ? false : true;
        }

        private bool ValidProfessor(string aUniversityId, string aFirstName, string aLastName, HttpPostedFileBase aProfessorImage) {
            if (string.IsNullOrEmpty(aFirstName)) {
                theValidationDictionary.AddError("FirstName", aFirstName, "First name is required.");
            }

            if (string.IsNullOrEmpty(aLastName)) {
                theValidationDictionary.AddError("LastName", aLastName, "Last name is required.");
            }

            if (string.IsNullOrEmpty(aUniversityId) || aUniversityId.Equals(Constants.SELECT)) {
                theValidationDictionary.AddError("UniversityId", aUniversityId, "A university is required.");
            }

            if (theProfessorRepository.GetProfessor(aUniversityId, aFirstName, aLastName) != null) {
                theValidationDictionary.AddError("FirstName", aFirstName, "That professor already exists in the system. Please do a search and review that professor.");
                theValidationDictionary.AddError("LastName", aLastName, string.Empty);
            }

            ValidProfessorImage(aProfessorImage);

            return theValidationDictionary.isValid;
        }

        private bool ValidProfessorSuggestedImage(HttpPostedFileBase aProfessorImage) {
            if (aProfessorImage == null || !ValidProfessorImage(aProfessorImage)) {
                theValidationDictionary.AddError("ProfessorImage", string.Empty, PhotoValidation.INVALID_IMAGE);
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidProfessorImage(HttpPostedFileBase aProfessorImage) {
            if (aProfessorImage != null && !PhotoValidation.IsValidImageFile(aProfessorImage.FileName)) {
                theValidationDictionary.AddError("ProfessorImage", aProfessorImage.FileName, PhotoValidation.INVALID_IMAGE);
            }

            return theValidationDictionary.isValid;
        }

        private string[] SplitIpProfessorName(string aProfessorName) {
            int myLastPeriod = aProfessorName.IndexOf(' ');
            string[] myName = new string[2];
            myName[0] = aProfessorName.Substring(0, myLastPeriod).Trim();
            myName[1] = aProfessorName.Substring(myLastPeriod + 1, aProfessorName.Length - myLastPeriod-1);
            return myName;
        }
    }
}
