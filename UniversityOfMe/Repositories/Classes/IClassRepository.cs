using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Classes {
    public interface IClassRepository {
        Class CreateClass(User aCreatedByUser, string aUniversityId, string aSubject, string aClassCode, string aClassTitle);
        Class GetClass(int aClubId);
        Class GetClass(string aSubject, string aCourse);
        Class GetClass(string aUniversityId, string aSubject, string aCourse);        
    }
}
