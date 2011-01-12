using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Controllers.ActionFilters;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Validation;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;

namespace HaveAVoice.Controllers.Users {
    public class BoardReplyController : HAVBaseController {
        private static string POST_REPLY_SUCCESS = "Board reply posted!";
        private static string EDIT_REPLY_SUCCES = "Board reply edited.";
        private static string DELETE_REPLY_SUCCESS = "Board reply deleted!";

        private static string VIEW_REPLY_ERROR = "Unable to retrieve the reply.";
        private static string POST_REPLY_ERROR = "Unable to reply to the board message. Please try again.";
        private static string EDIT_REPLY_ERROR = "Unable to edit the board reply. Please try again.";
        private static string DELETE_REPLY_ERROR = "Unable to delete the board reply. Please try again.";

        private IHAVBoardService theService;

        public BoardReplyController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
                theService = new HAVBoardService(new ModelStateWrapper(this.ModelState));
        }

        public BoardReplyController(IHAVBoardService aService, IHAVBaseService aBaseService)
            : base(aBaseService) {
                theService = aService;
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(int sourceUserId, int boardId, string message) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                if (theService.PostReplyToBoard(GetUserInformatonModel(), boardId, message)) {
                    TempData["Message"] = POST_REPLY_SUCCESS;
                }
            } catch (Exception myException) {
                LogError(myException, POST_REPLY_ERROR);
                TempData["Message"] = POST_REPLY_ERROR;
            }

            return RedirectToProfile(sourceUserId);
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

                BoardReplyWrapper myWrapper = BoardReplyWrapper.Build(myReply);
                return View("Edit", myWrapper);
            } catch (Exception myException) {
                LogError(myException, VIEW_REPLY_ERROR);
                return SendToErrorPage(VIEW_REPLY_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(BoardReplyWrapper aBoardReplyWrapper) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                if (theService.EditBoardReply(GetUserInformatonModel(), aBoardReplyWrapper.ToModel())) {
                    TempData["Message"] = EDIT_REPLY_SUCCES;
                }
            } catch (Exception myException) {
                LogError(myException, POST_REPLY_ERROR);
                TempData["Message"] = EDIT_REPLY_ERROR;
            }

            return RedirectToAction("Edit", new { id = aBoardReplyWrapper.Id });
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Delete(int boardId, int boardReplyId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUserInfo = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInfo, HAVPermission.Delete_Board_Reply, HAVPermission.Delete_Any_Board_Reply)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }
            try {
                theService.DeleteBoardReply(GetUserInformatonModel(), boardReplyId);
                TempData["Message"] = DELETE_REPLY_SUCCESS;
            } catch (Exception myException) {
                LogError(myException, DELETE_REPLY_ERROR);
                TempData["Message"] = DELETE_REPLY_ERROR;
            }

            return RedirectToAction("View", "Board", new { id = boardId });
        }
    }
}
