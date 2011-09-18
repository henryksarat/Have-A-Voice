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

            var myDescriptionDiv = new TagBuilder("div");

            var myTitleSpan = new TagBuilder("div");
            myTitleSpan.MergeAttribute("style", "font-weight:bold");

            var myNameLinked = new TagBuilder("a");
            myNameLinked.AddCssClass("itemlinked");
            myNameLinked.MergeAttribute("href", URLHelper.BuildProfessorUrl(theProfessor));
            myNameLinked.InnerHtml += "Professor " + theProfessor.FirstName + " " + theProfessor.LastName;

            myTitleSpan.InnerHtml += myNameLinked.ToString();

            var myUnviersityDiv = new TagBuilder("div");
            myUnviersityDiv.InnerHtml = theProfessor.University.UniversityName;

            myDescriptionDiv.InnerHtml += myTitleSpan.ToString();
            myDescriptionDiv.InnerHtml += myUnviersityDiv.ToString();

            myProfessorDiv.InnerHtml += myActionsDiv.ToString();
            myProfessorDiv.InnerHtml += myDescriptionDiv;

            return myProfessorDiv.ToString();
        }
    }
}