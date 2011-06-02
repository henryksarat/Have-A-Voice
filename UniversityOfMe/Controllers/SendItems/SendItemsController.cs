using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services;
using UniversityOfMe.Services.SendItems;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.SendItems {
    public class SendItemsController : UOFMeBaseController {
        private const string SENT_ITEM_SUCCESS = "Sent the user a ";
        private const string SEND_ITEM_ERROR = "Unable to send to the user. Please try again.";

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
                TempData["Message"] = SENT_ITEM_SUCCESS + sendItem.ToString().ToLower() + ".";
            } catch (Exception myException) {
                LogError(myException, SEND_ITEM_ERROR);
                TempData["Message"] = SEND_ITEM_ERROR;
            }

            return RedirectToAction("Show", "Profile", new { id = id });
        }
    }
}
