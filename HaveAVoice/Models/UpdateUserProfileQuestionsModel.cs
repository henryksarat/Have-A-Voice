using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Generic;
using HaveAVoice.Helpers.ProfileQuestions;

namespace HaveAVoice.Models {
    public class UpdateUserProfileQuestionsModel {
        public IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>> UserProfileQuestionsAnswered { get; set; }

        public UpdateUserProfileQuestionsModel() {
            UserProfileQuestionsAnswered = new List<Pair<UserProfileQuestion, QuestionAnswer>>();
        }
    }
}