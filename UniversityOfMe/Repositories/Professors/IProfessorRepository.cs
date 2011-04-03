using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Professors {
    public interface IProfessorRepository {
        void CreateProfessor(User aCreatingUser, Professor aProfessor);
        Professor GetProfessor(int aProfessorId);
        Professor GetProfessor(string aUniversityId, string aFirstname, string aLastname);
        IEnumerable<Professor> GetProfessorsByUniversity(string aUniversityId);
    }
}
