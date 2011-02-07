using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.SiteFeatures;
using HaveAVoice.Validation;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Controllers.ActionFilters;
using HaveAVoice.Controllers.Helpers;

namespace HaveAVoice.Controllers.Core {
    public class ContactUsController : HAVBaseController {
        private const string SUBMISSION_ERROR = "An error occurred while submitting your request. Please try again.";

        private IHAVContactUsService theContactUsService;
        private IValidationDictionary theValidationDictionary;

        public ContactUsController() :
            base(new HAVBaseService(new HAVBaseRepository())) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theContactUsService = new HAVContactUsService(theValidationDictionary);
        }

        public ContactUsController(IHAVBaseService baseService, IHAVContactUsService aContactUsService)
            : base(baseService) {
            theContactUsService = aContactUsService;
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create() {
            return View("Create");
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string email, string inquiryType, string inquiry) {
            try {
                bool myResult = theContactUsService.AddContactUsInquiry(email, inquiryType, inquiry);

                if (myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage("Inquiry processed. Thank you for the submission!");
                }
            } catch (Exception e) {
                TempData["Message"] = MessageHelper.ErrorMessage(SUBMISSION_ERROR);
                LogError(e, SUBMISSION_ERROR);
            }

            return RedirectToAction("Create");
        }
    }
}
