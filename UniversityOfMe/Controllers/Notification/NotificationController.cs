using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Users.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Dating;
using UniversityOfMe.UserInformation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Services.Notifications;
using UniversityOfMe.Models.View;
using System.Collections.Generic;
using Social.Generic.Models;

namespace UniversityOfMe.Controllers.Dating {
    public class NotificationController : UOFMeBaseController {
        private const string NOTIFICAITON_LOAD_ERROR = "An error occurred while loading your notifications. Please try again.";

        INotificationService theNotificationService;

        public NotificationController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theNotificationService = new NotificationService();
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                LoggedInListModel<NotificationModel> myLoggedInModel = new LoggedInListModel<NotificationModel>(myUserInfo.Details);
                IEnumerable<NotificationModel> myNotifications = theNotificationService.GetNotificationsForUser(GetUserInformatonModel().Details, 50);
                myLoggedInModel.Set(myNotifications);
                return View("List", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, NOTIFICAITON_LOAD_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(NOTIFICAITON_LOAD_ERROR);
                return RedirectToProfile();
            }
        }
    }
}
