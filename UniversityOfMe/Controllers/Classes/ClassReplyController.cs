using System;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.Classes;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Classes;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Classes {
    public class ClassReplyController : UOFMeBaseController {
        private const string REPLY_POSTED = "The reply to the class has been posted.";

        IClassService theClassService;
        IValidationDictionary theValidationDictionary;

        public ClassReplyController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theClassService = new ClassService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string universityId, int classId, string reply) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                bool myResult = theClassService.CreateClassReply(GetUserInformatonModel(), classId, reply);

                if (myResult) {
                    TempData["Message"] = REPLY_POSTED;
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = ErrorKeys.ERROR_MESSAGE;
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("DetailsWithClassId", "Class", new { classId = classId });
        }
    }
}
