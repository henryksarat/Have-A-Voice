using System;
using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Repositories.Email {
    public class EntityEmailRepository : IEmailRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void AddEmailJob(string anEmailDescription, string aFromEmail, string aToEmail, string aSubject, string aBody) {
            EmailJob myEmailJob = EmailJob.CreateEmailJob(0, anEmailDescription, aFromEmail, aToEmail, aSubject, aBody, DateTime.UtcNow, false, false);
            theEntities.AddToEmailJobs(myEmailJob);
            theEntities.SaveChanges();
        }
    }
}