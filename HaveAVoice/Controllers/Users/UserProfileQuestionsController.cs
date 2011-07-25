using System;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services;
using Social.User.Models;
using Social.User.Services;
using Social.Generic.Services;
using HaveAVoice.Services.Questions;
using HaveAVoice.Repositories.Questions;
using System.Collections.Generic;
using Social.Generic;
using HaveAVoice.Helpers.ProfileQuestions;
using Social.BaseWebsite.Models;
using Social.Generic.Models;
using System.Linq;

namespace HaveAVoice.Controllers.Users {
    public class UserProfileQuestionsController : HAVBaseController {
        private static string EDIT_SUCCESS = "Your answers to the questions have been saved!";
        private static string NO_SUGGESTIONS = "Currently there are no new friend suggestions for you. Make sure you answered the questionnaire under the settings menu.";

        private const string RETRIEVE_FAIL = "Error retreiving your answers to the profile questionaiire. Please try again.";
        private const string EDIT_FAIL = "Error updating your answers to the questions! Please try again.";
        private const string SUGGESTION_FAIL = "Error trying to find friend suggestions for you. Please try again.";

        private const string EDIT_VIEW = "Edit";
        private const string LIST_VIEW = "List";

        private IProfileQuestionService theProfileQuestionsService;

        public UserProfileQuestionsController() {
            theProfileQuestionsService = new ProfileQuestionService(new EntityProfileQuestionRepository());
        }

        public UserProfileQuestionsController(IBaseService<User> aBaseService, IProfileQuestionService aProfileQuestionService) {
            theProfileQuestionsService = aProfileQuestionService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>> myProfileQuestions = 
                new Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>>();
            ILoggedInModel<Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>>> myLoggedInWrapperModel =
                new LoggedInWrapperModel<Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>>>(myUser, SiteSection.MyProfile);
            try {

                myProfileQuestions = theProfileQuestionsService.GetProfileQuestionsGrouped(myUser);
                myLoggedInWrapperModel.Set(myProfileQuestions);
            } catch (Exception e) {
                LogError(e, RETRIEVE_FAIL);
                return SendToErrorPage(RETRIEVE_FAIL);
            }

            return View(EDIT_VIEW, myLoggedInWrapperModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(UpdateUserProfileQuestionsModel aSettings) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();
                theProfileQuestionsService.UpdateProfileQuestions(myUser, aSettings);
                TempData["Message"] += MessageHelper.SuccessMessage(EDIT_SUCCESS);
            } catch (Exception e) {
                LogError(e, EDIT_FAIL);
                TempData["Message"] += MessageHelper.ErrorMessage(EDIT_FAIL);
            }
            return RedirectToAction(EDIT_VIEW);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List(UpdateUserProfileQuestionsModel aSettings) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUser = GetUserInformatonModel();
            LoggedInListModel<FriendConnectionModel> myLoggedIn = new LoggedInListModel<FriendConnectionModel>(myUser.Details, SiteSection.FriendSuggestion);
            IEnumerable<FriendConnectionModel> myConnectionModel = new List<FriendConnectionModel>();

            try {
                myConnectionModel = theProfileQuestionsService.GetPossibleFriendConnections(myUser);

                if (myConnectionModel.Count<FriendConnectionModel>() == 0) {
                    TempData["Message"] = MessageHelper.NormalMessage(NO_SUGGESTIONS);
                }
            } catch (Exception e) {
                LogError(e, SUGGESTION_FAIL);
                TempData["Message"] += MessageHelper.ErrorMessage(SUGGESTION_FAIL);
            }

            myLoggedIn.Set(myConnectionModel);

            return View(LIST_VIEW, myLoggedIn);
        }
    }
}
