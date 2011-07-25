using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Models;
using HaveAVoice.Helpers.ProfileQuestions;

namespace HaveAVoice.Repositories.Questions {
    public class EntityProfileQuestionRepository : IProfileQuestionRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddAnswerToQuestion(User aUser, string aQuestion, int anAnswer) {
            UserProfileQuestionAnswer myAnswer = UserProfileQuestionAnswer.CreateUserProfileQuestionAnswer(0, aUser.Id, aQuestion, anAnswer);
            theEntities.AddToUserProfileQuestionAnswers(myAnswer);
            theEntities.SaveChanges();
        }

        public IEnumerable<User> FindUsersBasedOnQuestion(User aUser, string aQuestionName) {
            UserProfileQuestionAnswer myAnswer = GetProfileQuesitonAnswerForUser(aUser, aQuestionName);

            if (myAnswer == null) {
                return new List<User>();
            } else {
                return (from a in theEntities.UserProfileQuestionAnswers
                        where a.UserProfileQuestionName == aQuestionName
                        && a.Answer == myAnswer.Answer
                        && a.UserId != aUser.Id
                        select a.User);
            }
        }

        public IEnumerable<UserProfileQuestion> GetProfileQuestions() {
            return (from q in theEntities.UserProfileQuestions
                    select q);
        }

        public UserProfileQuestion GetProfileQuestion(string aQuestionName) {
            return (from q in theEntities.UserProfileQuestions
                    where q.Name == aQuestionName
                    select q).FirstOrDefault<UserProfileQuestion>();
        }

        public IEnumerable<UserProfileQuestionAnswer> GetProfileQuestionAnswersForUser(User aUser) {
            return (from a in theEntities.UserProfileQuestionAnswers
                    where a.UserId == aUser.Id
                    select a);
        }

        public void UpdateAnswersToQuestions(User aUser, IEnumerable<string> aYesAnswers, IEnumerable<string> aNoAnswers, IEnumerable<string> aDontKnowAnswers) {
            UpdateProfileQuestionAnswersWithoutSave(aUser, aYesAnswers, QuestionAnswer.Yes);
            UpdateProfileQuestionAnswersWithoutSave(aUser, aNoAnswers, QuestionAnswer.No);
            UpdateProfileQuestionAnswersWithoutSave(aUser, aDontKnowAnswers, QuestionAnswer.DontKnow);
            theEntities.SaveChanges();
        }

        private void UpdateProfileQuestionAnswersWithoutSave(User aUser, IEnumerable<string> aQuestions, QuestionAnswer aQuestionAnswer) {
            foreach (string aQuestion in aQuestions) {
                UserProfileQuestionAnswer myUserProfileQuestion = GetProfileQuesitonAnswerForUser(aUser, aQuestion);
                if (myUserProfileQuestion == null) {
                    myUserProfileQuestion = UserProfileQuestionAnswer.CreateUserProfileQuestionAnswer(0, aUser.Id, aQuestion, (int)aQuestionAnswer);
                    theEntities.AddToUserProfileQuestionAnswers(myUserProfileQuestion);
                } else {
                    myUserProfileQuestion.Answer = (int)aQuestionAnswer;
                    theEntities.ApplyCurrentValues(myUserProfileQuestion.EntityKey.EntitySetName, myUserProfileQuestion);
                }
            }
        }

        private UserProfileQuestionAnswer GetProfileQuesitonAnswerForUser(User aUser, string aQuestionName) {
            return (from a in theEntities.UserProfileQuestionAnswers
                    where a.UserId == aUser.Id
                    && a.UserProfileQuestionName == aQuestionName
                    select a).FirstOrDefault<UserProfileQuestionAnswer>();
        }
    }
}