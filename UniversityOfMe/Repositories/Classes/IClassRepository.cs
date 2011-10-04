using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Classes {
    public interface IClassRepository {
        void AddToClassBoard(User aPostedByUser, int aClassId, string aReply);
        void AddReplyToClassBoard(User aPostedByUser, int aClassBoardId, string aReply);
        void AddToClassEnrollment(User aStudentToEnroll, int aClassId);
        //Class CreateClass(User aCreatedByUser, string aUniversityId, string anAcademicTermId, string aClassCode, string aClassTitle, int aYear, string aDetails);
        void CreateReview(User aReviewingUser, int aClassId, int aRating, string aReview, bool anAnonymnous);
        void DeleteClassBoard(User aDeletingUser, int aBoardId);
        void DeleteClassBoardReply(User aDeletingUser, int aBoardReplyId);
        Class GetClass(int aClubId);
        Class GetClass(string aSubject, string aCourse, string aSection, string anAcademicTermId, int aYear);        
        ClassBoard GetClassBoard(int aClassBoardId);
        ClassBoardReply GetClassBoardReply(int aClassBoardReplyId);
        ClassEnrollment GetClassEnrollment(User aUser, int aClassId);
        IEnumerable<Class> GetClassesEnrolledIn(User aUser, string aUniversityId);
        IEnumerable<Class> GetClassesForUniversity(string aUniversityId);
        ClassReview GetClassReview(User aUser, int aClassId);
        IEnumerable<ClassEnrollment> GetEnrolledInClass(int aClassId);
        void MarkClassBoardAsViewed(User aUser, int aClassId);
        void MarkClassBoardReplyAsViewed(User aUser, int aClassBoardId);
        void RemoveFromClassEnrollment(User aStudentToRemove, int aClassId);
    }
}
