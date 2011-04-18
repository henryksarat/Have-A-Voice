using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;

namespace UniversityOfMe.Repositories {
    public class EntityUniversityRepository : IUniversityRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void AddUserToUniversity(int aUserId, string aUniversityEmail) {
            UniversityEmail myUniversity = GetUniversityEmailFromEmail(aUniversityEmail);
            UserUniversity myUserUniversity = UserUniversity.CreateUserUniversity(0, aUserId, myUniversity.Email);
            theEntities.AddToUserUniversities(myUserUniversity);
            theEntities.SaveChanges();
        }

        public IEnumerable<AcademicTerm> GetAcademicTerms() {
            return (from a in theEntities.AcademicTerms
                    select a).ToList<AcademicTerm>();
        }

        public University GetUniversity(string aUniversityId) {
            return (from u in theEntities.Universities
                    where u.Id == aUniversityId
                    select u).FirstOrDefault<University>();
        }

        private UniversityEmail GetUniversityEmailFromEmail(string anEmail) {
            return (from ue in theEntities.UniversityEmails 
                    where ue.Email == anEmail
                    select ue).FirstOrDefault<UniversityEmail>();
        }

        public IEnumerable<University> Universities() {
            return (from u in theEntities.Universities
                    select u).ToList<University>();
        }

        public IEnumerable<string> UniversityEmails(string aUniversityId) {
            return (from ue in theEntities.UniversityEmails
                    where ue.UniversityId == aUniversityId
                    select ue.Email).ToList<string>();
        }

        public IEnumerable<string> ValidEmails() {
            return (from u in theEntities.UniversityEmails
                    select u.Email).ToList<string>();
        }
    }
}