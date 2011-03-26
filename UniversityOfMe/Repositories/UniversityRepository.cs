using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;

namespace UniversityOfMe.Repositories {
    public class UniversityRepository : IUniversityRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public IEnumerable<string> ValidEmails() {
            return (from u in theEntities.Universities
                    select u.EmailExtension).ToList<string>();
        }
    }
}