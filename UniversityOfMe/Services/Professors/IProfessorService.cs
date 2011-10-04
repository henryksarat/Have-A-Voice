using System.Collections.Generic;
using UniversityOfMe.Models;
using Social.Generic.Models;
using System.Web;

namespace UniversityOfMe.Services.Professors {
    public interface IProfessorService {
        bool CreateProfessor(UserInformationModel<User> aCreatingUser, string aUniversityId, string aFirstName, string aLastName, HttpPostedFileBase aProfessorImage);
        Professor GetProfessor(int aProfessorId);
        Professor GetProfessor(string aUniversityId, string aProfessorName);
        IEnumerable<Professor> GetProfessorsAssociatedWithClass(int aClassId);
        bool CreateProfessorImageSuggestion(UserInformationModel<User> aSuggestingUser, int aProfessorId, HttpPostedFileBase aProfessorImage);
        IEnumerable<Professor> GetProfessorsForUniversity(string aUniversityId);
        bool IsProfessorExists(string aUniversityId, string aFullname);
    }
}