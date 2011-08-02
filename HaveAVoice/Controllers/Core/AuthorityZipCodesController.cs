using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Services.UserFeatures;
using Social.Admin.Exceptions;
using Social.Admin.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Controllers.Core {
    public class AuthorityZipCodesController : HAVBaseController {
        private const string ZIP_CODES_ASSIGNED = "Zip Codes assigned to user.";

        private const string ZIP_CODES_ERROR = "Error assigning the zip codes to the user.";
        private const string ZIP_CODES_RETRIEVE_ERROR = "Error retrieving the zip codes for the user.";

        private IHAVAuthorityVerificationService theAuthService;
        private IValidationDictionary theValidationDictionary;

        
        public AuthorityZipCodesController() {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theAuthService = new HAVAuthorityVerificationService(theValidationDictionary);
        }

        public AuthorityZipCodesController(IHAVAuthorityVerificationService anAuthService) {
            theAuthService = anAuthService;
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Create_Authority_Verification_Token)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }
            return View("Create");
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string email, string zipCodes) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            try {
                bool myResult = theAuthService.AddZipCodesForUser(myUserInformation, email, zipCodes);
                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(ZIP_CODES_ASSIGNED);
                    return RedirectToAction("List", new { email = email });
                }
            } catch (PermissionDenied e) {
                TempData["Message"] += MessageHelper.ErrorMessage(e.Message);
            } catch (Exception e) {
                LogError(e, ZIP_CODES_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(ZIP_CODES_ERROR);
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult List(string email) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Create_Authority_Verification_Token)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }

            IEnumerable<AuthorityViewableZipCode> myZipCodes = new List<AuthorityViewableZipCode>();

            try {
                myZipCodes = theAuthService.GetAuthorityViewableZipCodes(myUserInformation, email);
            } catch (PermissionDenied e) {
                TempData["Message"] += MessageHelper.ErrorMessage(e.Message);
            } catch (Exception e) {
                LogError(e, ZIP_CODES_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(ZIP_CODES_RETRIEVE_ERROR);
            }

            return View("List", myZipCodes);
        }
    }
}