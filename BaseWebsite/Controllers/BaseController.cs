using System.Web.Mvc;
using Social.Generic.Services;
using Social.Users.Services;
using Social.Authentication.Services;
using Social.Authentication;
using Social.Generic.Models;
using System;
using Social.Authentication.Helpers;
using System.Web.Security;
using System.Web;

namespace BaseWebsite.Controllers {
    //T = User
    //U = Role
    //V = Permission
    //W = UserRole
    //X = PrivacySetting
    //Y = RolePermission
    //Z = WhoIsOnline
    public abstract class BaseController<T, U, V, W, X, Y, Z> : Controller {
        private const string AUTHENTICATION_DURING_COOKIE_ERROR = "An error occurred while trying to authentication when grabbing the login info from a cookie.";
        private const string AFTER_AUTHENTICATION_ERROR = "An error occurred after authentication after a cookie.";
        private const string READ_ME_ERROR = "An error occurred while reading the read me credentials.";

        public static IUserInformation<T, Z> theUserInformation;
        private IBaseService<T> theErrorService;
        private IAuthenticationService<T, U, V, W, X, Y> theAuthService;
        private IWhoIsOnlineService<T, Z> theWhoIsOnlineService;

        public BaseController(IBaseService<T> baseService, IUserInformation<T, Z> aUserInformation, IAuthenticationService<T, U, V, W, X, Y> anAuthService, IWhoIsOnlineService<T, Z> aWhoIsOnlineService) {
            theErrorService = baseService;
            theUserInformation = aUserInformation;
            theAuthService = anAuthService;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        protected abstract AbstractUserModel<T> GetSocialUserInformation();
        protected abstract AbstractUserModel<T> CreateSocialUserModel(T aUser);

        protected abstract IProfilePictureStrategy<T> ProfilePictureStrategy();
        
        protected abstract string UserEmail();
        protected abstract string UserPassword();
        protected abstract int UserId();

        protected abstract string ErrorMessage(string aMessage);
        protected abstract string NormalMessage(string aMessage);
        protected abstract string SuccessMessage(string aMessage);
        protected abstract string WarningMessage(string aMessage);


        protected abstract ActionResult RedirectToProfile();

        protected IUserInformation<T, Z> GetUserInformationInstance() {
            return theUserInformation;
        }

        protected T GetUserInformaton() {
            UserInformationModel<T> myUserInformation = GetUserInformatonModel();
            return myUserInformation != null ? myUserInformation.Details : default(T);
        }

        protected void ForceUserInformationRefresh() {
            theUserInformation.ForceUserInformationClear();
        }

        protected UserInformationModel<T> GetUserInformatonModel() {
            if (HttpContext.User.Identity.IsAuthenticated) {
                return theUserInformation.GetUserInformaton();
            } else {
                //HACK JOB OF THE YEAR:
                //Since FormsAuth times out after 5 minutes for SOME reason I ALWAYS create a cookie on login
                //then if we cant read forms auth I read the cookie AND then set the AUTH cookie in IsLoggedIn().. here I just read from the cookie and hit the database until
                //next time when Forms Auth kicks in.
                T myUser = default(T);
                try {
                    myUser = theAuthService.ReadRememberMeCredentials();
                } catch (Exception myException) {
                    LogError(myException, READ_ME_ERROR);
                }
                AbstractUserModel<T> mySocialUserModel = CreateSocialUserModel(myUser);
                if (mySocialUserModel != null) {
                    return theUserInformation.GetUserInformaton(mySocialUserModel.Id);
                } else {
                    return null;
                }
            }
        }
        
        protected bool IsLoggedIn() {
            if (!HttpContext.User.Identity.IsAuthenticated) {

                T myUser = default(T);

                try {
                    myUser = theAuthService.ReadRememberMeCredentials();
                } catch (Exception myException) {
                    LogError(myException, READ_ME_ERROR);
                }

                if (myUser != null) {
                    UserInformationModel<T> userModel = null;
                    AbstractUserModel<T> mySocialUserModel = CreateSocialUserModel(myUser);
                    try {
                        userModel = theAuthService.CreateUserInformationModel(mySocialUserModel, ProfilePictureStrategy());
                    } catch (Exception e) {
                        LogError(e, AUTHENTICATION_DURING_COOKIE_ERROR);
                    }

                    if (userModel != null) {
                        try {
                            /*
                            theWhoIsOnlineService.AddToWhoIsOnline(userModel.Details, HttpContext.Request.UserHostAddress);
                            
                            FormsAuthenticationTicket myTicket = new FormsAuthenticationTicket(
                                1,
                                userModel.UserId.ToString(),
                                DateTime.Now,
                                DateTime.Now.AddHours(40),
                                true,
                                userModel.UserId.ToString());
                            */
                            //string myEncryptedTicket = FormsAuthentication.Encrypt(myTicket);

                            //HttpCookie myAuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, myEncryptedTicket);

                            FormsAuthentication.SetAuthCookie(userModel.UserId.ToString(), true);
                            
                            //myAuthCookie.Expires = myTicket.Expiration;

                            
                            //System.Web.HttpContext.Current.Response.Cookies.Add(myAuthCookie);
                            //theAuthService.CreateRememberMeCredentials(CreateSocialUserModel(userModel.Details));
                            return true;
                        } catch (Exception e) {
                            LogError(e, AFTER_AUTHENTICATION_ERROR);
                        }
                    }
                }
            }

            return HttpContext.User.Identity.IsAuthenticated;
        }
        
        protected ActionResult SendToErrorPage(string error) {
            AddErrorToSession(error);
            return RedirectToAction("Error", "Shared");
        }

        protected ActionResult SendToResultPage(string title, string details) {
            AddMessageToSession(title, details);
            return RedirectToAction("Result", "Shared");
        }

        protected ActionResult SendToResultPage(string details) {
            return SendToResultPage(null, details);
        }

        protected void LogError(Exception anException, string aDetails) {
            AbstractUserModel<T> mySocialUserInfo = GetSocialUserInformation();
            theErrorService.LogError(mySocialUserInfo, anException, aDetails);
        }

        protected ActionResult RedirectToLogin() {
            TempData["Message"] += NormalMessage("You must be logged in to do that.");
            return RedirectToAction("Login", "Authentication");
        }

        protected ActionResult RedirectToProfile(int anId) {
            return RedirectToAction("Show", "Profile", new { id = anId });
        }

        protected ActionResult RedirectToHomePage() {
            return RedirectToAction("Main", "Home");
        }

        protected IAuthenticationService<T, U, V, W, X, Y> GetAuthenticationService() {
            return theAuthService;
        }

        private void AddMessageToSession(string title, string details) {
            MessageModel messageModel = new MessageModel();
            messageModel.Title = title;
            messageModel.Details = details;
            Session["Message"] = messageModel;
        }

        private void AddErrorToSession(string error) {
            Session["ErrorMessage"] = new StringModel(error);
        }
    }
}
