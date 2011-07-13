using System.Linq;
using System.Web.Mvc;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Models.View.Search {
    public class GeneralPostingSearchResult : ISearchResult {
        private GeneralPosting theGeneralPosting;

        public GeneralPostingSearchResult(GeneralPosting aGeneralPosting) {
            theGeneralPosting = aGeneralPosting;
        }

        public string CreateResult() {
            var myGeneralPostingDiv = new TagBuilder("div");
            myGeneralPostingDiv.AddCssClass("res-con post clearfix");

            var myActionsDiv = new TagBuilder("div");
            myActionsDiv.AddCssClass("actions");

            myActionsDiv.InnerHtml += "Date of Last Post: " + LocalDateHelper.ToLocalTime(theGeneralPosting.DateTimeStamp);
            myActionsDiv.InnerHtml += "<br />";
            myActionsDiv.InnerHtml += "Number of Posts: " + theGeneralPosting.GeneralPostingReplies.Count;

            var myTitleSpan = new TagBuilder("span");
            myTitleSpan.AddCssClass("title");

            var myTitleLinked = new TagBuilder("a");
            myTitleLinked.AddCssClass("itemlinked");
            myTitleLinked.MergeAttribute("href", URLHelper.BuildGeneralPostingsUrl(theGeneralPosting));
            myTitleLinked.InnerHtml += theGeneralPosting.Title;

            myTitleSpan.InnerHtml += myTitleLinked.ToString();

            var myAuthorSpan = new TagBuilder("span");
            myAuthorSpan.AddCssClass("tiny");
            myAuthorSpan.InnerHtml += "By " + NameHelper.FullName(theGeneralPosting.User) + " on " + LocalDateHelper.ToLocalTime(theGeneralPosting.DateTimeStamp);

            myGeneralPostingDiv.InnerHtml += myActionsDiv.ToString();
            myGeneralPostingDiv.InnerHtml += myTitleSpan.ToString();
            myGeneralPostingDiv.InnerHtml += myAuthorSpan.ToString();
            myGeneralPostingDiv.InnerHtml += "<br />";
            myGeneralPostingDiv.InnerHtml += TextShortener.Shorten(theGeneralPosting.Body, 50);

            return myGeneralPostingDiv.ToString();
        }
    }
}