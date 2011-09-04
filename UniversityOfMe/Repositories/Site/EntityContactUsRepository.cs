using System;
using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;
using UniversityOfMe.Helpers;
using Social.Site.Repositories;

namespace UniversityOfMe.Repositories.Site {
    public class EntityContactUsRepository : IContactUsRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void AddContactUserInquiry(string anEmail, string anInquiryType, string aInquiry) {
            ContactUs myContactUs = ContactUs.CreateContactUs(0, anInquiryType, aInquiry, anEmail, DateTime.UtcNow);
            theEntities.AddToContactUs(myContactUs);
            theEntities.SaveChanges();
        }
    }
}