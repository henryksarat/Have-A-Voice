using Social.User.Models;
namespace Social.User.Repositories {
    public interface IPasswordRepository<T> {
        void UpdateUserForgotPasswordHash(string anEmail, string aHashCode);
        AbstractUserModel<T> GetUserByEmailAndForgotPasswordHash(string anEmail, string aHashCode);
        void ChangePassword(int aUserId, string aPassword);
    }
}