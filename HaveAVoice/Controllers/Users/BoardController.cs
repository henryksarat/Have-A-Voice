using System;
using System.Web.Mvc;
using HaveAVoice.Controllers.ActionFilters;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services;
using Social.Admin.Helpers;
using Social.Board.Services;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Controllers.Users {
    public class BoardController : HAVBaseController {
        private static string POST_BOARD_SUCCESS = "Board message posted!";
        private static string EDIT_BOARD_SUCCES = "Board message edited.";
        private static string DELETE_BOARD_SUCCESS = "Board message deleted!";

        private static string VIEW_BOARD_ERROR = "Unable to retrieve the board.";
        private static string POST_BOARD_ERROR = "Unable to make the post to the board. Please try again.";
        private static string EDIT_BOARD_ERROR = "Unable to edit the board message. Please try again.";
        private static string DELETE_BOARD_ERROR = "Unable to delete the board message. Please try again.";

        private IBoardService<User, Board, BoardReply> theService;

        public BoardController() : base(new HAVBaseService(new HAVBaseRepository())) {
                theService = new BoardService<User, Board, BoardReply>(new ModelStateWrapper(this.ModelState), new EntityHAVBoardRepository());
        }

        public BoardController(IBoardService<User, Board, BoardReply> aService, IHAVBaseService aBaseService) : base(aBaseService) {
                theService = aService;
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformatonModel().Details;
            LoggedInWrapperModel<Board> myModel = new LoggedInWrapperModel<Board>(myUser, SiteSection.Board); 
            
            try {
                myModel.Model = theService.GetBoard(GetUserInformatonModel(), id);
            } catch (Exception myException) {
                LogError(myException, VIEW_BOARD_ERROR);
                return SendToErrorPage(VIEW_BOARD_ERROR);
            }

            return View("Details", myModel);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(int sourceUserId, string boardMessage) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myPostingUser = GetUserInformatonModel();
            try {
                bool myBoardResult = theService.PostToBoard(myPostingUser, sourceUserId, boardMessage);
                if (myBoardResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(POST_BOARD_SUCCESS);
                } else {
                    TempData["Message"] = MessageHelper.ErrorMessage(POST_BOARD_ERROR);
                    TempData["BoardMessage"] = boardMessage;
                }
            } catch (Exception myException) {
                LogError(myException, POST_BOARD_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(POST_BOARD_ERROR);
                TempData["BoardMessage"] = boardMessage;
            }

            return RedirectToAction("Show", "Profile", new { id = sourceUserId });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                Board myBoard = theService.GetBoard(GetUserInformatonModel(), id);
                if (GetUserInformatonModel().Details.Id != myBoard.PostedUserId) {
                    return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
                }
                return View("Edit", myBoard);
            } catch(Exception myException) {
                LogError(myException, VIEW_BOARD_ERROR);
                return SendToErrorPage(VIEW_BOARD_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(SocialBoardModel aBoard) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                bool myBoardResult = theService.EditBoardMessage(GetUserInformatonModel(), aBoard);
                if (myBoardResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(EDIT_BOARD_SUCCES);
                }
            } catch (Exception myException) {
                LogError(myException, POST_BOARD_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(EDIT_BOARD_ERROR);
            }

            return RedirectToAction("Edit", new { id = aBoard.Id });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Delete(int profileUserId, int boardId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInfo = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInfo, SocialPermission.Delete_Board_Message, SocialPermission.Delete_Any_Board_Message)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }
            try {
                theService.DeleteBoardMessage(myUserInfo, boardId);
                TempData["Message"] = MessageHelper.SuccessMessage(DELETE_BOARD_SUCCESS);
            } catch (Exception myException) {
                LogError(myException, DELETE_BOARD_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(DELETE_BOARD_ERROR);
            }

            return RedirectToAction("Show", "Profile", new { id = profileUserId });
        }
    }
}
