using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories {
    public interface IUniversityRepository {
        void AddUserToUniversity(int aUserId, string aUniversityEmail);
        IEnumerable<AcademicTerm> GetAcademicTerms();
        IEnumerable<University> Universities();
        IEnumerable<string> UniversityEmails(string aUniversityId);
        IEnumerable<string> ValidEmails();
    }
}
