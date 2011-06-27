using System;
using System.Web.Mvc;
using BaseWebsite.Helpers;
using Social.Admin.Helpers;
using Social.Authentication;
using Social.Authentication.Services;
using Social.Board.Repositories;
using Social.Board.Services;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using Social.Validation;
using Social.Admin.Exceptions;

namespace BaseWebsite.Controllers.Boards {
    public abstract class AbstractBoardReplyController<T, U, V, W, X, Y, Z, A, B> : BaseController<T, U, V, W, X, Y, Z> {
        private static string POST_REPLY_SUCCESS = "Board reply posted!";
        private static string EDIT_REPLY_SUCCES = "Board reply edited.";
        private static string DELETE_REPLY_SUCCESS = "Board reply deleted!";

        private static string VIEW_REPLY_ERROR = "Unable to retrieve the reply.";
        private static string POST_REPLY_ERROR = "Unable to reply to the board message. Please try again.";
        private static string EDIT_REPLY_ERROR = "Unable to edit the board reply. Please try again.";
        private static string DELETE_REPLY_ERROR = "Unable to delete the board reply. Please try again.";

        private IBoardService<T, A, B> theService;

        public AbstractBoardReplyController(IBaseService<T> aBaseService, IUserInformation<T, Z> aUserInformation, IAuthenticationService<T, U, V, W, X, Y> anAuthService,
                                            IWhoIsOnlineService<T, Z> aWhoIsOnlineService, IBoardRepository<T, A, B> aBoardRepo) : base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
            theService = new BoardService<T, A, B>(new ModelStateWrapper(this.ModelState), aBoardRepo);
        }

        protected abstract int GetBoardReplyUserId(B aBoardReply);

        protected ActionResult Create(int boardId, string boardReply, BaseSiteSection source, int sourceId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                if (theService.PostReplyToBoard(GetUserInformatonModel(), boardId, boardReply)) {
                    TempData["Message"] = SuccessMessage(POST_REPLY_SUCCESS);
                }
            } catch(PermissionDenied) {
                TempData["Message"] = WarningMessage(ErrorKeys.PERMISSION_DENIED);
            } catch (Exception myException) {
                LogError(myException, POST_REPLY_ERROR);
                TempData["Message"] = ErrorMessage(POST_REPLY_ERROR);
            }

            if (source == BaseSiteSection.Profile) {
                return RedirectToProfile(sourceId);
            } else {
                return RedirectToAction("Details", "Board", new { id = sourceId });
            }
        }

        protected ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                B myReply = theService.FindBoardReply(id);
                if (UserId() != GetBoardReplyUserId(myReply)) {
                    return SendToErrorPage(ErrorKeys.PAGE_NOT_FOUND);
                }
                return View("Edit", myReply);
            } catch (Exception myException) {
                LogError(myException, VIEW_REPLY_ERROR);
                return SendToErrorPage(VIEW_REPLY_ERROR);
            }
        }

        protected ActionResult Edit(AbstractBoardReplyModel<B> aBoardReply) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                if (theService.EditBoardReply(GetUserInformatonModel(), aBoardReply)) {
                    TempData["Message"] = SuccessMessage(EDIT_REPLY_SUCCES);
                }
            } catch (Exception myException) {
                LogError(myException, POST_REPLY_ERROR);
                TempData["Message"] = ErrorMessage(EDIT_REPLY_ERROR);
            }

            return RedirectToAction("Edit", new { id = aBoardReply.Id });
        }

        protected ActionResult Delete(int sourceId, int boardReplyId, string controller, string action) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<T> myUserInfo = GetUserInformatonModel();
                if (!PermissionHelper<T>.AllowedToPerformAction(myUserInfo, SocialPermission.Delete_Board_Reply, SocialPermission.Delete_Any_Board_Reply)) {
                    TempData["Message"] = WarningMessage(ErrorKeys.PERMISSION_DENIED);
                } else {
                    theService.DeleteBoardReply(GetUserInformatonModel(), boardReplyId);
                    TempData["Message"] = SuccessMessage(DELETE_REPLY_SUCCESS);
                }
            } catch(PermissionDenied) {
                TempData["Message"] = WarningMessage(ErrorKeys.PERMISSION_DENIED);
            } catch (Exception myException) {
                LogError(myException, DELETE_REPLY_ERROR);
                TempData["Message"] = ErrorMessage(DELETE_REPLY_ERROR);
            }

            return RedirectToAction(action, controller, new { id = sourceId });
        }
    }
}
