using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Validation;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Models.View;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Controllers.Helpers;

namespace HaveAVoice.Controllers.Core {
    public class AuthorityVerificationController : HAVBaseController {
        private const string TOKEN_CREATED_AND_SENT_SUCCESS = "A token has been created and sent to the authority.";
        private const string INVALID_TOKEN = "The token is either invalid or wasn't assigned to that email. If this is erroneous please contact us by clicking the \"Contact Us\" link on the bottom of this page and filling out the form.";

        private const string TOKEN_CREATED_AND_SENT_ERROR = "An error has occurred while created and sending the token. Please try again.";
        private const string TOKEN_VERIFICATION_ERROR = "An error has occurred while verifying the token for the email. Please try again.";

        private const string CREATE_VIEW = "Create";
        private const string VERIFY_VIEW = "Verify";

        private IHAVAuthorityVerificationService theAuthService;
        private IValidationDictionary theValidationDictionary;

        
        public AuthorityVerificationController() :
            base(new HAVBaseService(new HAVBaseRepository())) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theAuthService = new HAVAuthorityVerificationService(theValidationDictionary);
        }

        public AuthorityVerificationController(IHAVBaseService baseService, IHAVAuthorityVerificationService anAuthService)
            : base(baseService) {
            theAuthService = anAuthService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUserInformation = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Create_Authority_Verification_Token)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }
            return View(CREATE_VIEW);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string email, string authorityType) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUserInformation = GetUserInformatonModel();
            try {
                if (theAuthService.RequestTokenForAuthority(myUserInformation, email, authorityType)) {
                    return SendToResultPage(TOKEN_CREATED_AND_SENT_SUCCESS);
                } 
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                LogError(e, TOKEN_CREATED_AND_SENT_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(TOKEN_CREATED_AND_SENT_ERROR);
            }

            return View(CREATE_VIEW);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Verify(string token, string authorityType) {
            return View(VERIFY_VIEW, new Pair<string, string>() {
                First = token,
                Second = authorityType
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Verify(string email, string token, string authorityType) {
            try {
                if (theAuthService.IsValidToken(email, token, authorityType)) {
                    return RedirectToAction("CreateAuthority", "User", new { token = token, email = email, authorityType = authorityType });
                }
                ViewData["Message"] = MessageHelper.NormalMessage(INVALID_TOKEN);
            } catch (Exception e) {
                LogError(e, TOKEN_VERIFICATION_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(TOKEN_VERIFICATION_ERROR);
            }

            return View(VERIFY_VIEW, new Pair<string, string>() {
                First = token,
                Second = authorityType
            });
        }
    }
}