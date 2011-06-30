using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Classes {
    public class EntityClassRepository : IClassRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void AddToClassBoard(User aPostedByUser, int aClassId, string aReply) {
            ClassBoard myBoard = ClassBoard.CreateClassBoard(0, aClassId, aPostedByUser.Id, aReply, DateTime.UtcNow, false);
            theEntities.AddToClassBoards(myBoard);

            theEntities.SaveChanges();

            UpdateClassEnrollmentsForNewBoardPostWithoutSave(aPostedByUser, aClassId, myBoard);

            ClassBoardViewState myViewState = ClassBoardViewState.CreateClassBoardViewState(0, aPostedByUser.Id, myBoard.Id, true, DateTime.UtcNow);
            theEntities.AddToClassBoardViewStates(myViewState);

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

        public void AddReplyToClassBoard(User aPostedByUser, int aClassBoardId, string aReply) {
            ClassBoardReply myClassBoardReply = ClassBoardReply.CreateClassBoardReply(0, aPostedByUser.Id, aClassBoardId, aReply, DateTime.UtcNow, false);
            theEntities.AddToClassBoardReplies(myClassBoardReply);

            ClassBoard myBoard = GetClassBoard(aClassBoardId);

            DateTime myDateTime = DateTime.UtcNow;

            UpdateBoardViewedStateWithoutSave(aPostedByUser, aClassBoardId, myDateTime);

            VerifyAuthorOfBoardHasViewStateWithoutSave(aPostedByUser, aClassBoardId, myBoard, myDateTime);

            theEntities.SaveChanges();
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

        public void DeleteClassBoard(User aDeletingUser, int aBoardId) {
            ClassBoard myClassBoard = GetClassBoard(aBoardId);
            myClassBoard.Deleted = true;
            myClassBoard.DeletedByUserId = aDeletingUser.Id;
            myClassBoard.DeletedByDateTimeStamp = DateTime.UtcNow;

            IEnumerable<ClassBoardViewState> myViewStates = GetClassBoardViewStates(aBoardId);
            foreach (ClassBoardViewState myViewState in myViewStates) {
                theEntities.DeleteObject(myViewState);
            }

            UpdateClassEnrollmentsToNotReferToDeletedBoardWithoutSave(myClassBoard);

            theEntities.SaveChanges();
        }

        public void DeleteClassBoardReply(User aDeletingUser, int aBoardReplyId) {
            ClassBoardReply myReply = GetClassBoardReply(aBoardReplyId);
            myReply.Deleted = true;
            myReply.DeletedByUserId = aDeletingUser.Id;
            myReply.DeletedDateTimeStamp = DateTime.UtcNow;

            IEnumerable<ClassBoardReply> myClassBoardReplies = GetClassBoardRepliesForUser(aDeletingUser, myReply.ClassBoardId);
            //Didn't save the one we are deleting right now
            if (myClassBoardReplies.Count<ClassBoardReply>() == 1) {
                ClassBoardViewState myViewState = GetClassBoardViewState(aDeletingUser, myReply.ClassBoardId);
                theEntities.DeleteObject(myViewState);
            }

            theEntities.SaveChanges();
        }

        public ClassBoard GetClassBoard(int aClassBoardId) {
            return (from cb in theEntities.ClassBoards
                    where cb.Id == aClassBoardId
                    && !cb.Deleted
                    select cb).FirstOrDefault<ClassBoard>();
        }

        public ClassBoardReply GetClassBoardReply(int aClassBoardReplyId) {
            return (from r in theEntities.ClassBoardReplies
                    where r.Id == aClassBoardReplyId
                    && !r.Deleted
                    select r).FirstOrDefault<ClassBoardReply>();
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

        public ClassEnrollment GetClassEnrollment(User aUser, int aClassId) {
            return (from c in theEntities.ClassEnrollments
                    where c.UserId == aUser.Id && c.ClassId == aClassId
                    select c).FirstOrDefault<ClassEnrollment>();
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

        public void MarkClassBoardReplyAsViewed(User aUser, int aClassBoardId) {
            ClassBoardViewState myViewState = GetClassBoardViewState(aUser, aClassBoardId);
            if (myViewState != null) {
                myViewState.Viewed = true;
                myViewState.DateTimeStamp = DateTime.UtcNow;
                theEntities.SaveChanges();
            }
        }

        public void RemoveFromClassEnrollment(User aStudentToRemove, int aClassId) {
            ClassEnrollment myEnrollment = GetClassEnrollment(aStudentToRemove, aClassId);
            theEntities.DeleteObject(myEnrollment);
            theEntities.SaveChanges();
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

        private ClassBoardViewState GetClassBoardViewState(User aUser, int aClassBoardId) {
            return (from v in theEntities.ClassBoardViewStates
                    where v.ClassBoardId == aClassBoardId
                    && v.UserId == aUser.Id
                    select v).FirstOrDefault<ClassBoardViewState>();
        }

        private IEnumerable<ClassBoardViewState> GetClassBoardViewStates(int aClassBoardId) {
            return (from v in theEntities.ClassBoardViewStates
                    where v.ClassBoardId == aClassBoardId
                    select v);
        }

        private IEnumerable<ClassBoardReply> GetClassBoardRepliesForUser(User aUser, int aClassBoardId) {
            return (from r in theEntities.ClassBoardReplies
                    where r.UserId == aUser.Id
                    && r.ClassBoardId == aClassBoardId
                    && !r.Deleted
                    select r);
        }

        private void UpdateBoardViewedStateWithoutSave(User aPostedByUser, int aClassBoardId, DateTime myDateTime) {
            IEnumerable<ClassBoardViewState> myViewStates = GetClassBoardViewStates(aClassBoardId);
            bool myHasBoardViewedState = false;

            foreach (ClassBoardViewState myViewState in myViewStates) {
                if (aPostedByUser.Id == myViewState.UserId) {
                    myViewState.Viewed = true;
                    myHasBoardViewedState = true;
                } else {
                    myViewState.Viewed = false;
                }

                myViewState.DateTimeStamp = myDateTime;
            }

            if (!myHasBoardViewedState) {
                ClassBoardViewState myViewState = ClassBoardViewState.CreateClassBoardViewState(0, aPostedByUser.Id, aClassBoardId, true, myDateTime);
                theEntities.AddToClassBoardViewStates(myViewState);
            }
        }

        private void UpdateClassEnrollmentsForNewBoardPostWithoutSave(User aPostedByUser, int aClassId, ClassBoard myBoard) {
            IEnumerable<ClassEnrollment> myEnrollments = GetClassEnrollments(aClassId);
            DateTime myCurrentDateTime = DateTime.UtcNow;

            foreach (ClassEnrollment myClassEnrollment in myEnrollments) {
                if (aPostedByUser.Id == myClassEnrollment.UserId) {
                    myClassEnrollment.BoardViewed = true;
                } else {
                    myClassEnrollment.BoardViewed = false;
                }
                myClassEnrollment.LastBoardPost = myCurrentDateTime;
                myClassEnrollment.LastClassBoardId = myBoard.Id;
            }
        }

        private void UpdateClassEnrollmentsToNotReferToDeletedBoardWithoutSave(ClassBoard myClassBoard) {
            IEnumerable<ClassEnrollment> myEnrollments = GetClassEnrollments(myClassBoard.ClassId);
            foreach (ClassEnrollment myClassEnrollment in myEnrollments) {
                if (myClassEnrollment.LastClassBoardId == myClassBoard.Id) {
                    myClassEnrollment.BoardViewed = true;
                    myClassEnrollment.LastBoardPost = null;
                    myClassEnrollment.LastClassBoardId = null;
                }
            }
        }

        private void VerifyAuthorOfBoardHasViewStateWithoutSave(User aPostedByUser, int aClassBoardId, ClassBoard myBoard, DateTime myDateTime) {
            ClassBoardViewState myAuthorViewState = GetClassBoardViewState(aPostedByUser, aClassBoardId);
            if (myAuthorViewState == null) {
                myAuthorViewState = ClassBoardViewState.CreateClassBoardViewState(0, myBoard.UserId, aClassBoardId, false, myDateTime);
                theEntities.AddToClassBoardViewStates(myAuthorViewState);
            }
        }
    }
}