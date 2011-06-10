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

namespace UniversityOfMe.Controllers.Classes {
    public class ClassBoardController : UOFMeBaseController {
        private const string REPLY_POSTED = "The reply to the class has been posted.";

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
                    TempData["Message"] = MessageHelper.SuccessMessage(REPLY_POSTED);
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("DetailsWithClassId", "Class", new { classId = classId, classViewType = ClassViewType.Discussion });
        }
    }
}
