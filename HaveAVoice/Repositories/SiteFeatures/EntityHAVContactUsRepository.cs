using System;
using HaveAVoice.Models;
using Social.Site.Repositories;

namespace HaveAVoice.Repositories.SiteFeatures {
    public class EntityHAVContactUsRepository : IContactUsRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();
        public void AddContactUserInquiry(string anEmail, string anInquiryType, string anInquiry) {
            ContactU myContactUs = ContactU.CreateContactU(0, anInquiryType, anInquiry, DateTime.UtcNow, anEmail);
            theEntities.AddToContactUs(myContactUs);
            theEntities.SaveChanges();
        }
    }
}