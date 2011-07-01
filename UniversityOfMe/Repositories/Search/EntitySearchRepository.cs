using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Search {
    public class EntitySearchRepository : ISearchRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public IEnumerable<Class> GetClassByTitle(string aTitle) {
            return (from c in theEntities.Classes
                    where c.ClassTitle.Contains(aTitle)
                    select c);
        }
    }
}