using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Classes;
using UniversityOfMe.UserInformation;
using UniversityOfMe.Helpers;
using Social.Admin.Exceptions;

namespace UniversityOfMe.Controllers.Classes {
    public class ClassBoardReplyController : UOFMeBaseController {
        private const string REPLY_POSTED = "The reply to class message was posted.";
        private const string REPLY_DELETED = "The reply to class message was deleted.";

        IClassService theClassService;
        IValidationDictionary theValidationDictionary;

        public ClassBoardReplyController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theClassService = new ClassService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(int classId, int classBoardId, string boardMessage) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                bool myResult = theClassService.AddReplyToClassBoard(GetUserInformatonModel(), classBoardId, boardMessage);

                if (myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(REPLY_POSTED);
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Details", "ClassBoard", new { classId = classId, classBoardId = classBoardId });
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Delete(int classId, int classBoardId, int classBoardReplyId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theClassService.DeleteClassBoardReply(GetUserInformatonModel(), classBoardReplyId);
                TempData["Message"] = MessageHelper.SuccessMessage(REPLY_DELETED);
            } catch(PermissionDenied) {
                TempData["Message"] = MessageHelper.WarningMessage(ErrorKeys.PERMISSION_DENIED);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }
            
            return RedirectToAction("Details", "ClassBoard", new { classId = classId, classBoardId = classBoardId });
        }
    }
}
