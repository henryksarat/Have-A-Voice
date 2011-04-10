using System;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Services.UserFeatures;

namespace HaveAVoice.Controllers.Users {
    public class NotificationController : HAVBaseController {
        private const string NOTIFICATION_ERROR = "Error retrieving notifications.";
        private const string NO_NOTIFICAITONS = "There are no notifications.";

        private const string LIST_VIEW = "List";

        private IHAVNotificationService theNotificationService;

        public NotificationController() {
            theNotificationService = new HAVNotificationService();
        }

        public NotificationController(IHAVNotificationService aNoticiationService) {
            theNotificationService = aNoticiationService;
        }

        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            User myUser = GetUserInformatonModel().Details;
            LoggedInListModel<NotificationModel> myModel = new LoggedInListModel<NotificationModel>(myUser, SiteSection.Notification);
            try {
                myModel.Models = theNotificationService.GetNotifications(myUser);

                if (myModel.Models.Count<NotificationModel>() == 0) {
                    ViewData["Message"] = MessageHelper.NormalMessage(NO_NOTIFICAITONS);
                }
            } catch (Exception myException) {
                LogError(myException, NOTIFICATION_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(NOTIFICATION_ERROR);
            }

            return View(LIST_VIEW, myModel);
        }
    }
}
