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
using HaveAVoice.Models.Validation;
using HaveAVoice.Services.UserFeatures;
using System.Text;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers.UserInformation;



namespace HaveAVoice.Controllers.Users
{
    public class UserController : HAVBaseController {
        private static string NOT_ALLOWED = "You are not allowed to view that page.";
        private static string EDIT_SUCCESS = "Your account has been edited successfully!";
        private static string CREATE_ACCOUNT_SUCCESS = "User account created! But before you can start doing "
                        + "anything on the site you have to activate your account. Please check your aEmail "
                        + " on how to activate your account.";
        private static string EMAIL_ERROR = "Coulnd't send myException-mail so the User has been activated.";
        private static string CREATE_ACCOUNT_ERROR_MESSAGE = "Your account was not created because of something on our end, "
                        + "sorry!. Please wait a few minutes and try again.";
        private static string CREATE_ACCOUNT_ERROR = "Unable to create a user account.";
        private static string ERROR = "An error has occurred, please try again.";

        
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

        public ActionResult Index() {
            if (Session["UserInformation"] == null)
                return View("Login");
            else
                return RedirectToAction("Index", "Message");
        }

        public ActionResult Create() {
            IEnumerable<SelectListItem> states =
                new SelectList(HAVConstants.STATES, "Select");
            return View("Create", new CreateUserModelBuilder().States(states));
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

        public ActionResult Login() {
            return View("Login");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(string email, string password, bool rememberMe) {
            UserInformationModel userModel = null;
            try {
                userModel = theService.AuthenticateUser(email, password);
            } catch (Exception e) {
                LogError(e, "Unable to authenticate the User.");
                ViewData["Message"] = "Error authenticating. Please try again.";
                return View("Login");
            }

            if (userModel != null) {
                theService.AddToWhoIsOnline(userModel.Details, HttpContext.Request.UserHostAddress);

                CreateUserInformationSession(userModel);
                if (rememberMe) {
                    theService.CreateRememberMeCredentials(userModel.Details);
                }
            } else {
                ViewData["Message"] = "Incorrect aUsername and aPassword combination.";
                return View("Login");
            }

            return RedirectToAction("LoggedIn", "Home");
        }

        private void CreateUserInformationSession(UserInformationModel userModel) {
            Session["UserInformation"] = userModel;
        }

        public ActionResult ActivateAccount(string id) {
            try {
                UserInformationModel userModel = theService.ActivateNewUser(id);
                CreateUserInformationSession(userModel);
                return View("Index");
            } catch (NullUserException) {
                ViewData["Message"] = "That activation code doesn't exist for a user.";
            } catch (NullRoleException e) {
                LogError(e, "Unable to activate the account because of a role issue.");
                ViewData["Message"] = "Couldn't activate the account because of something on our end. Please try again later.";
            } catch(Exception e) {
                LogError(e,  "Couldn't activate the account for some reason.");
                ViewData["Message"] = "Error while activating your account, please try again.";
            }
            return View("ActivateAccount");   
        }

        public ActionResult LogOut() {
            theService.RemoveFromWhoIsOnline(GetUserInformaton(), HttpContext.Request.UserHostAddress);
            Session.Clear();
            return View("Login");
        }

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

        public override ActionResult SendToResultPage(string title, string details) {
            return SendToResultPage(SiteSectionsEnum.User, title, details);
        }

        public ActionResult Edit() {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
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

        public ActionResult UserPictures() {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            } 
            int myUserId = GetUserInformaton().Id;
            IEnumerable<UserPicture> userPictures = theService.GetUserPictures(myUserId);
            UserPicture profilePicture = theService.GetProfilePicture(myUserId);

            return View("UserPictures", new UserPicturesModel(profilePicture, userPictures, new List<int>()));

        }

        [ActionName("UserPictures")]
        [AcceptParameter(Name = "button", Value = "SetProfilePicture")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserPictures_SetProfilePicture(UserPicturesModel aUserPicturesModel) {
            if (aUserPicturesModel.SelectedUserPictures.Count != 1) {
                ViewData["Message"] = "Please select only ONE image to be your profile image.";
            } else {
                try {
                    theService.SetToProfilePicture(aUserPicturesModel.SelectedUserPictures.First(), GetUserInformaton());
                    return SendToResultPage("New profile picture set!");
                } catch (Exception e) {
                    LogError(e, "Error setting profile picture. Please try again.");
                    ViewData["Message"] = "An error occurred while trying to set your new profile picture.";
                }
            }        

            return View("UserPictures", aUserPicturesModel);
        }

        [ActionName("UserPictures")]
        [AcceptParameter(Name = "button", Value = "Delete")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserPictures_Delete(UserPicturesModel aUserPicturesModel) {
            if(aUserPicturesModel.SelectedUserPictures.Count == 0) {
                ViewData["Message"] = "To delete a picture you have to at least select one.";
            } else {
                try {
                    theService.DeleteUserPictures(aUserPicturesModel.SelectedUserPictures);
                    return SendToResultPage("Pictures deleted.");
                } catch(Exception e) {
                    LogError(e, "Error deleting userToListenTo pictures.");
                    ViewData["Message"] = "An error occurred while trying to delete the pictures.";
                }
            }

            return View("UserPictures", aUserPicturesModel);
        }


        public ActionResult EditPrivacy() {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            } 
            User myUser = GetUserInformaton();
            UserPrivacySetting myPrivacy;
            try {
                myPrivacy = theService.GetUserPrivacySettings(myUser);
            } catch (Exception e) {
                LogError(e, new StringBuilder().AppendFormat("Error retrieving the user privacy settings. [userId={0}]", myUser.Id).ToString());
                return SendToErrorPage("Error retrieving your privacy settings. Please try again."); 
            }           
            
            return View("EditPrivacy", myPrivacy);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPrivacy(UserPrivacySetting aUserPrivacySetting) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            } 
            User myUser = GetUserInformaton();
            try {
                theService.UpdatePrivacySettings(myUser, aUserPrivacySetting);
                return SendToResultPage(EDIT_SUCCESS);
            } catch (Exception e) {
                LogError(e, new StringBuilder().AppendFormat("Unable to update the user privacy settings. [userId={0};userPrivacySettingsId{1}]", myUser.Id, aUserPrivacySetting.Id).ToString());
                ViewData["Message"] = "Error updating your privacy settings. Please try again.";

            }
            return View("EditPrivacy", aUserPrivacySetting);
        }

        public ActionResult ForgotPassword() {
            return View("ForgotPassword");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ForgotPassword(string email) {
            try {
                if (theService.ForgotPasswordRequest(email)) {
                    return SendToResultPage("An email has been sent to the email you provided to help reset your password.");
                }
            } catch (EmailException e) {
                LogError(e, "Error sending the email.");
                ViewData["Message"] = ERROR;
            } catch (Exception e) {
                LogError(e, new StringBuilder().AppendFormat("Unable to perform the forgot password function. [email={0}]", email).ToString());
                ViewData["Message"] = ERROR;
            }

            return View("ForgotPassword");
        }

        public ActionResult ChangePassword(string forgotPasswordHash) {
            
            return View("ChangePassword", new StringWrapper(forgotPasswordHash));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangePassword(string email, string forgotPasswordHash, string password, string retypedPassword) {
            try {
                if (theService.ChangePassword(email, forgotPasswordHash, password, retypedPassword)) {
                    return SendToResultPage("Your password has been changed.");
                }
            } catch (Exception e) {
                LogError(e, new StringBuilder().AppendFormat("Unable to change the password. [email={0}]", email).ToString());
                ViewData["Message"] = "Unable to change the password. Please try again.";
            }

            return View("ForgotPassword", new StringWrapper(forgotPasswordHash));
        }

        public ActionResult Gallery() {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            int myUserId = GetUserInformaton().Id;

            IEnumerable<UserPicture> myPictures = new List<UserPicture>();
            try {
                myPictures = theService.GetUserPictures(myUserId);
            } catch (Exception e) {
                LogError(e, new StringBuilder().AppendFormat("Unable to get the user pictures. [userId={0}]", myUserId).ToString());
                SendToErrorPage("Unable to load your gallery. Please try again.");
            }

            return View("Gallery", myPictures);
        }

        public ActionResult BecomeAFan(int id) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
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
    }
}