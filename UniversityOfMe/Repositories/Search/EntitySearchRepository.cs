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
                    where c.Title.Contains(aTitle)
                    && c.UniversityId == aUniversityId
                    select c);
        }

        public IEnumerable<Class> GetClassByClassCode(string aUniversityId, string aClassCode) {
            return (from c in theEntities.Classes
                    where (c.Subject + c.Course).Contains(aClassCode)
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

        public IEnumerable<TextBook> GetTextBookByTitle(string aUniversityId, string aTitle) {
            return (from t in theEntities.TextBooks
                    where t.BookTitle.Contains(aTitle)
                    && t.Active
                    && t.UniversityId == aUniversityId
                    select t);
        }

        public IEnumerable<TextBook> GetTextBookByClassCode(string aUniversityId, string aClassCode) {
            return (from t in theEntities.TextBooks
                    where (t.ClassSubject + t.ClassCourse).Contains(aClassCode)
                    && t.Active
                    && t.UniversityId == aUniversityId
                    select t);
        }

        /// <summary>
        /// Split this off becuase it's used to GET ALL for the search results
        /// I did this originally so all pages will be crawled
        /// </summary>
        /// 
        public IEnumerable<Class> GetClassByTitle(string aTitle) {
            return (from c in theEntities.Classes
                    where c.Title.Contains(aTitle)
                    select c);
        }

        public IEnumerable<Class> GetClassByClassCode(string aClassCode) {
            return (from c in theEntities.Classes
                    where (c.Subject + c.Course).Contains(aClassCode)
                    select c);
        }

        public IEnumerable<Event> GetEventByTitle(string aTitle) {
            return (from e in theEntities.Events
                    where e.Title.Contains(aTitle)
                    && !e.Deleted
                    && e.StartDate > DateTime.UtcNow
                    select e);
        }

        public IEnumerable<Event> GetEventByInformation(string anInformation) {
            return (from e in theEntities.Events
                    where e.Information.Contains(anInformation)
                    && !e.Deleted
                    && e.StartDate > DateTime.UtcNow
                    select e);
        }

        public IEnumerable<GeneralPosting> GetGeneralPostingByTitle(string aTitle) {
            return (from p in theEntities.GeneralPostings
                    where p.Title.Contains(aTitle)
                    select p);
        }

        public IEnumerable<GeneralPosting> GetGeneralPostingByBody(string aBody) {
            return (from p in theEntities.GeneralPostings
                    where p.Body.Contains(aBody)
                    select p);
        }

        public IEnumerable<TextBook> GetTextBookByTitle(string aTitle) {
            return (from t in theEntities.TextBooks
                    where t.BookTitle.Contains(aTitle)
                    && t.Active
                    select t);
        }

        public IEnumerable<TextBook> GetTextBookByClassCode(string aClassCode) {
            return (from t in theEntities.TextBooks
                    where (t.ClassSubject + t.ClassCourse).Contains(aClassCode)
                    && t.Active
                    select t);
        }


        public IEnumerable<User> GetUserByName(int aUserId, string aName) {
            return (from u in theEntities.Users
                    where (u.FirstName + " " + u.LastName).Contains(aName)
                    && u.Id != aUserId
                    select u);
        }

        public IEnumerable<User> GetUserByName(string aName) {
            return (from u in theEntities.Users
                    where (u.FirstName + " " + u.LastName).Contains(aName)
                    select u);
        }


        public IEnumerable<University> GetUniversities() {
            return (from u in theEntities.Universities
                    select u);
        }
    }
}