using System.Collections.Generic;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Services.Classes {
    public interface IClassService {
        Class CreateClass(UserInformationModel<User> aCreatedByUser, CreateClassModel aCreateClassModel);
        bool CreateClassReply(UserInformationModel<User> aPostedByUser, int aClassId, string aReply);
        Class GetClass(UserInformationModel<User> aViewingUser, string aClassUrlString);
        Class GetClass(UserInformationModel<User> aViewingUser, int aClassId);
        IEnumerable<Class> GetClassesForUniversity(string aUniversityId);
        bool IsClassExists(string aClassUrlString);
    }
}