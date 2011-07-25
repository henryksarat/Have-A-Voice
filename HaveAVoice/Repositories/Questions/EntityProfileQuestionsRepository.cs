using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Models;
using HaveAVoice.Helpers.ProfileQuestions;
using HaveAVoice.Models.View;

namespace HaveAVoice.Repositories.Questions {
    public class EntityProfileQuestionRepository : IProfileQuestionRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddAnswerToQuestion(User aUser, string aQuestion, int anAnswer) {
            UserProfileQuestionAnswer myAnswer = UserProfileQuestionAnswer.CreateUserProfileQuestionAnswer(0, aUser.Id, aQuestion, anAnswer);
            theEntities.AddToUserProfileQuestionAnswers(myAnswer);
            theEntities.SaveChanges();
        }

        public IEnumerable<FriendConnectionModel> FindUsersBasedOnQuestion(User aUser, IEnumerable<string> aQuestionNames) {
            Dictionary<string, int> myQuestionToAnswers = new Dictionary<string, int>();
            foreach (string myQuestionName in aQuestionNames) {
                UserProfileQuestionAnswer myAnswer = GetProfileQuesitonAnswerForUser(aUser, myQuestionName);
                if (myAnswer != null) {
                    myQuestionToAnswers.Add(myQuestionName, myAnswer.Answer);
                }
            }

            if (myQuestionToAnswers.Keys.Count == 0) {
                return new List<FriendConnectionModel>();
            } else {
                List<FriendConnectionModel> myFriendConnections = new List<FriendConnectionModel>();

                foreach(string myQuestionName in aQuestionNames) {
                    if (myQuestionToAnswers.Keys.Contains(myQuestionName)) {
                        int myAnswer = myQuestionToAnswers[myQuestionName];
                        IEnumerable<FriendConnectionModel> myResults = (from a in theEntities.UserProfileQuestionAnswers
                                                                        where a.UserProfileQuestionName == myQuestionName
                                                                        && a.Answer == myAnswer
                                                                        && a.UserId != aUser.Id
                                                                        select new FriendConnectionModel() {
                                                                            User = a.User,
                                                                            QuestionConnectionMadeFrom = a.UserProfileQuestion
                                                                        });
                        myFriendConnections.AddRange(myResults);
                    }
                }

                return myFriendConnections;
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

        public void UpdateAnswersToQuestions(User aUser, IEnumerable<string> aYesAnswers, 
            IEnumerable<string> aNoAnswers, IEnumerable<string> anAnswersToDeleteDueToNoAnswer) {
            UpdateProfileQuestionAnswersWithoutSave(aUser, aYesAnswers, QuestionAnswer.Yes);
            UpdateProfileQuestionAnswersWithoutSave(aUser, aNoAnswers, QuestionAnswer.No);
            DeleteProfileQuestionAnswersWithoutSave(aUser, anAnswersToDeleteDueToNoAnswer);
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

        private void DeleteProfileQuestionAnswersWithoutSave(User aUser, IEnumerable<string> aQuestions) {
            foreach (string aQuestion in aQuestions) {
                UserProfileQuestionAnswer myUserProfileQuestionAnswer = GetProfileQuesitonAnswerForUser(aUser, aQuestion);
                if (myUserProfileQuestionAnswer != null) {
                    theEntities.DeleteObject(myUserProfileQuestionAnswer);
                    theEntities.SaveChanges();
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