using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversityOfMe.Services {
    public interface IUniversityService {
        bool IsValidUniversityEmailAddress(string anEmail);
        IEnumerable<string> ValidEmails();
    }
}
