using System.Collections.Generic;
using HaveAVoice.Models;
using HaveAVoice.Models.View;

namespace HaveAVoice.Repositories.Questions {
    public interface IProfileQuestionRepository {
        void AddAnswerToQuestion(User aUser, string aQuestion, int anAnswer);
        void AddUserToIgnoreList(User aUser, int aUserIdToIgnore);
        IEnumerable<int> GetIgnoringUsers(User aSourceUser);
        IEnumerable<UserProfileQuestionAnswer> GetProfileQuestionAnswersForUser(User aUser);
        UserProfileQuestion GetProfileQuestion(string aQuestionName);
        IEnumerable<UserProfileQuestion> GetProfileQuestions();
        void UpdateAnswersToQuestions(User aUser, IEnumerable<string> aYesAnswers, 
            IEnumerable<string> aNoAnswers, IEnumerable<string> aQuestionNamesForDontKnowAkaThirdOption, IEnumerable<string> anAnswersToDeleteDueToNoAnswer);
        IEnumerable<FriendConnectionModel> FindUsersBasedOnQuestion(User aUser, IEnumerable<string> aQuestionNames);
    }
}
