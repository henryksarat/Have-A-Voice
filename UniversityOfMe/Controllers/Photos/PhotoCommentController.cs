using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Services.Photos;
using UniversityOfMe.UserInformation;
using UniversityOfMe.Repositories;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Controllers.Photos {
    public class PhotoCommentController : UOFMeBaseController {
        private const string COMMENT_POSTED = "Photo comment posted!";
        private const string COMMENT_ERROR = "An error has occurred while posting your comment. Please try again.";

        private IValidationDictionary theValidation;
        private IPhotoCommentService thePhotoCommentService;

        public PhotoCommentController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidation = new ModelStateWrapper(this.ModelState);
            thePhotoCommentService = new PhotoCommentService(theValidation);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(int id, string comment) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();

            try {
                bool myResult = thePhotoCommentService.AddCommentToPhoto(myUser, id, comment);
                if (myResult) {
                    TempData["Message"] = COMMENT_POSTED;
                }
            } catch (Exception myException) {
                LogError(myException, COMMENT_ERROR);
                TempData["Message"] = COMMENT_ERROR;
            }

            return RedirectToAction("Display", "Photo", new { id = id });
        }
    }
}
