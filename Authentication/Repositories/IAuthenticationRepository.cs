using System.Collections.Generic;
using Social.Generic.Models;
using Social.User.Models;

namespace Social.Authentication.Repositories {
    public interface IAuthenticationRepository<T, U> {
        T FindUserByActivationCode(string activationCode);
        AbstractUserModel<T> GetAbstractUserByActivationCode(string activationCode);
        IEnumerable<AbstractPermissionModel<U>> FindPermissionsForUser(T aUser);
        T FindUserByCookieHash(int userId, string cookieHash);
        T UpdateCookieHashCreationDate(T user);
    }
}