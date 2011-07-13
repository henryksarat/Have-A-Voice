using System.Collections;
using System.Collections.Generic;
using UniversityOfMe.Models;
namespace UniversityOfMe.Repositories.Search {
    public interface ISearchRepository {
        IEnumerable<Class> GetClassByTitle(string aUniversityId, string aTitle);
        IEnumerable<Class> GetClassByClassCode(string aUniversityId, string aClassCode);
        IEnumerable<Event> GetEventByTitle(string aUniversityId, string aTitle);
        IEnumerable<Event> GetEventByInformation(string aUniversityId, string anInformation);
        IEnumerable<GeneralPosting> GetGeneralPostingByTitle(string aUniversityId, string aTitle);
        IEnumerable<GeneralPosting> GetGeneralPostingByBody(string aUniversityId, string aBody);
        IEnumerable<Club> GetOrganizationByName(string aUniversityId, string aName);
        IEnumerable<Professor> GetProfessorByName(string aUniversityId, string aName);
        IEnumerable<TextBook> GetTextBookByTitle(string aUniversityId, string aTitle);
        IEnumerable<TextBook> GetTextBookByClassCode(string aUniversityId, string aClassCode);
        IEnumerable<User> GetUserByName(int aUserId, string aName);
    }
}