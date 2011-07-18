using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using HaveAVoice.Controllers;
using HaveAVoice.Services.Groups;
using HaveAVoice.Models;
using HaveAVoice.Controllers.Helpers;

namespace HaveAVoice.Controllers.Groups {
    public class GroupBoardController : HAVBaseController {
        private const string POSTED_TO_BOARD = "Posted to the clubs board!";

        private const string POSTING_TO_BOARD_ERROR = "An error occurred while posting to the club board. Please try again.";

        IGroupService theGroupService;

        public GroupBoardController() {
            IValidationDictionary myModelState = new ModelStateWrapper(this.ModelState);
            theGroupService = new GroupService(myModelState);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string boardMessage, int groupId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            try {
                bool myResult = theGroupService.PostToGroupBoard(myUserInformation, groupId, boardMessage);
                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(POSTED_TO_BOARD);
                }
            } catch (Exception myException) {
                LogError(myException, POSTING_TO_BOARD_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(POSTING_TO_BOARD_ERROR);
            }

            return RedirectToAction("Details", "Group", new { id = groupId });
        }
    }
}
