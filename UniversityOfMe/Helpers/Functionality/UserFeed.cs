using System.Collections.Generic;
using System.Linq;
using Social.Generic.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Helpers.Functionality {
    public static class UserFeed {
        public static IEnumerable<UserFeedModel> GetUserFeed(User aUser, int aLimit) {
            List<UserFeedModel> myFeed = new List<UserFeedModel>();
            string myFullName = NameHelper.FullName(aUser);

            myFeed.AddRange(from t in aUser.TextBooks
                            where t.Active
                            select new UserFeedModel() {
                                FeedString = myFullName + " is " + (t.BuySell.Equals("S") ? "<span class=\"sell\">Selling</span> " : " looking to <span class=\"buy\">Buy</span> ") + " \"" + t.BookTitle + "\" book",
                                CssClass = "book",
                                FeedType = FeedType.Textbook,
                                DateTimeStamp = t.DateTimeStamp
                            });
            myFeed.AddRange(from ce in aUser.ClassEnrollments
                            where ce.UserId == aUser.Id
                            && PrivacyHelper.PrivacyAllows(ce.User, SocialPrivacySetting.Display_Class_Enrollment)
                            select new UserFeedModel() {
                                FeedString = myFullName + " is attending \"" + ce.Class.ClassCode + "\"",
                                CssClass = "class",
                                FeedType = FeedType.ClassEnrollment,
                                DateTimeStamp = ce.DateTimeStamp
                            });
            return myFeed.OrderByDescending(f => f.DateTimeStamp).Take(aLimit);
        }
    }
}