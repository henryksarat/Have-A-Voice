using System;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Email {
    public class EntityEmailRepository : IEmailRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddEmailJob(string anEmailDescription, string aFromEmail, string aToEmail, string aSubject, string aBody) {
            EmailJob myEmailJob = EmailJob.CreateEmailJob(0, anEmailDescription, aFromEmail, aToEmail, aSubject, aBody, DateTime.UtcNow, false, false);
            theEntities.AddToEmailJobs(myEmailJob);
            theEntities.SaveChanges();
        }
    }
}