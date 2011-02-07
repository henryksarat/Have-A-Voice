using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.SiteFeatures {
    public class EntityHAVContactUsRepository : IHAVContactUsRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();
        public void AddContactUserInquiry(string anEmail, string anInquiryType, string anInquiry) {
            ContactU myContactUs = ContactU.CreateContactU(0, anInquiryType, anInquiry, DateTime.UtcNow, anEmail);
            theEntities.AddToContactUs(myContactUs);
            theEntities.SaveChanges();
        }
    }
}