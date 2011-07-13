using System.Linq;
using System.Web.Mvc;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Models.View.Search {
    public class ProfessorSearchResult : ISearchResult {
        private Professor theProfessor;

        public ProfessorSearchResult(Professor aProfessor) {
            theProfessor = aProfessor;
        }
        public string CreateResult() {
            var myProfessorDiv = new TagBuilder("div");
            myProfessorDiv.AddCssClass("res-con professor clearfix");

            var myActionsDiv = new TagBuilder("div");
            myActionsDiv.AddCssClass("actions");

            var myRatingDiv = new TagBuilder("div");
            myRatingDiv.AddCssClass("rating");

            int mySumVotes = theProfessor.ProfessorReviews.Sum(r => r.Rating);
            int myTotalVotes = theProfessor.ProfessorReviews.Count;

            if (myTotalVotes != 0) {
                myRatingDiv.InnerHtml += StarHelper.AveragedFiveStarImages(mySumVotes, myTotalVotes);
                myRatingDiv.InnerHtml += "(" + myTotalVotes + " ratings )";
            } else {
                myRatingDiv.InnerHtml += "no reviews yet";
            }

            myActionsDiv.InnerHtml += myRatingDiv.ToString();

            var myTitleSpan = new TagBuilder("span");
            myTitleSpan.AddCssClass("title");

            var myNameLinked = new TagBuilder("a");
            myNameLinked.AddCssClass("itemlinked");
            myNameLinked.MergeAttribute("href", URLHelper.BuildProfessorUrl(theProfessor));
            myNameLinked.InnerHtml += "Prof. " + theProfessor.FirstName + " " + theProfessor.LastName;

            myTitleSpan.InnerHtml += myNameLinked.ToString();
            
            myProfessorDiv.InnerHtml += myActionsDiv.ToString();
            myProfessorDiv.InnerHtml += myTitleSpan.ToString();

            return myProfessorDiv.ToString();
        }
    }
}