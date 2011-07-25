using System.Collections.Generic;
using HaveAVoice.Helpers.ProfileQuestions;
using HaveAVoice.Models;
using Social.Generic;
using Social.Generic.Models;

namespace HaveAVoice.Services.Questions {
    public interface IProfileQuestionService {
        Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>> GetProfileQuestionsGrouped(User aUser);
        void UpdateProfileQuestions(UserInformationModel<User> aUserInfo, UpdateUserProfileQuestionsModel anUpdateUserProfileQuestionsModel);
    }
}