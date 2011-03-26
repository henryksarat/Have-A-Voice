using Social.User.Models;

namespace Social.User {
    public interface IUserRepository<T> {
        AbstractUserModel<T> CreateUser(T userToCreate);
        void DeleteUser(T userToDelete);
        bool EmailRegistered(string email);
        bool ShortUrlTaken(string aShortUrl);
    }
}