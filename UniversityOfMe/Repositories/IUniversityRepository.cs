using System.Collections.Generic;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories {
    public interface IUniversityRepository {
        void AddUserToUniversity(int aUserId, string aUniversityEmail);
        IEnumerable<AcademicTerm> GetAcademicTerms();
        University GetUniversity(string aUniversityId);
        IEnumerable<University> Universities();
        IEnumerable<string> UniversityEmails(string aUniversityId);
        IEnumerable<string> ValidEmails();
    }
}
