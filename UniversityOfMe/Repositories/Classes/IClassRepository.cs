using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Classes {
    public interface IClassRepository {
        Class CreateClass(User aCreatedByUser, string aUniversityId, string anAcademicTermId, string aClassCode, string aClassTitle, int aYear, string aDetails);
        void CreateClassReply(User aPostedByUser, int aClassId, string aReply);
        Class GetClass(int aClubId);
        Class GetClass(string aClassCode, string anAcademicTermId, int aYear);
        IEnumerable<Class> GetClassesForUniversity(string aUniversityId);
    }
}
