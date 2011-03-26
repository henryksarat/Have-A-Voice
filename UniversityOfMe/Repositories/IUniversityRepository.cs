using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversityOfMe.Repositories {
    public interface IUniversityRepository {
        IEnumerable<string> ValidEmails();
    }
}
