﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Models.View;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Services;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Controllers.ActionFilters;

namespace HaveAVoice.Controllers.Users
{
    public class BoardController : HAVBaseController {
        private static string POST_BOARD_SUCCESS = "Board message posted!";
        private static string EDIT_BOARD_SUCCES = "Board message edited.";
        private static string DELETE_BOARD_SUCCESS = "Board message deleted!";

        private static string VIEW_BOARD_ERROR = "Unable to retrieve the board.";
        private static string POST_BOARD_ERROR = "Unable to make the post to the board. Please try again.";
        private static string EDIT_BOARD_ERROR = "Unable to edit the board message. Please try again.";
        private static string DELETE_BOARD_ERROR = "Unable to delete the board message. Please try again.";

        private IHAVBoardService theService;

        public BoardController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
                theService = new HAVBoardService(new ModelStateWrapper(this.ModelState));
        }

        public BoardController(IHAVBoardService aService, IHAVBaseService aBaseService)
            : base(aBaseService) {
                theService = aService;
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult View(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            BoardModel myBoardModel;

            try {
                myBoardModel = theService.GetBoard(GetUserInformatonModel(), id);
            } catch (Exception myException) {
                LogError(myException, VIEW_BOARD_ERROR);
                return SendToErrorPage(VIEW_BOARD_ERROR);
            }

            return View("View", myBoardModel);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(int sourceUserId, string message) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myPostingUser = GetUserInformatonModel();
            try {
                bool myBoardResult = theService.PostToBoard(myPostingUser, sourceUserId, message);
                if (myBoardResult) {
                    TempData["Message"] = POST_BOARD_SUCCESS;
                }
            } catch (Exception myException) {
                LogError(myException, POST_BOARD_ERROR);
                TempData["Message"] = POST_BOARD_ERROR;
            }

            return RedirectToAction("Show", "Profile", new { id = sourceUserId });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            BoardWrapper myBoardWrapper;
            try {
                Board myBoard = theService.FindBoard(id);

                if (GetUserInformatonModel().Details.Id != myBoard.PostedUserId) {
                    return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
                }

                myBoardWrapper = BoardWrapper.Build(myBoard);
            } catch(Exception myException) {
                LogError(myException, VIEW_BOARD_ERROR);
                return SendToErrorPage(VIEW_BOARD_ERROR);
            }

            return View("Edit", myBoardWrapper);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(BoardWrapper aBoard) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                bool myBoardResult =  theService.EditBoardMessage(GetUserInformatonModel(), aBoard.ToModel());
                if (myBoardResult) {
                    TempData["Message"] = EDIT_BOARD_SUCCES;
                }
            } catch (Exception myException) {
                LogError(myException, POST_BOARD_ERROR);
                TempData["Message"] = EDIT_BOARD_ERROR;
            }

            return RedirectToAction("Edit", new { id = aBoard.Id });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Delete(int profileUserId, int boardId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUserInfo = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInfo, HAVPermission.Delete_Board_Message, HAVPermission.Delete_Any_Board_Message)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }
            try {
                theService.DeleteBoardMessage(myUserInfo, boardId);
                TempData["Message"] = DELETE_BOARD_SUCCESS;
            } catch (Exception myException) {
                LogError(myException, DELETE_BOARD_ERROR);
                TempData["Message"] = DELETE_BOARD_ERROR;
            }

            return RedirectToAction("Show", "Profile", new { id = profileUserId });
        }
    }
}
