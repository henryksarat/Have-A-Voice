using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVFeedbackService {
        bool AddFeedback(User aUser, string aFeedback);
        IEnumerable<Feedback> GetAllFeedback();
    }
}