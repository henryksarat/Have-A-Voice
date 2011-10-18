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

            var mySellBook = new TagBuilder("a");
            mySellBook.MergeAttribute("href", URLHelper.CreateTextBook(theClass.UniversityId, theClass.Subject, theClass.Course));
            mySellBook.MergeAttribute("class", "itemlinked");
            mySellBook.InnerHtml = "Sell a book for this class";

            var mySearchBook = new TagBuilder("a");
            mySearchBook.MergeAttribute("href", URLHelper.SearchTextbooks(theClass.Subject, theClass.Course));
            mySearchBook.MergeAttribute("class", "itemlinked");
            mySearchBook.InnerHtml = "Search for textbooks tagged to this class";
            
            myClassDiv.InnerHtml += theClass.Subject + theClass.Course + ":" + theClass.Title;
            myClassDiv.InnerHtml += "<br />";
            myClassDiv.InnerHtml += mySellBook.ToString();
            myClassDiv.InnerHtml += "<br />";
            myClassDiv.InnerHtml += mySearchBook.ToString();

            
            return myClassDiv.ToString();
        }


        public System.DateTime GetDateTime() {
            throw new System.NotImplementedException();
        }


        public string CreateFrontPageResult() {
            throw new System.NotImplementedException();
        }
    }
}