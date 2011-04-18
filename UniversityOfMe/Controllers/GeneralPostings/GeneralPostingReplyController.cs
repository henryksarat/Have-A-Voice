using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.GeneralPostings;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Classes {
    public class GeneralPostingReplyController : UOFMeBaseController {
        
        private const string REPLY_POSTED = "The reply to the posting has been posted.";

        IGeneralPostingService theGeneralPostingService;
        IValidationDictionary theValidationDictionary;

        public GeneralPostingReplyController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
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
                    TempData["Message"] = REPLY_POSTED;
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = ErrorKeys.ERROR_MESSAGE;
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Details", "GeneralPosting", new { id = generalPostingId });
        }
        
        protected override AbstractUserModel<User> GetSocialUserInformation() {
            return SocialUserModel.Create(GetUserInformaton());
        }

        protected override AbstractUserModel<User> CreateSocialUserModel(User aUser) {
            return SocialUserModel.Create(aUser);
        }

        protected override IProfilePictureStrategy<User> ProfilePictureStrategy() {
            return new ProfilePictureStrategy();
        }

        protected override string UserEmail() {
            return GetUserInformaton().Email;
        }

        protected override string UserPassword() {
            return GetUserInformaton().Password;
        }

        protected override int UserId() {
            return GetUserInformaton().Id;
        }

        protected override string ErrorMessage(string aMessage) {
            return aMessage;
        }

        protected override string NormalMessage(string aMessage) {
            return aMessage;
        }

        protected override string SuccessMessage(string aMessage) {
            return aMessage;
        }
    }
}
