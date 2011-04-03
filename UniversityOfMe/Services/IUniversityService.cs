using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Services {
    public interface IUniversityService {
        void AddUserToUniversity(User aUser);
        IDictionary<string, string> CreateAllUniversitiesDictionaryEntry();
        IDictionary<string, string> CreateAcademicTermsDictionaryEntry();
        bool IsValidUniversityEmailAddress(string anEmail);
        IEnumerable<string> ValidEmails();
        bool IsFromUniversity(User aUser, string aUniversityId);
    }
}
