using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Scraper {
    public class EntityScraperRepository : IScraperRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public bool ClassExists(string aUniversityId, string aSubject, string aCourse) {
            return (from c in theEntities.Classes
                    where c.UniversityId == aUniversityId
                    && c.Subject == aSubject
                    && c.Course == aCourse
                    select c).Count() > 0 ? true : false;
        }

        public Class CreateClass(string aUniversityId, int aCreatedById, string aSubject, string aCourse, string aTitle) {
            Class myClass = Class.CreateClass(0, aUniversityId, aCreatedById, aSubject, aCourse, aTitle, DateTime.UtcNow);
            theEntities.AddToClasses(myClass);
            theEntities.SaveChanges();
            return myClass;
        }

        public Class GetClass(string aUniversityId, string aSubject, string aCourse) {
            return (from c in theEntities.Classes
                    where c.UniversityId == aUniversityId
                    && c.Subject == aSubject
                    && c.Course == aCourse
                    select c).FirstOrDefault<Class>();
        }

        public void CreateClassProfessor(int aProfessorId, int aClassId) {
            ClassProfessor myClassProfessor = ClassProfessor.CreateClassProfessor(0, aClassId, aProfessorId);
            theEntities.AddToClassProfessors(myClassProfessor);
            theEntities.SaveChanges();
        }
    }
}