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

namespace UniversityOfMe.Services.Classes {
    public class ClassService : IClassService {
        private IValidationDictionary theValidationDictionary;
        private IClassRepository theClassRepository;

        public ClassService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityClassRepository()) { }

        public ClassService(IValidationDictionary aValidationDictionary, IClassRepository aClassRepo) {
            theValidationDictionary = aValidationDictionary;
            theClassRepository = aClassRepo;
        }

        public bool AddToClassBoard(UserInformationModel<User> aPostedByUser, int aClassId, string aClassBoard) {
            if (!ValidBoardMessage(aClassBoard)) {
                return false;
            }

            theClassRepository.AddToClassBoard(aPostedByUser.Details, aClassId, aClassBoard);

            return true;
        }

        public bool AddReplyToClassBoard(UserInformationModel<User> aPostedByUser, int aClassBoardId, string aReply) {
            if (!ValidBoardMessage(aReply)) {
                return false;
            }

            theClassRepository.AddReplyToClassBoard(aPostedByUser.Details, aClassBoardId, aReply);

            return true;
        }

        public void AddToClassEnrollment(UserInformationModel<User> aStudentToEnroll, int aClassId) {
            theClassRepository.AddToClassEnrollment(aStudentToEnroll.Details, aClassId);
        }

        public Class CreateClass(UserInformationModel<User> aCreatedByUser, CreateClassModel aCreateClassModel) {
            if (!ValidClass(aCreateClassModel)) {
                return null;
            }

            Class myClass= theClassRepository.CreateClass(aCreatedByUser.Details, aCreateClassModel.UniversityId, 
                                                          aCreateClassModel.AcademicTermId, aCreateClassModel.ClassCode.Trim().ToUpper(), 
                                                          aCreateClassModel.ClassTitle.Trim(), aCreateClassModel.Year, aCreateClassModel.Details);

            return myClass;
        }

        public bool CreateClassReview(UserInformationModel<User> aReviewingUser, int aClassId, string aRating, string aReview, bool anAnonymnous) {
            if (!ValidClassReview(aReviewingUser.Details, aClassId, aRating, aReview)) {
                return false;
            }
            
            theClassRepository.CreateReview(aReviewingUser.Details, aClassId, int.Parse(aRating), aReview, anAnonymnous);

            return true;
        }

        public void DeleteClassBoard(UserInformationModel<User> aDeletingUser, int aBoardId) {
            ClassBoard myClassBoard = theClassRepository.GetClassBoard(aBoardId);
            if (myClassBoard != null) {
                if (myClassBoard.UserId == aDeletingUser.UserId
                    || PermissionHelper<User>.AllowedToPerformAction(aDeletingUser, SocialPermission.Delete_Any_Class_Board)) {
                    theClassRepository.DeleteClassBoard(aDeletingUser.Details, aBoardId);
                } else {
                    throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
                }
            }
        }

        public void DeleteClassBoardReply(UserInformationModel<User> aDeletingUser, int aBoardReplyId) {
            ClassBoardReply myClassBoardReply = theClassRepository.GetClassBoardReply(aBoardReplyId);
            if (myClassBoardReply != null) {
                if (myClassBoardReply.UserId == aDeletingUser.UserId
                    || PermissionHelper<User>.AllowedToPerformAction(aDeletingUser, SocialPermission.Delete_Any_Class_Board)) {
                        theClassRepository.DeleteClassBoardReply(aDeletingUser.Details, aBoardReplyId);
                } else {
                    throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
                }
            }
        }

        public Class GetClass(UserInformationModel<User> aViewingUser, int aClassId, ClassViewType aClassViewType) {
            if (aClassViewType == ClassViewType.Discussion) {
                theClassRepository.MarkClassBoardAsViewed(aViewingUser.Details, aClassId);
            }
            return theClassRepository.GetClass(aClassId);
        }

        public Class GetClass(UserInformationModel<User> aViewingUser, string aClassUrlString, ClassViewType aClassViewType) {
            string[] mySplitClass = URLHelper.FromUrlFriendly(aClassUrlString);
            
            Class myClass = theClassRepository.GetClass(mySplitClass[0], mySplitClass[1], int.Parse(mySplitClass[2]));
            if (aClassViewType == ClassViewType.Discussion) {
                if (aViewingUser != null) {
                    theClassRepository.MarkClassBoardAsViewed(aViewingUser.Details, myClass.Id);
                }
            }
            return myClass;
        }

        public ClassBoard GetClassBoard(UserInformationModel<User> aViewingUser, int aClassBoardId) {
            ClassBoard myClassBoard = theClassRepository.GetClassBoard(aClassBoardId);
            ClassEnrollment myClassEnrollment = theClassRepository.GetClassEnrollment(aViewingUser.Details, myClassBoard.ClassId);

            if (myClassEnrollment == null) {
                throw new PermissionDenied("You must be enrolled in the class to view this class discussion message.");
            }

            theClassRepository.MarkClassBoardReplyAsViewed(aViewingUser.Details, aClassBoardId);

            return myClassBoard;
        }

        public IEnumerable<Class> GetClassesForUniversity(string aUniversityId) {
            return theClassRepository.GetClassesForUniversity(aUniversityId);
        }

        public IEnumerable<ClassEnrollment> GetEnrolledInClass(int aClassId) {
            return theClassRepository.GetEnrolledInClass(aClassId)
                .Where(ce => PrivacyHelper.PrivacyAllows(ce.User, SocialPrivacySetting.Display_Class_Enrollment));  
        }

        public bool IsClassExists(string aClassUrlString) {
            string[] mySplitClass = URLHelper.FromUrlFriendly(aClassUrlString);

            if (mySplitClass.Length != 3) {
                return false;
            }

            Class myClass = theClassRepository.GetClass(mySplitClass[0], mySplitClass[1], int.Parse(mySplitClass[2]));
            return myClass == null ? false : true;
        }

        public void RemoveFromClassEnrollment(UserInformationModel<User> aStudentToRemove, int aClassId) {
            theClassRepository.RemoveFromClassEnrollment(aStudentToRemove.Details, aClassId);
        }

        private bool HasReviewedClass(User aReviwer, int aClassId) {
            ClassReview myReview = theClassRepository.GetClassReview(aReviwer, aClassId);
            return myReview != null;
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

            if (IsClassExists(URLHelper.ToUrlFriendly(aCreateClassModel.ClassCode + " " + aCreateClassModel.AcademicTermId + " " + aCreateClassModel.Year.ToString()))) {
                theValidationDictionary.AddError("Class", string.Empty, "That class already exists for that academic term and year. Please find it and post in there.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidClassReview(User aUser, int aClassId, string aRating, string aReview) {
            int myParsedNumber;
            int.TryParse(aRating, out myParsedNumber);
            if (!RangeValidation.IsWithinRange(myParsedNumber, 1, 5)) {
                theValidationDictionary.AddError("Rating", myParsedNumber.ToString(), "A rating between 1 and 5 is required.");
            }

            if (string.IsNullOrEmpty(aReview)) {
                theValidationDictionary.AddError("Review", aReview, "A review is required.");
            }

            if (theClassRepository.GetClassReview(aUser, aClassId) != null) {
                theValidationDictionary.AddError("Review", aReview, "You have already reviewed this class.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidBoardMessage(string aBody) {
            if (string.IsNullOrEmpty(aBody)) {
                theValidationDictionary.AddError("BoardMessage", aBody, "Text is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}
