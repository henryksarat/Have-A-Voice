using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace Social.User.Repositories {
    public interface IAuthenticationRepository<T> {
        T FindUserByActivationCode(string activationCode);
        IEnumerable<Permission> FindPermissionsForUser(T aUser);
        T FindUserByCookieHash(int userId, string cookieHash);
        T UpdateCookieHashCreationDate(T user);
    }
}