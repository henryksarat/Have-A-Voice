using System.Collections.Generic;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Questions {
    public interface IProfileQuestionRepository {
        void AddAnswerToQuestion(User aUser, string aQuestion, int anAnswer);
        IEnumerable<UserProfileQuestionAnswer> GetProfileQuestionAnswersForUser(User aUser);
        IEnumerable<UserProfileQuestion> GetProfileQuestions();
        void UpdateAnswersToQuestions(User aUser, IEnumerable<string> aYesAnswers, IEnumerable<string> aNoAnswers, IEnumerable<string> aDontKnowAnswers);
    }
}
