using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Validation;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;
using HaveAVoice.Controllers.ActionFilters;

namespace HaveAVoice.Controllers.Users
{
    public class FeedbackController : HAVBaseController {
        private static string FEEDBACK_ERROR = "An error occurred, please try again shortly.";
        private static string FEEDBACK_SUCCESS = "Feedback submitted, thank you!";
        private static string PAGE_NOT_FOUND = "You do not have access.";
        private static string ERROR = "An error occurred. Please try again.";

        private IHAVFeedbackService theFeedbackService;
        public FeedbackController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
             ModelStateWrapper myModelWrapper = new ModelStateWrapper(this.ModelState);
             theFeedbackService = new HAVFeedbackService(myModelWrapper);
        }

        public FeedbackController(IHAVBaseService aBaseService, IHAVFeedbackService aService)
            : base(aBaseService) {
            theFeedbackService = aService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            return View("Create");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult View() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!HAVPermissionHelper.AllowedToPerformAction(GetUserInformatonModel(), HAVPermission.View_Feedback)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }

            IEnumerable<Feedback> myFeedback;
            
            try {
                myFeedback = theFeedbackService.GetAllFeedback().ToList<Feedback>();
            } catch (Exception e) {
                LogError(e, ERROR);
                return SendToErrorPage(ERROR);
            }

            if (myFeedback.Count<Feedback>() == 0) {
                ViewData["Message"] = "There is no feedback.";
            }

            return View("View", myFeedback);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            return View("Create");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string feedback) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformatonModel().Details;
            try {
                if (theFeedbackService.AddFeedback(myUser, feedback)) {
                    TempData["Message"] = FEEDBACK_SUCCESS;
                    return RedirectToAction("Create");
                }
            } catch (Exception e) {
                LogError(e, FEEDBACK_ERROR);
                ViewData["Message"] = FEEDBACK_ERROR;
            }

            return View("Create");
        }

        protected override ActionResult SendToResultPage(string title, string details) {
            return SendToResultPage(SiteSectionsEnum.Feedback, title, details);
        }
    }
}
