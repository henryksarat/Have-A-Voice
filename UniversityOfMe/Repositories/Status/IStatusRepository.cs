using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Status {
    public interface IUserStatusRepository {
        void CreateUserStatus(User aUser, University aCurrentUniversity, string aStatus, bool anEveryone);
        UserStatus GetUserStatus(int aStatusId);
        void DeleteUserStatus(int aStatusId);
        UserStatus GetLatestUserStatusForUser(User aUser);
        IEnumerable<UserStatus> GetLatestUserStatuses(string aUniversityId);
    }
}
