using System.Collections.Generic;
using HaveAVoice.Models;
namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVSearchRepository {
        IEnumerable<User> UserSearch(string aSearchString);
        IEnumerable<Issue> IssueSearch(string aSearchString);
    }
}