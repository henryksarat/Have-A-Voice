using Social.User.Models;

namespace Social.User {
    public interface IUserRepository<T, U, V> {
        AbstractUserModel<T> CreateUser(T userToCreate);
        void DeleteUser(T userToDelete);
        bool EmailRegistered(string email);
        bool ShortUrlTaken(string aShortUrl);

        void UpdateUser(T userToEdit);
        T DeleteUserFromRole(int userId, int roleId);
        void RemoveUserFromRole(T aUser, U aRole);
        V AddUserToRole(T user, U role);
    }
}