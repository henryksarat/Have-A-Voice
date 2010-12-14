using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.Repositories.UserFeatures {
    public interface IHAVFeedbackRepository {
        void AddFeedback(User aUser, string aFeedback);
        IEnumerable<Feedback> GetAllFeedback();
    }
}