using System;
using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;
using UniversityOfMe.Helpers;
using UniversityOfMe.Repositories.Helpers;
using UniversityOfMe.Helpers.Badges;

namespace UniversityOfMe.Repositories.Marketplace {
    public class EntityMarketplaceRepository : IMarketplaceRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public MarketplaceItem AddItemToMarketplace(string aUniversityId, User aUser, string anItemType, string aTitle, string aDescription, double aPrice, string anImageName, DateTime anExpireListing) {
            MarketplaceItem myItemSelling = MarketplaceItem.CreateMarketplaceItem(0, aUniversityId, aUser.Id, anItemType, aTitle, aDescription, aPrice, DateTime.UtcNow, anExpireListing, false, false);
            if (!string.IsNullOrEmpty(anImageName)) {
                myItemSelling.ImageName = anImageName;
            }
            theEntities.AddToMarketplaceItems(myItemSelling);
            theEntities.SaveChanges();
            return myItemSelling;
        }

        public void DeleteItem(int anItemId) {
            MarketplaceItem myItemSelling = GetItem(anItemId);
            myItemSelling.Deleted = true;
            theEntities.ApplyCurrentValues(myItemSelling.EntityKey.EntitySetName, myItemSelling);
            theEntities.SaveChanges();
        }

        public User GetSeller(int anItemId) {
            return (from i in theEntities.MarketplaceItems
                    where i.Id == anItemId
                    select i.User).FirstOrDefault<User>();
        }

        public MarketplaceItem GetItem(int anItemId) {
            return (from i in theEntities.MarketplaceItems
                    where i.Id == anItemId
                    && !i.Sold
                    && !i.Deleted
                    && i.ExpireListing >= DateTime.Now
                    select i).FirstOrDefault<MarketplaceItem>();            
        }

        public IEnumerable<ItemType> GetItemTypes() {
            return (from t in theEntities.ItemTypes
                    select t);
        }

        public IEnumerable<MarketplaceItem> GetAllLatestItemsSellingForAnyUniversity() {
            return (from i in theEntities.MarketplaceItems
                    where !i.Sold
                    && !i.Deleted
                    && i.ExpireListing >= DateTime.Now
                    select i).OrderByDescending(i2 => i2.DateTimeStamp);
        }

        public IEnumerable<MarketplaceItem> GetAllLatestItemsSellingInUniversity(string aUniversityId) {
            return (from i in theEntities.MarketplaceItems
                    where i.UniversityId == aUniversityId
                    && !i.Sold
                    && !i.Deleted
                    && i.ExpireListing >= DateTime.Now
                    select i).OrderByDescending(i2 => i2.DateTimeStamp);
        }

        public IEnumerable<MarketplaceItem> GetLatestItemsSellingByTypeInUniversity(string aUniversityId, string anItemType) {
            return (from i in theEntities.MarketplaceItems
                    where i.UniversityId == aUniversityId
                    && !i.Sold
                    && !i.Deleted
                    && i.ItemTypeId == anItemType
                    && i.ExpireListing >= DateTime.Now
                    select i).OrderByDescending(i2 => i2.DateTimeStamp);
        }

        public IEnumerable<MarketplaceItem> GetLatestItemsSellingByTypeAndTitleInUniversity(string aUniversityId, string anItemType, string aTitle) {
            return (from i in theEntities.MarketplaceItems
                    where i.UniversityId == aUniversityId
                    && !i.Sold
                    && !i.Deleted
                    && i.ItemTypeId == anItemType
                    && i.Title.Contains(aTitle)
                    && i.ExpireListing >= DateTime.Now
                    select i).OrderByDescending(i2 => i2.DateTimeStamp);
        }

        public IEnumerable<MarketplaceItem> GetLatestItemsSellingByItemAndTitleForAnyUniversity(string anItemType, string aTitle) {
            return (from i in theEntities.MarketplaceItems
                    where !i.Sold
                    && !i.Deleted
                    && i.ItemTypeId == anItemType
                    && i.Title.Contains(aTitle)
                    && i.ExpireListing >= DateTime.Now
                    select i).OrderByDescending(i2 => i2.DateTimeStamp);
        }

        public IEnumerable<MarketplaceItem> GetLatestItemsSellingByTitleForAllUniversitiesAndTypes(string aTitle) {
            return (from i in theEntities.MarketplaceItems
                    where !i.Sold
                    && !i.Deleted
                    && i.Title.Contains(aTitle)
                    && i.ExpireListing >= DateTime.Now
                    select i).OrderByDescending(i2 => i2.DateTimeStamp);
        }

        public void MarkItemAsSold(int anItemId) {
            MarketplaceItem myItemSelling = GetItem(anItemId);
            myItemSelling.Sold = true;
            theEntities.ApplyCurrentValues(myItemSelling.EntityKey.EntitySetName, myItemSelling);
            theEntities.SaveChanges();
        }

        public void UpdateItem(MarketplaceItem anItem) {
            theEntities.ApplyCurrentValues(anItem.EntityKey.EntitySetName, anItem);
            theEntities.SaveChanges();
        }


        public IEnumerable<MarketplaceItem> GetLatestItemsSellingInUniversityByTitle(string aUniversityId, string aSearchString) {
            return (from i in theEntities.MarketplaceItems
                    where i.UniversityId == aUniversityId
                    && !i.Sold
                    && !i.Deleted
                    && i.Title.Contains(aSearchString)
                    && i.ExpireListing >= DateTime.Now
                    select i).OrderByDescending(i2 => i2.DateTimeStamp);
        }

        public MarketplaceItem GetNewestMarketplaceItem() {
            return (from i in theEntities.MarketplaceItems
                    where !i.Sold
                    && !i.Deleted
                    && i.ExpireListing >= DateTime.Now
                    select i).OrderByDescending(i2 => i2.DateTimeStamp).FirstOrDefault<MarketplaceItem>();
        }
    }
}