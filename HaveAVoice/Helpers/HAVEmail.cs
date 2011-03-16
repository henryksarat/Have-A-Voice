using System.Net.Mail;
using System.Net;
using System;
using HaveAVoice.Exceptions;

namespace HaveAVoice.Helpers {
    public class HAVEmail : IHAVEmail{
        private const string MAILSERVER = "relay-hosting.secureserver.net";
        private const string FROM_EMAIL = "haveavoice.accounts@haveavoice.com";
       
        public void SendEmail(string aToEmail, string aSubject, string aBody) {
            MailMessage myMailMessage = new MailMessage(FROM_EMAIL, aToEmail, aSubject, aBody);
            myMailMessage.IsBodyHtml = false;
            SmtpClient mySmtpClient = new SmtpClient(MAILSERVER, 25);
            mySmtpClient.Send(myMailMessage);
        }
    }
}