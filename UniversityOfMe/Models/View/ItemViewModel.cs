using System;
using System.ComponentModel.DataAnnotations;
using UniversityOfMe.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Models.View {
    public class ItemViewModel {
        public int ItemId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UniversityId { get; set; }

        public IEnumerable<SelectListItem> ItemTypes { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Title { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ItemType { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }

        public HttpPostedFileBase Image { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Price { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ImageUrl { get; set; }

        public string ExpireListing { get; set; }

        public ItemViewModel() {
            ExpireListing = DateTime.Today.Date.AddDays(90).ToString("MM-dd-yyyy");
        }

        public ItemViewModel(MarketplaceItem anItemSelling) {
            ItemId = anItemSelling.Id;
            UniversityId = anItemSelling.UniversityId;
            Title = anItemSelling.Title;
            Description = anItemSelling.Description;
            Price = anItemSelling.Price.ToString();
            ImageUrl = PhotoHelper.ItemSellingPhoto(anItemSelling);

            TimeZone myLocalZone = TimeZone.CurrentTimeZone;
            DateTime myStartDateLocal = myLocalZone.ToLocalTime(anItemSelling.ExpireListing);

            ExpireListing = myStartDateLocal.ToString("MM-dd-yyyy");
        }

        public DateTime GetExpireDate() {
            return Convert.ToDateTime(ExpireListing);
        }
    }
}