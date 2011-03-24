using System.Collections.Generic;

namespace Social.User.Services {
    public interface IUserRetrievalService<T> {
        T GetUser(int aUserId);
        T GetUserByShortUrl(string aShortUrl);
        T GetUser(string anEmail, string aPassword);
        T GetUser(string anEmail);
        IEnumerable<T> GetUsersByNameSearch(string aNamePortion);
    }
}