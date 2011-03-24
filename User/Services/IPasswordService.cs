namespace Social.User {
    public interface IPasswordService<T> {
        bool ForgotPasswordRequest(string aBaseUrl, string anEmail);
        bool ChangePassword(string anEmail, string aForgotPasswordHash, string aPassword, string aRetypedPassword);
    }
}