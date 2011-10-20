using System.Collections;
using System.Collections.Generic;
using UniversityOfMe.Models;
namespace UniversityOfMe.Repositories.Search {
    public interface ISearchRepository {
        IEnumerable<Class> GetClassByTitle(string aUniversityId, string aTitle);
        IEnumerable<Class> GetClassByClassCode(string aUniversityId, string aClassCode);
        IEnumerable<TextBook> GetTextBookByTitle(string aUniversityId, string aTitle);
        IEnumerable<TextBook> GetTextBookByClassCode(string aUniversityId, string aClassCode);

        IEnumerable<Class> GetClassByTitle(string aTitle);
        IEnumerable<Class> GetClassByClassCode(string aClassCode);
        IEnumerable<TextBook> GetTextBookByTitle(string aTitle);
        IEnumerable<TextBook> GetTextBookByClassCode(string aClassCode);

        IEnumerable<User> GetUserByName(int aUserId, string aName);
        IEnumerable<User> GetUserByName(string aName);

        IEnumerable<University> GetUniversities();
    }
}