using System.Collections.Generic;
using System.Text.RegularExpressions;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories.Classes;
using System;
using System.Linq;
using Social.Generic.Helpers;
using Social.Admin.Helpers;
using Social.Admin.Exceptions;
using UniversityOfMe.Services.Professors;

namespace UniversityOfMe.Services.Classes {
    public class ClassService : IClassService {
        private IValidationDictionary theValidationDictionary;
        private IClassRepository theClassRepository;
        private IProfessorService theProfessorService;

        public ClassService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new ProfessorService(aValidationDictionary), new EntityClassRepository()) { }

        public ClassService(IValidationDictionary aValidationDictionary, IProfessorService aProfessorService, IClassRepository aClassRepo) {
            theValidationDictionary = aValidationDictionary;
            theClassRepository = aClassRepo;
            theProfessorService = aProfessorService;
        }

        public Class CreateClass(UserInformationModel<User> aCreatedByUser, CreateClassModel aCreateClassModel) {
            if (!ValidClass(aCreateClassModel)) {
                return null;
            }

            Class myClass= theClassRepository.CreateClass(aCreatedByUser.Details, aCreateClassModel.UniversityId,
                                                          aCreateClassModel.ClassSubject.Trim().ToUpper(),
                                                          aCreateClassModel.ClassCourse.Trim().ToUpper(),
                                                          aCreateClassModel.ClassTitle.Trim());

            return myClass;
        }

        public ClassDetailsModel GetClass(UserInformationModel<User> aViewingUser, int aClassId, ClassViewType aClassViewType) {
            Class myClass = theClassRepository.GetClass(aClassId);

            IEnumerable<Professor> myProfessors = new List<Professor>();

            return new ClassDetailsModel() {
                Class = myClass,
                Professors = myProfessors
            };
        }

        private bool ValidClass(CreateClassModel aCreateClassModel) {
            if (string.IsNullOrEmpty(aCreateClassModel.UniversityId) || aCreateClassModel.UniversityId.Equals(Constants.SELECT)) {
                theValidationDictionary.AddError("UniversityId", aCreateClassModel.UniversityId, "A university is required.");
            }

            Regex myAlphaNumericRegex = new Regex("^[a-zA-Z0-9]+$");
            if (!myAlphaNumericRegex.IsMatch(aCreateClassModel.ClassSubject)) {
                theValidationDictionary.AddError("ClassSubject", aCreateClassModel.ClassSubject, "A class code must be provided and must be only letters and numbers.");
            }

            if (!myAlphaNumericRegex.IsMatch(aCreateClassModel.ClassCourse)) {
                theValidationDictionary.AddError("ClassCourse", aCreateClassModel.ClassCourse, "A class code must be provided and must be only letters and numbers.");
            }

            if (string.IsNullOrEmpty(aCreateClassModel.ClassTitle)) {
                theValidationDictionary.AddError("ClassTitle", aCreateClassModel.ClassTitle, "A class title must be provided.");
            }

            if (theClassRepository.GetClass(aCreateClassModel.ClassSubject, aCreateClassModel.ClassCourse) != null) {
                theValidationDictionary.AddError("Class", string.Empty, "That class already exists.");
            }

            

            return theValidationDictionary.isValid;
        }
    }
}
