using System.Web.Mvc;
using Social.Admin.Helpers;
using Social.Authentication;
using Social.Authentication.Services;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Services;
using Social.Users.Services;

namespace BaseWebsite.Controllers.Admin {
    public abstract class AbstractAdminController<T, U, V, W, X, Y, Z> : BaseController<T, U, V, W, X, Y, Z> {
        public AbstractAdminController(IBaseService<T> aBaseService, IUserInformation<T, Z> aUserInformation, IAuthenticationService<T, U, V, W, X, Y> anAuthService,
                                      IWhoIsOnlineService<T, Z> aWhoIsOnlineService) :
            base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) { }

        protected ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!PermissionHelper<T>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.View_Admin)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }
            return View("Index");
        }
    }
}
