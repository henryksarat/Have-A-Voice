using System.Collections.Generic;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Services.Marketplace {
    public interface IMarketplaceService {
        MarketplaceItem CreateItemListing(UserInformationModel<User> aCreatingUser, ItemViewModel anItemViewModel);
        IDictionary<string, string> CreateItemTypesDictionaryEntry();
        bool EditItem(UserInformationModel<User> aUserInfo, ItemViewModel anItemViewModel);
        void DeleteItem(UserInformationModel<User> aUserInfo, int anItemId);
        MarketplaceItem GetItem(int aGetItem);
        IEnumerable<ItemType> GetItemTypes();
        IEnumerable<MarketplaceItem> GetAllLatestItemsSellingForAnyUniversity();
        IEnumerable<MarketplaceItem> GetAllLatestItemsSellingInUniversity(string aUniversityId);
        IEnumerable<MarketplaceItem> GetLatestItemsSellingInUniversityByItem(string aUniversityId, string anItemType);
        IEnumerable<MarketplaceItem> GetLatestItemsSellingInUniversityByTitle(string aUniversityId, string aSearchString);
        IEnumerable<MarketplaceItem> GetLatestItemsSellingInUniversityByItemAndTitle(string aUniversityId, string anItemType, string aTitle);
        IEnumerable<MarketplaceItem> GetLatestItemsSellingByTypeAndTitleForAnyUniversity(string anItemType, string aTitle);
        IEnumerable<MarketplaceItem> GetLatestItemsSellingByTitleForAllUniversitiesAndTypes(string aTitle);
        MarketplaceItem GetNewestMarketplaceItem();
        void MarkItemAsSold(UserInformationModel<User> aUserInfo, int anItemId);
    }
}