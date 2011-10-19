using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Helpers;
using UniversityOfMe.Helpers.Badges;
using UniversityOfMe.Helpers;
using UniversityOfMe.Helpers.Email;
using UniversityOfMe.Helpers.Configuration;
using System.Web.Mvc;

namespace UniversityOfMe.Repositories.Classes {
    public class EntityClassRepository : IClassRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public Class CreateClass(User aCreatedByUser, string aUniversityId, string aSubject, string aClassCode, string aClassTitle) {
            Class myClass = Class.CreateClass(0, aUniversityId, aCreatedByUser.Id, aSubject, aClassCode, aClassTitle, DateTime.UtcNow);
            theEntities.AddToClasses(myClass);
            theEntities.SaveChanges();
            return myClass;
        }

        public Class GetClass(int aClubId) {
            return (from c in theEntities.Classes
                    where c.Id == aClubId
                    select c).FirstOrDefault<Class>();
        }

        public Class GetClass(string aSubject, string aCourse) {
            return (from c in theEntities.Classes
                    where c.Subject == aSubject
                    && c.Course == aCourse
                    select c).FirstOrDefault<Class>();
        }

        public Class GetClass(string aUniversityId, string aSubject, string aCourse) {
            return (from c in theEntities.Classes
                    where c.Subject == aSubject
                    && c.Course == aCourse
                    && c.UniversityId == aUniversityId
                    select c).FirstOrDefault<Class>();
        }
    }
}