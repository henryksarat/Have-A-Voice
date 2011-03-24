namespace Social.Email {
    public interface IEmail {
        void SendEmail(string toEmail, string subject, string body);
    }
}
