using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using Social.Admin.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Controllers.Core {
    public class AuthorityVerificationController : HAVBaseController {
        private const string INVALID_TOKEN = "The token is either invalid or wasn't assigned to that email. If this is erroneous please contact us by clicking the \"Contact Us\" link on the bottom of this page and filling out the form.";

        private const string TOKEN_CREATED_AND_SENT_ERROR = "An error has occurred while created and sending the token. Please try again.";
        private const string TOKEN_VERIFICATION_ERROR = "An error has occurred while verifying the token for the email. Please try again.";
        private const string USER_POSITION_ERROR = "Unable to get the user positions. Please try again.";

        private const string USER_CONTROLLER = "User";
        private const string CREATE_AUTHORITY_ACTION = "CreateAuthority";
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

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Create_Authority_Verification_Token)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }
            try {
                IEnumerable<UserPosition> myUserPositions = theAuthService.GetUserPositions();

                IDictionary<string, string> myUserPositionDictionary = new Dictionary<string, string>();

                myUserPositionDictionary.Add("Select", "Select");

                foreach (UserPosition myUserPosition in myUserPositions) {
                    myUserPositionDictionary.Add(myUserPosition.Position, myUserPosition.Display);
                }

                return View(CREATE_VIEW, new SelectList(myUserPositionDictionary, "Key", "Value"));
            } catch (Exception myException) {
                LogError(myException, USER_POSITION_ERROR);
                return SendToErrorPage(USER_POSITION_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string email, string authorityType, string authorityPosition) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            try {
                if (theAuthService.RequestTokenForAuthority(myUserInformation, email, authorityType, authorityPosition)) {
                    return SendToResultPage("A token has been created and sent to the authority with the email " + email);
                } 
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                LogError(e, TOKEN_CREATED_AND_SENT_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(TOKEN_CREATED_AND_SENT_ERROR);
            }

            return RedirectToAction(CREATE_VIEW);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Verify(string token, string authorityType, string authorityPosition) {
            return View(VERIFY_VIEW, new AuthorityVerificationModel() {
               Token = token,
               AuthorityType = authorityType,
               AuthorityPosition = authorityPosition
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Verify(string email, AuthorityVerificationModel model) {
            try {
                if (theAuthService.IsValidToken(email, model.Token, model.AuthorityType, model.AuthorityPosition)) {
                    return RedirectToAction(CREATE_AUTHORITY_ACTION, USER_CONTROLLER, new {
                        token = model.Token, 
                        email = email,
                        authorityType = model.AuthorityType,
                        authorityPosition = model.AuthorityPosition
                    });
                }
                TempData["Message"] = MessageHelper.NormalMessage(INVALID_TOKEN);
            } catch (Exception e) {
                LogError(e, TOKEN_VERIFICATION_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(TOKEN_VERIFICATION_ERROR);
            }

            return RedirectToAction(VERIFY_VIEW, new {
                token = model.Token,
                authorityType = model.AuthorityType,
                authorityPosition = model.AuthorityPosition
            });
        }
    }
}