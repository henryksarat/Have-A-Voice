using System.Collections.Generic;
using UniversityOfMe.Models.View.Search;

namespace UniversityOfMe.Services.Search {
    public interface ISearchService {
        IEnumerable<ISearchResult> GetSearchResults(string aSearchString);
    }
}