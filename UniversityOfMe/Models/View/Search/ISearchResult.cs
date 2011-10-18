using System;
namespace UniversityOfMe.Models.View.Search {
    public interface ISearchResult {
        string CreateResult();
        string CreateFrontPageResult();
        DateTime GetDateTime();
    }
}