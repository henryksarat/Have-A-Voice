using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models.SocialWrappers;
using Social.Generic;
using Social.Generic.Helpers;
using Social.User.Models;
using HaveAVoice.Helpers.ProfileQuestions;

namespace HaveAVoice.Models.View {
    public class UpdateUserProfileQuestionsModelBinder : IModelBinder {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            User myUser = HAVUserInformationFactory.GetUserInformation().Details;
            UpdateUserProfileQuestionsModel myModel = new UpdateUserProfileQuestionsModel();
            List<Pair<UserProfileQuestion, QuestionAnswer>> myAllPairs = new List<Pair<UserProfileQuestion, QuestionAnswer>>();

            foreach (ProfileQuestion mySetting in Enum.GetValues(typeof(ProfileQuestion))) {
                string myAnswer = BinderHelper.GetA(bindingContext, mySetting.ToString());
                QuestionAnswer myQuestionAnswer = QuestionAnswer.NoAnswer;
                
                if (!string.IsNullOrEmpty(myAnswer)) {
                    //There is a special case if the question is about political affiliation, 
                    //and it's only with enums to make them more clear, the underlying int values match up..
                    //This is done to make the code more understandable
                    if (mySetting.Equals(ProfileQuestion.POLITICAL_AFFILIATION)) {
                        PoliticalAffiliation myPoliticalAffiliation = (PoliticalAffiliation)Enum.Parse(typeof(PoliticalAffiliation), myAnswer);
                        myQuestionAnswer = (QuestionAnswer)(int)myPoliticalAffiliation;
                    } else if (mySetting.Equals(ProfileQuestion.ABORTION)) {
                        AbortionAnswer myAbortionAnswer = (AbortionAnswer)Enum.Parse(typeof(AbortionAnswer), myAnswer);
                        myQuestionAnswer = (QuestionAnswer)(int)myAbortionAnswer;
                    } else {
                        myQuestionAnswer = (QuestionAnswer)Enum.Parse(typeof(QuestionAnswer), myAnswer);
                    }
                }


                UserProfileQuestion myUserProfileQuestion = UserProfileQuestion.CreateUserProfileQuestion(mySetting.ToString(), 0, string.Empty, string.Empty, string.Empty, 0);
                Pair<UserProfileQuestion, QuestionAnswer> myPair = new Pair<UserProfileQuestion, QuestionAnswer>() {
                    First = myUserProfileQuestion,
                    Second = myQuestionAnswer
                };
                myAllPairs.Add(myPair);
            }

            myModel.UserProfileQuestionsAnswered = myAllPairs;

            return myModel;      
        }
    }
}
