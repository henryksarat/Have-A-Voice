using System.Collections.Generic;
using Social.Generic.Models;

namespace Social.User.Services {
    public interface IUserRetrievalService<T> {
        T GetUser(int aUserId);
        T GetUserByShortUrl(string aShortUrl);
        T GetUser(string anEmail, string aPassword);
        AbstractUserModel<T> GetAbstractUser(int aUserId);
        AbstractUserModel<T> GetAbstractUser(string anEmail, string aPassword);
        T GetUser(string anEmail);
        IEnumerable<T> GetUsersByNameSearch(string aNamePortion);
    }
}