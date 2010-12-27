using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVFeedbackRepository : IHAVFeedbackRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddFeedback(User aUser, string aFeedback) {
            Feedback myFeedback = Feedback.CreateFeedback(0, aFeedback, DateTime.UtcNow, aUser.Id);
            theEntities.AddToFeedbacks(myFeedback);
            theEntities.SaveChanges();
        }

        public IEnumerable<Feedback> GetAllFeedback() {
            return theEntities.Feedbacks.ToList<Feedback>();
        }
    }
}