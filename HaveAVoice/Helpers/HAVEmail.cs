using System.Net.Mail;
using System.Net;
using System;
using HaveAVoice.Exceptions;

namespace HaveAVoice.Helpers {
    public class HAVEmail : IHAVEmail{
        const string MAILSERVER = "mail.haveavoice.us";
        const string FROM_EMAIL = "henryksarat@haveavoice.us";
        const string SMTP_USER = "henryksarat@haveavoice.us";
        const string SMTP_PASSWORD = "aPassword";

       
        public void SendEmail(string toEmail, string subject, string body) {
            MailMessage message = new MailMessage(FROM_EMAIL, toEmail, subject, body);
            SmtpClient smtpClient = new SmtpClient(MAILSERVER);
            NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(SMTP_USER, SMTP_PASSWORD);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = SMTPUserInfo;
            smtpClient.Send(message);
        }
    }
}