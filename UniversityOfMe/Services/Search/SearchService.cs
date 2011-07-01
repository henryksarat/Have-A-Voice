using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Professors;
using UniversityOfMe.Repositories.Notifications;
using System.Collections.Generic;
using UniversityOfMe.Models.View;
using System;
using UniversityOfMe.Helpers;
using System.Linq;
using UniversityOfMe.Repositories.Classes;
using UniversityOfMe.Repositories.Search;
using UniversityOfMe.Models.View.Search;

namespace UniversityOfMe.Services.Search {
    public class SearchService : ISearchService {
        private ISearchRepository theSearchRepository;

        public SearchService()
            : this(new EntitySearchRepository()) { }

        public SearchService(ISearchRepository aSearchRepo) {
            theSearchRepository = aSearchRepo;
        }

        public IEnumerable<ISearchResult> GetSearchResults(string aSearchString) {
            IEnumerable<Class> myClassResult = theSearchRepository.GetClassByTitle(aSearchString);
            List<ISearchResult> mySearchResult = new List<ISearchResult>();

            mySearchResult.AddRange(myClassResult.Select(r => new ClassSearchResult(r)));

            return mySearchResult;
        }
    }
}
