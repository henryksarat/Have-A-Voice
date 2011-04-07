using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Services.TextBooks;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Photo.Exceptions;
using Social.Validation;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.TextBooks;

namespace UniversityOfMe.Controllers.Clubs {
 
    public class TextBookController : BaseController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        private const string TEXTBOOK_ADDED = "Textbook posted successfully!";
        private const string NO_TEXTBOOKS = "There are no textbooks currently being sold and there is no one looking for textbooks currently.";
        private const string MARKED_NON_ACTIVE = "The textbook entry has been marked as being non-active.";

        private const string TEXTBOOK_GET_ERROR = "Error getting the textbooks for your unviersity. Please try again.";
        private const string TEXTBOOK_IMAGE_UPLOAD_ERROR = "I'm sorry we wern't able to upload your textbook image for some reason. Please try again.";
        private const string GET_TEXTBOOK_CONDITIONS_ERROR = "We were unable to retrieve the textbook conditions you can select. Please refresh the page.";

        IValidationDictionary theValidation;
        ITextBookService theTextBookService;

        public TextBookController()
            : base(new BaseService<User>(new EntityBaseRepository()),
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())),
                   InstanceHelper.CreateAuthencationService(),
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            theValidation = new ModelStateWrapper(this.ModelState);
            theTextBookService = new TextBookService(theValidation);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            IDictionary<string, string> myBuySellTypes = theTextBookService.CreateBuySellDictionaryEntry();
            IDictionary<string, string> myBookConditionTypes = DictionaryHelper.DictionaryWithSelect();

            try {
                myBookConditionTypes = theTextBookService.CreateTextBookConditionsDictionaryEntry();
            } catch (Exception myException) {
                LogError(myException, GET_TEXTBOOK_CONDITIONS_ERROR);
                ViewData["Message"] = GET_TEXTBOOK_CONDITIONS_ERROR;
            }

            return View("Create", new CreateTextBookModel() {
                UniversityId = universityId,
                BuySellOptions = new SelectList(myBuySellTypes, "Value", "Key"),
                TextBookConditions = new SelectList(myBookConditionTypes, "Value", "Key")
            });
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(CreateTextBookModel textbook) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            
            try {
                bool myResult = theTextBookService.CreateTextBook(myUserInformation, textbook);

                if (myResult) {
                    TempData["Message"] = TEXTBOOK_ADDED;
                    return RedirectToAction("List");
                }
            } catch(PhotoException myException) {
                LogError(myException, TEXTBOOK_IMAGE_UPLOAD_ERROR);
                TempData["Message"] = TEXTBOOK_IMAGE_UPLOAD_ERROR;
                theValidation.ForceModleStateExport();
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = ErrorKeys.ERROR_MESSAGE;
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(int id) {
            try {
                TextBook myTextBook = theTextBookService.GetTextBook(id);

                if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, myTextBook.UniversityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                return View("Details", myTextBook);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToResultPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            IEnumerable<TextBook> myTextBooks = new List<TextBook>();

            try {
                myTextBooks = theTextBookService.GetTextBooksForUniversity(universityId);

                if (myTextBooks.Count<TextBook>() == 0) {
                    ViewData["Message"] = NO_TEXTBOOKS;
                }
            } catch (Exception myException) {
                LogError(myException, TEXTBOOK_GET_ERROR);
                ViewData["Message"] = TEXTBOOK_GET_ERROR;
            }

            return View("List", myTextBooks);
        }

        [AcceptVerbs(HttpVerbs.Post), ImportModelStateFromTempData]
        public ActionResult MarkAsNonActive(int id) {
            try {
                bool myResult = theTextBookService.MarkAsNotActive(GetUserInformatonModel(), id);

                if(myResult) {
                    TempData["Message"] = MARKED_NON_ACTIVE;
                }

                return RedirectToAction("List");
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = ErrorKeys.ERROR_MESSAGE;
                return RedirectToAction("Details", new { id = id });
            }
        }

        protected override AbstractUserModel<User> GetSocialUserInformation() {
            return SocialUserModel.Create(GetUserInformaton());
        }

        protected override AbstractUserModel<User> CreateSocialUserModel(User aUser) {
            return SocialUserModel.Create(aUser);
        }

        protected override IProfilePictureStrategy<User> ProfilePictureStrategy() {
            return new ProfilePictureStrategy();
        }

        protected override string UserEmail() {
            return GetUserInformaton().Email;
        }

        protected override string UserPassword() {
            return GetUserInformaton().Password;
        }

        protected override int UserId() {
            return GetUserInformaton().Id;
        }

        protected override string ErrorMessage(string aMessage) {
            return aMessage;
        }

        protected override string NormalMessage(string aMessage) {
            return aMessage;
        }

        protected override string SuccessMessage(string aMessage) {
            return aMessage;
        }
    }
}
