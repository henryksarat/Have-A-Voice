using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Classes {
    public class EntityClassRepository : IClassRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void AddToClassBoard(User aPostedByUser, int aClassId, string aReply) {
            ClassBoard myBoard = ClassBoard.CreateClassBoard(0, aClassId, aPostedByUser.Id, aReply, DateTime.UtcNow);
            theEntities.AddToClassBoards(myBoard);

            IEnumerable<ClassEnrollment> myEnrollments = GetClassEnrollments(aClassId);
            DateTime myCurrentDateTime = DateTime.UtcNow;

            foreach (ClassEnrollment myClassEnrollment in myEnrollments) {
                if (aPostedByUser.Id == myClassEnrollment.UserId) {
                    myClassEnrollment.BoardViewed = true;
                } else {
                    myClassEnrollment.BoardViewed = false;
                    myClassEnrollment.LastBoardPost = myCurrentDateTime;
                }
            }

            theEntities.SaveChanges();
        }

        public void AddToClassEnrollment(User aStudentToEnroll, int aClassId) {
            ClassEnrollment myCurrentEnrollment = GetClassEnrollment(aStudentToEnroll, aClassId);
            if (myCurrentEnrollment == null) {
                int myClassBoardCount = GetClassBoardCount(aClassId);
                bool myViewedBoard = myClassBoardCount == 0 ? true : false;
                ClassEnrollment myEnrollment = ClassEnrollment.CreateClassEnrollment(0, aClassId, aStudentToEnroll.Id, DateTime.UtcNow, myViewedBoard);
                theEntities.AddToClassEnrollments(myEnrollment);
                theEntities.SaveChanges();
            }
        }

        public Class CreateClass(User aCreatedByUser, string aUniversityId, string anAcademicTermId, string aClassCode, string aClassTitle, int aYear, string aDetails) {
            Class myClass = Class.CreateClass(0, aUniversityId, aCreatedByUser.Id, anAcademicTermId, aClassCode, aClassTitle, aYear, DateTime.UtcNow);
            myClass.Details = aDetails;
            theEntities.AddToClasses(myClass);
            theEntities.SaveChanges();
            return myClass;
        }

        public void CreateReview(User aReviewingUser, int aClassId, int aRating, string aReview, bool anAnonymnous) {
            ClassReview myReview = ClassReview.CreateClassReview(0, aClassId, aReviewingUser.Id, aRating, aReview, anAnonymnous, DateTime.UtcNow);
            theEntities.AddToClassReviews(myReview);
            theEntities.SaveChanges();
        }

        public Class GetClass(int aClubId) {
            return (from c in theEntities.Classes
                    where c.Id == aClubId
                    select c).FirstOrDefault<Class>();
        }

        public Class GetClass(string aClassCode, string anAcademicTermId, int aYear) {
            return (from c in theEntities.Classes
                    where c.ClassCode == aClassCode
                    && c.AcademicTermId == anAcademicTermId
                    && c.Year == aYear
                    select c).FirstOrDefault<Class>();
        }

        public IEnumerable<Class> GetClassesForUniversity(string aUniversityId) {
            return (from c in theEntities.Classes
                    where c.UniversityId == aUniversityId
                    select c).ToList<Class>();
        }

        public ClassReview GetClassReview(User aUser, int aClassId) {
            return (from cr in theEntities.ClassReviews
                    where cr.UserId == aUser.Id && cr.ClassId == aClassId
                    select cr).FirstOrDefault<ClassReview>();
        }

        public IEnumerable<ClassEnrollment> GetEnrolledInClass(int aClassId) {
            return (from ce in theEntities.ClassEnrollments
                    where ce.ClassId == aClassId
                    select ce).ToList<ClassEnrollment>();
        }

        public void MarkClassBoardAsViewed(User aUser, int aClassId) {
            ClassEnrollment myEnrollment = GetClassEnrollment(aUser, aClassId);
            if (myEnrollment != null) {
                myEnrollment.BoardViewed = true;
                theEntities.SaveChanges();
            }
        }

        public void RemoveFromClassEnrollment(User aStudentToRemove, int aClassId) {
            ClassEnrollment myEnrollment = GetClassEnrollment(aStudentToRemove, aClassId);
            theEntities.DeleteObject(myEnrollment);
            theEntities.SaveChanges();
        }

        private ClassEnrollment GetClassEnrollment(User aUser, int aClassId) {
            return (from c in theEntities.ClassEnrollments
                    where c.UserId == aUser.Id && c.ClassId == aClassId
                    select c).FirstOrDefault<ClassEnrollment>();
        }

        private IEnumerable<ClassEnrollment> GetClassEnrollments(int aClassId) {
            return (from ce in theEntities.ClassEnrollments
                    where ce.ClassId == aClassId
                    select ce);
        }

        private int GetClassBoardCount(int aClassId) {
            return (from cb in theEntities.ClassBoards
                    where cb.ClassId == aClassId
                    select cb).Count<ClassBoard>();
        }
    }
}