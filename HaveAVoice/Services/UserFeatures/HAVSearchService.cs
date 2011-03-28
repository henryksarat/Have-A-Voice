using System.Collections.Generic;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVSearchService : IHAVSearchService {
        private IHAVSearchRepository theRepository;

        public HAVSearchService()
            : this(new EntityHAVSearchRepository()) { }

        public HAVSearchService(IHAVSearchRepository aRepository) {
            theRepository = aRepository;
        }

        public string UserSearch(string aSearchString) {
            string mySearchResult = string.Empty;

            IEnumerable<User> myUsers = theRepository.UserSearch(aSearchString);

            foreach (User myUser in myUsers) {
                mySearchResult += string.Format("{0}|\r\n", NameHelper.FullName(myUser));
            }

            return mySearchResult;
        }

        public string IssueSearch(string aSearchString) {
            string mySearchResult = string.Empty;

            IEnumerable<Issue> myIssues = theRepository.IssueSearch(aSearchString);

            foreach (Issue myIssue in myIssues) {
                mySearchResult += string.Format("{0}|\r\n", myIssue.Title);
            }

            return mySearchResult;
        }
    }
}
