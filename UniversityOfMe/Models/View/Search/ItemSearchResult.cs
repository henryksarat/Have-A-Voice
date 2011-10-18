using System.Linq;
using System.Web.Mvc;
using UniversityOfMe.Helpers;
using UniversityOfMe.Helpers.Format;
using Social.Generic.Models;

namespace UniversityOfMe.Models.View.Search {
    public class ItemSearchResult : ISearchResult {
        public UserInformationModel<User> UserInformationModel { private get; set; }
        private MarketplaceItem theItem;

        public ItemSearchResult(MarketplaceItem anItem) {
            theItem = anItem;
        }

        public string CreateResult() {
            var myItemDiv = new TagBuilder("div");
            myItemDiv.AddCssClass("res-con " + theItem.ItemTypeId.ToLower() + " clearfix");

            var myActionsDiv = new TagBuilder("div");
            myActionsDiv.AddCssClass("actions");

            if (UserInformationModel == null || theItem.UserId != UserInformationModel.UserId) {
                var myContactSeller = new TagBuilder("a");
                myContactSeller.AddCssClass("pm");
                myContactSeller.MergeAttribute("href", URLHelper.SendMessageUrl(theItem.User, "Regarding " + theItem.ItemTypeId + ": " + theItem.Title));
                myContactSeller.InnerHtml += "Contact Seller";

                myActionsDiv.InnerHtml += myContactSeller.ToString();
            } else {
                myActionsDiv.InnerHtml += "You are selling this item";
            }

            var myImage = new TagBuilder("img");
            myImage.MergeAttribute("src", PhotoHelper.ItemSellingPhoto(theItem));

            var myItemDescriptionDiv = new TagBuilder("div");
            myItemDescriptionDiv.AddCssClass("flft");

            var myTitleSpan = new TagBuilder("span");
            myTitleSpan.AddCssClass("title");

            var myNameLinked = new TagBuilder("a");
            myNameLinked.AddCssClass("itemlinked");
            myNameLinked.MergeAttribute("href", URLHelper.ItemDetails(theItem));
            myNameLinked.InnerHtml += theItem.Title;

            myTitleSpan.InnerHtml += myNameLinked.ToString();

            myItemDescriptionDiv.InnerHtml += myTitleSpan.ToString();
            myItemDescriptionDiv.InnerHtml += "<br />";
            myItemDescriptionDiv.InnerHtml += "Type: " + theItem.ItemTypeId;
            myItemDescriptionDiv.InnerHtml += "<br />";
            myItemDescriptionDiv.InnerHtml += "Asking Price: " + MoneyFormatHelper.Format(theItem.Price);
            myItemDescriptionDiv.InnerHtml += "<br />";
            myItemDescriptionDiv.InnerHtml += TextShortener.Shorten(theItem.Description, 50);

            myItemDiv.InnerHtml += myActionsDiv.ToString();
            myItemDiv.InnerHtml += myImage.ToString();
            myItemDiv.InnerHtml += myItemDescriptionDiv.ToString();

            return myItemDiv.ToString();
        }


        public System.DateTime GetDateTime() {
            return theItem.DateTimeStamp;
        }


        public string CreateFrontPageResult() {
            var myItemDiv = new TagBuilder("div");
            myItemDiv.AddCssClass("res-con " + theItem.ItemTypeId.ToLower() + " clearfix");

            var myImage = new TagBuilder("img");
            myImage.MergeAttribute("src", PhotoHelper.ItemSellingPhoto(theItem));

            var myItemDescriptionDiv = new TagBuilder("div");
            myItemDescriptionDiv.AddCssClass("flft");

            var myTitleSpan = new TagBuilder("span");
            myTitleSpan.AddCssClass("title");

            var myNameLinked = new TagBuilder("a");
            myNameLinked.AddCssClass("itemlinked");
            myNameLinked.MergeAttribute("href", URLHelper.ItemDetails(theItem));
            myNameLinked.InnerHtml += theItem.Title;

            myTitleSpan.InnerHtml += myNameLinked.ToString();

            myItemDescriptionDiv.InnerHtml += myTitleSpan.ToString();
            myItemDescriptionDiv.InnerHtml += "<br />";
            myItemDescriptionDiv.InnerHtml += "Type: " + theItem.ItemTypeId;
            myItemDescriptionDiv.InnerHtml += "<br />";
            myItemDescriptionDiv.InnerHtml += "Asking Price: " + MoneyFormatHelper.Format(theItem.Price);
            myItemDescriptionDiv.InnerHtml += "<br />";
            myItemDescriptionDiv.InnerHtml += TextShortener.Shorten(theItem.Description, 50);

            myItemDiv.InnerHtml += myImage.ToString();
            myItemDiv.InnerHtml += myItemDescriptionDiv.ToString();

            return myItemDiv.ToString();
        }
    }
}