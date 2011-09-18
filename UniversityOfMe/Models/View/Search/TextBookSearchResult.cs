using System.Linq;
using System.Web.Mvc;
using UniversityOfMe.Helpers;
using UniversityOfMe.Helpers.Format;
using Social.Generic.Models;

namespace UniversityOfMe.Models.View.Search {
    public class TextBookSearchResult : ISearchResult {
        public UserInformationModel<User> UserInformationModel { private get; set; }
        private TextBook theTextBook;

        public TextBookSearchResult(TextBook aTextBook) {
            theTextBook = aTextBook;
        }

        public string CreateResult() {
            var myTextBookDiv = new TagBuilder("div");
            myTextBookDiv.AddCssClass("res-con textbook clearfix");

            var myActionsDiv = new TagBuilder("div");
            myActionsDiv.AddCssClass("actions");

            if (UserInformationModel == null || theTextBook.UserId != UserInformationModel.UserId) {
                var myContactSeller = new TagBuilder("a");
                myContactSeller.AddCssClass("pm");
                myContactSeller.MergeAttribute("href", URLHelper.SendMessageUrl(theTextBook.User, "Regarding Book: " + theTextBook.BookTitle));
                myContactSeller.InnerHtml += "Contact Seller";

                myActionsDiv.InnerHtml += myContactSeller.ToString();
            } else {
                myActionsDiv.InnerHtml += "You are selling this book";
            }

            var myImage = new TagBuilder("img");
            myImage.MergeAttribute("src", PhotoHelper.TextBookPhoto(theTextBook));

            var myBookDescriptionDiv = new TagBuilder("div");
            myBookDescriptionDiv.AddCssClass("flft");

            var myTitleSpan = new TagBuilder("span");
            myTitleSpan.AddCssClass("title");

            var myNameLinked = new TagBuilder("a");
            myNameLinked.AddCssClass("itemlinked");
            myNameLinked.MergeAttribute("href", URLHelper.BuildTextbookUrl(theTextBook));
            myNameLinked.InnerHtml += theTextBook.BookTitle;

            myTitleSpan.InnerHtml += myNameLinked.ToString();

            myBookDescriptionDiv.InnerHtml += myTitleSpan.ToString();
            myBookDescriptionDiv.InnerHtml += "<br />";
            myBookDescriptionDiv.InnerHtml += "Condition: " + theTextBook.TextBookCondition.Display;
            myBookDescriptionDiv.InnerHtml += "<br />";
            myBookDescriptionDiv.InnerHtml += "Associated Class: " + theTextBook.ClassCode;
            myBookDescriptionDiv.InnerHtml += "<br />";
            myBookDescriptionDiv.InnerHtml += "Edition: " + theTextBook.Edition;
            myBookDescriptionDiv.InnerHtml += "Asking Price: " + MoneyFormatHelper.Format(theTextBook.Price);
            myBookDescriptionDiv.InnerHtml += "<br />";
            myBookDescriptionDiv.InnerHtml += TextShortener.Shorten(theTextBook.Details, 50);

            myTextBookDiv.InnerHtml += myActionsDiv.ToString();
            myTextBookDiv.InnerHtml += myImage.ToString();
            myTextBookDiv.InnerHtml += myBookDescriptionDiv.ToString();

            return myTextBookDiv.ToString();
        }
    }
}