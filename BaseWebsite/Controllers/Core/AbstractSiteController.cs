using System;
using System.Web.Mvc;
using Social.Authentication;
using Social.Authentication.Services;
using Social.Generic.Services;
using Social.Site.Services;
using Social.Users.Services;
using Social.Validation;
using Social.Site.Repositories;

namespace BaseWebsite.Controllers.Core {
    public abstract class AbstractSiteController<T, U, V, W, X, Y, Z> : BaseController<T, U, V, W, X, Y, Z> {
        private const string SUBMISSION_ERROR = "An error occurred while submitting your request. Please try again.";

        private IContactUsService theContactUsService;

        public AbstractSiteController(IBaseService<T> aBaseService,
                                      IUserInformation<T, Z> aUserInformation,
                                      IAuthenticationService<T, U, V, W, X, Y> anAuthService,
                                      IWhoIsOnlineService<T, Z> aWhoIsOnlineService,
                                      IContactUsRepository aContactRepo) :
            base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
            IValidationDictionary theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theContactUsService = new ContactUsService(theValidationDictionary, aContactRepo);
        }

        protected ActionResult ContactUs() {
            return View("ContactUs");
        }

        protected ActionResult ContactUs(string anEmail, string anInquiryType, string aInquiry) {
            try {
                bool myResult = theContactUsService.AddContactUsInquiry(anEmail, anInquiryType, aInquiry);

                if (myResult) {
                    TempData["Message"] += SuccessMessage("Inquiry processed. Thank you for the submission!");
                }
            } catch (Exception e) {
                TempData["Message"] += ErrorMessage(SUBMISSION_ERROR);
                LogError(e, SUBMISSION_ERROR);
            }

            return RedirectToAction("ContactUs");
        }

        protected ActionResult Privacy() {
            return View("Privacy");
        }

        protected ActionResult AboutUs() {
            return View("AboutUs");
        }

        protected ActionResult Terms() {
            return View("Terms");
        }
    }
}
