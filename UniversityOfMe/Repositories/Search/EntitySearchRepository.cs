using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using System;

namespace UniversityOfMe.Repositories.Search {
    public class EntitySearchRepository : ISearchRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public IEnumerable<Class> GetClassByTitle(string aUniversityId, string aTitle) {
            return (from c in theEntities.Classes
                    where c.ClassTitle.Contains(aTitle)
                    && c.UniversityId == aUniversityId
                    select c);
        }

        public IEnumerable<Class> GetClassByClassCode(string aUniversityId, string aClassCode) {
            return (from c in theEntities.Classes
                    where c.ClassCode.Contains(aClassCode)
                    && c.UniversityId == aUniversityId
                    select c);
        }

        public IEnumerable<Event> GetEventByTitle(string aUniversityId, string aTitle) {
            return (from e in theEntities.Events
                    where e.Title.Contains(aTitle)
                    && !e.Deleted
                    && e.StartDate > DateTime.UtcNow
                    && e.UniversityId == aUniversityId
                    select e);
        }

        public IEnumerable<Event> GetEventByInformation(string aUniversityId, string anInformation) {
            return (from e in theEntities.Events
                    where e.Information.Contains(anInformation)
                    && !e.Deleted
                    && e.StartDate > DateTime.UtcNow
                    && e.UniversityId == aUniversityId
                    select e);
        }

        public IEnumerable<GeneralPosting> GetGeneralPostingByTitle(string aUniversityId, string aTitle) {
            return (from p in theEntities.GeneralPostings
                    where p.Title.Contains(aTitle)
                    && p.UniversityId == aUniversityId
                    select p);
        }

        public IEnumerable<GeneralPosting> GetGeneralPostingByBody(string aUniversityId, string aBody) {
            return (from p in theEntities.GeneralPostings
                    where p.Body.Contains(aBody)
                    && p.UniversityId == aUniversityId
                    select p);
        }

        public IEnumerable<Club> GetOrganizationByName(string aUniversityId, string aName) {
            return (from o in theEntities.Clubs
                    where o.Name.Contains(aName)
                    && o.Active
                    && o.UniversityId == aUniversityId
                    select o);
        }

        public IEnumerable<Professor> GetProfessorByName(string aUniversityId, string aName) {
            return (from p in theEntities.Professors
                    where (p.FirstName + " " + p.LastName).Contains(aName)
                    && p.UniversityId == aUniversityId
                    select p);
        }

        public IEnumerable<TextBook> GetTextBookByTitle(string aUniversityId, string aTitle) {
            return (from t in theEntities.TextBooks
                    where t.BookTitle.Contains(aTitle)
                    && t.Active
                    && t.UniversityId == aUniversityId
                    select t);
        }

        public IEnumerable<TextBook> GetTextBookByClassCode(string aUniversityId, string aClassCode) {
            return (from t in theEntities.TextBooks
                    where t.ClassCode.Contains(aClassCode)
                    && t.Active
                    && t.UniversityId == aUniversityId
                    select t);
        }

        public IEnumerable<User> GetUserByName(int aUserId, string aName) {
            return (from u in theEntities.Users
                    where (u.FirstName + " " + u.LastName).Contains(aName)
                    && u.Id != aUserId
                    select u);
        }
    }
}