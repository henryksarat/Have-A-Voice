namespace Social.Email {
    public interface IEmail {
        void SendEmail(string anEmailDescription, string toEmail, string subject, string body);
    }
}
