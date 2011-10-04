using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Professors {
    public interface IProfessorRepository {
        void CreateProfessor(User aCreatingUser, string aUniversityId, string aFirstName, string aLastName, string aProfessorImage);
        void CreateProfessorSuggestedPicture(User aSuggestingUser, int aProfessorId, string aProfessorImage);
        Professor GetProfessor(int aProfessorId);
        Professor GetProfessor(string aUniversityId, string aFirstname, string aLastname);
        IEnumerable<Professor> GetProfessorsAssociatedWithClass(int aClassId);
        IEnumerable<Professor> GetProfessorsByUniversity(string aUniversityId);
    }
}
