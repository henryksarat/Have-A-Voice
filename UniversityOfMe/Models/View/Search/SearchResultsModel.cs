using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Helpers;
using UniversityOfMe.Helpers.Search;
using System.Web.Mvc;

namespace UniversityOfMe.Models.View.Search {
    public class SearchResultsModel {
        public IEnumerable<ISearchResult> SearchResults { get; set; }
        public int TotalResults { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public SearchFilter SearchType { get; set; }
        public string SearchString { get; set; }
        public IEnumerable<SelectListItem> SearchByOptions { get; set; }
        public IEnumerable<SelectListItem> OrderByOptions { get; set; }
        public IEnumerable<SelectListItem> UniversityOptions { get; set; }
    }
}