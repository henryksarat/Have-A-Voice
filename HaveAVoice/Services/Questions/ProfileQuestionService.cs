using System.Collections.Generic;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.Petitions;
using Social.Admin.Exceptions;
using Social.Admin.Helpers;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;
using HaveAVoice.Repositories.Questions;
using Social.Generic;
using HaveAVoice.Helpers.ProfileQuestions;
using Social.User.Models;
using System.Linq;
using System;

namespace HaveAVoice.Services.Questions {
    public class ProfileQuestionService : IProfileQuestionService {
        private IProfileQuestionRepository theProfileQuestionRepository;

        public ProfileQuestionService()
            : this(new EntityProfileQuestionRepository()) { }

        public ProfileQuestionService(IProfileQuestionRepository aPetitionRepo) {
            theProfileQuestionRepository = aPetitionRepo;
        }

        public IEnumerable<FriendConnectionModel> GetPossibleFriendConnections(UserInformationModel<User> aUserInfo) {
            IEnumerable<User> myQuestionMatches = theProfileQuestionRepository.FindUsersBasedOnQuestion(aUserInfo.Details, ProfileQuestion.POLITICAL_AFFILIATION.ToString()).ToList();

            IEnumerable<int> myFriendRequestsAccepted = aUserInfo.Details.FriendedBy.Select(f => f.SourceUser).Select(f => f.Id).ToList();
            IEnumerable<int> myFriendRequestsSent = aUserInfo.Details.Friends.Select(f => f.SourceUser).Select(f => f.Id).ToList();
            IEnumerable<User> myFinalQuestionMatches = (from m in myQuestionMatches
                                                 where !myFriendRequestsSent.Contains(m.Id) && !myFriendRequestsAccepted.Contains(m.Id)
                                                 select m).ToList();
            UserProfileQuestion myQuestion = theProfileQuestionRepository.GetProfileQuestion(ProfileQuestion.POLITICAL_AFFILIATION.ToString());

            IEnumerable<FriendConnectionModel> myFriendConnectionModel = myFinalQuestionMatches.Select(m => new FriendConnectionModel() {
                User = m,
                QuestionConnectionMadeFrom = myQuestion
            }).ToList();

            return myFriendConnectionModel;
        }

        public Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>> GetProfileQuestionsGrouped(User aUser) {
            Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>> myProfileQuestionSelection = new Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>>();

            IEnumerable<UserProfileQuestionAnswer> myQuestionAnswers = theProfileQuestionRepository.GetProfileQuestionAnswersForUser(aUser);
            IEnumerable<UserProfileQuestion> myQuestionsAnswered = myQuestionAnswers.Select(a => a.UserProfileQuestion);
            IEnumerable<UserProfileQuestion> myAllProfileQuestions = theProfileQuestionRepository.GetProfileQuestions();
            IEnumerable<UserProfileQuestion> myExcludedPrivacySettings = myAllProfileQuestions.Except(myQuestionsAnswered);

            List<Pair<UserProfileQuestion, QuestionAnswer>> myPairedQuestions = new List<Pair<UserProfileQuestion, QuestionAnswer>>();

            foreach (UserProfileQuestionAnswer myAnswer in myQuestionAnswers) {
                myPairedQuestions.Add(new Pair<UserProfileQuestion, QuestionAnswer>() {
                    First = myAnswer.UserProfileQuestion,
                    Second = (QuestionAnswer)Enum.ToObject(typeof(QuestionAnswer), (int)myAnswer.Answer)
                });
            }

            foreach (UserProfileQuestion myQuestion in myExcludedPrivacySettings) {
                myPairedQuestions.Add(new Pair<UserProfileQuestion, QuestionAnswer>() {
                    First = myQuestion,
                    Second = QuestionAnswer.None
                });
            }

            myPairedQuestions = myPairedQuestions.OrderBy(p => p.First.ListOrder).ToList();

            IEnumerable<string> myGroups = (from p in myAllProfileQuestions
                                            select p.QuestionGroup).Distinct<string>().ToList<string>();

            foreach (string myGroup in myGroups) {
                IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>> myGroupPair = (from p in myPairedQuestions
                                                          where p.First.QuestionGroup == myGroup
                                                          select new Pair<UserProfileQuestion, QuestionAnswer>() {
                                                              First = p.First,
                                                              Second = p.Second
                                                          }).ToList();
                myProfileQuestionSelection.Add(myGroup, myGroupPair);
            }


            return myProfileQuestionSelection;
        }


        public void UpdateProfileQuestions(UserInformationModel<User> aUserInfo, UpdateUserProfileQuestionsModel anUpdateUserProfileQuestionsModel) {
            IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>> myQuestionAnswers = anUpdateUserProfileQuestionsModel.UserProfileQuestionsAnswered;
            IEnumerable<string> myQuestionNamesForYes = myQuestionAnswers.Where(qa => qa.Second == QuestionAnswer.Yes).Select(qa => qa.First.Name);
            IEnumerable<string> myQuestionNamesForNo = myQuestionAnswers.Where(qa => qa.Second == QuestionAnswer.No).Select(qa => qa.First.Name);
            IEnumerable<string> myQuestionNamesForDontKnow = myQuestionAnswers.Where(qa => qa.Second == QuestionAnswer.DontKnow).Select(qa => qa.First.Name);

            theProfileQuestionRepository.UpdateAnswersToQuestions(aUserInfo.Details, myQuestionNamesForYes, myQuestionNamesForNo, myQuestionNamesForDontKnow);
        }
    }
}
