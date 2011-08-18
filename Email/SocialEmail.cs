using System.Net.Mail;
using System.Net;

namespace Social.Email {
    public class SocialEmail : IEmail{
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        //private const string MAILSERVER = "relay-hosting.secureserver.net";
        private const string MAILSERVER = "mail.haveavoice.com";
        //private const string FROM_EMAIL = "haveavoice.accounts@haveavoice.com";
        private const string FROM_EMAIL = "haveavoice.accounts@haveavoice.com";
        private const string PASSWORD = "zztop06T";
=======
        private const string MAILSERVER = "relay-hosting.secureserver.net";
        private const string FROM_EMAIL = "haveavoice.accounts@haveavoice.com";
>>>>>>> parent of 5eb5d36... Made the site work on host gator, email only doesnt work.
=======
        private const string MAILSERVER = "relay-hosting.secureserver.net";
        private const string FROM_EMAIL = "haveavoice.accounts@haveavoice.com";
>>>>>>> parent of 5eb5d36... Made the site work on host gator, email only doesnt work.
=======
        private const string MAILSERVER = "relay-hosting.secureserver.net";
        private const string FROM_EMAIL = "haveavoice.accounts@haveavoice.com";
>>>>>>> parent of 5eb5d36... Made the site work on host gator, email only doesnt work.
       
        public void SendEmail(string aToEmail, string aSubject, string aBody) {
            MailMessage myMailMessage = new MailMessage(FROM_EMAIL, aToEmail, aSubject, aBody);
            myMailMessage.IsBodyHtml = true;
            SmtpClient mySmtpClient = new SmtpClient(MAILSERVER, 25);
            NetworkCredential basicCredential =
                new NetworkCredential(FROM_EMAIL, PASSWORD);
            mySmtpClient.Credentials = basicCredential;

            mySmtpClient.Send(myMailMessage);
        }
    }
}