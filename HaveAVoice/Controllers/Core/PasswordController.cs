using System;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using Social.Validation;

namespace HaveAVoice.Controllers.Core {
    public class PasswordController : HAVBaseController {
        private const string EMAIL_SENT = "An email has been sent to the email you provided to help reset your password.";
        private const string PASSWORD_CHANGED = "Your password has been changed.";

        private const string EMAIL_ERROR = "Error sending email.";
        private const string FORGOT_PASSWORD_ERROR = "Unable to perform the forgot password function.";

        private const string REQUEST_VIEW = "Request";
        private const string PROCESS_VIEW = "Process";
        private const string ERROR_MESSAGE_VIEWDATA = "Message";

        private IHAVPasswordService thePasswordService;
        private IValidationDictionary theValidationDictionary;

        public PasswordController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            thePasswordService = new HAVPasswordService(theValidationDictionary);
        }

        public PasswordController(IHAVBaseService aBaseService, IHAVPasswordService aPasswordService)
            : base(aBaseService) {
                thePasswordService = aPasswordService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Request() {
            return View(REQUEST_VIEW);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Request(string email) {
            try {
                if (thePasswordService.ForgotPasswordRequest(email)) {
                    return SendToResultPage(EMAIL_SENT);
                }
            } catch (EmailException e) {
                LogError(e, EMAIL_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(HAVConstants.ERROR);
            } catch (Exception e) {
                LogError(e, FORGOT_PASSWORD_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(HAVConstants.ERROR);
            }

            return View(REQUEST_VIEW);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Process(string id) {
            return View(PROCESS_VIEW, new StringWrapper(id));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Process(string email, string forgotPasswordHash, string password, string retypedPassword) {
            try {
                if (thePasswordService.ChangePassword(email, forgotPasswordHash, password, retypedPassword)) {
                    return SendToResultPage(PASSWORD_CHANGED);
                }
            } catch (Exception e) {
                LogError(e, HAVConstants.ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(HAVConstants.ERROR);
            }

            return View(PROCESS_VIEW, new StringWrapper(forgotPasswordHash));
        }
    }
}
