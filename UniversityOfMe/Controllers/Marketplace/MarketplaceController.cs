using System;
using System.Collections.Generic;
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
using UniversityOfMe.Services.Marketplace;
using UniversityOfMe.UserInformation;
using Social.Admin.Exceptions;
using Social.Generic.Exceptions;

namespace UniversityOfMe.Controllers.Marketplace {
 
    public class MarketplaceController : UOFMeBaseController {
        private const string ITEM_ADDED = "Item posted to the marketplace successfully!";
        private const string ITEM_EDITED = "Item edited successfully!";
        private const string MARKED_NON_ACTIVE = "The item has been marked as sold.";
        
        private const string GET_ITEM_TYPES_ERROR = "We were unable to retrieve the item types you can select. Please refresh the page.";
        private const string ITEM_IMAGE_UPLOAD_ERROR = "I'm sorry we wern't able to upload your image for some reason. Please try again.";
        private const string ITEM_GET_ERROR = "Error retrieving the item. Please try again.";
        private const string ITEM_EDIT_ERROR = "Error editing the listing. Please try again.";
        private const string ITEM_PHOTO_UPLOAD_ERROR = "An error occurred while uploading the photo. Everything else was saved but try uploading the listing photo again.";

        IValidationDictionary theValidation;
        IMarketplaceService theMarketplaceService;
        IUniversityRepository theUniversityRepo;

        public MarketplaceController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidation = new ModelStateWrapper(this.ModelState);
            theMarketplaceService = new MarketplaceService(theValidation);
            theUniversityRepo = new EntityUniversityRepository();
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            IDictionary<string, string> myItemTypes = DictionaryHelper.DictionaryWithSelect();

            try {
                myItemTypes = theMarketplaceService.CreateItemTypesDictionaryEntry();
            } catch (Exception myException) {
                LogError(myException, GET_ITEM_TYPES_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(GET_ITEM_TYPES_ERROR);
            }

            try {
                LoggedInWrapperModel<ItemViewModel> myLoggedIn = new LoggedInWrapperModel<ItemViewModel>(GetUserInformatonModel().Details);
                ItemViewModel myCreateItem = new ItemViewModel() {
                    UniversityId = universityId,
                    ItemTypes = new SelectList(myItemTypes, "Value", "Key")
                };
                myLoggedIn.Set(myCreateItem);
                return View("Create", myLoggedIn);
            } catch(Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(ItemViewModel anItemModel) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            
            try {
                MarketplaceItem myItem = theMarketplaceService.CreateItemListing(myUserInformation, anItemModel);

                if (myItem != null) {
                    TempData["Message"] += MessageHelper.SuccessMessage(ITEM_ADDED);
                    return RedirectToAction("Details", new { id = myItem.Id });
                }
            } catch(PhotoException myException) {
                LogError(myException, ITEM_IMAGE_UPLOAD_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(ITEM_IMAGE_UPLOAD_ERROR);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
                
            }

            theValidation.ForceModleStateExport();

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(int id) {
            try {
                MarketplaceItem myItem = theMarketplaceService.GetItem(id);
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                User myUser = myUserInfo == null ? null : myUserInfo.Details;

                LoggedInWrapperModel<MarketplaceItem> myLoggedIn = new LoggedInWrapperModel<MarketplaceItem>(myUser);
                myLoggedIn.Set(myItem);
                //myLoggedIn.LeftNavigation.BackgroundImage = theUniversityRepo.GetUniversity(myItem.UniversityId).Image;

                return View("Details", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToResultPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Delete(int id) {
            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                theMarketplaceService.DeleteItem(myUserInfo, id);
                return RedirectToHomePage();
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToResultPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult MarkAsSold(int id) {
            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                theMarketplaceService.MarkItemAsSold(myUserInfo, id);
                return RedirectToHomePage();
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

            IDictionary<string, string> myItemTypes = DictionaryHelper.DictionaryWithSelect();

            try {
                myItemTypes = theMarketplaceService.CreateItemTypesDictionaryEntry();
            } catch (Exception myException) {
                LogError(myException, GET_ITEM_TYPES_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(GET_ITEM_TYPES_ERROR);
                return RedirectToAction("Details", new { id = id });
            }

            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                MarketplaceItem myItem = theMarketplaceService.GetItem(id);

                LoggedInWrapperModel<ItemViewModel> myLoggedIn = new LoggedInWrapperModel<ItemViewModel>(GetUserInformatonModel().Details);
                ItemViewModel myCreateItemModel = new ItemViewModel(myItem) {
                    UniversityId = universityId,
                    ItemTypes = new SelectList(myItemTypes, "Value", "Key", myItem.ItemTypeId)
                };
                myLoggedIn.Set(myCreateItemModel);

                return View("Edit", myLoggedIn);
            } catch (PermissionDenied myException) {
                TempData["Message"] += MessageHelper.WarningMessage(myException.Message);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += MessageHelper.ErrorMessage(ITEM_GET_ERROR);
            }

            return RedirectToAction("Details", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(ItemViewModel aViewModel) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                bool myResult = theMarketplaceService.EditItem(myUserInfo, aViewModel);

                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(ITEM_EDITED);
                    return RedirectToAction("Details", new { id = aViewModel.ItemId });
                }
            } catch (PhotoException anException) {
                LogError(anException, anException.Message);
                TempData["Message"] += MessageHelper.ErrorMessage(ITEM_PHOTO_UPLOAD_ERROR);
            } catch (CustomException anException) {
                LogError(anException, anException.Message);
                TempData["Message"] += MessageHelper.ErrorMessage(ITEM_PHOTO_UPLOAD_ERROR);
            } catch (Exception myException) {
                LogError(myException, ITEM_EDIT_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(ITEM_EDIT_ERROR);
                theValidation.ForceModleStateExport();
            }

            return RedirectToAction("Edit", new { id = aViewModel.ItemId });
        }
    }
}
