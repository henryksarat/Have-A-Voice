using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories {
    public interface IUniversityRepository {
        void AddUserToUniversity(int aUserId, string aUniversityEmail);
        IEnumerable<string> ValidEmails();
    }
}
