using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Photo.Exceptions;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.TextBooks;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Clubs {
 
    public class TextBookController : UOFMeBaseController {
        private const string TEXTBOOK_ADDED = "Textbook posted successfully!";
        private const string NO_TEXTBOOKS = "There are no textbooks currently being sold and there is no one looking for textbooks currently.";
        private const string MARKED_NON_ACTIVE = "The textbook entry has been marked as being non-active.";

        private const string TEXTBOOK_GET_ERROR = "Error getting the textbooks for your unviersity. Please try again.";
        private const string TEXTBOOK_IMAGE_UPLOAD_ERROR = "I'm sorry we wern't able to upload your textbook image for some reason. Please try again.";
        private const string GET_TEXTBOOK_CONDITIONS_ERROR = "We were unable to retrieve the textbook conditions you can select. Please refresh the page.";

        IValidationDictionary theValidation;
        ITextBookService theTextBookService;

        public TextBookController() {
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

            try {
                LoggedInWrapperModel<CreateTextBookModel> myLoggedIn = new LoggedInWrapperModel<CreateTextBookModel>(GetUserInformatonModel().Details);
                CreateTextBookModel myCreateTextbookModel = new CreateTextBookModel() {
                    UniversityId = universityId,
                    BuySellOptions = new SelectList(myBuySellTypes, "Value", "Key"),
                    TextBookConditions = new SelectList(myBookConditionTypes, "Value", "Key")
                };
                myLoggedIn.Set(myCreateTextbookModel);
                return View("Create", myLoggedIn);
            } catch(Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
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

                LoggedInWrapperModel<TextBook> myLoggedIn = new LoggedInWrapperModel<TextBook>(GetUserInformatonModel().Details);
                myLoggedIn.Set(myTextBook);

                return View("Details", myLoggedIn);
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
                LoggedInListModel<TextBook> myLoggedIn = new LoggedInListModel<TextBook>(GetUserInformatonModel().Details);
                myTextBooks = theTextBookService.GetTextBooksForUniversity(universityId);
                myLoggedIn.Set(myTextBooks);

                if (myTextBooks.Count<TextBook>() == 0) {
                    ViewData["Message"] = NO_TEXTBOOKS;
                }

                return View("List", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, TEXTBOOK_GET_ERROR);
                return SendToErrorPage(TEXTBOOK_GET_ERROR);
            }
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
    }
}
