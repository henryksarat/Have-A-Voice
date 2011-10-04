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
            myFeed.AddRange(from ce in aUser.ClassEnrollments
                            where ce.UserId == aUser.Id
                            && PrivacyHelper.PrivacyAllows(ce.User, SocialPrivacySetting.Display_Class_Enrollment)
                            select new UserFeedModel() {
                                FeedString = TextShortener.Shorten(myFullName, 7) + " is attending <a class=\"feedlink\" href=\"" + URLHelper.BuildClassDiscussionUrl(ce.Class) + "\">" + ce.Class.Subject + ce.Class.Course + "</a>",
                                CssClass = "class",
                                FeedType = FeedType.ClassEnrollment,
                                DateTimeStamp = ce.DateTimeStamp
                            });

            myFeed.AddRange(from e in aUser.Events
                            where e.UserId == aUser.Id
                            && e.EndDate >= DateTime.UtcNow
                            && (e.EntireSchool || myIsFriend)
                            && e.EditedDateTimeStamp == null
                            select new UserFeedModel() {
                                FeedString = TextShortener.Shorten(myFullName, 7) + " created the event <a class=\"feedlink\" href=\"" + URLHelper.EventUrl(e) + "\">" + TextShortener.Shorten(e.Title, 15) + "</a>",
                                CssClass = "event",
                                FeedType = FeedType.Event,
                                DateTimeStamp = e.CreatedDateTimeStamp
                            });

            myFeed.AddRange(from e in aUser.Events
                            where e.UserId == aUser.Id
                            && e.EndDate >= DateTime.UtcNow
                            && (e.EntireSchool || myIsFriend)
                            && e.EditedDateTimeStamp != null
                            select new UserFeedModel() {
                                FeedString = TextShortener.Shorten(myFullName, 7) + " updated the event <a class=\"feedlink\" href=\"" + URLHelper.EventUrl(e) + "\">" + TextShortener.Shorten(e.Title, 15) + "</a>",
                                CssClass = "event",
                                FeedType = FeedType.Event,
                                DateTimeStamp = (DateTime)e.EditedDateTimeStamp
                            });
            myFeed.AddRange(from e in aUser.EventAttendences
                            where e.UserId == aUser.Id
                            && e.Event.EndDate >= DateTime.UtcNow
                            select new UserFeedModel() {
                                FeedString = TextShortener.Shorten(myFullName, 7) + " is attending the event <a class=\"feedlink\" href=\"" + URLHelper.EventUrl(e.Event) + "\">" + TextShortener.Shorten(e.Event.Title, 15) + "</a>",
                                CssClass = "event",
                                FeedType = FeedType.Event,
                                DateTimeStamp = (DateTime)e.Event.StartDate
                            });
            myFeed.AddRange(from cm in aUser.ClubMembers
                            where cm.ClubMemberUserId == aUser.Id
                            && !cm.Deleted
                            && cm.Approved == UOMConstants.APPROVED
                            select new UserFeedModel() {
                                FeedString = TextShortener.Shorten(myFullName, 7) + " is in the organization <a class=\"feedlink\" href=\"" + URLHelper.BuildOrganizationUrl(cm.Club) + "\">" + TextShortener.Shorten(cm.Club.Name, 15) + "</a>",
                                CssClass = "organization",
                                FeedType = FeedType.OrganizationEnrollment,
                                DateTimeStamp = cm.ApprovedDateTimeStamp != null ? (DateTime)cm.ApprovedDateTimeStamp : (DateTime)cm.DateTimeStamp
                            });

            return myFeed.OrderByDescending(f => f.DateTimeStamp).Take(aLimit);
        }
    }
}