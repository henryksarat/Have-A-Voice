using System;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Services;
using System.Web.Routing;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.UserInformation;

namespace HaveAVoice.Controllers  {
    public abstract class HAVBaseController : Controller {
        private IHAVBaseService theErrorService;
        public IUserInformation theUserInformation;

        protected override void Initialize(RequestContext rc) {
            base.Initialize(rc);
            HAVUserInformationFactory.SetInstance(UserInformation.Instance());
            theErrorService.ResetConnection();
        }
       
        public HAVBaseController(IHAVBaseService baseService) {
            theErrorService = baseService;
        }

        protected User GetUserInformaton() {
            UserInformationModel myUserInformation = GetUserInformatonModel();
            return myUserInformation != null ? myUserInformation.Details : null;
        }

        protected UserInformationModel GetUserInformatonModel() {
            return HAVUserInformationFactory.GetUserInformation();
        }

        protected bool IsLoggedIn() {
            return HAVUserInformationFactory.IsLoggedIn();
        }

        protected ActionResult SendToErrorPage(string error) {
            AddErrorToSession(error);
            return RedirectToAction("Error", "Shared");
        }

        protected ActionResult SendToResultPage(SiteSectionsEnum siteSection, string title, string details) {
            AddMessageToSession(siteSection, title, details);
            return RedirectToAction("Result", "Shared");
        }

        public void AddMessageToSession(SiteSectionsEnum siteSection, string title, string details) {
            MessageModel messageModel = new MessageModel();
            messageModel.Origin = siteSection;
            messageModel.Title = title;
            messageModel.Details = details;
            Session["Message"] = messageModel;
        }

        public void AddErrorToSession(string error) {
            ErrorModel errorModel = new ErrorModel();
            errorModel.ErrorMessage = error;
            Session["ErrorMessage"] = errorModel;
        }

        protected ActionResult SendToResultPage(string details) {
            return SendToResultPage(null, details);
        }

        protected void LogError(Exception anException, string aDetails) {
            theErrorService.LogError(anException, aDetails);
        }
        public abstract ActionResult SendToResultPage(string aTitle, string aDetails);
    }
}
