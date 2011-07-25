using System.Collections.Generic;
using HaveAVoice.Helpers.ProfileQuestions;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using Social.Generic;
using Social.Generic.Models;

namespace HaveAVoice.Services.Questions {
    public interface IProfileQuestionService {
        IEnumerable<FriendConnectionModel> GetPossibleFriendConnections(UserInformationModel<User> aUserInfo);
        Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>> GetProfileQuestionsGrouped(User aUser);
        void UpdateProfileQuestions(UserInformationModel<User> aUserInfo, UpdateUserProfileQuestionsModel anUpdateUserProfileQuestionsModel);
    }
}