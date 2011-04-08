using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Classes {
    public class EntityClassRepository : IClassRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public Class CreateClass(User aCreatedByUser, string aUniversityId, string anAcademicTermId, string aClassCode, string aClassTitle, int aYear, string aDetails) {
            Class myClass = Class.CreateClass(0, aUniversityId, aCreatedByUser.Id, anAcademicTermId, aClassCode, aClassTitle, aYear, DateTime.UtcNow);
            myClass.Details = aDetails;
            theEntities.AddToClasses(myClass);
            theEntities.SaveChanges();
            return myClass;
        }

        public void CreateClassReply(User aPostedByUser, int aClassId, string aReply) {
            ClassReply myReply = ClassReply.CreateClassReply(0, aClassId, aPostedByUser.Id, aReply, DateTime.UtcNow);
            theEntities.AddToClassReplies(myReply);
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
    }
}