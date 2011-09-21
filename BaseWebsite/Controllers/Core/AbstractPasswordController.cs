using System;
using System.Web.Mvc;
using Social.Authentication;
using Social.Authentication.Services;
using Social.Email;
using Social.Email.Exceptions;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.User;
using Social.User.Repositories;
using Social.User.Services;
using Social.Users.Services;
using Social.Validation;
using Social.Generic.Exceptions;

namespace BaseWebsite.Controllers.Core {
    public abstract class AbstractPasswordController<T, U, V, W, X, Y, Z> : BaseController<T, U, V, W, X, Y, Z> {
        private const string EMAIL_SENT = "An email has been sent to the email you provided to help reset your password.";
        private const string PASSWORD_CHANGED = "Your password has been changed.";

        private const string EMAIL_ERROR = "Error sending email.";
        private const string FORGOT_PASSWORD_ERROR = "Unable to perform the forgot password function.";

        private const string REQUEST_VIEW = "Request";
        private const string PROCESS_VIEW = "Process";
        private const string ERROR_MESSAGE_VIEWDATA = "Message";

        private IPasswordService<T> thePasswordService;

        public AbstractPasswordController(IBaseService<T> aBaseService,
                                          IUserInformation<T, Z> aUserInformation,
                                          IAuthenticationService<T, U, V, W, X, Y> anAuthService,
                                          IWhoIsOnlineService<T, Z> aWhoIsOnlineService,
                                          IUserRetrievalRepository<T> aUserRetrievalRepo,
                                          IPasswordRepository<T> aPasswordRepo,
                                          IEmail anEmailService) :
            base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
            IValidationDictionary myValidationDictionary = new ModelStateWrapper(this.ModelState);
            IUserRetrievalService<T> myUserRetrievalService = new UserRetrievalService<T>(aUserRetrievalRepo);
            thePasswordService = new PasswordService<T>(myValidationDictionary,
                                                           anEmailService,
                                                           myUserRetrievalService, 
                                                           aPasswordRepo);
        }

        new protected ActionResult Request() {
            return View(REQUEST_VIEW);
        }

        new protected ActionResult Request(string aBaseUrl, string anEmail, string aSubject, string aBody) {
            try {
                if (thePasswordService.ForgotPasswordRequest(aBaseUrl, anEmail, aSubject, aBody)) {
                    return SendToResultPage(EMAIL_SENT);
                }
            } catch (NotActivatedException e) {
                return RedirectToAction("Activation", "User", new { message = 3 });
            } catch (EmailException e) {
                LogError(e, EMAIL_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            } catch (Exception e) {
                LogError(e, FORGOT_PASSWORD_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return View(REQUEST_VIEW);
        }

        protected ActionResult Process(string anId) {
            return View(PROCESS_VIEW, new StringModel(anId));
        }

        protected ActionResult Process(string anEmail, string aForgotPasswordHash, string aPassword, string aRetypedPassword) {
            try {
                if (thePasswordService.ChangePassword(anEmail, aForgotPasswordHash, aPassword, aRetypedPassword)) {
                    return SendToResultPage(PASSWORD_CHANGED);
                }
            } catch (Exception e) {
                LogError(e, ErrorKeys.ERROR_MESSAGE);
                ViewData[ERROR_MESSAGE_VIEWDATA] = ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return View(PROCESS_VIEW, new StringModel(aForgotPasswordHash));
        }
    }
}
