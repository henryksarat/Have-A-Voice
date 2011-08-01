using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Authentication.Services;
using Social.Email;
using Social.Email.Exceptions;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.User.Helpers;
using Social.User.Repositories;
using Social.User.Services;
using Social.Users.Services;
using Social.Validation;

namespace BaseWebsite.Controllers.Users {
    //T = User
    //U = Role
    //V = Permission
    //W = UserRole
    //X = PrivacySetting
    //Y = RolePermission
    //Z = WhoIsOnline
    public abstract class AbstractUserController<T, U, V, W, X, Y, Z> : BaseController<T, U, V, W, X, Y, Z> {
        private const string CREATE_ACCOUNT_TITLE = "User account created...almost!";
        private const string CREATE_ACCOUNT_SUCCESS = "An email has been sent to the email you provided. Follow the instructions in the email to activate your account so you can login and start using the site.";
        private const string EMAIL_ERROR = "Couldn't send activation e-mail so the User has been activated.";
        private const string CREATE_ACCOUNT_ERROR_MESSAGE = "An error has occurred. Please try again.";
        private const string CREATE_ACCOUNT_ERROR = "Unable to create a user account.";

        
        private IUserService<T, U, W> theRegistrationService;
        private IValidationDictionary theValidationDictionary;

        public AbstractUserController(IBaseService<T> aBaseService, 
                                      IUserInformation<T, Z> aUserInformation, 
                                      IAuthenticationService<T, U, V, W, X, Y> anAuthService, 
                                      IWhoIsOnlineService<T, Z> aWhoIsOnlineService,
                                      IUserRepository<T, U, W> aUserRegistrationRepo)
            : base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theRegistrationService = new UserService<T, U, W>(theValidationDictionary, aUserRegistrationRepo, new SocialEmail());
        }

        protected ActionResult Index() {
            if (Session["UserInformation"] == null) {
                return View("Login");
            } else {
                return RedirectToAction("Index", "Message");
            }
        }

        protected ActionResult Create(AbstractUserModel<T> aUserToCreate, bool captchaValid, bool anAgreement, string aBaseUrl, 
                                      string anActivationSubject, string anActivationBody, 
                                      IRegistrationStrategy<T> aRegistrationStrategy) {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            try {
                bool myResult = theRegistrationService.CreateUser(aUserToCreate, captchaValid, 
                                                                  anAgreement, HttpContext.Request.UserHostAddress, 
                                                                  aBaseUrl, anActivationSubject, anActivationBody, 
                                                                  aRegistrationStrategy);
                if (myResult) {
                    return SendToResultPage(CREATE_ACCOUNT_TITLE, CREATE_ACCOUNT_SUCCESS);
                }
            } catch (EmailException e) {
                LogError(e, EMAIL_ERROR);
                return SendToResultPage("User account created! You can login and start posting!");
            } catch (Exception e) {
                ViewData["Message"] = ErrorMessage(CREATE_ACCOUNT_ERROR_MESSAGE);
                LogError(e, CREATE_ACCOUNT_ERROR);
            }
            return null;
        }
    }
}