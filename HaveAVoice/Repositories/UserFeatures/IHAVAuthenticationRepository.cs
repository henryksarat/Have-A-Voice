using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVAuthenticationRepository {
        User FindUserByActivationCode(string activationCode);
        IEnumerable<Permission> GetPermissionsForUser(User aUser);
        User FindUserByCookieHash(int userId, string cookieHash);
        User UpdateCookieHashCreationDate(User user);
    }
}