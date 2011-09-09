using AutomatedEmail.Repositories;
using System;
using Amazon.S3;
using System.Collections;
using AutomatedEmail;
using System.Collections.Generic;
using AutomatedEmail.Models;
using System.Linq;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace AutomatedEmail.Services {
    public class EmailService : IEmailService {
        private IEmailRepository theEmailRepo;

        public EmailService()
            : this(new EntityEmailRepository()) { }

        public EmailService(IEmailRepository aEmailRepository) {
            theEmailRepo = aEmailRepository;
        }

        public void SendEmails(AmazonSimpleEmailServiceClient aClient) {
            IEnumerable<EmailJob> myEmailsToSendOut = theEmailRepo.GetEmailJobsToBeSent();
            if (myEmailsToSendOut.Count<EmailJob>() != 0) {
                ConsoleMessageWithDate("Have to send emails: " + myEmailsToSendOut.Count<EmailJob>());
                foreach (EmailJob myEmailJob in myEmailsToSendOut) {
                    SendEmailRequest myRequest = new SendEmailRequest();

                    List<string> mySendTo = new List<string>();
                    mySendTo.Add(myEmailJob.ToEmail);

                    Content mySubject = new Content(myEmailJob.Subject);
                    Body myBody = new Body();
                    myBody.Html = new Content(myEmailJob.Body);

                    myRequest.Destination = new Destination(mySendTo);
                    myRequest.Message = new Message(mySubject, myBody);
                    myRequest.Source = myEmailJob.FromEmail;


                    //Change flag with the send in between so we can track if shit happened
                    theEmailRepo.MarkEmailPresentToTrue(myEmailJob.Id);
                    aClient.SendEmail(myRequest);
                    theEmailRepo.MarkEmailPostsentToTrue(myEmailJob.Id);

                    ConsoleMessageWithDate("A " + myEmailJob.EmailDescription + " has been sent to " + myEmailJob.ToEmail + " from " + myEmailJob.FromEmail);
                }
            } else {
                ConsoleMessageWithDate("No emails required to be sent out");
            }
        }

        private void ConsoleMessageWithDate(string aMessage) {
            Console.WriteLine(DateTime.UtcNow.ToString() + ": " + aMessage);
        }
    }
}