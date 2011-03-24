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
    public class BoardReplyController : HAVBaseController {
        private static string POST_REPLY_SUCCESS = "Board reply posted!";
        private static string EDIT_REPLY_SUCCES = "Board reply edited.";
        private static string DELETE_REPLY_SUCCESS = "Board reply deleted!";

        private static string VIEW_REPLY_ERROR = "Unable to retrieve the reply.";
        private static string POST_REPLY_ERROR = "Unable to reply to the board message. Please try again.";
        private static string EDIT_REPLY_ERROR = "Unable to edit the board reply. Please try again.";
        private static string DELETE_REPLY_ERROR = "Unable to delete the board reply. Please try again.";

        private IBoardService<User, Board, BoardReply> theService;

        public BoardReplyController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
                theService = new BoardService<User, Board, BoardReply>(new ModelStateWrapper(this.ModelState), new EntityHAVBoardRepository());
        }

        public BoardReplyController(IBoardService<User, Board, BoardReply> aService, IHAVBaseService aBaseService) : base(aBaseService) {
                theService = aService;
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(int boardId, string boardReply, SiteSection source, int sourceId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                if (theService.PostReplyToBoard(GetUserInformatonModel(), boardId, boardReply)) {
                    TempData["Message"] = MessageHelper.SuccessMessage(POST_REPLY_SUCCESS);
                }
            } catch (Exception myException) {
                LogError(myException, POST_REPLY_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(POST_REPLY_ERROR);
            }

            if (source == SiteSection.Profile) {
                return RedirectToProfile(sourceId);
            } else {
                return RedirectToAction("Details", "Board", new { id = sourceId });
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                BoardReply myReply = theService.FindBoardReply(id);
                if (GetUserInformatonModel().Details.Id != myReply.UserId) {
                    return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
                }
                return View("Edit", myReply);
            } catch (Exception myException) {
                LogError(myException, VIEW_REPLY_ERROR);
                return SendToErrorPage(VIEW_REPLY_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(SocialBoardReplyModel aBoardReply) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                if (theService.EditBoardReply(GetUserInformatonModel(), aBoardReply)) {
                    TempData["Message"] = MessageHelper.SuccessMessage(EDIT_REPLY_SUCCES);
                }
            } catch (Exception myException) {
                LogError(myException, POST_REPLY_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(EDIT_REPLY_ERROR);
            }

            return RedirectToAction("Edit", new { id = aBoardReply.Id });
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Delete(int boardId, int boardReplyId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInfo = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInfo, SocialPermission.Delete_Board_Reply, SocialPermission.Delete_Any_Board_Reply)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }
            try {
                theService.DeleteBoardReply(GetUserInformatonModel(), boardReplyId);
                TempData["Message"] = MessageHelper.SuccessMessage(DELETE_REPLY_SUCCESS);
            } catch (Exception myException) {
                LogError(myException, DELETE_REPLY_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(DELETE_REPLY_ERROR);
            }

            return RedirectToAction("View", "Board", new { id = boardId });
        }
    }
}
