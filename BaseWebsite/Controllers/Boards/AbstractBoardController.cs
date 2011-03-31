﻿using System;
using System.Web.Mvc;
using Social.Admin.Helpers;
using Social.Authentication;
using Social.Authentication.Services;
using Social.BaseWebsite.Models;
using Social.Board.Repositories;
using Social.Board.Services;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using Social.Validation;

namespace BaseWebsite.Controllers.Boards {
    public abstract class AbstractBoardController<T, U, V, W, X, Y, Z, A, B> : BaseController<T, U, V, W, X, Y, Z> {
        private static string POST_BOARD_SUCCESS = "Board message posted!";
        private static string EDIT_BOARD_SUCCES = "Board message edited.";
        private static string DELETE_BOARD_SUCCESS = "Board message deleted!";

        private static string VIEW_BOARD_ERROR = "Unable to retrieve the board.";
        private static string POST_BOARD_ERROR = "Unable to make the post to the board. Please try again.";
        private static string EDIT_BOARD_ERROR = "Unable to edit the board message. Please try again.";
        private static string DELETE_BOARD_ERROR = "Unable to delete the board message. Please try again.";

        private IBoardService<T, A, B> theService;

        public AbstractBoardController(IBaseService<T> aBaseService, IUserInformation<T, Z> aUserInformation, IAuthenticationService<T, U, V, W, X, Y> anAuthService,
                                       IWhoIsOnlineService<T, Z> aWhoIsOnlineService, IBoardRepository<T, A, B> aBoardRepo) : base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
            theService = new BoardService<T, A, B>(new ModelStateWrapper(this.ModelState), aBoardRepo);
        }

        protected abstract ILoggedInModel<A> CreateLoggedInWrapperModel(T aUser);
        protected abstract int GetPostedByUserId(A aMessage);

        protected ActionResult Details(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            T myUser = GetUserInformatonModel().Details;
            ILoggedInModel<A> myModel = CreateLoggedInWrapperModel(myUser); 
            
            try {
                myModel.Set(theService.GetBoard(GetUserInformatonModel(), id));
            } catch (Exception myException) {
                LogError(myException, VIEW_BOARD_ERROR);
                return SendToErrorPage(VIEW_BOARD_ERROR);
            }

            return View("Details", myModel);
        }

        protected ActionResult Create(int sourceUserId, string boardMessage) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<T> myPostingUser = GetUserInformatonModel();
            try {
                bool myBoardResult = theService.PostToBoard(myPostingUser, sourceUserId, boardMessage);
                if (myBoardResult) {
                    TempData["Message"] = SuccessMessage(POST_BOARD_SUCCESS);
                } else {
                    TempData["Message"] = ErrorMessage(POST_BOARD_ERROR);
                    TempData["BoardMessage"] = boardMessage;
                }
            } catch (Exception myException) {
                LogError(myException, POST_BOARD_ERROR);
                TempData["Message"] = ErrorMessage(POST_BOARD_ERROR);
                TempData["BoardMessage"] = boardMessage;
            }

            return RedirectToAction("Show", "Profile", new { id = sourceUserId });
        }

        protected ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                A myBoard = theService.GetBoard(GetUserInformatonModel(), id);
                if (UserId() != GetPostedByUserId(myBoard)) {
                    return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
                }
                return View("Edit", myBoard);
            } catch(Exception myException) {
                LogError(myException, VIEW_BOARD_ERROR);
                return SendToErrorPage(VIEW_BOARD_ERROR);
            }
        }

        protected ActionResult Edit(AbstractBoardModel<A> aBoard) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                bool myBoardResult = theService.EditBoardMessage(GetUserInformatonModel(), aBoard);
                if (myBoardResult) {
                    TempData["Message"] = SuccessMessage(EDIT_BOARD_SUCCES);
                }
            } catch (Exception myException) {
                LogError(myException, POST_BOARD_ERROR);
                TempData["Message"] = ErrorMessage(EDIT_BOARD_ERROR);
            }

            return RedirectToAction("Edit", new { id = aBoard.Id });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Delete(int profileUserId, int boardId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<T> myUserInfo = GetUserInformatonModel();
            if (!PermissionHelper<T>.AllowedToPerformAction(myUserInfo, SocialPermission.Delete_Board_Message, SocialPermission.Delete_Any_Board_Message)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }
            try {
                theService.DeleteBoardMessage(myUserInfo, boardId);
                TempData["Message"] = SuccessMessage(DELETE_BOARD_SUCCESS);
            } catch (Exception myException) {
                LogError(myException, DELETE_BOARD_ERROR);
                TempData["Message"] = ErrorMessage(DELETE_BOARD_ERROR);
            }

            return RedirectToAction("Show", "Profile", new { id = profileUserId });
        }
    }
}
