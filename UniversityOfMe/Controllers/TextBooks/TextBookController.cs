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
using UniversityOfMe.Helpers.Textbook;
using Social.Admin.Exceptions;
using Social.Generic.Exceptions;
using System.IO;

namespace UniversityOfMe.Controllers.Clubs {
 
    public class TextBookController : UOFMeBaseController {
        private const string TEXTBOOK_ADDED = "Textbook posted successfully!";
        private const string TEXTBOOK_EDITED = "Textbook has been edited successfully!";
        private const string NO_TEXTBOOKS = "There are no textbooks currently being sold and there is no one looking for textbooks currently.";
        private const string NO_TEXTBOOKS_SEARCH = "Your search came back with nothing.";
        private const string MARKED_NON_ACTIVE = "The textbook entry has been marked as being non-active.";
        private const string TEXTBOOK_DELETED = "The textbook entry has been deleted.";

        private const string TEXTBOOKS_GET_ERROR = "Error getting the textbooks for your unviersity. Please try again.";
        private const string TEXTBOOK_IMAGE_UPLOAD_ERROR = "I'm sorry we wern't able to upload your textbook image for some reason. Please try again.";
        private const string GET_TEXTBOOK_CONDITIONS_ERROR = "We were unable to retrieve the textbook conditions you can select. Please refresh the page.";
        private const string TEXTBOOK_GET_ERROR = "Error retrieving the textbook. Please try again.";
        private const string TEXTBOOK_EDIT_ERROR = "Error editing the textbook. Please try again.";
        private const string TEXTBOOK_PHOTO_UPLOAD_ERROR = "An error occurred while uploading the textbook photo. Everything else was saved but try uploading the textbook photo again.";
        private const string TEXTBOOK_DELETED_ERROR = "An error has occurred while deleting the textbook entry. Please try again.";

        IValidationDictionary theValidation;
        ITextBookService theTextBookService;

        public TextBookController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
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
                ViewData["Message"] = MessageHelper.ErrorMessage(GET_TEXTBOOK_CONDITIONS_ERROR);
            }

            try {
                LoggedInWrapperModel<TextBookViewModel> myLoggedIn = new LoggedInWrapperModel<TextBookViewModel>(GetUserInformatonModel().Details);
                TextBookViewModel myCreateTextbookModel = new TextBookViewModel() {
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
        public ActionResult Create(TextBookViewModel textbook) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            
            try {
                bool myResult = theTextBookService.CreateTextBook(myUserInformation, textbook);

                if (myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(TEXTBOOK_ADDED);
                    return RedirectToAction("List");
                }
            } catch(PhotoException myException) {
                LogError(myException, TEXTBOOK_IMAGE_UPLOAD_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(TEXTBOOK_IMAGE_UPLOAD_ERROR);
                theValidation.ForceModleStateExport();
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
                theValidation.ForceModleStateExport();
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                theTextBookService.DeleteTextBook(myUserInformation, id);
                TempData["Message"] = MessageHelper.SuccessMessage(TEXTBOOK_DELETED);
                return RedirectToAction("List");
            } catch (FileNotFoundException myException) {
                LogError(myException, TEXTBOOK_DELETED_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(TEXTBOOK_DELETED_ERROR);
            } catch (Exception myException) {
                LogError(myException, TEXTBOOK_DELETED_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(TEXTBOOK_DELETED_ERROR);
            }

            return RedirectToAction("Details", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

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

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Edit(string universityId, int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            IDictionary<string, string> myBuySellTypes = theTextBookService.CreateBuySellDictionaryEntry();
            IDictionary<string, string> myBookConditionTypes = DictionaryHelper.DictionaryWithSelect();

            try {
                myBookConditionTypes = theTextBookService.CreateTextBookConditionsDictionaryEntry();
            } catch (Exception myException) {
                LogError(myException, GET_TEXTBOOK_CONDITIONS_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(GET_TEXTBOOK_CONDITIONS_ERROR);
                return RedirectToAction("Details", new { id = id });
            }

            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                TextBook myTextBook = theTextBookService.GetTextBookForEdit(myUserInfo, id);

                LoggedInWrapperModel<TextBookViewModel> myLoggedIn = new LoggedInWrapperModel<TextBookViewModel>(GetUserInformatonModel().Details);
                TextBookViewModel myCreateTextbookModel = new TextBookViewModel(myTextBook) {
                    UniversityId = universityId,
                    BuySellOptions = new SelectList(myBuySellTypes, "Value", "Key", myTextBook.BuySell),
                    TextBookConditions = new SelectList(myBookConditionTypes, "Value", "Key", myTextBook.TextBookConditionId)
                };
                myLoggedIn.Set(myCreateTextbookModel);

                return View("Edit", myLoggedIn);
            } catch (PermissionDenied myException) {
                TempData["Message"] = MessageHelper.WarningMessage(myException.Message);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = MessageHelper.ErrorMessage(TEXTBOOK_GET_ERROR);
            }

            return RedirectToAction("Details", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(TextBookViewModel aViewModel) {
           if (!IsLoggedIn()) {
                return RedirectToLogin();
            }


            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                bool myResult = theTextBookService.EditTextBook(myUserInfo, aViewModel);

                if (myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(TEXTBOOK_EDITED);
                    return RedirectToAction("Details", new { id = aViewModel.TextBookId });
                }
            } catch (PhotoException anException) {
                LogError(anException, anException.Message);
                TempData["Message"] = MessageHelper.ErrorMessage(TEXTBOOK_PHOTO_UPLOAD_ERROR);
            } catch (CustomException anException) {
                LogError(anException, anException.Message);
                TempData["Message"] = MessageHelper.ErrorMessage(TEXTBOOK_PHOTO_UPLOAD_ERROR);
            } catch (Exception myException) {
                LogError(myException, TEXTBOOK_EDIT_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(TEXTBOOK_EDIT_ERROR);
                theValidation.ForceModleStateExport();
            }

            return RedirectToAction("Edit", new { id = aViewModel.TextBookId });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List(string universityId) {
            return RedirectToAction("Textbook", "Search", new { searchString = string.Empty, page = 1 });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult MarkAsNonActive(int id) {
            try {
                bool myResult = theTextBookService.MarkAsNotActive(GetUserInformatonModel(), id);

                if(myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(MARKED_NON_ACTIVE);
                }

                return RedirectToAction("List");
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
                return RedirectToAction("Details", new { id = id });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(string universityId, string searchOption, string searchString, string orderByOption) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            IEnumerable<TextBook> myTextBooks = new List<TextBook>();

            try {
                LoggedInWrapperModel<TextbookListModel> myLoggedIn = new LoggedInWrapperModel<TextbookListModel>(GetUserInformatonModel().Details);
                myTextBooks = theTextBookService.SearchTextBooksWithinUniversity(universityId, searchOption, searchString, orderByOption);

                TextbookListModel myTextbookListModel = CreateTextBookListModel(myTextBooks, searchOption, orderByOption);

                myLoggedIn.Set(myTextbookListModel);

                if (myTextBooks.Count<TextBook>() == 0) {
                    ViewData["Message"] = MessageHelper.NormalMessage(NO_TEXTBOOKS_SEARCH);
                }

                return View("List", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, TEXTBOOKS_GET_ERROR);
                return SendToErrorPage(TEXTBOOKS_GET_ERROR);
            }
        }

        private TextbookListModel CreateTextBookListModel(IEnumerable<TextBook> aTextBooks, string aSearchOptionSelected, string anOrderByOptionSelected) {
            IDictionary<string, string> myOrderByOptionsDictionary = DictionaryHelper.DictionaryWithSelect();
            myOrderByOptionsDictionary.Add("Lowest Price", "LowestPrice");
            myOrderByOptionsDictionary.Add("Highest Price", "HighestPrice");
            myOrderByOptionsDictionary.Add("Most Recent Posting", "Date");

            IDictionary<string, string> mySearchOptionDictionary = DictionaryHelper.DictionaryWithSelect();
            mySearchOptionDictionary.Add("Book Title", "Title");
            mySearchOptionDictionary.Add("Class Code", "ClassCode");


            return new TextbookListModel() {
                OrderByOptions = new SelectList(myOrderByOptionsDictionary, "Value", "Key", anOrderByOptionSelected),
                SearchOptions = new SelectList(mySearchOptionDictionary, "Value", "Key", aSearchOptionSelected),
                Textbooks = aTextBooks
            };
        }
    }
}
