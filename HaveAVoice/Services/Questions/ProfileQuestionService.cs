using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Helpers.ProfileQuestions;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.Questions;
using Social.Generic;
using Social.Generic.Models;

namespace HaveAVoice.Services.Questions {
    public class ProfileQuestionService : IProfileQuestionService {
        private IProfileQuestionRepository theProfileQuestionRepository;

        public ProfileQuestionService()
            : this(new EntityProfileQuestionRepository()) { }

        public ProfileQuestionService(IProfileQuestionRepository aPetitionRepo) {
            theProfileQuestionRepository = aPetitionRepo;
        }

        public void IgnoreUserForFutureFriendSuggestions(UserInformationModel<User> aUser, int aUserToIgnore) {
            theProfileQuestionRepository.AddUserToIgnoreList(aUser.Details, aUserToIgnore);
        }

        public IEnumerable<FriendConnectionModel> GetPossibleFriendConnections(UserInformationModel<User> aUserInfo, int aNumberOfConnections) {
            List<string> myQuestionNames = new List<string>();
            myQuestionNames.Add(ProfileQuestion.ABORTION.ToString());
            myQuestionNames.Add(ProfileQuestion.DEATH_PENALTY.ToString());
            myQuestionNames.Add(ProfileQuestion.GUNS_LAW.ToString());
            myQuestionNames.Add(ProfileQuestion.POLITICAL_AFFILIATION.ToString());

            IEnumerable<FriendConnectionModel> myQuestionMatches =
                theProfileQuestionRepository.FindUsersBasedOnQuestion(aUserInfo.Details, myQuestionNames);

            IEnumerable<int> myUsersIgnoring = theProfileQuestionRepository.GetIgnoringUsers(aUserInfo.Details).ToList();

            IEnumerable<int> myFriendRequestsToBeAccepted = aUserInfo.Details.FriendedBy.Select(f => f.FriendUser).Select(f => f.Id).ToList();
            IEnumerable<int> myFriendRequestsSent = aUserInfo.Details.Friends.Select(f => f.SourceUser).Select(f => f.Id).ToList();
            IEnumerable<FriendConnectionModel> myFinalQuestionMatches = (from m in myQuestionMatches
                                                                         where !myFriendRequestsSent.Contains(m.User.Id) 
                                                                         && !myFriendRequestsToBeAccepted.Contains(m.User.Id) 
                                                                         && !myUsersIgnoring.Contains(m.User.Id)
                                                                        select m).ToList();


            myFinalQuestionMatches = myFinalQuestionMatches.OrderBy(f => f.User.Id);

            List<FriendConnectionModel> myUniqueFriendConnectionMatches = new List<FriendConnectionModel>();

            int myLastUserId = 0;
            foreach (FriendConnectionModel myFriendConnection in myFinalQuestionMatches) {
                if (myLastUserId != myFriendConnection.User.Id) {
                    myUniqueFriendConnectionMatches.Add(myFriendConnection);
                    myLastUserId = myFriendConnection.User.Id;
                }
            }

            Random myRandom = new Random();
            myUniqueFriendConnectionMatches = myUniqueFriendConnectionMatches.OrderBy(u => myRandom.Next()).Take(aNumberOfConnections).ToList();
            return myUniqueFriendConnectionMatches;
        }

        public Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>> GetProfileQuestionsGrouped(User aUser) {
            Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>> myProfileQuestionSelection = new Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>>();

            List<UserProfileQuestionAnswer> myQuestionAnswers = theProfileQuestionRepository.GetProfileQuestionAnswersForUser(aUser).ToList();
            List<UserProfileQuestion> myQuestionsAnswered = myQuestionAnswers.Select(a => a.UserProfileQuestion).ToList();
            List<UserProfileQuestion> myAllProfileQuestions = theProfileQuestionRepository.GetProfileQuestions().ToList();
            List<UserProfileQuestion> myExcludedPrivacySettings = myAllProfileQuestions.Except(myQuestionsAnswered).ToList();

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
                    Second = QuestionAnswer.NoAnswer
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
            IEnumerable<string> myResponsesToDeleteSinceNoAnswer = myQuestionAnswers.Where(qa => qa.Second == QuestionAnswer.NoAnswer).Select(qa => qa.First.Name);

            theProfileQuestionRepository.UpdateAnswersToQuestions(aUserInfo.Details, myQuestionNamesForYes, myQuestionNamesForNo, myResponsesToDeleteSinceNoAnswer);
        }
    }
}
