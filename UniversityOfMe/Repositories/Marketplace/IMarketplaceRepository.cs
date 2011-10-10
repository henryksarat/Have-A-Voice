using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Marketplace {
    public interface IMarketplaceRepository {
        MarketplaceItem AddItemToMarketplace(string aUniversityId, User aUser, string anItemType, string aTitle, string aDescription, double aPrice, string anImageUrl, DateTime anExpireListing);
        void DeleteItem(int anItemId);
        MarketplaceItem GetItem(int anItemId);
        User GetSeller(int anItemId);
        IEnumerable<ItemType> GetItemTypes();
        IEnumerable<MarketplaceItem> GetAllLatestItemsSellingForAnyUniversity();
        IEnumerable<MarketplaceItem> GetAllLatestItemsSellingInUniversity(string aUniversityId);
        IEnumerable<MarketplaceItem> GetLatestItemsSellingByTypeInUniversity(string aUniversityId, string anItemType);
        IEnumerable<MarketplaceItem> GetLatestItemsSellingByTypeAndTitleInUniversity(string aUniversityId, string anItemType, string aTitle);
        IEnumerable<MarketplaceItem> GetLatestItemsSellingByItemAndTitleForAnyUniversity(string anItemType, string aTitle);
        IEnumerable<MarketplaceItem> GetLatestItemsSellingByTitleForAllUniversitiesAndTypes(string aTitle);
        IEnumerable<MarketplaceItem> GetLatestItemsSellingInUniversityByTitle(string aUniversityId, string aSearchString);
        MarketplaceItem GetNewestMarketplaceItem();
        void MarkItemAsSold(int anItemId);
        void UpdateItem(MarketplaceItem anItem);
    }
}
