using System.Net.Mail;

namespace Social.Email {
    public class SocialEmail : IEmail{
        //private const string MAILSERVER = "relay-hosting.secureserver.net";
        private const string MAILSERVER = "mail.haveavoice.us";
        //private const string FROM_EMAIL = "haveavoice.accounts@haveavoice.com";
        private const string FROM_EMAIL = "no-reply@haveavoice.us";
        private const string PASSWORD = "zztop06T";
       
        public void SendEmail(string aToEmail, string aSubject, string aBody) {
            MailMessage myMailMessage = new MailMessage(FROM_EMAIL, aToEmail, aSubject, aBody);
            myMailMessage.IsBodyHtml = false;
            SmtpClient mySmtpClient = new SmtpClient(MAILSERVER, 25);
            mySmtpClient.Send(myMailMessage);
        }
    }
}