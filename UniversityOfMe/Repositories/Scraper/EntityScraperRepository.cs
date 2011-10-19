using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Scraper {
    public class EntityScraperRepository : IScraperRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public bool ProfessorExists(string aUniversityId, string aFirstname, string aLastName) {
            return (from p in theEntities.Professors
                    where p.FirstName == aFirstname
                    && p.LastName == aLastName
                    && p.UniversityId == aUniversityId
                    select p).Count() > 0 ? true : false;
        }

        public Professor CreateProfessor(string aUniversityId, int aCreatedById, string aFirstName, string aLastName) {
            Professor myProfessor = Professor.CreateProfessor(0, aUniversityId, aCreatedById, aFirstName, aLastName, string.Empty, DateTime.UtcNow);
            theEntities.AddToProfessors(myProfessor);
            theEntities.SaveChanges();
            return myProfessor;
        }

        public Professor GetProfessor(string aUniversityId, string aFirstname, string aLastName) {
            return (from p in theEntities.Professors
                    where p.UniversityId == aUniversityId
                    && p.FirstName == aFirstname
                    && p.LastName == aLastName
                    select p).FirstOrDefault<Professor>();
        }

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