using System.Web.Mvc;

namespace HaveAVoice.Controllers  {
    public abstract class BaseController : Controller {
        /*
        private const string AUTHENTICATION_DURING_COOKIE_ERROR = "An error occurred while trying to authentication when grabbing the login info from a cookie.";
        private const string AFTER_AUTHENTICATION_ERROR = "An error occurred after authentication after a cookie.";
        private const string READ_ME_ERROR = "An error occurred while reading the read me credentials.";

        public IUserInformation<User, WhoIsOnline> theUserInformation;

        private IBaseService<User> theErrorService;
        private IHAVAuthenticationService theAuthService;
        private IWhoIsOnlineService<User, WhoIsOnline> theWhoIsOnlineService;

        public BaseController(IBaseService<User> baseService) :
            this(baseService, new HAVAuthenticationService(), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository())) { }

        public BaseController(IBaseService<User> baseService, IHAVAuthenticationService anAuthService, IWhoIsOnlineService<User, WhoIsOnline> aWhoIsOnlineService) {
            theErrorService = baseService;
            theAuthService = anAuthService;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        protected override void Initialize(RequestContext rc) {
            base.Initialize(rc);
            HAVUserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository())));
        }

        protected User GetUserInformaton() {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            return myUserInformation != null ? myUserInformation.Details : null;
        }

        protected AbstractUserModel<User> GetSocialUserInformation() {
            return SocialUserModel.Create(GetUserInformaton());
        }

        protected UserInformationModel<User> GetUserInformatonModel() {
            return HAVUserInformationFactory.GetUserInformation();
        }

        protected void RefreshUserInformation() {
            UserInformationModel<User> myUserInformationModel = GetUserInformatonModel();
            try {
                myUserInformationModel = 
                    theAuthService.RefreshUserInformationModel(myUserInformationModel.Details.Email, myUserInformationModel.Details.Password, new ProfilePictureStrategy());
            } catch (Exception myException) {
                LogError(myException, String.Format("Big problem! Was unable to refresg the user information model for userid={0}", myUserInformationModel.Details.Id));
            }
            Session["UserInformation"] = myUserInformationModel;
        }

        protected bool IsLoggedIn() {
            if (!HAVUserInformationFactory.IsLoggedIn()) {

                User myUser = null;

                try {
                    myUser = theAuthService.ReadRememberMeCredentials();
                } catch (Exception myException) {
                    LogError(myException, READ_ME_ERROR);
                }

                if (myUser != null) {
                    UserInformationModel<User> userModel = null;
                    AbstractUserModel<User> mySocialUserModel = SocialUserModel.Create(myUser);
                    try {
                        userModel = theAuthService.CreateUserInformationModel(mySocialUserModel, new ProfilePictureStrategy());
                    } catch (Exception e) {
                        LogError(e, AUTHENTICATION_DURING_COOKIE_ERROR);
                    }

                    if (userModel != null) {
                        try {
                            theWhoIsOnlineService.AddToWhoIsOnline(userModel.Details, HttpContext.Request.UserHostAddress);

                            CreateUserInformationSession(userModel);
                            theAuthService.CreateRememberMeCredentials(SocialUserModel.Create(userModel.Details));
                        } catch (Exception e) {
                            LogError(e, AFTER_AUTHENTICATION_ERROR);
                        }
                    }
                }
            }

            return HAVUserInformationFactory.IsLoggedIn();
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
            AbstractUserModel<User> mySocialUserInfo = GetSocialUserInformation();
            theErrorService.LogError(mySocialUserInfo, anException, aDetails);
        }

        protected ActionResult RedirectToLogin() {
            TempData["Message"] = MessageHelper.NormalMessage("You must be logged in to do that.");
            return RedirectToAction("Login", "Authentication");
        }

        protected ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }

        protected ActionResult RedirectToProfile(int anId) {
            return RedirectToAction("Show", "Profile", new { id = anId });
        }

        protected ActionResult RedirectToHomePage() {
            return RedirectToAction("NotLoggedIn", "Home");
        }

        private void AddMessageToSession(string title, string details) {
            MessageModel messageModel = new MessageModel();
            messageModel.Title = title;
            messageModel.Details = details;
            Session["Message"] = messageModel;
        }

        private void AddErrorToSession(string error) {
            ErrorModel errorModel = new ErrorModel();
            errorModel.ErrorMessage = error;
            Session["ErrorMessage"] = errorModel;
        }

        private void CreateUserInformationSession(UserInformationModel<User> aUserModel) {
            Session["UserInformation"] = aUserModel;
        }
         * */
    }
}
