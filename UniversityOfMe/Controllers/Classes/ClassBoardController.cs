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
using UniversityOfMe.Models.View;
using Social.Generic.Models;

namespace UniversityOfMe.Controllers.Classes {
    public class ClassBoardController : UOFMeBaseController {
        private const string REPLY_POSTED = "The reply to the class has been posted.";
        private const string BOARD_DELETED = "The class message was deleted.";

        IClassService theClassService;
        IValidationDictionary theValidationDictionary;

        public ClassBoardController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theClassService = new ClassService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string universityId, int classId, string boardMessage) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                bool myResult = theClassService.AddToClassBoard(GetUserInformatonModel(), classId, boardMessage);

                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(REPLY_POSTED);
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("DetailsWithClassId", "Class", new { classId = classId, classViewType = ClassViewType.Discussion });
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Details(int classId, int classBoardId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                ClassBoard myBoard = theClassService.GetClassBoard(myUserInformation, classBoardId);
                LoggedInWrapperModel<ClassBoard> myLoggedIn = new LoggedInWrapperModel<ClassBoard>(myUserInformation.Details);
                myLoggedIn.Set(myBoard);
                return View("Details", myLoggedIn);
            } catch(PermissionDenied myException) {
                TempData["Message"] += MessageHelper.WarningMessage(myException.Message);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToAction("DetailsWithClassId", "Class", new { classId = classId, classViewType = ClassViewType.Discussion });
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Delete(int classId, int classBoardId, SiteSection source) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theClassService.DeleteClassBoard(GetUserInformatonModel(), classBoardId);
                TempData["Message"] += MessageHelper.SuccessMessage(BOARD_DELETED);
                return RedirectToAction("DetailsWithClassId", "Class", new { classId = classId, classViewType = ClassViewType.Discussion });
            } catch(PermissionDenied) {
                TempData["Message"] += MessageHelper.WarningMessage(ErrorKeys.PERMISSION_DENIED);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            string myController = "Class";
            string myAction = "DetailsWithClassId";
            object myRoute =  new { classId = classId, classViewType = ClassViewType.Discussion };

            if(source == SiteSection.ClassBoard) {
                myController = "ClassBoard";
                myAction = "Details";
                myRoute = new { classId = classId, classBoardId = classBoardId };
            }
            
            return RedirectToAction(myAction, myController, myRoute);
        }
    }
}
