using System;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.Clubs;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Clubs {
    public class ClubMemberController : UOFMeBaseController {
        public const string USER_REMVOED = "User removed successfully!";
        public const string REMOVE_ERROR = "Error while removing the member from the club. Please try again.";

        IClubService theClubService;
        IUniversityService theUniversityService;

        public ClubMemberController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            IValidationDictionary myModelState = new ModelStateWrapper(this.ModelState);
            theClubService = new ClubService(myModelState);
            theUniversityService = new UniversityService();
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Remove(int userId, int clubId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                bool myResult = theClubService.RemoveClubMember(GetUserInformatonModel(), userId, clubId);

                if (myResult) {
                    TempData["Message"] = USER_REMVOED;
                }
            } catch (Exception myException) {
                LogError(myException, REMOVE_ERROR);
                TempData["Message"] = REMOVE_ERROR;
            }

            return RedirectToAction("Details", "Club", new { id = clubId });
        }
    }
}
