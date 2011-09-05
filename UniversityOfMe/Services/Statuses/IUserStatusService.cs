using System.Collections.Generic;
using System.Web;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using System;

namespace UniversityOfMe.Services.Status {
    public interface IUserStatusService {
        bool CreateUserStatus(UserInformationModel<User> aUserInfo, string aStatus);
        void DeleteUserStatus(UserInformationModel<User> aUserInfo, int aStatusId);
        UserStatus GetLatestUserStatusForUser(UserInformationModel<User> aUserInfo);
        IEnumerable<UserStatus> GetLatestUserStatusesWithinUniversity(UserInformationModel<User> aUser, string aUniversityId, int aLimit);
    }
}