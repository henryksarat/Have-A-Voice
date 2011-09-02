using System;
using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Repositories.Email {
    public class EntityEmailRepository : IEmailRepository {
        private AutomatedEmailEntities theEntities = new AutomatedEmailEntities();

        public void AddEmailJob(string aFromEmail, string aToEmail, string aSubject, string aBody) {
            EmailJob myEmailJob = EmailJob.CreateEmailJob(0, aFromEmail, aToEmail, aSubject, aBody, DateTime.UtcNow, false, false);
            theEntities.AddToEmailJobs(myEmailJob);
            theEntities.SaveChanges();
        }

        public IEnumerable<EmailJob> GetEmailJobsToBeSent() {
            return (from e in theEntities.EmailJobs
                    where e.PresentSent == false && e.PostsentSent == false
                    select e);
        }

        public void MarkEmailPresentToTrue(int anId) {
            EmailJob myEmailJob = GetEmailJob(anId);
            myEmailJob.PresentSent = true;
            theEntities.ApplyCurrentValues(myEmailJob.EntityKey.EntitySetName, myEmailJob);
            theEntities.SaveChanges();
        }

        public void MarkEmailPostsentToTrue(int anId) {
            EmailJob myEmailJob = GetEmailJob(anId);
            myEmailJob.PostsentSent = true;
            theEntities.ApplyCurrentValues(myEmailJob.EntityKey.EntitySetName, myEmailJob);
            theEntities.SaveChanges();
        }

        private EmailJob GetEmailJob(int anId) {
            return (from e in theEntities.EmailJobs
                    where e.Id == anId
                    select e).FirstOrDefault<EmailJob>();
        }
    }
}