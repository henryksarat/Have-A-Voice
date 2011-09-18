using System.Linq;
using System.Web.Mvc;
using UniversityOfMe.Helpers;
using UniversityOfMe.Helpers.Functionality;
using Social.Generic.Models;

namespace UniversityOfMe.Models.View.Search {
    public class EventSearchResult : ISearchResult {
        public UserInformationModel<User> UserInformationModel { private get; set; }
        private Event theEvent;
        
        public EventSearchResult(Event anEvent) {
            theEvent = anEvent;
        }

        public string CreateResult() {
            var myEventDiv = new TagBuilder("div");
            myEventDiv.AddCssClass("res-con event clearfix");

            var myActionsDiv = new TagBuilder("div");
            myActionsDiv.AddCssClass("actions");

            int myTotalPostsToEventBoard = theEvent.EventBoards.Count;
            var myPostToEventBoard = new TagBuilder("a");
            myPostToEventBoard.MergeAttribute("href", URLHelper.EventUrl(theEvent));
            myPostToEventBoard.InnerHtml += myTotalPostsToEventBoard + " Post to Event Board";

            int myTotalAttending = theEvent.EventAttendences.Count;
            var myAddFriend = new TagBuilder("a");
            myAddFriend.MergeAttribute("href", URLHelper.EventUrl(theEvent));
            myAddFriend.InnerHtml += myTotalAttending + " Attending";

            var myAttending = new TagBuilder("a");
            var myAttendingText = new TagBuilder("a");
            if (UserInformationModel == null || UserInformationModel.UserId != theEvent.UserId) {
                if (UserInformationModel != null && EventHelper.IsAttending(UserInformationModel.Details, theEvent)) {
                    myAttending.AddCssClass("remove");
                    myAttending.MergeAttribute("href", URLHelper.BuildEventUnAttendUrl(theEvent));
                    myAttendingText.MergeAttribute("href", URLHelper.BuildEventUnAttendUrl(theEvent));
                    myAttendingText.InnerHtml += "I am not attending";
                } else {
                    myAttending.AddCssClass("add");
                    myAttending.MergeAttribute("href", URLHelper.BuildEventAttendUrl(theEvent));
                    myAttendingText.MergeAttribute("href", URLHelper.BuildEventAttendUrl(theEvent));
                    myAttendingText.InnerHtml += "I am attending";
                }
            }

            myActionsDiv.InnerHtml += myPostToEventBoard.ToString();
            myActionsDiv.InnerHtml += "<br />";
            myActionsDiv.InnerHtml += myAddFriend.ToString();
            myActionsDiv.InnerHtml += "<br />";
            myActionsDiv.InnerHtml += myAttending.ToString();
            myActionsDiv.InnerHtml += myAttendingText;

            var myTitleSpan = new TagBuilder("span");
            myTitleSpan.AddCssClass("title");

            var myTitleLinked = new TagBuilder("a");
            myTitleLinked.AddCssClass("itemlinked");
            myTitleLinked.MergeAttribute("href", URLHelper.EventUrl(theEvent));
            myTitleLinked.InnerHtml += theEvent.Title;

            myTitleSpan.InnerHtml += myTitleLinked.ToString();

            myEventDiv.InnerHtml += myActionsDiv.ToString();
            myEventDiv.InnerHtml += myTitleSpan.ToString();
            myEventDiv.InnerHtml += "<br />";
            myEventDiv.InnerHtml += theEvent.University.UniversityName;
            myEventDiv.InnerHtml += "<br />";
            myEventDiv.InnerHtml += TextShortener.Shorten(theEvent.Information, 50);
            myEventDiv.InnerHtml += "<br />";
            myEventDiv.InnerHtml += LocalDateHelper.ToLocalTime(theEvent.StartDate);
            myEventDiv.InnerHtml += "<br />";
            myEventDiv.InnerHtml += LocalDateHelper.ToLocalTime(theEvent.EndDate);

            return myEventDiv.ToString();
        }
    }
}