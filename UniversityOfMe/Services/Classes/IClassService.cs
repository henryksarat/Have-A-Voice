using System.Collections.Generic;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Services.Classes {
    public interface IClassService {
        Class CreateClass(UserInformationModel<User> aCreatedByUser, CreateClassModel aCreateClassModel);
        ClassDetailsModel GetClass(UserInformationModel<User> aViewingUser, int aClassId, ClassViewType aClassViewType);
    }
}