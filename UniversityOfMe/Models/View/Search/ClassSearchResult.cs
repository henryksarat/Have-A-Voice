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

            myRatingDiv.InnerHtml += StarHelper.AveragedFiveStarImages(mySumVotes, myTotalVotes);
            myRatingDiv.InnerHtml += "(" + myTotalVotes + " ratings";

            var myDiscussionPosts = new TagBuilder("a");
            myDiscussionPosts.MergeAttribute("href", "#");
            myDiscussionPosts.InnerHtml += myTotalVotes + " Discussion Posts";

            int myTotalEnrolled = theClass.ClassEnrollments.Count;

            var myEnrolled = new TagBuilder("a");
            myEnrolled.MergeAttribute("href", "#");
            myEnrolled.InnerHtml += myTotalEnrolled + " Enrolled";

            myActionsDiv.InnerHtml += myRatingDiv.ToString();
            myActionsDiv.InnerHtml += myDiscussionPosts.ToString();
            myActionsDiv.InnerHtml += "<br />";
            myActionsDiv.InnerHtml += myEnrolled.ToString();

            var myTitleSpan = new TagBuilder("span");
            myTitleSpan.AddCssClass("title");
            myTitleSpan.InnerHtml += theClass.ClassCode;

            var myBreak = new TagBuilder("br");

            myClassDiv.InnerHtml += myActionsDiv.ToString();
            myClassDiv.InnerHtml += myTitleSpan.ToString();
            myClassDiv.InnerHtml += "<br />";
            myClassDiv.InnerHtml += theClass.ClassTitle;
            myClassDiv.InnerHtml += "<br />";
            myClassDiv.InnerHtml += theClass.AcademicTerm.DisplayName + " " + theClass.Year;

            return myClassDiv.ToString();
        }
    }
}