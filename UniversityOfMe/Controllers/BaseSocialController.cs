using System.Web.Mvc;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Services;
using Social.Generic.Models;
using Social.Generic.Services;
using UniversityOfMe.Models;
using Social.Users.Services;
using UniversityOfMe.Repositories;
using Social.User.Services;
using UniversityOfMe.Repositories.UserRepos;
using UniversityOfMe.Repositories.AuthenticationRepos;
using UniversityOfMe.Repositories.AdminRepos;

namespace UniversityOfMe.Controllers  {
    public abstract class BaseSocialController : Controller {
        public IUserInformation<User, WhoIsOnline> theUserInformation;

        private IBaseService<User> theErrorService;
        private IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting> theAuthService;
        private IWhoIsOnlineService<User, WhoIsOnline> theWhoIsOnlineService;

        public BaseSocialController(IBaseService<User> baseService) :
            this(baseService, 
                new AuthenticationService<User, Role, Permission, UserRole, PrivacySetting>(
                    new UserRetrievalService<User>(new EntityUserRetrievalRepository()),
                    new UserPrivacySettingsService<User, PrivacySetting>(new EntityUserPrivacySettingsRepository()),
                    new EntityAuthenticationRepository(),
                    new EntityUserRepository(),
                    new EntityRoleRepository()),
                new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())) { }

        public BaseSocialController(IBaseService<User> baseService, 
                                    IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting> anAuthService, 
                                    IWhoIsOnlineService<User, WhoIsOnline> aWhoIsOnlineService) {
            theErrorService = baseService;
            theAuthService = anAuthService;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        protected ActionResult SendToErrorPage(string error) {
            AddErrorToSession(error);
            return RedirectToAction("Error", "Shared");
        }

        protected ActionResult SendToResultPage(string aDetails) {
            AddMessageToSession(aDetails);
            return RedirectToAction("Result", "Shared");
        }

        private void AddMessageToSession(string aDetails) {
            Session["Message"] = new StringModel(aDetails);
        }

        private void AddErrorToSession(string anError) {
            Session["ErrorMessage"] = new StringModel(anError);
        }
    }
}
