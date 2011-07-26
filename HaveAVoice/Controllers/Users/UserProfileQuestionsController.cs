using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers.ProfileQuestions;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.Questions;
using HaveAVoice.Services.Questions;
using Social.BaseWebsite.Models;
using Social.Generic;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Generic.ActionFilters;

namespace HaveAVoice.Controllers.Users {
    public class UserProfileQuestionsController : HAVBaseController {
        private const string EDIT_SUCCESS = "Your answers to the questions have been saved!";
        private const string NO_SUGGESTIONS = "Currently there are no new friend suggestions for you. Make sure you answered the questionnaire under the settings menu.";
        private const string IGNORE_SUCCESS = "You will no longer receive friend suggestions for that user.";

        private const string RETRIEVE_FAIL = "Error retreiving your answers to the profile questionaiire. Please try again.";
        private const string EDIT_FAIL = "Error updating your answers to the questions! Please try again.";
        private const string SUGGESTION_FAIL = "Error trying to find friend suggestions for you. Please try again.";
        private const string IGNORE_FAIL = "An error occurred while trying to ignore that user. Please try again later.";

        private const string EDIT_VIEW = "Edit";
        private const string LIST_VIEW = "List";

        private IProfileQuestionService theProfileQuestionsService;

        public UserProfileQuestionsController() {
            theProfileQuestionsService = new ProfileQuestionService(new EntityProfileQuestionRepository());
        }

        public UserProfileQuestionsController(IBaseService<User> aBaseService, IProfileQuestionService aProfileQuestionService) {
            theProfileQuestionsService = aProfileQuestionService;
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
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

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
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

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult List(UpdateUserProfileQuestionsModel aSettings) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUser = GetUserInformatonModel();
            LoggedInListModel<FriendConnectionModel> myLoggedIn = new LoggedInListModel<FriendConnectionModel>(myUser.Details, SiteSection.FriendSuggestion);
            IEnumerable<FriendConnectionModel> myConnectionModel = new List<FriendConnectionModel>();

            try {
                myConnectionModel = theProfileQuestionsService.GetPossibleFriendConnections(myUser, 3);

                if (myConnectionModel.Count<FriendConnectionModel>() == 0) {
                    TempData["Message"] += MessageHelper.NormalMessage(NO_SUGGESTIONS);
                }
            } catch (Exception e) {
                LogError(e, SUGGESTION_FAIL);
                TempData["Message"] += MessageHelper.ErrorMessage(SUGGESTION_FAIL);
            }

            myLoggedIn.Set(myConnectionModel);

            return View(LIST_VIEW, myLoggedIn);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult IgnoreUser(int userToIgnore, string controllerRedirect, string actionRedirect) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();
                theProfileQuestionsService.IgnoreUserForFutureFriendSuggestions(myUser, userToIgnore);
                TempData["Message"] += MessageHelper.SuccessMessage(IGNORE_SUCCESS);
            } catch (Exception e) {
                LogError(e, SUGGESTION_FAIL);
                TempData["Message"] += MessageHelper.SuccessMessage(IGNORE_FAIL);
            }

            return RedirectToAction(actionRedirect, controllerRedirect);
        }
    }
}
