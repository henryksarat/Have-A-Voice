using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Models.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Models.Validation;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;

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

        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            return View("Index");
        }

        public ActionResult View() {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(string feedback) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            User myUser = GetUserInformatonModel().Details;
            try {
                if (theFeedbackService.AddFeedback(myUser, feedback)) {
                    return SendToResultPage(FEEDBACK_SUCCESS);
                }
            } catch (Exception e) {
                LogError(e, FEEDBACK_ERROR);
                ViewData["Message"] = FEEDBACK_ERROR;
            }

            return View("Index");
        }

        public override ActionResult SendToResultPage(string title, string details) {
            return SendToResultPage(SiteSectionsEnum.Complaint, title, details);
        }
    }
}
