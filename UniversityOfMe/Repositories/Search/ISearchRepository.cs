using System.Collections;
using System.Collections.Generic;
using UniversityOfMe.Models;
namespace UniversityOfMe.Repositories.Search {
    public interface ISearchRepository {
        IEnumerable<Class> GetClassByTitle(string aTitle);
    }
}
