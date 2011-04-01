using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Services {
    public interface IUniversityService {
        void AddUserToUniversity(User aUser);
        bool IsValidUniversityEmailAddress(string anEmail);
        IEnumerable<string> ValidEmails();
    }
}
