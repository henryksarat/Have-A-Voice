using System.Collections.Generic;
using System.Linq;
using Social.Generic.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using System;
using UniversityOfMe.UserInformation;
using Social.Generic.Models;

namespace UniversityOfMe.Helpers.Functionality {
    public static class UserFeed {
        public static IEnumerable<UserFeedModel> GetUserFeed(User aUser, int aLimit) {
            List<UserFeedModel> myFeed = new List<UserFeedModel>();
            string myFullName = NameHelper.FullName(aUser);
            UserInformationModel<User> myUserInfo = UserInformationFactory.GetUserInformation();
            bool myIsFriend = myUserInfo != null && FriendHelper.IsFriend(aUser, myUserInfo.Details);
            
            myFeed.AddRange(from t in aUser.TextBooks
                            where t.Active
                            select new UserFeedModel() {
                                FeedString = TextShortener.Shorten(myFullName, 7) + " is <span class=\"sell\">Selling</span> the book <a class=\"feedlink\" href=\"" + URLHelper.BuildTextbookUrl(t) + "\">" + TextShortener.Shorten(t.BookTitle, 15) + "</a>",
                                CssClass = "book",
                                FeedType = FeedType.Textbook,
                                DateTimeStamp = t.DateTimeStamp
                            });

            return myFeed.OrderByDescending(f => f.DateTimeStamp).Take(aLimit);
        }
    }
}