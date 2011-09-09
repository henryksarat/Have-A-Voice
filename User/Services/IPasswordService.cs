namespace Social.User {
    public interface IPasswordService<T> {
        bool ForgotPasswordRequest(string aBaseUrl, string anEmail, string aSubject, string aBody);
        bool ChangePassword(string anEmail, string aForgotPasswordHash, string aPassword, string aRetypedPassword);
    }
}