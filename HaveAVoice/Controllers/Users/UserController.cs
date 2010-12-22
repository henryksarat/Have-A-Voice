using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.ActionMethods;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Validation;
using HaveAVoice.Services.UserFeatures;
using System.Text;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers.UserInformation;



namespace HaveAVoice.Controllers.Users
{
    public class UserController : HAVBaseController {
        private static string EDIT_SUCCESS = "Your account has been edited successfully!";
        private static string CREATE_ACCOUNT_SUCCESS = "User account created! But before you can start doing "
                        + "anything on the site you have to activate your account. Please check your aEmail "
                        + " on how to activate your account.";
        private static string EMAIL_ERROR = "Couldn't send activation e-mail so the User has been activated.";
        private static string CREATE_ACCOUNT_ERROR_MESSAGE = "An error has occurred. Please try again.";
        private static string CREATE_ACCOUNT_ERROR = "Unable to create a user account.";

        
        private IHAVUserService theService;
        private IValidationDictionary theValidationDictionary;

        public UserController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theService = new HAVUserService(theValidationDictionary);
        }

        public UserController(IHAVUserService service, IHAVBaseService baseService)
            : base(baseService) {
            theService = service;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index() {
            if (Session["UserInformation"] == null)
                return View("Login");
            else
                return RedirectToAction("Index", "Message");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create() {
            IEnumerable<SelectListItem> myStates = new SelectList(HAVConstants.STATES, "Select");
            return View("Create", new CreateUserModelBuilder().States(myStates));
        }

        [CaptchaValidator]  
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(CreateUserModelBuilder aUserToCreate, bool captchaValid) {
            try {
                bool myResult = theService.CreateUser(aUserToCreate.Build(), captchaValid, aUserToCreate.Agreement(), HttpContext.Request.UserHostAddress);
                if (myResult) {
                    return SendToResultPage(CREATE_ACCOUNT_SUCCESS);
                }
            } catch (EmailException e) {
                LogError(e, EMAIL_ERROR);
                return SendToResultPage("User account created! You can login and start posting!");
            } catch (Exception e) {
                ViewData["Message"] = CREATE_ACCOUNT_ERROR_MESSAGE;
                LogError(e, CREATE_ACCOUNT_ERROR);
            } 

            return View("Create", aUserToCreate);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult UserList() {
            try {
                List<UserDetailsModel> userList = theService.GetUserList(GetUserInformaton()).ToList<UserDetailsModel>();

                if (userList.Count == 0) {
                    ViewData["Message"] = "There are no registered users.";
                }
                return View("UserList", userList);
            } catch (Exception exception) {
                LogError(exception, "Unable to get the User List");
                return SendToErrorPage("Unable to retrieve the userToListenTo list, please try again."); 
            }
        }

        public ActionResult Edit() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            } 
            User myUser = GetUserInformaton();

            EditUserModel myModel = null;

            try {
                myModel = theService.GetUserForEdit(myUser);
            } catch (Exception e) {
                LogError(e, "Unable to get the model to edit the user");
                SendToErrorPage("Unable to edit your settings. Please try again.");
            }

            return View("Edit", myModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(EditUserModel userToEdit) {
            try {
                if (theService.EditUser(userToEdit)) {
                    return SendToResultPage(EDIT_SUCCESS);
                }
            } catch (Exception exception) {
                LogError(exception, "Error editing the user.");
                ViewData["Message"] = "An error has occurred please try your submission again later.";
            }

            return View("Edit", userToEdit);
        }

        public ActionResult BecomeAFan(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            try {
                theService.AddFan(myUser, id);
            } catch (Exception e) {
                LogError(e, "Unable to add the myUser as a listener.");
                return SendToErrorPage("Unable to become a fan. Please try again.");
            }

            return RedirectToAction("Show", "Profile", id);
        }

        public ActionResult FanFeed() {
            return null;
        }

        protected override ActionResult SendToResultPage(string title, string details) {
            return SendToResultPage(SiteSectionsEnum.User, title, details);
        }
    }
}