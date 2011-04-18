using System.Collections.Generic;
using UniversityOfMe.Models;
using Social.Generic.Models;

namespace UniversityOfMe.Services.Professors {
    public interface IProfessorService {
        bool CreateProfessor(UserInformationModel<User> aCreatingUser, Professor aProfessor);
        Professor GetProfessor(int aProfessorId);
        Professor GetProfessor(UserInformationModel<User> aViewingUser, string aUniversityId, string aProfessorName);
        IEnumerable<Professor> GetProfessorsForUniversity(string aUniversityId);
        bool IsProfessorExists(string aUniversityId, string aFullname);
    }
}