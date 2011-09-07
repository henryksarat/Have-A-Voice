using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Users.Services;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.SendItems;
using UniversityOfMe.UserInformation;
using Social.Generic.Models;

namespace UniversityOfMe.Controllers.SendItems {
    public class SendItemsController : UOFMeBaseController {
        private const string SENT_ITEM_SUCCESS = "Sent the user a ";
        private const string SEND_ITEM_ERROR = "Unable to send to the user. Please try again.";

        private const string ITEM_MARKED_AS_SEEN = "The item that was sent has been marked as seen.";
        private const string ITEM_MARKED_AS_SEEN_ERROR = "Error marking the item as seen.";

        private ISendItemsService theSendItemsService;

        public SendItemsController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theSendItemsService = new SendItemsService();
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult SendItem(int id, SendItemOptions sendItem) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                theSendItemsService.SendItemToUser(id, GetUserInformatonModel().Details, sendItem);
                TempData["Message"] += MessageHelper.SuccessMessage(SENT_ITEM_SUCCESS + sendItem.ToString().ToLower() + ".");
            } catch (Exception myException) {
                LogError(myException, SEND_ITEM_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(SEND_ITEM_ERROR);
            }

            return RedirectToAction("Show", "Profile", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult MarkAsSeen(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                theSendItemsService.MarkItemAsSeen(myUserInfo, id);
                TempData["Message"] += MessageHelper.SuccessMessage(ITEM_MARKED_AS_SEEN);
            } catch (Exception myException) {
                LogError(myException, ITEM_MARKED_AS_SEEN_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(ITEM_MARKED_AS_SEEN_ERROR);
            }

            return RedirectToProfile();
        }

    }
}
