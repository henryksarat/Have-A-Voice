using Social.Generic.Models;

namespace Social.User.Repositories {
    //T = User
    //U = Role
    //V = UserRole
    public interface IUserRepository<T, U, V> {
        V AddUserToRole(T user, U role);
        AbstractUserModel<T> CreateUser(T userToCreate, string aNotConfirmedRoleName);
        void DeleteUser(T userToDelete);
        T DeleteUserFromRole(int userId, int roleId);
        bool EmailRegistered(string email);
        void RemoveUserFromRole(T aUser, U aRole);
        bool ShortUrlTaken(string aShortUrl);
        void UpdateUser(T userToEdit);
    }
}