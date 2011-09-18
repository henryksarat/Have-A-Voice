using System.Linq;
using System.Web.Mvc;
using UniversityOfMe.Helpers;
using Social.Generic.Models;

namespace UniversityOfMe.Models.View.Search {
    public class UserSearchResult : ISearchResult {
        public UserInformationModel<User> UserInformationModel { private get; set; }
        private User theUser;

        public UserSearchResult(User aUser) {
            theUser = aUser;
        }

        public string CreateResult() {
            var myUserDiv = new TagBuilder("div");
            myUserDiv.AddCssClass("res-con friend clearfix");

            var myActionsDiv = new TagBuilder("div");
            myActionsDiv.AddCssClass("actions");

            var mySendMessage = new TagBuilder("a");
            mySendMessage.MergeAttribute("href", URLHelper.SendMessageUrl(theUser));
            mySendMessage.AddCssClass("pm");
            mySendMessage.InnerHtml += "Send a Message";

            var mySendBeer = new TagBuilder("a");
            mySendBeer.AddCssClass("beer");
            mySendBeer.MergeAttribute("href", URLHelper.SendItemUrl(theUser, SendItemOptions.BEER));
            mySendBeer.InnerHtml += "Send Beer";

            myActionsDiv.InnerHtml += mySendMessage.ToString();
            myActionsDiv.InnerHtml += "<br />";
            if (UserInformationModel != null && FriendHelper.IsPendingFriendRequest(UserInformationModel.Details, theUser)) {
                myActionsDiv.InnerHtml += "Pending Friend Request";
                myActionsDiv.InnerHtml += "<br />";
            } else if (UserInformationModel == null || !FriendHelper.IsFriend(UserInformationModel.Details, theUser)) {
                var myAddFriend = new TagBuilder("a");
                myAddFriend.AddCssClass("addfriend");
                myAddFriend.MergeAttribute("href", URLHelper.AddFriendUrl(theUser));
                myAddFriend.InnerHtml += "Add as Friend";
                myActionsDiv.InnerHtml += myAddFriend.ToString();
                myActionsDiv.InnerHtml += "<br />";
            } else {
                var myRemoveFriend = new TagBuilder("a");
                myRemoveFriend.AddCssClass("defriend");
                myRemoveFriend.MergeAttribute("href", URLHelper.RemoveFriendUrl(theUser));
                myRemoveFriend.InnerHtml += "Remove as Friend";
                myActionsDiv.InnerHtml += myRemoveFriend.ToString();
                myActionsDiv.InnerHtml += "<br />";
            }
            myActionsDiv.InnerHtml += mySendBeer.ToString();

            var myImageSrc = new TagBuilder("img");
            myImageSrc.MergeAttribute("href", URLHelper.ProfileUrl(theUser));
            myImageSrc.MergeAttribute("src", PhotoHelper.ProfilePicture(theUser));

            var myTitleSpan = new TagBuilder("span");
            myTitleSpan.AddCssClass("title");

            var myNameLinked = new TagBuilder("a");
            myNameLinked.AddCssClass("itemlinked");
            myNameLinked.MergeAttribute("href", URLHelper.ProfileUrl(theUser));
            myNameLinked.InnerHtml += NameHelper.FullName(theUser);

            myTitleSpan.InnerHtml += myNameLinked.ToString();

            myUserDiv.InnerHtml += myActionsDiv.ToString();
            myUserDiv.InnerHtml += myImageSrc.ToString();
            myUserDiv.InnerHtml += myTitleSpan.ToString();
            myUserDiv.InnerHtml += "<br />";
            myUserDiv.InnerHtml += UniversityHelper.GetMainUniversity(theUser).UniversityName;

            return myUserDiv.ToString();
        }
    }
}