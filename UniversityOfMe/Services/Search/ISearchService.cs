using Social.Generic.Models;
using UniversityOfMe.Helpers.Search;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View.Search;

namespace UniversityOfMe.Services.Search {
    public interface ISearchService {
        SearchResultsModel GetAllSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage);
        SearchResultsModel GetAllSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, string aUniversityId);

        SearchResultsModel GetClassSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy);
        SearchResultsModel GetClassSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy, string aUniversityId);

        SearchResultsModel GetTextBookSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy);
        SearchResultsModel GetTextBookSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy, string aUniversityId);

        SearchResultsModel MarketplaceSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy, string anItemType);
        SearchResultsModel MarketplaceSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy, string anItemType, string aUniversityId);
        
        SearchResultsModel GetUserSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy);
        SearchResultsModel GetUserSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy, string aUniversityId);
    }
}