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
using UniversityOfMe.Helpers.Constants;
using Social.Generic.Helpers;
using UniversityOfMe.Helpers.Search;
using System.Web.Mvc;
using UniversityOfMe.Helpers.Configuration;
using UniversityOfMe.Services.Marketplace;

namespace UniversityOfMe.Services.Search {
    public class SearchService : ISearchService {
        private static DateTime OLD_DATE = new DateTime(1987, 05, 03);

        private ISearchRepository theSearchRepository;
        private IMarketplaceService theMarketplaceSerivce;

        public SearchService()
            : this(new EntitySearchRepository(), new MarketplaceService(null)) { }

        public SearchService(ISearchRepository aSearchRepo, IMarketplaceService aMarketplaceServce) {
            theSearchRepository = aSearchRepo;
            theMarketplaceSerivce = aMarketplaceServce;
        }

        public SearchResultsModel GetAllSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage) {
            IEnumerable<TextBook> myTextBooks = new List<TextBook>();
            IEnumerable<MarketplaceItem> myItems = new List<MarketplaceItem>();

            if (aUserInformation != null) {
                string myUniversityId = UniversityHelper.GetMainUniversity(aUserInformation.Details).Id;
                myTextBooks = theSearchRepository.GetTextBookByTitle(myUniversityId, aSearchString);
                myItems = theMarketplaceSerivce.GetLatestItemsSellingInUniversityByTitle(myUniversityId, aSearchString);
            } else {
                myTextBooks = theSearchRepository.GetTextBookByTitle(aSearchString);
                myItems = theMarketplaceSerivce.GetLatestItemsSellingByTitleForAllUniversitiesAndTypes(aSearchString);
            }

            List<ISearchResult> mySearchResult = new List<ISearchResult>();

            mySearchResult.AddRange(myTextBooks.Select(r => new TextBookSearchResult(r) {
                UserInformationModel = aUserInformation
            }));
            mySearchResult.AddRange(myItems.Select(r => new ItemSearchResult(r) {
                UserInformationModel = aUserInformation
            }));

            mySearchResult = mySearchResult.OrderByDescending(i => i.GetDateTime()).ToList();

            int myTotalResults = myItems.Count<MarketplaceItem>() + myTextBooks.Count<TextBook>();
            SearchResultsModel myModel = 
                BuildSearchResultsModel(mySearchResult, SearchFilter.ALL, aPage, 
                                        aSearchString, myTotalResults, SearchByForAll(), 
                                        SearchBy.None, OrderByForAll(), OrderBy.None);
            return myModel;
        }


        public SearchResultsModel GetClassSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, 
                                                        int aPage, SearchBy aSearchBy, OrderBy anOrderBy) {
            IEnumerable<Class> myClasses = new List<Class>();

            if (aUserInformation != null) {
                string myUniversityId = UniversityHelper.GetMainUniversity(aUserInformation.Details).Id;
                if (aSearchBy == SearchBy.Title) {
                    myClasses = theSearchRepository.GetClassByTitle(myUniversityId, aSearchString);
                } else if (aSearchBy == SearchBy.ClassCode) {
                    myClasses = theSearchRepository.GetClassByClassCode(myUniversityId, aSearchString);
                }
            } else {
                if (aSearchBy == SearchBy.Title) {
                    myClasses = theSearchRepository.GetClassByTitle(aSearchString);
                } else if (aSearchBy == SearchBy.ClassCode) {
                    myClasses = theSearchRepository.GetClassByClassCode(aSearchString);
                }
            }

            if (anOrderBy == OrderBy.Title) {
                myClasses = myClasses.OrderBy(r => r.Title);
            } else if (anOrderBy == OrderBy.ClassCode) {
                myClasses = myClasses.OrderBy(r => r.Subject + r.Course + r.Section);
            } else if (anOrderBy == OrderBy.LatestPost) {
                myClasses = myClasses.OrderByDescending(
                    r => r.ClassBoards
                        .Where(r2=> !r2.Deleted)
                        .OrderByDescending(r2 => r2.DateTimeStamp)
                        .FirstOrDefault<ClassBoard>() != null ? 
                        r.ClassBoards
                        .Where(r2=> !r2.Deleted)
                        .OrderByDescending(r2 => r2.DateTimeStamp)
                        .FirstOrDefault<ClassBoard>().DateTimeStamp :
                        OLD_DATE
                    );
            }

            List<ISearchResult> mySearchResult = new List<ISearchResult>();

            mySearchResult.AddRange(myClasses.Select(r => new ClassSearchResult(r)));

            int myTotalResults = myClasses.Count<Class>();

            SearchResultsModel myModel = 
                BuildSearchResultsModel(mySearchResult, SearchFilter.CLASS, aPage, aSearchString, 
                                        myTotalResults, SearchByForClass(), aSearchBy,
                                        OrderByForClass(), anOrderBy);
            return myModel;
        }

        public SearchResultsModel MarketplaceSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage, SearchBy aSearchBy, OrderBy anOrderBy, string anItemType) {
            IEnumerable<MarketplaceItem> myItems = new List<MarketplaceItem>();
            if (aUserInformation != null) {
                string myUniversityId = UniversityHelper.GetMainUniversity(aUserInformation.Details).Id;
                if (aSearchBy == SearchBy.Title) {
                    myItems = theMarketplaceSerivce.GetLatestItemsSellingInUniversityByItemAndTitle(myUniversityId, anItemType, aSearchString);
                }
            } else {
                if (aSearchBy == SearchBy.Title) {
                    myItems = theMarketplaceSerivce.GetLatestItemsSellingByTypeAndTitleForAnyUniversity(anItemType, aSearchString);
                }
            }

            if (anOrderBy == OrderBy.Title) {
                myItems = myItems.OrderBy(r => r.Title);
            } else if (anOrderBy == OrderBy.LowestPrice) {
                myItems = myItems.OrderBy(r => r.Price);
            } else if (anOrderBy == OrderBy.HighestPrice) {
                myItems = myItems.OrderByDescending(r => r.Price);
            } else if (anOrderBy == OrderBy.Newest) {
                myItems = myItems.OrderBy(r => r.DateTimeStamp);
            }

            List<ISearchResult> mySearchResult = new List<ISearchResult>();

            mySearchResult.AddRange(myItems.Select(r => new ItemSearchResult(r) {
                UserInformationModel = aUserInformation
            }));

            int myTotalResults = myItems.Count();

            SearchResultsModel myModel =
                BuildSearchResultsModel(mySearchResult, (SearchFilter)Enum.Parse(typeof(SearchFilter), anItemType), aPage,
                                        aSearchString, myTotalResults, SearchByForItems(),
                                        aSearchBy, OrderByForItems(), anOrderBy);
            return myModel;
        }

        public SearchResultsModel GetTextBookSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, 
                                                           int aPage, SearchBy aSearchBy, OrderBy anOrderBy) {
            IEnumerable<TextBook> myTextBooks = new List<TextBook>();
            if (aUserInformation != null) {
                string myUniversityId = UniversityHelper.GetMainUniversity(aUserInformation.Details).Id;
                if (aSearchBy == SearchBy.Title) {
                    myTextBooks = theSearchRepository.GetTextBookByTitle(myUniversityId, aSearchString);
                } else if (aSearchBy == SearchBy.ClassCode) {
                    myTextBooks = theSearchRepository.GetTextBookByClassCode(myUniversityId, aSearchString);
                }
            } else {
                if (aSearchBy == SearchBy.Title) {
                    myTextBooks = theSearchRepository.GetTextBookByTitle(aSearchString);
                } else if (aSearchBy == SearchBy.ClassCode) {
                    myTextBooks = theSearchRepository.GetTextBookByClassCode(aSearchString);
                }
            }

            if (anOrderBy == OrderBy.Title) {
                myTextBooks = myTextBooks.OrderBy(r => r.BookTitle);
            } else if (anOrderBy == OrderBy.ClassCode) {
                myTextBooks = myTextBooks.OrderBy(r => r.ClassSubject + r.ClassCourse);
            } else if (anOrderBy == OrderBy.LowestPrice) {
                myTextBooks = myTextBooks.OrderBy(r => r.Price);
            } else if (anOrderBy == OrderBy.HighestPrice) {
                myTextBooks = myTextBooks.OrderByDescending(r => r.Price);
            }

            List<ISearchResult> mySearchResult = new List<ISearchResult>();

            mySearchResult.AddRange(myTextBooks.Select(r => new TextBookSearchResult(r) {
                UserInformationModel = aUserInformation
            }));

            int myTotalResults = myTextBooks.Count<TextBook>();

            SearchResultsModel myModel = 
                BuildSearchResultsModel(mySearchResult, SearchFilter.TEXTBOOK, aPage, 
                                        aSearchString, myTotalResults, SearchByForTextbook(), 
                                        aSearchBy, OrderByForTextbook(), anOrderBy);
            return myModel;
        }

        public SearchResultsModel GetUserSearchResults(UserInformationModel<User> aUserInformation, string aSearchString,
                                                 int aPage, SearchBy aSearchBy, OrderBy anOrderBy) {
            IEnumerable<User> myUsers = new List<User>();

            if (aSearchBy == SearchBy.Name) {
                if (aUserInformation == null) {
                    myUsers = theSearchRepository.GetUserByName(aSearchString);
                } else {
                    myUsers = theSearchRepository.GetUserByName(aUserInformation.UserId, aSearchString);
                }
            }

            if (anOrderBy == OrderBy.Name) {
                myUsers = myUsers.OrderBy(r => r.FirstName + " " + r.LastName);
            }

            List<ISearchResult> mySearchResult = new List<ISearchResult>();

            mySearchResult.AddRange(myUsers.Select(r => new UserSearchResult(r) {
                UserInformationModel = aUserInformation
            }));

            int myTotalResults = myUsers.Count<User>();

            SearchResultsModel myModel =
                BuildSearchResultsModel(mySearchResult, SearchFilter.USER, aPage,
                                        aSearchString, myTotalResults, SearchByForPeople(),
                                        aSearchBy, OrderByForPeople(), anOrderBy);
            return myModel;
        }

        private SearchResultsModel BuildSearchResultsModel(List<ISearchResult> aResults, Helpers.Search.SearchFilter aSearchType,
                        int aPage, string aSearchString, int aTotalResults, IDictionary<string, string> aSearchByOptions,
                        SearchBy aSearchBySelected, IDictionary<string, string> anOrderByOptions, OrderBy anOrderBySelected) {

                            int myToGet = (aPage - 1) * SiteConfiguration.ResultsPerPage() + SiteConfiguration.ResultsPerPage() > aResults.Count
                                ? aTotalResults % SiteConfiguration.ResultsPerPage()
                                : SiteConfiguration.ResultsPerPage();
                            IEnumerable<ISearchResult> myFinalResultSet = aResults.GetRange((aPage - 1) * SiteConfiguration.ResultsPerPage(), myToGet);
            return new SearchResultsModel() {
                SearchType = aSearchType,
                SearchResults = myFinalResultSet,
                CurrentPage = aPage,
                TotalPages = (int)Math.Ceiling((double)aTotalResults / (double)SiteConfiguration.ResultsPerPage()),
                TotalResults = aTotalResults,
                SearchString = aSearchString,
                SearchByOptions = new SelectList(aSearchByOptions, "Value", "Key", aSearchBySelected),
                OrderByOptions = new SelectList(anOrderByOptions, "Value", "Key", anOrderBySelected)
            };
        }

        private IDictionary<string, string> SearchByForAll() {
            IDictionary<string, string> mySearchByOptionsDictionary = new Dictionary<string, string>();
            return mySearchByOptionsDictionary;
        }

        private IDictionary<string, string> OrderByForAll() {
            IDictionary<string, string> myOrderByOptionsDictionary = new Dictionary<string, string>();
            return myOrderByOptionsDictionary;
        }

        private IDictionary<string, string> SearchByForClass() {
            IDictionary<string, string> mySearchByOptionsDictionary = new Dictionary<string, string>();
            mySearchByOptionsDictionary.Add(SearchBy.Title.ToString(), SearchBy.Title.ToString());            
            mySearchByOptionsDictionary.Add("Class Code", SearchBy.ClassCode.ToString());
            return mySearchByOptionsDictionary;
        }

        private IDictionary<string, string> OrderByForClass() {
            IDictionary<string, string> myOrderByOptionsDictionary = new Dictionary<string, string>();
            myOrderByOptionsDictionary.Add(OrderBy.Title.ToString(), OrderBy.Title.ToString());
            myOrderByOptionsDictionary.Add("Class Code", OrderBy.ClassCode.ToString());
            myOrderByOptionsDictionary.Add("Latest Discussion Post", OrderBy.LatestPost.ToString());
            return myOrderByOptionsDictionary;
        }

        private IDictionary<string, string> SearchByForEvent() {
            IDictionary<string, string> mySearchByOptionsDictionary = new Dictionary<string, string>();
            mySearchByOptionsDictionary.Add(SearchBy.Title.ToString(), SearchBy.Title.ToString());
            mySearchByOptionsDictionary.Add(SearchBy.Description.ToString(), SearchBy.Description.ToString());
            return mySearchByOptionsDictionary;
        }

        private IDictionary<string, string> OrderByForEvent() {
            IDictionary<string, string> myOrderByOptionsDictionary = new Dictionary<string, string>();
            myOrderByOptionsDictionary.Add("Closest Start Date", OrderBy.ClosestStartDate.ToString());
            myOrderByOptionsDictionary.Add(OrderBy.Title.ToString(), OrderBy.Title.ToString());
            myOrderByOptionsDictionary.Add("Latest Discussion Post", OrderBy.LatestPost.ToString());
            myOrderByOptionsDictionary.Add("Higest Attending Members", OrderBy.HighestAttendingMembers.ToString());
            myOrderByOptionsDictionary.Add("Lowest Attending Members", OrderBy.LowestAttendingMembers.ToString());
            return myOrderByOptionsDictionary;
        }

        private IDictionary<string, string> SearchByForGeneralPosting() {
            IDictionary<string, string> mySearchByOptionsDictionary = new Dictionary<string, string>();
            mySearchByOptionsDictionary.Add(SearchBy.Title.ToString(), SearchBy.Title.ToString());
            mySearchByOptionsDictionary.Add(SearchBy.Body.ToString(), SearchBy.Body.ToString());
            return mySearchByOptionsDictionary;
        }

        private IDictionary<string, string> OrderByForGeneralPosting() {
            IDictionary<string, string> myOrderByOptionsDictionary = new Dictionary<string, string>();
            myOrderByOptionsDictionary.Add("Latest Post", OrderBy.LatestPost.ToString());
            myOrderByOptionsDictionary.Add(OrderBy.Title.ToString(), OrderBy.Title.ToString());
            return myOrderByOptionsDictionary;
        }

        private IDictionary<string, string> SearchByForPeople() {
            IDictionary<string, string> mySearchByOptionsDictionary = new Dictionary<string, string>();
            mySearchByOptionsDictionary.Add(SearchBy.Name.ToString(), SearchBy.Name.ToString());
            return mySearchByOptionsDictionary;
        }

        private IDictionary<string, string> OrderByForPeople() {
            IDictionary<string, string> myOrderByOptionsDictionary = new Dictionary<string, string>();
            myOrderByOptionsDictionary.Add(OrderBy.Name.ToString(), OrderBy.Name.ToString());
            return myOrderByOptionsDictionary;
        }

        private IDictionary<string, string> SearchByForOrganization() {
            IDictionary<string, string> mySearchByOptionsDictionary = new Dictionary<string, string>();
            mySearchByOptionsDictionary.Add(SearchBy.Name.ToString(), SearchBy.Name.ToString());
            return mySearchByOptionsDictionary;
        }

        private IDictionary<string, string> OrderByForOrganization() {
            IDictionary<string, string> myOrderByOptionsDictionary = new Dictionary<string, string>();
            myOrderByOptionsDictionary.Add(OrderBy.Name.ToString(), OrderBy.Name.ToString());
            return myOrderByOptionsDictionary;
        }

        private IDictionary<string, string> SearchByForProfessor() {
            IDictionary<string, string> mySearchByOptionsDictionary = new Dictionary<string, string>();
            mySearchByOptionsDictionary.Add(SearchBy.Name.ToString(), SearchBy.Name.ToString());
            return mySearchByOptionsDictionary;
        }

        private IDictionary<string, string> OrderByForProfessor() {
            IDictionary<string, string> myOrderByOptionsDictionary = new Dictionary<string, string>();
            myOrderByOptionsDictionary.Add(OrderBy.Name.ToString(), OrderBy.Name.ToString());
            return myOrderByOptionsDictionary;
        }

        private IDictionary<string, string> SearchByForTextbook() {
            IDictionary<string, string> mySearchByOptionsDictionary = new Dictionary<string, string>();
            mySearchByOptionsDictionary.Add(SearchBy.Title.ToString(), SearchBy.Title.ToString());
            mySearchByOptionsDictionary.Add(SearchBy.ClassCode.ToString(), SearchBy.ClassCode.ToString());
            return mySearchByOptionsDictionary;
        }

        private IDictionary<string, string> OrderByForTextbook() {
            IDictionary<string, string> myOrderByOptionsDictionary = new Dictionary<string, string>();
            myOrderByOptionsDictionary.Add(OrderBy.Title.ToString(), OrderBy.Title.ToString());
            myOrderByOptionsDictionary.Add(OrderBy.ClassCode.ToString(), OrderBy.ClassCode.ToString());
            myOrderByOptionsDictionary.Add("Lowest Price", OrderBy.LowestPrice.ToString());
            myOrderByOptionsDictionary.Add("Highest Price", OrderBy.HighestPrice.ToString());
            return myOrderByOptionsDictionary;
        }

        private IDictionary<string, string> SearchByForItems() {
            IDictionary<string, string> mySearchByOptionsDictionary = new Dictionary<string, string>();
            mySearchByOptionsDictionary.Add(SearchBy.Title.ToString(), SearchBy.Title.ToString());
            return mySearchByOptionsDictionary;
        }

        private IDictionary<string, string> OrderByForItems() {
            IDictionary<string, string> myOrderByOptionsDictionary = new Dictionary<string, string>();
            myOrderByOptionsDictionary.Add(OrderBy.Newest.ToString(), OrderBy.Newest.ToString());
            myOrderByOptionsDictionary.Add(OrderBy.Title.ToString(), OrderBy.Title.ToString());
            myOrderByOptionsDictionary.Add("Lowest Price", OrderBy.LowestPrice.ToString());
            myOrderByOptionsDictionary.Add("Highest Price", OrderBy.HighestPrice.ToString());
            return myOrderByOptionsDictionary;
        }
    }
}
