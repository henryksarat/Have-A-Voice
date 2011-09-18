using System.Linq;
using System.Web.Mvc;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Models.View.Search {
    public class ClassSearchResult : ISearchResult {
        private Class theClass;

        public ClassSearchResult(Class aClass) {
            theClass = aClass;
        }
        public string CreateResult() {
            var myClassDiv = new TagBuilder("div");
            myClassDiv.AddCssClass("res-con class clearfix");

            var myActionsDiv = new TagBuilder("div");
            myActionsDiv.AddCssClass("actions");

            var myRatingDiv = new TagBuilder("div");
            myRatingDiv.AddCssClass("rating");

            int mySumVotes = theClass.ClassReviews.Sum(r => r.Rating);
            int myTotalVotes = theClass.ClassReviews.Count;

            if (myTotalVotes != 0) {
                myRatingDiv.InnerHtml += StarHelper.AveragedFiveStarImages(mySumVotes, myTotalVotes);
                if (myTotalVotes == 1) {
                    myRatingDiv.InnerHtml += "(" + myTotalVotes + " rating)";
                } else {
                    myRatingDiv.InnerHtml += "(" + myTotalVotes + " ratings)";
                }
            } else {
                myRatingDiv.InnerHtml += "no reviews yet";
            }

            int myDiscussionPostsCount = theClass.ClassBoards.Where(b => !b.Deleted).Count<ClassBoard>();

            var myDiscussionPosts = new TagBuilder("a");
            myDiscussionPosts.MergeAttribute("href", URLHelper.BuildClassDiscussionUrl(theClass));
            myDiscussionPosts.InnerHtml += 
                myDiscussionPostsCount == 1 ? myDiscussionPostsCount + " Discussion Post" : myDiscussionPostsCount + " Discussion Posts";
            
            int myTotalEnrolled = theClass.ClassEnrollments.Count<ClassEnrollment>(e => PrivacyHelper.PrivacyAllows(e.User, Social.Generic.Helpers.SocialPrivacySetting.Display_Class_Enrollment)) ;

            var myEnrolled = new TagBuilder("a");
            myEnrolled.MergeAttribute("href", URLHelper.BuildClassDiscussionUrl(theClass));
            myEnrolled.InnerHtml += myTotalEnrolled + " Enrolled";

            myActionsDiv.InnerHtml += myRatingDiv.ToString();
            myActionsDiv.InnerHtml += myDiscussionPosts.ToString();
            myActionsDiv.InnerHtml += "<br />";
            myActionsDiv.InnerHtml += myEnrolled.ToString();

            var myTitleSpan = new TagBuilder("span");
            myTitleSpan.AddCssClass("title");

            var myNameLinked = new TagBuilder("a");
            myNameLinked.AddCssClass("itemlinked");
            myNameLinked.MergeAttribute("href", URLHelper.BuildClassDiscussionUrl(theClass));
            myNameLinked.InnerHtml += theClass.ClassCode;

            myTitleSpan.InnerHtml += myNameLinked.ToString();

            var myBreak = new TagBuilder("br");

            myClassDiv.InnerHtml += myActionsDiv.ToString();
            myClassDiv.InnerHtml += myTitleSpan.ToString();
            myClassDiv.InnerHtml += "<br />";
            myClassDiv.InnerHtml += theClass.ClassCode;
            myClassDiv.InnerHtml += "<br />";
            myClassDiv.InnerHtml += theClass.AcademicTerm.DisplayName + " " + theClass.Year;

            return myClassDiv.ToString();
        }
    }
}