using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.Services.UserFeatures {
    public interface IHAVFeedbackService {
        bool AddFeedback(User aUser, string aFeedback);
        IEnumerable<Feedback> GetAllFeedback();
    }
}