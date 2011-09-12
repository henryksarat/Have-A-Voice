using UniversityOfMe.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using UniversityOfMe.Helpers.Configuration;
using UniversityOfMe.Helpers.Email;

namespace UniversityOfMe.Repositories.Dating {
    public class FlirtingRepository : IFlirtingRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void CreateFlirt(User aUserPostingFlirt, string aUniversityId, string anAdjective, string aDeliciousTreat, 
            string anAnimal, string aHairColor, string aGender, string aMessage, int aTaggedUserId,
            string aWhere) {
            AnonymousFlirt myAnonymousFlirt = 
                AnonymousFlirt.CreateAnonymousFlirt(0, aUniversityId, aUserPostingFlirt.Id, aGender, aHairColor, 
                anAdjective, aDeliciousTreat, anAnimal, aMessage, DateTime.UtcNow, false);
            if (aTaggedUserId != 0) {
                myAnonymousFlirt.TaggedUserId = aTaggedUserId;
                User myToUser = GetUser(aTaggedUserId);

                AddEmailJobForNewFlirtWithoutSave(myToUser.Email, aUniversityId, anAdjective, 
                    aDeliciousTreat, anAnimal, aHairColor, aGender, aWhere, aMessage);
            }
            if (!string.IsNullOrEmpty(aWhere)) {
                myAnonymousFlirt.Location = aWhere;
            }

            theEntities.AddToAnonymousFlirts(myAnonymousFlirt);
            theEntities.SaveChanges();
        }

        public IEnumerable<AnonymousFlirt> GetAnonymousFlirtsWithinUniversity(string aUniversityId) {
            return (from f in theEntities.AnonymousFlirts
                    where f.UniversityId == aUniversityId
                    select f).OrderByDescending(f => f.DateTimeStamp);
        }

        private void AddEmailJobForNewFlirtWithoutSave(string aToEmail, string aUniversityId, 
            string anAdjective, string aDeliciousTreat, 
            string anAnimal, string aHairColor, string aGender, 
            string aLocation, string aMessage) {

            string myFromEmail = SiteConfiguration.NotificationsEmail();
            string mySubject = EmailContent.AnonymousFlirtSubject();
            string myBody = EmailContent.AnonymousFlirtBody(aUniversityId, anAdjective, aDeliciousTreat, anAnimal, aHairColor, aGender, aLocation, aMessage);


            EmailJob myEmailJob = EmailJob.CreateEmailJob(0, EmailType.FLIRT.ToString(), myFromEmail,
                aToEmail, mySubject, myBody, DateTime.UtcNow, false, false);
            theEntities.AddToEmailJobs(myEmailJob);
        }


        private User GetUser(int aUserId) {
            return (from u in theEntities.Users
                    where u.Id == aUserId
                    select u).FirstOrDefault<User>();
        }
    }
}