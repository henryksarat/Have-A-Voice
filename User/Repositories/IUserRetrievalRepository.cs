using System.Collections.Generic;

namespace Social.User.Repositories {
    public interface IUserRetrievalRepository<T> {
        T GetUser(int anId);
        T GetUserByShortUrl(string aShortUrl);
        T GetUser(string anEmail, string aPassword);
        T GetUser(string anEmail);
        IEnumerable<T> GetUsersByNameContains(string aNamePortion);
    }
}