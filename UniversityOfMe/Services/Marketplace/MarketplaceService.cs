using System;
using System.Collections.Generic;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories.Marketplace;
using Social.Photo.Helpers;
using UniversityOfMe.Helpers.AWS;
using UniversityOfMe.Helpers.Configuration;
using UniversityOfMe.Helpers.Constants;
using Social.Photo.Exceptions;
using System.Web;
using Social.Generic.Exceptions;

namespace UniversityOfMe.Services.Marketplace {
    public class MarketplaceService : IMarketplaceService {
        private IValidationDictionary theValidationDictionary;
        private IMarketplaceRepository theMarketplaceRepository;

        public MarketplaceService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityMarketplaceRepository()) { }

        public MarketplaceService(IValidationDictionary aValidationDictionary, IMarketplaceRepository aMarketplaceRepository) {
            theValidationDictionary = aValidationDictionary;
            theMarketplaceRepository = aMarketplaceRepository;
        }


        public MarketplaceItem CreateItemListing(UserInformationModel<User> aCreatingUser, ItemViewModel anItemViewModel) {
            if (!ValidItem(anItemViewModel)) {
                return null;
            }

            string myImageName = string.Empty;

            if (anItemViewModel.Image != null) {
                try {
                    myImageName = AWSPhotoHelper.TakeImageAndResizeAndUpload(anItemViewModel.Image,
                        AWSHelper.GetClient(),
                        SiteConfiguration.MarketplacePhotosBucket(),
                        anItemViewModel.Title.GetHashCode().ToString(),
                        MarketplaceConstants.ITEM_MAX_SIZE);
                } catch (Exception myException) {
                    throw new PhotoException("Unable to upload the image.", myException);
                }
            }

            MarketplaceItem myItem = theMarketplaceRepository.AddItemToMarketplace(
                anItemViewModel.UniversityId,
                aCreatingUser.Details,
                anItemViewModel.ItemType,
                anItemViewModel.Title,
                anItemViewModel.Description,
                double.Parse(anItemViewModel.Price),
                myImageName,
                TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(anItemViewModel.ExpireListing)));

            return myItem;
        }

        public IDictionary<string, string> CreateItemTypesDictionaryEntry() {
            IDictionary<string, string> myDictionary = DictionaryHelper.DictionaryWithSelect();
            IEnumerable<ItemType> myItemTypes = theMarketplaceRepository.GetItemTypes();
            foreach (ItemType myItemType in myItemTypes) {
                myDictionary.Add(myItemType.Id, myItemType.DisplayName);
            }
            return myDictionary;
        }

        public void DeleteItem(UserInformationModel<User> aUserInfo, int anItemId) {
            User myOwner = theMarketplaceRepository.GetSeller(anItemId);

            if (aUserInfo.Details.Id == myOwner.Id) {
                theMarketplaceRepository.DeleteItem(anItemId);
            }
        }

        public bool EditItem(UserInformationModel<User> aUserInfo, ItemViewModel anItemViewModel) {
            if (!ValidItem(anItemViewModel)) {
                return false;
            }

            MarketplaceItem myItem = GetItem(anItemViewModel.ItemId);
            myItem.Title = anItemViewModel.Title;
            myItem.Description = anItemViewModel.Description;
            myItem.Price = double.Parse(anItemViewModel.Price);
            myItem.ItemTypeId = anItemViewModel.ItemType;
            myItem.ExpireListing = TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(anItemViewModel.ExpireListing));

            theMarketplaceRepository.UpdateItem(myItem);

            if (anItemViewModel.Image != null) {
                string myOldItemPicture = myItem.ImageName;

                UpdateItemPhoto(anItemViewModel.ItemId.ToString(), anItemViewModel.Image, myItem);

                if (!string.IsNullOrEmpty(myOldItemPicture)) {
                    AWSPhotoHelper.PhysicallyDeletePhoto(AWSHelper.GetClient(), SiteConfiguration.TextbookPhotosBucket(), myOldItemPicture);
                }
            }

            return true;
        }

        public MarketplaceItem GetItem(int anItemId) {
            return theMarketplaceRepository.GetItem(anItemId);
        }

        public IEnumerable<ItemType> GetItemTypes() {
            return theMarketplaceRepository.GetItemTypes();
        }

        public IEnumerable<MarketplaceItem> GetAllLatestItemsSellingForAnyUniversity() {
            return theMarketplaceRepository.GetAllLatestItemsSellingForAnyUniversity();
        }

        public IEnumerable<MarketplaceItem> GetAllLatestItemsSellingInUniversity(string aUniversityId) {
            return theMarketplaceRepository.GetAllLatestItemsSellingInUniversity(aUniversityId);
        }

        public IEnumerable<MarketplaceItem> GetLatestItemsSellingInUniversityByItem(string aUniversityId, string anItemType) {
            return theMarketplaceRepository.GetLatestItemsSellingByTypeInUniversity(aUniversityId, anItemType);
        }

        public IEnumerable<MarketplaceItem> GetLatestItemsSellingInUniversityByItemAndTitle(string aUniversityId, string anItemType, string aTitle) {
            return theMarketplaceRepository.GetLatestItemsSellingByTypeAndTitleInUniversity(aUniversityId, anItemType, aTitle);
        }

        public IEnumerable<MarketplaceItem> GetLatestItemsSellingByTypeAndTitleForAnyUniversity(string anItemType, string aTitle) {
            return theMarketplaceRepository.GetLatestItemsSellingByItemAndTitleForAnyUniversity(anItemType, aTitle);
        }

        public IEnumerable<MarketplaceItem> GetLatestItemsSellingByTitleForAllUniversitiesAndTypes(string aTitle) {
            return theMarketplaceRepository.GetLatestItemsSellingByTitleForAllUniversitiesAndTypes(aTitle);
        }

        public void MarkItemAsSold(UserInformationModel<User> aUserInfo, int anItemId) {
            User myOwner = theMarketplaceRepository.GetSeller(anItemId);

            if (aUserInfo.Details.Id == myOwner.Id) {
                theMarketplaceRepository.MarkItemAsSold(anItemId);
            }
        }

        private bool ValidItem(ItemViewModel anItemViewModel) {
            if (string.IsNullOrEmpty(anItemViewModel.UniversityId)) {
                theValidationDictionary.AddError("UniversityId", anItemViewModel.UniversityId, "A university must be present.");
            }

            if (string.IsNullOrEmpty(anItemViewModel.Title)) {
                theValidationDictionary.AddError("Title", anItemViewModel.Title, "You must give your item a title.");
            } else if (anItemViewModel.Title.Length > 100) {
                theValidationDictionary.AddError("Title", anItemViewModel.Title, "The title can only be 100 characters long.");
            }

            double myNum;
            if (!double.TryParse(anItemViewModel.Price, out myNum) || myNum < 0.01) {
                theValidationDictionary.AddError("Price", anItemViewModel.Price.ToString(), "The asking price must be above zero.");
            }

            if (!DropDownItemValidation.IsValid(anItemViewModel.ItemType)) {
                theValidationDictionary.AddError("ItemType", anItemViewModel.ItemType, "You must spcify an item type.");
            }

            if (anItemViewModel.Image != null && !PhotoValidation.IsValidImageFile(anItemViewModel.Image.FileName)) {
                theValidationDictionary.AddError("Image", anItemViewModel.Image.FileName, PhotoValidation.INVALID_IMAGE);
            }

            DateTime myStartTimeUtc = TimeZoneInfo.ConvertTimeToUtc(anItemViewModel.GetExpireDate());

            if (anItemViewModel.ExpireListing == null) {
                theValidationDictionary.AddError("ExpireListing", anItemViewModel.ExpireListing.ToString(), "Invalid date.");
            } else if (myStartTimeUtc <= DateTime.UtcNow) {
                theValidationDictionary.AddError("ExpireListing", anItemViewModel.ExpireListing.ToString(), "The expire date for the listing must be later than now.");
            }

            return theValidationDictionary.isValid;
        }

        private void UpdateItemPhoto(string anId, HttpPostedFileBase aTextBookImage, MarketplaceItem anItem) {
            string myImageName = string.Empty;

            try {
                myImageName = AWSPhotoHelper.TakeImageAndResizeAndUpload(aTextBookImage,
                        AWSHelper.GetClient(),
                        SiteConfiguration.MarketplacePhotosBucket(),
                        anId,
                        MarketplaceConstants.ITEM_MAX_SIZE);
            } catch (Exception myException) {
                throw new PhotoException("Error while resizing and uploading the photo. ", myException);
            }
            try {
                anItem.ImageName = myImageName;
                theMarketplaceRepository.UpdateItem(anItem);
            } catch (Exception myException) {
                AWSPhotoHelper.PhysicallyDeletePhoto(AWSHelper.GetClient(), SiteConfiguration.TextbookPhotosBucket(), myImageName);
                throw new CustomException("Error while updating the item to the new item photo.", myException);
            }
        }


        public IEnumerable<MarketplaceItem> GetLatestItemsSellingInUniversityByTitle(string aUniversityId, string aSearchString) {
            return theMarketplaceRepository.GetLatestItemsSellingInUniversityByTitle(aUniversityId, aSearchString);
        }

        public MarketplaceItem GetNewestMarketplaceItem() {
            return theMarketplaceRepository.GetNewestMarketplaceItem();
        }
    }
}
