using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVFeedbackRepository : HAVBaseRepository, IHAVFeedbackRepository {
        public void AddFeedback(User aUser, string aFeedback) {
            Feedback myFeedback = Feedback.CreateFeedback(0, aFeedback, DateTime.UtcNow, aUser.Id);
            GetEntities().AddToFeedbacks(myFeedback);
            GetEntities().SaveChanges();
        }

        public IEnumerable<Feedback> GetAllFeedback() {
            return GetEntities().Feedbacks.ToList<Feedback>();
        }
    }
}