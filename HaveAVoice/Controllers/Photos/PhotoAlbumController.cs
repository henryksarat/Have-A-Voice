using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.ActionMethods;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Services.Helpers;
using System.Web;
using HaveAVoice.Controllers.ActionFilters;

namespace HaveAVoice.Controllers.Users.Photos {
    public class PhotoAlbumController : HAVBaseController {
        private static string CREATED_SUCCESS = "Photo album created!";

        private static string ALBUM_LIST_ERROR = "Error retrieving your photo album list. Please try again.";
        private static string CREATED_FAIL = "Error creating photo album. Please try again.";

        private static string LIST_VIEW = "List";
        

        private IHAVPhotoService thePhotoService;

        public PhotoAlbumController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            thePhotoService = new HAVPhotoService();
        }

        public PhotoAlbumController(IHAVPhotoService aPhotoService, IHAVBaseService aBaseService)
            : base(aBaseService) {
                thePhotoService = aPhotoService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();

            LoggedInListModel<PhotoAlbum> myModel = new LoggedInListModel<PhotoAlbum>(myUser, SiteSection.Photos);
            try {
                myModel.Models = thePhotoService.GetPhotoAlbumsForUser(myUser);
            } catch (Exception e) {
                LogError(e, ALBUM_LIST_ERROR);
                return SendToErrorPage(ALBUM_LIST_ERROR);
            }

            return View(LIST_VIEW, myModel);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Add(string name, string description) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                User myUser = GetUserInformatonModel().Details;
                thePhotoService.CreatePhotoAlbum(myUser, name, description);
                TempData["Message"] = CREATED_SUCCESS;
                return RedirectToAction(LIST_VIEW);
            } catch (Exception myException) {
                TempData["Message"] = CREATED_FAIL;
                LogError(myException, CREATED_FAIL);
            }

            return RedirectToAction(LIST_VIEW);
        }
    }
}
