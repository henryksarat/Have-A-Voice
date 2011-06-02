using System.Collections.Generic;
using Social.Generic.Models;

namespace Social.User.Repositories {
    public interface IUserRetrievalRepository<T> {
        AbstractUserModel<T> GetAbstractUser(int aUserId);
        AbstractUserModel<T> GetAbstractUser(string anEmail, string aPassword);
        T GetUser(int anId);
        T GetUserByShortUrl(string aShortUrl);
        T GetUser(string anEmail, string aPassword);
        T GetUser(string anEmail);
        IEnumerable<T> GetUsersByNameContains(string aNamePortion);
    }
}