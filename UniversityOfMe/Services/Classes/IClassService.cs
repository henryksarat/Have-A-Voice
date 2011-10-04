using System.Collections.Generic;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Services.Classes {
    public interface IClassService {
        bool AddToClassBoard(UserInformationModel<User> aPostedByUser, int aClassId, string aReply);
        bool AddReplyToClassBoard(UserInformationModel<User> aPostedByUser, int aClassId, string aReply);
        void AddToClassEnrollment(UserInformationModel<User> aStudentToEnroll, int aClassId);
        //Class CreateClass(UserInformationModel<User> aCreatedByUser, CreateClassModel aCreateClassModel);
        bool CreateClassReview(UserInformationModel<User> aReviewingUser, int aClassId, string aRating, string aReview, bool anAnonymnous);
        void DeleteClassBoard(UserInformationModel<User> aDeletingUser, int aBoardId);
        void DeleteClassBoardReply(UserInformationModel<User> aDeletingUser, int aBoardReplyId);
        ClassDetailsModel GetClass(UserInformationModel<User> aViewingUser, string aClassUrlString, ClassViewType aClassViewType);
        ClassDetailsModel GetClass(UserInformationModel<User> aViewingUser, int aClassId, ClassViewType aClassViewType);
        ClassBoard GetClassBoard(UserInformationModel<User> aViewingUser, int aClassBoardId);
        IEnumerable<ClassEnrollment> GetEnrolledInClass(int aClassId);
        IEnumerable<Class> GetClassesForUniversity(string aUniversityId);
        IEnumerable<Class> GetClassesStudentIsEnrolledIn(UserInformationModel<User> aUser, string aUniversityId);
        bool IsClassExists(string aClassUrlString);
        void RemoveFromClassEnrollment(UserInformationModel<User> aStudentToRemove, int aClassId);
    }
}