using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.GeneralPostings;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Classes {
    public class GeneralPostingReplyController : UOFMeBaseController {
        
        private const string REPLY_POSTED = "The reply to the posting has been posted.";

        IGeneralPostingService theGeneralPostingService;
        IValidationDictionary theValidationDictionary;

        public GeneralPostingReplyController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theGeneralPostingService = new GeneralPostingService(theValidationDictionary);
        }
    
        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string universityId, int generalPostingId, string reply) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                bool myResult = theGeneralPostingService.CreateGeneralPostingReply(GetUserInformatonModel(), generalPostingId, reply);

                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(REPLY_POSTED);
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Details", "GeneralPosting", new { id = generalPostingId });
        }
    }
}
