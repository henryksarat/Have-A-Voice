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
using HaveAVoice.Validation;

namespace HaveAVoice.Controllers.Users.Photos {
    public class PhotoAlbumController : HAVBaseController {
        private static string CREATED_SUCCESS = "Photo album created!";

        private static string GET_ALBUM_ERROR = "Error retrieving the album. Please try again.";
        private static string ALBUM_LIST_ERROR = "Error retrieving your photo album list. Please try again.";
        private static string CREATED_FAIL = "Error creating photo album. Please try again.";

        private static string LIST_VIEW = "List";
        private static string DETAILS_VIEW = "Details";

        private IHAVPhotoAlbumService thePhotoAlbumService;

        public PhotoAlbumController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
                thePhotoAlbumService = new HAVPhotoAlbumService(new ModelStateWrapper(this.ModelState));
        }

        public PhotoAlbumController(IHAVPhotoAlbumService aPhotoAlbumService, IHAVBaseService aBaseService)
            : base(aBaseService) {
                thePhotoAlbumService = aPhotoAlbumService;
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new string[] { })]
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            return ListPhotoAlbums(myUser, myUser.Id);
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "id" })]
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult List(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            return ListPhotoAlbums(myUser, id);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string name, string description) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                User myUser = GetUserInformatonModel().Details;
                bool myResult = thePhotoAlbumService.CreatePhotoAlbum(myUser, name, description);
                if (myResult) {
                    TempData["Message"] = CREATED_SUCCESS;
                }
            } catch (Exception myException) {
                TempData["Message"] = CREATED_FAIL;
                LogError(myException, CREATED_FAIL);
            }

            return RedirectToAction(LIST_VIEW);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Details(int albumId, int sourceUserId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUser = GetUserInformatonModel();
            LoggedInWrapperModel<PhotoAlbum> myPhotoAlbum = new LoggedInWrapperModel<PhotoAlbum>(myUser.Details, SiteSection.Photos);
            try {
                myPhotoAlbum.Model = thePhotoAlbumService.GetPhotoAlbum(myUser, sourceUserId, albumId);
            } catch (NotFriendException e) {
                return SendToErrorPage(HAVConstants.NOT_FRIEND);
            } catch (Exception myException) {
                TempData["Message"] = GET_ALBUM_ERROR;
                LogError(myException, GET_ALBUM_ERROR);
                return RedirectToAction(LIST_VIEW);
            }

            return View(DETAILS_VIEW, myPhotoAlbum);
        }

        private ActionResult ListPhotoAlbums(User aRequestingUser, int aUserIfOfAlbum) {
            LoggedInListModel<PhotoAlbum> myModel = new LoggedInListModel<PhotoAlbum>(aRequestingUser, SiteSection.Photos);
            try {
                myModel.Models = thePhotoAlbumService.GetPhotoAlbumsForUser(aRequestingUser, aUserIfOfAlbum);
            } catch (NotFriendException e) {
                return SendToErrorPage(HAVConstants.NOT_FRIEND);
            } catch (Exception e) {
                LogError(e, ALBUM_LIST_ERROR);
                return SendToErrorPage(ALBUM_LIST_ERROR);
            }

            return View(LIST_VIEW, myModel);
        }
    }
}
