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
using HaveAVoice.Controllers.Helpers;

namespace HaveAVoice.Controllers.Users.Photos {
    public class PhotoAlbumController : HAVBaseController {
        private static string CREATED_SUCCESS = "Photo album created!";
        private const string EDIT_SUCCESS = "Photo album edited successfully!";
        private const string DELETE_SUCCESS = "Photo album deleted successfully!";

        private const string USER_RETRIEVAL_ERROR = "Error retrieving the user.";
        private const string GET_ALBUM_ERROR = "Error retrieving the album. Please try again.";
        private const string ALBUM_LIST_ERROR = "Error retrieving your photo album list. Please try again.";
        private const string CREATED_FAIL = "Error creating photo album. Please try again.";
        private const string EDIT_ERROR = "Error editing the photo album. Please try again.";
        private const string DELETE_ERROR = "Error editing the photo album. Please try again.";

        private const string LIST_VIEW = "List";
        private const string DETAILS_VIEW = "Details";
        private const string EDIT_VIEW = "Edit";
        private const string DELETE_VIEW = "Delete";

        private IHAVPhotoAlbumService thePhotoAlbumService;
        private IHAVUserRetrievalService theUserRetrievalService;

        public PhotoAlbumController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
                thePhotoAlbumService = new HAVPhotoAlbumService(new ModelStateWrapper(this.ModelState));
                theUserRetrievalService = new HAVUserRetrievalService();
        }

        public PhotoAlbumController(IHAVPhotoAlbumService aPhotoAlbumService, IHAVUserRetrievalService aUserRetrievalService, IHAVBaseService aBaseService)
            : base(aBaseService) {
                thePhotoAlbumService = aPhotoAlbumService;
                theUserRetrievalService = aUserRetrievalService;
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
                    TempData["Message"] = MessageHelper.SuccessMessage(CREATED_SUCCESS);
                }
            } catch (Exception myException) {
                TempData["Message"] = MessageHelper.ErrorMessage(CREATED_FAIL);
                LogError(myException, CREATED_FAIL);
            }

            return RedirectToAction(LIST_VIEW);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUser = GetUserInformatonModel();
            try {
                thePhotoAlbumService.DeletePhotoAlbum(myUser, id);
                TempData["Message"] = MessageHelper.SuccessMessage(DELETE_SUCCESS);
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                TempData["Message"] = MessageHelper.ErrorMessage(DELETE_ERROR);
                LogError(e, DELETE_ERROR);
            }

            return RedirectToAction(LIST_VIEW);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Details(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUser = GetUserInformatonModel();
            LoggedInWrapperModel<PhotoAlbum> myPhotoAlbum = new LoggedInWrapperModel<PhotoAlbum>(myUser.Details, SiteSection.Photos);
            try {
                myPhotoAlbum.Model = thePhotoAlbumService.GetPhotoAlbum(myUser, id);
            } catch (NotFriendException e) {
                return SendToErrorPage(HAVConstants.NOT_FRIEND);
            } catch (Exception myException) {
                TempData["Message"] = MessageHelper.ErrorMessage(GET_ALBUM_ERROR);
                LogError(myException, GET_ALBUM_ERROR);
                return RedirectToAction(LIST_VIEW);
            }

            return View(DETAILS_VIEW, myPhotoAlbum);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData, ImportModelStateFromTempData]
        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUser = GetUserInformatonModel();
            LoggedInWrapperModel<PhotoAlbum> myPhotoAlbum = new LoggedInWrapperModel<PhotoAlbum>(myUser.Details, SiteSection.Photos);
            try {
                myPhotoAlbum.Model = thePhotoAlbumService.GetPhotoAlbumForEdit(myUser, id);
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                TempData["Message"] = MessageHelper.ErrorMessage(GET_ALBUM_ERROR);
                LogError(e, GET_ALBUM_ERROR);
                return RedirectToAction(LIST_VIEW);
            }

            return View(EDIT_VIEW, myPhotoAlbum);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(int albumId, string name, string description) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUser = GetUserInformatonModel();
            try {
                bool myResult = thePhotoAlbumService.EditPhotoAlbum(myUser, albumId, name, description);
                if (myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(EDIT_SUCCESS);
                    return RedirectToAction(LIST_VIEW);
                }
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                TempData["Message"] = MessageHelper.ErrorMessage(EDIT_ERROR);
                LogError(e, EDIT_ERROR);
            }

            return RedirectToAction(EDIT_VIEW);
        }

        private ActionResult ListPhotoAlbums(User aRequestingUser, int aUserIdOfAlbum) {
            User myUserOfAlbum;
            try {
                myUserOfAlbum = theUserRetrievalService.GetUser(aUserIdOfAlbum);
            } catch (Exception e) {
                LogError(e, USER_RETRIEVAL_ERROR);
                return SendToErrorPage(ALBUM_LIST_ERROR);
            }

            LoggedInListModel<PhotoAlbum> myModel = new LoggedInListModel<PhotoAlbum>(myUserOfAlbum, SiteSection.Photos, aUserIdOfAlbum);

            try {
                myModel.Models = thePhotoAlbumService.GetPhotoAlbumsForUser(aRequestingUser, aUserIdOfAlbum);
            } catch (NotFriendException e) {
                return SendToErrorPage(HAVConstants.NOT_FRIEND);
            } catch (Exception e) {
                LogError(e, ALBUM_LIST_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(ALBUM_LIST_ERROR);
            }

            return View(LIST_VIEW, myModel);
        }
    }
}
