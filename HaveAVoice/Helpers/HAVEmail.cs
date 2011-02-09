﻿using System.Net.Mail;
using System.Net;
using System;
using HaveAVoice.Exceptions;

namespace HaveAVoice.Helpers {
    public class HAVEmail : IHAVEmail{
        private const string MAILSERVER = "mail.haveavoice.com";
        private const string FROM_EMAIL = "account.activation@haveavoice.com";
        private const string SMTP_USER = "account.activation@haveavoice.com";
        private const string SMTP_PASSWORD = "mPass01";

       
        public void SendEmail(string aToEmail, string aSubject, string aBody) {
            MailMessage myMailMessage = new MailMessage(FROM_EMAIL, aToEmail, aSubject, aBody);
            myMailMessage.IsBodyHtml = true;
            SmtpClient mySmtpClient = new SmtpClient(MAILSERVER);
            NetworkCredential mySmtopUserInfo = new System.Net.NetworkCredential(SMTP_USER, SMTP_PASSWORD);
            mySmtpClient.UseDefaultCredentials = false;
            mySmtpClient.Credentials = mySmtopUserInfo;
            mySmtpClient.Send(myMailMessage);
        }
    }
}