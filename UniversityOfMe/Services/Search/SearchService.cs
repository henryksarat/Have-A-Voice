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

namespace UniversityOfMe.Services.Search {
    public class SearchService : ISearchService {
        private static DateTime OLD_DATE = new DateTime(1987, 05, 03);

        private ISearchRepository theSearchRepository;

        public SearchService()
            : this(new EntitySearchRepository()) { }

        public SearchService(ISearchRepository aSearchRepo) {
            theSearchRepository = aSearchRepo;
        }

        public SearchResultsModel GetAllSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, int aPage) {
            string myUniversityId = UniversityHelper.GetMainUniversity(aUserInformation.Details).Id;
            IEnumerable<Class> myClassResult = theSearchRepository.GetClassByTitle(myUniversityId, aSearchString);
            IEnumerable<Professor> myProfessorResult = theSearchRepository.GetProfessorByName(myUniversityId, aSearchString);
            IEnumerable<GeneralPosting> myGeneralPostings = theSearchRepository.GetGeneralPostingByTitle(myUniversityId, aSearchString);
            IEnumerable<User> myUsers = theSearchRepository.GetUserByName(aUserInformation.UserId, aSearchString);
            IEnumerable<Event> myEvents = theSearchRepository.GetEventByTitle(myUniversityId, aSearchString);
            IEnumerable<Club> myOrganizations = theSearchRepository.GetOrganizationByName(myUniversityId, aSearchString);
            IEnumerable<TextBook> myTextBooks = theSearchRepository.GetTextBookByTitle(myUniversityId, aSearchString);

            List<ISearchResult> mySearchResult = new List<ISearchResult>();

            mySearchResult.AddRange(myUsers.Select(r => new UserSearchResult(r) {
                UserInformationModel = aUserInformation
            }));
            mySearchResult.AddRange(myOrganizations.Select(r => new OrganizationSearchResult(r) {
                UserInformationModel = aUserInformation
            }));
            mySearchResult.AddRange(myClassResult.Select(r => new ClassSearchResult(r)));
            mySearchResult.AddRange(myProfessorResult.Select(r => new ProfessorSearchResult(r)));
            mySearchResult.AddRange(myGeneralPostings.Select(r => new GeneralPostingSearchResult(r)));
            mySearchResult.AddRange(myEvents.Select(r => new EventSearchResult(r) {
                UserInformationModel = aUserInformation
            }));
            mySearchResult.AddRange(myTextBooks.Select(r => new TextBookSearchResult(r) {
                UserInformationModel = aUserInformation
            }));

            int myTotalResults = myClassResult.Count<Class>() + myProfessorResult.Count<Professor>() + myGeneralPostings.Count<GeneralPosting>() +
                                 myUsers.Count<User>() + myEvents.Count<Event>() + myOrganizations.Count<Club>() + myTextBooks.Count<TextBook>();
            SearchResultsModel myModel = 
                BuildSearchResultsModel(mySearchResult, SearchFilter.All, aPage, 
                                        aSearchString, myTotalResults, SearchByForAll(), 
                                        SearchBy.None, OrderByForAll(), OrderBy.None);
            return myModel;
        }

        public SearchResultsModel GetProfessorSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, 
                                                            int aPage, SearchBy aSearchBy, OrderBy anOrderBy) {
            string myUniversityId = UniversityHelper.GetMainUniversity(aUserInformation.Details).Id;
            IEnumerable<Professor> myProfessors = new List<Professor>();

            if (aSearchBy == SearchBy.Name) {
                myProfessors = theSearchRepository.GetProfessorByName(myUniversityId, aSearchString);
            }

            if (anOrderBy == OrderBy.Name) {
                myProfessors = myProfessors.OrderBy(r => r.FirstName + " " + r.LastName);
            }

            List<ISearchResult> mySearchResult = new List<ISearchResult>();

            mySearchResult.AddRange(myProfessors.Select(r => new ProfessorSearchResult(r)));

            int myTotalResults = myProfessors.Count<Professor>();

            SearchResultsModel myModel = 
                BuildSearchResultsModel(mySearchResult, SearchFilter.Professor, aPage, 
                                        aSearchString, myTotalResults, SearchByForProfessor(), 
                                        aSearchBy, OrderByForProfessor(), anOrderBy);
            return myModel;
        }

        public SearchResultsModel GetClassSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, 
                                                        int aPage, SearchBy aSearchBy, OrderBy anOrderBy) {
            string myUniversityId = UniversityHelper.GetMainUniversity(aUserInformation.Details).Id;
            IEnumerable<Class> myClasses = new List<Class>();
            if (aSearchBy == SearchBy.Title) {
                myClasses = theSearchRepository.GetClassByTitle(myUniversityId, aSearchString);
            } else if (aSearchBy == SearchBy.ClassCode) {
                myClasses = theSearchRepository.GetClassByClassCode(myUniversityId, aSearchString);
            }

            if (anOrderBy == OrderBy.Title) {
                myClasses = myClasses.OrderBy(r => r.ClassTitle);
            } else if (anOrderBy == OrderBy.ClassCode) {
                myClasses = myClasses.OrderBy(r => r.ClassCode);
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
                BuildSearchResultsModel(mySearchResult, SearchFilter.Class, aPage, aSearchString, 
                                        myTotalResults, SearchByForClass(), aSearchBy,
                                        OrderByForClass(), anOrderBy);
            return myModel;
        }

        public SearchResultsModel GetEventSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, 
                                                        int aPage, SearchBy aSearchBy, OrderBy anOrderBy) {
            string myUniversityId = UniversityHelper.GetMainUniversity(aUserInformation.Details).Id;
            IEnumerable<Event> myEvents = new List<Event>();

            if (aSearchBy == SearchBy.Title) {
                myEvents = theSearchRepository.GetEventByTitle(myUniversityId, aSearchString);
            } else if (aSearchBy == SearchBy.Description) {
                myEvents = theSearchRepository.GetEventByInformation(myUniversityId, aSearchString);
            }
            if(anOrderBy == OrderBy.ClosestStartDate) {
                myEvents = myEvents.OrderBy(r => r.StartDate);
            } else if (anOrderBy == OrderBy.Title) {
                myEvents = myEvents.OrderBy(r => r.Title);
            } else if (anOrderBy == OrderBy.LatestPost) {
                myEvents = myEvents.OrderByDescending(
                    r => r.EventBoards
                        .OrderByDescending(r2 => r2.DateTimeStamp)
                        .FirstOrDefault<EventBoard>() != null ?
                        r.EventBoards
                        .OrderByDescending(r2 => r2.DateTimeStamp)
                        .FirstOrDefault<EventBoard>().DateTimeStamp :
                        OLD_DATE
                    );
            } else if (anOrderBy == OrderBy.HighestAttendingMembers) {
                myEvents = myEvents.OrderByDescending(r => r.EventAttendences.Count);
            } else if (anOrderBy == OrderBy.LowestAttendingMembers) {
                myEvents = myEvents.OrderBy(r => r.EventAttendences.Count);
            }

            User myUser = aUserInformation.Details;

            myEvents = (from e in myEvents
                        where (e.EntireSchool == true
                        || (!e.EntireSchool && FriendHelper.IsFriend(myUser, e.User)))
                        select e);

            List<ISearchResult> mySearchResult = new List<ISearchResult>();

            mySearchResult.AddRange(myEvents.Select(r => new EventSearchResult(r) {
                UserInformationModel = aUserInformation
            }));

            int myTotalResults = myEvents.Count<Event>();

            SearchResultsModel myModel = 
                BuildSearchResultsModel(mySearchResult, SearchFilter.Event, aPage, 
                                        aSearchString, myTotalResults, SearchByForEvent(), 
                                        aSearchBy, OrderByForEvent(), anOrderBy);
            return myModel;
        }

        public SearchResultsModel GetOrganizationSearchResults(UserInformationModel<User> aUserInformation, string aSearchString,
                                                               int aPage, SearchBy aSearchBy, OrderBy anOrderBy) {
            string myUniversityId = UniversityHelper.GetMainUniversity(aUserInformation.Details).Id;
            IEnumerable<Club> myClubs = new List<Club>();

            if (aSearchBy == SearchBy.Name) {
                myClubs = theSearchRepository.GetOrganizationByName(myUniversityId, aSearchString);
            }

            if (anOrderBy == OrderBy.Name) {
                myClubs = myClubs.OrderBy(r => r.Name);
            }

            List<ISearchResult> mySearchResult = new List<ISearchResult>();

            mySearchResult.AddRange(myClubs.Select(r => new OrganizationSearchResult(r) {
                UserInformationModel = aUserInformation
            }));

            int myTotalResults = myClubs.Count<Club>();

            SearchResultsModel myModel =
                BuildSearchResultsModel(mySearchResult, SearchFilter.User, aPage,
                                        aSearchString, myTotalResults, SearchByForOrganization(),
                                        aSearchBy, OrderByForOrganization(), anOrderBy);
            return myModel;
        }

        public SearchResultsModel GetGeneralPostingSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, 
                                                                 int aPage, SearchBy aSearchBy, OrderBy anOrderBy) {
            string myUniversityId = UniversityHelper.GetMainUniversity(aUserInformation.Details).Id;
            IEnumerable<GeneralPosting> myGeneralPostings = new List<GeneralPosting>();

            if (aSearchBy == SearchBy.Title) {
                myGeneralPostings = theSearchRepository.GetGeneralPostingByTitle(myUniversityId, aSearchString);
            } else if (aSearchBy == SearchBy.Description) {
                myGeneralPostings = theSearchRepository.GetGeneralPostingByBody(myUniversityId, aSearchString); 
            }

            if (anOrderBy == OrderBy.Title) {
                myGeneralPostings = myGeneralPostings.OrderBy(r => r.Title);
            } else if (anOrderBy == OrderBy.LatestPost) {
                myGeneralPostings = myGeneralPostings.OrderByDescending(
                    r => r.GeneralPostingReplies
                        .OrderByDescending(r2 => r2.DateTimeStamp)
                        .FirstOrDefault<GeneralPostingReply>() != null ?
                        r.GeneralPostingReplies
                        .OrderByDescending(r2 => r2.DateTimeStamp)
                        .FirstOrDefault<GeneralPostingReply>().DateTimeStamp :
                        r.DateTimeStamp
                    );
            }

            List<ISearchResult> mySearchResult = new List<ISearchResult>();

            mySearchResult.AddRange(myGeneralPostings.Select(r => new GeneralPostingSearchResult(r)));

            int myTotalResults = myGeneralPostings.Count<GeneralPosting>();

            SearchResultsModel myModel = 
                BuildSearchResultsModel(mySearchResult, SearchFilter.GeneralPosting, aPage, 
                                        aSearchString, myTotalResults, SearchByForGeneralPosting(), aSearchBy,
                                        OrderByForGeneralPosting(), anOrderBy);
            return myModel;
        }

        public SearchResultsModel GetTextBookSearchResults(UserInformationModel<User> aUserInformation, string aSearchString, 
                                                           int aPage, SearchBy aSearchBy, OrderBy anOrderBy) {
            string myUniversityId = UniversityHelper.GetMainUniversity(aUserInformation.Details).Id;
            IEnumerable<TextBook> myTextBooks = new List<TextBook>();

            if (aSearchBy == SearchBy.Title) {
                myTextBooks = theSearchRepository.GetTextBookByTitle(myUniversityId, aSearchString);
            } else if (aSearchBy == SearchBy.ClassCode) {
                myTextBooks = theSearchRepository.GetTextBookByClassCode(myUniversityId, aSearchString);
            }

            if (anOrderBy == OrderBy.Title) {
                myTextBooks = myTextBooks.OrderBy(r => r.BookTitle);
            } else if (anOrderBy == OrderBy.ClassCode) {
                myTextBooks = myTextBooks.OrderBy(r => r.ClassCode);
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
                BuildSearchResultsModel(mySearchResult, SearchFilter.Textbook, aPage, 
                                        aSearchString, myTotalResults, SearchByForTextbook(), 
                                        aSearchBy, OrderByForTextbook(), anOrderBy);
            return myModel;
        }

        public SearchResultsModel GetUserSearchResults(UserInformationModel<User> aUserInformation, string aSearchString,
                                                 int aPage, SearchBy aSearchBy, OrderBy anOrderBy) {
            IEnumerable<User> myUsers = new List<User>();

            if (aSearchBy == SearchBy.Name) {
                myUsers = theSearchRepository.GetUserByName(aUserInformation.UserId, aSearchString);
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
                BuildSearchResultsModel(mySearchResult, SearchFilter.User, aPage,
                                        aSearchString, myTotalResults, SearchByForPeople(),
                                        aSearchBy, OrderByForPeople(), anOrderBy);
            return myModel;
        }

        private SearchResultsModel BuildSearchResultsModel(List<ISearchResult> aResults, Helpers.Search.SearchFilter aSearchType,
                        int aPage, string aSearchString, int aTotalResults, IDictionary<string, string> aSearchByOptions,
                        SearchBy aSearchBySelected, IDictionary<string, string> anOrderByOptions, OrderBy anOrderBySelected) {

            int myToGet = (aPage - 1) * SearchConstants.RESULTS_PER_PAGE + SearchConstants.RESULTS_PER_PAGE > aResults.Count
                                ? aTotalResults % SearchConstants.RESULTS_PER_PAGE
                                : SearchConstants.RESULTS_PER_PAGE;
            IEnumerable<ISearchResult> myFinalResultSet = aResults.GetRange((aPage - 1) * SearchConstants.RESULTS_PER_PAGE, myToGet);
            return new SearchResultsModel() {
                SearchType = aSearchType,
                SearchResults = myFinalResultSet,
                CurrentPage = aPage,
                TotalPages = (int)Math.Ceiling((double)aTotalResults / (double)SearchConstants.RESULTS_PER_PAGE),
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
    }
}
