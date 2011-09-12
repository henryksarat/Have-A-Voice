using UniversityOfMe.Models.View;
using UniversityOfMe.Models;
using Social.Generic.Models;
using System.Collections;
using System.Collections.Generic;

namespace UniversityOfMe.Services.Dating {
    public interface IFlirtingService {
        FlirtModel GetFlirtModel();
        FlirtModel GetFlirtModel(int aTaggedUserId);
        bool CreateFlirt(UserInformationModel<User> aUserInfoModel, string aUniversityId, FlirtModel aFlirtModel);
        IEnumerable<AnonymousFlirt> GetFlirtsWithinUniversity(string aUniversityId, int aLimit);
    }
}
