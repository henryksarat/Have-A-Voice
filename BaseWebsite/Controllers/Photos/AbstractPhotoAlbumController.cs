using System;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Services;
using Social.BaseWebsite.Models;
using Social.Friend.Exceptions;
using Social.Friend.Repositories;
using Social.Friend.Services;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Exceptions;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Photo.Repositories;
using Social.Photo.Services;
using Social.User.Repositories;
using Social.User.Services;
using Social.Users.Services;
using Social.Validation;

namespace BaseWebsite.Controllers.Photos {
    public abstract class AbstractPhotoAlbumController<T, U, V, W, X, Y, Z, A, B, C>  : BaseController<T, U, V, W, X, Y, Z> {
        private static string CREATED_SUCCESS = "Photo album created!";
        private const string EDIT_SUCCESS = "Photo album edited successfully!";
        private const string DELETE_SUCCESS = "Photo album deleted successfully!";
        private const string NO_ALBUMS = "There are currently no albums created.";

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

        private IPhotoAlbumService<T, A, B, C> thePhotoAlbumService;
        private IUserRetrievalService<T> theUserRetrievalService;

        public AbstractPhotoAlbumController(IBaseService<T> aBaseService, IUserInformation<T, Z> aUserInformation, IAuthenticationService<T, U, V, W, X, Y> anAuthService,
                                            IWhoIsOnlineService<T, Z> aWhoIsOnlineService, IPhotoAlbumRepository<T, A, B> aPhotoAlbumRepo, IPhotoRepository<T, B> aPhotoRepo,
                                            IFriendRepository<T, C> aFriendRepo, IUserRetrievalRepository<T> aUserRetrievalRepo)
            : base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
                IValidationDictionary myValuationDictionary = new ModelStateWrapper(this.ModelState);
                thePhotoAlbumService = new PhotoAlbumService<T, A, B, C>(myValuationDictionary,
                    new PhotoService<T, A, B, C>(new FriendService<T, C>(aFriendRepo), aPhotoAlbumRepo, aPhotoRepo),
                    new FriendService<T, C>(aFriendRepo),
                    aPhotoAlbumRepo);
                theUserRetrievalService = new UserRetrievalService<T>(aUserRetrievalRepo);
        }

        protected abstract ILoggedInModel<A> CreateLoggedInWrapperModel(T aUser);
        protected abstract ILoggedInListModel<A> CreateLoggedInListModel(T myUserOfAlbum, T aRequestingUser);

        protected ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            T myUser = GetUserInformaton();
            return ListPhotoAlbums(myUser, UserId());
        }

        protected ActionResult List(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            T myUser = GetUserInformaton();
            return ListPhotoAlbums(myUser, id);
        }

        protected ActionResult Create(string name, string description) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                T myUser = GetUserInformatonModel().Details;
                bool myResult = thePhotoAlbumService.CreatePhotoAlbum(myUser, name, description);
                if (myResult) {
                    TempData["Message"] = SuccessMessage(CREATED_SUCCESS);
                }
            } catch (Exception myException) {
                TempData["Message"] = ErrorMessage(CREATED_FAIL);
                LogError(myException, CREATED_FAIL);
            }

            return RedirectToAction(LIST_VIEW);
        }

        protected ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<T> myUser = GetUserInformatonModel();
            try {
                thePhotoAlbumService.DeletePhotoAlbum(CreateSocialUserModel(myUser.Details), id);
                TempData["Message"] = SuccessMessage(DELETE_SUCCESS);
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                TempData["Message"] = ErrorMessage(DELETE_ERROR);
                LogError(e, DELETE_ERROR);
            }

            return RedirectToAction(LIST_VIEW);
        }

        protected ActionResult Details(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<T> myUser = GetUserInformatonModel();
            ILoggedInModel<A> myPhotoAlbum = CreateLoggedInWrapperModel(myUser.Details);
            try {
                myPhotoAlbum.Set(thePhotoAlbumService.GetPhotoAlbum(CreateSocialUserModel(myUser.Details), id));
            } catch (NotFriendException e) {
                return SendToErrorPage(ErrorKeys.NOT_FRIEND);
            } catch (Exception myException) {
                TempData["Message"] = ErrorMessage(GET_ALBUM_ERROR);
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
            UserInformationModel<T> myUser = GetUserInformatonModel();
            ILoggedInModel<A> myPhotoAlbum = CreateLoggedInWrapperModel(myUser.Details);
            try {
                myPhotoAlbum.Set(thePhotoAlbumService.GetPhotoAlbumForEdit(CreateSocialUserModel(myUser.Details), id));
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                TempData["Message"] = ErrorMessage(GET_ALBUM_ERROR);
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
            UserInformationModel<T> myUser = GetUserInformatonModel();
            try {
                bool myResult = thePhotoAlbumService.EditPhotoAlbum(CreateSocialUserModel(myUser.Details), albumId, name, description);
                if (myResult) {
                    TempData["Message"] = SuccessMessage(EDIT_SUCCESS);
                    return RedirectToAction(LIST_VIEW);
                }
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                TempData["Message"] = ErrorMessage(EDIT_ERROR);
                LogError(e, EDIT_ERROR);
            }

            return RedirectToAction(EDIT_VIEW);
        }

        private ActionResult ListPhotoAlbums(T aRequestingUser, int aUserIdOfAlbum) {
            T myUserOfAlbum;
            try {
                myUserOfAlbum = theUserRetrievalService.GetUser(aUserIdOfAlbum);
            } catch (Exception e) {
                LogError(e, USER_RETRIEVAL_ERROR);
                return SendToErrorPage(ALBUM_LIST_ERROR);
            }

            ILoggedInListModel<A> myModel = CreateLoggedInListModel(myUserOfAlbum, aRequestingUser);

            try {
                myModel.Set(thePhotoAlbumService.GetPhotoAlbumsForUser(CreateSocialUserModel(aRequestingUser), aUserIdOfAlbum));
                if (myModel.Get().Count<A>() == 0) {
                    TempData["Message"] = NO_ALBUMS;
                }
            } catch (NotFriendException e) {
                return SendToErrorPage(ErrorKeys.NOT_FRIEND);
            } catch (Exception e) {
                LogError(e, ALBUM_LIST_ERROR);
                ViewData["Message"] = ErrorMessage(ALBUM_LIST_ERROR);
            }

            return View(LIST_VIEW, myModel);
        }
    }
}
