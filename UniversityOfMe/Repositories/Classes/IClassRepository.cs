using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Classes {
    public interface IClassRepository {
        void AddToClassBoard(User aPostedByUser, int aClassId, string aReply);
        void AddToClassEnrollment(User aStudentToEnroll, int aClassId);
        Class CreateClass(User aCreatedByUser, string aUniversityId, string anAcademicTermId, string aClassCode, string aClassTitle, int aYear, string aDetails);
        void CreateReview(User aReviewingUser, int aClassId, int aRating, string aReview, bool anAnonymnous);
        Class GetClass(int aClubId);
        Class GetClass(string aClassCode, string anAcademicTermId, int aYear);
        IEnumerable<Class> GetClassesForUniversity(string aUniversityId);
        ClassReview GetClassReview(User aUser, int aClassId);
        IEnumerable<ClassEnrollment> GetEnrolledInClass(int aClassId);
        void MarkClassBoardAsViewed(User aUser, int aClassId);
        void RemoveFromClassEnrollment(User aStudentToRemove, int aClassId);
    }
}
