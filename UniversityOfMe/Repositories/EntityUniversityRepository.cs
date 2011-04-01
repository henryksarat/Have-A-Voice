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

        public IEnumerable<string> ValidEmails() {
            return (from u in theEntities.UniversityEmails
                    select u.Email).ToList<string>();
        }

        private UniversityEmail GetUniversityEmailFromEmail(string anEmail) {
            return (from ue in theEntities.UniversityEmails 
                    where ue.Email == anEmail
                    select ue).FirstOrDefault<UniversityEmail>();
        }
    }
}