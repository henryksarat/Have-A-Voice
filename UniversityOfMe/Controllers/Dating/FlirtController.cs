using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Users.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Dating;
using UniversityOfMe.UserInformation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models.View;
using Social.Generic.Models;
using Social.Validation;
using System.Collections.Generic;

namespace UniversityOfMe.Controllers.Dating {
    public class FlirtController : UOFMeBaseController {
        private const string ANONYMOUS_FLIRT_SENT = "Anonymous flirt has been sent successfully.";
        private const string ANONYMOUS_FLIRT_SEND_ERROR = "Anonymous flirt has not been sent. Please try again.";

        IFlirtingService theFlirtingService;
        IValidationDictionary theValidationDictionary;

        public FlirtController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theFlirtingService = new FlirtingService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUser = GetUserInformatonModel();
            LoggedInWrapperModel<FlirtModel> myLoggedIn = new LoggedInWrapperModel<FlirtModel>(myUser.Details);

            try {
                FlirtModel myModel = theFlirtingService.GetFlirtModel();
                myLoggedIn.Set(myModel);
                return View("Create", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult CreateWithTaggedUser(int taggedUserId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUser = GetUserInformatonModel();
            LoggedInWrapperModel<FlirtModel> myLoggedIn = new LoggedInWrapperModel<FlirtModel>(myUser.Details);

            try {
                FlirtModel myModel = theFlirtingService.GetFlirtModel(taggedUserId);
                myLoggedIn.Set(myModel);
                return View("Create", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(FlirtModel aModel) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();
                string myUniversityId = UniversityHelper.GetMainUniversity(myUser.Details).Id;
                bool myResult = theFlirtingService.CreateFlirt(myUser, myUniversityId, aModel);
                
                if (myResult) {
                    TempData["Message"] += SuccessMessage(ANONYMOUS_FLIRT_SENT);
                    return RedirectToHomePage();
                }
            } catch (Exception myException) {
                theValidationDictionary.ForceModleStateExport();
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
            }

            if (aModel.TaggedUserId == 0) {
                return RedirectToAction("Create");
            } else {
                return RedirectToAction("CreateWithTaggedUser", new { taggedUserId = aModel.TaggedUserId });
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult List() {
            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                string myUniversity = UniversityHelper.GetMainUniversity(myUserInfo.Details).Id;
                LoggedInListModel<AnonymousFlirt> myLoggedInModel = new LoggedInListModel<AnonymousFlirt>(myUserInfo.Details);
                IEnumerable<AnonymousFlirt> myUserStatuses = theFlirtingService.GetFlirtsWithinUniversity(myUniversity, 25);
                myLoggedInModel.Set(myUserStatuses);

                return View("List", myLoggedInModel);
            } catch (Exception e) {
                LogError(e, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToHomePage();
        }
    }
}
