using System.Collections.Generic;
using UniversityOfMe.Models.View.Search;
using UniversityOfMe.Helpers.Search;
using Social.Generic.Models;
using UniversityOfMe.Models;

namespace UniversityOfMe.Services.Search {
    public interface ISearchService {
        SearchResultsModel GetAllSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage);
        SearchResultsModel GetClassSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy);
        SearchResultsModel GetEventSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy);
        SearchResultsModel GetGeneralPostingSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy);
        SearchResultsModel GetOrganizationSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy);
        SearchResultsModel GetProfessorSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy);
        SearchResultsModel GetTextBookSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy);
        SearchResultsModel GetUserSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy);
    }
}