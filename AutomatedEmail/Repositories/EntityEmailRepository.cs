using System;
using System.Linq;
using System.Collections.Generic;
using AutomatedEmail.Models;

namespace AutomatedEmail.Repositories {
    public class EntityEmailRepository : IEmailRepository {
        private AutomatedEmailEntities theEntities = new AutomatedEmailEntities();

        public IEnumerable<EmailJob> GetEmailJobsToBeSent() {
            return (from e in theEntities.EmailJobs
                    where e.PresentSent == false && e.PostsentSent == false
                    select e).ToList<EmailJob>();
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
            myEmailJob.DateTimeStampSent = DateTime.UtcNow;
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