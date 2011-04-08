using Social.Generic.Constants;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Classes;
using UniversityOfMe.Services.Classes;
using Social.Generic.Models;
using UniversityOfMe.Models.View;
using System.Collections.Generic;
using UniversityOfMe.Helpers;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;

namespace HaveAVoice.Services.Classes {
    public class ClassService : IClassService {
        private IValidationDictionary theValidationDictionary;
        private IClassRepository theClassRepository;

        public ClassService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityClassRepository()) { }

        public ClassService(IValidationDictionary aValidationDictionary, IClassRepository aClassRepo) {
            theValidationDictionary = aValidationDictionary;
            theClassRepository = aClassRepo;
        }


        public Class CreateClass(UserInformationModel<User> aCreatedByUser, CreateClassModel aCreateClassModel) {
            if (!ValidClass(aCreateClassModel)) {
                return null;
            }

            Class myClass= theClassRepository.CreateClass(aCreatedByUser.Details, aCreateClassModel.UniversityId, 
                                                          aCreateClassModel.AcademicTermId, aCreateClassModel.ClassCode.ToUpper(), 
                                                          aCreateClassModel.ClassTitle, aCreateClassModel.Year, aCreateClassModel.Details);

            return myClass;
        }

        public bool CreateClassReply(UserInformationModel<User> aPostedByUser, int aClassId, string aReply) {
            if (!ValidClass(aReply)) {
                return false;
            }

            theClassRepository.CreateClassReply(aPostedByUser.Details, aClassId, aReply);

            return true;
        }

        public Class GetClass(UserInformationModel<User> aViewingUser, int aClassId) {
            return theClassRepository.GetClass(aClassId);
        }

        public Class GetClass(UserInformationModel<User> aViewingUser, string aClassUrlString) {
            string[] mySplitClass = URLHelper.FromUrlFriendly(aClassUrlString);
            return theClassRepository.GetClass(mySplitClass[0], mySplitClass[1], int.Parse(mySplitClass[2]));
        }

        public IEnumerable<Class> GetClassesForUniversity(string aUniversityId) {
            return theClassRepository.GetClassesForUniversity(aUniversityId);
        }

        public bool IsClassExists(string aClassUrlString) {
            string[] mySplitClass = URLHelper.FromUrlFriendly(aClassUrlString);

            if (mySplitClass.Length != 3) {
                return false;
            }

            Class myClass = theClassRepository.GetClass(mySplitClass[0], mySplitClass[1], int.Parse(mySplitClass[2]));
            return myClass == null ? false : true;
        }

        private bool ValidClass(CreateClassModel aCreateClassModel) {
            if (string.IsNullOrEmpty(aCreateClassModel.UniversityId) || aCreateClassModel.UniversityId.Equals(Constants.SELECT)) {
                theValidationDictionary.AddError("UniversityId", aCreateClassModel.UniversityId, "A university is required.");
            }

            if (!DropDownItemValidation.IsValid(aCreateClassModel.Year.ToString())) {
                theValidationDictionary.AddError("Year", aCreateClassModel.Year.ToString(), "A year is required for the class.");
            }

            if (!DropDownItemValidation.IsValid(aCreateClassModel.AcademicTermId)) {
                theValidationDictionary.AddError("AcademicTermId", aCreateClassModel.AcademicTermId, "A academic quarter must be selected for the class.");
            }

            Regex myAlphaNumericRegex = new Regex("^[a-zA-Z0-9]+$");
            if (!myAlphaNumericRegex.IsMatch(aCreateClassModel.ClassCode)) {
                theValidationDictionary.AddError("ClassCode", aCreateClassModel.ClassCode, "A class code must be provided and must be only letters and numbers.");
            }

            if (string.IsNullOrEmpty(aCreateClassModel.ClassTitle)) {
                theValidationDictionary.AddError("ClassTitle", aCreateClassModel.ClassTitle, "A class title must be provided.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidClass(string aBody) {
            if (string.IsNullOrEmpty(aBody)) {
                theValidationDictionary.AddError("Reply", aBody, "Text is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}
