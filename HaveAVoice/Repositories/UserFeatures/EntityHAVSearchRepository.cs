using System.Linq;
using HaveAVoice.Models;
using System.Collections;
using System.Collections.Generic;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVSearchRepository : IHAVSearchRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<User> UserSearch(string aSearchString) {
            return (from u in theEntities.Users
                    where u.FirstName.Contains(aSearchString)
                    || u.LastName.Contains(aSearchString)
                    orderby u.LastName
                    select u).Take(10);
        }

        public IEnumerable<Issue> IssueSearch(string aSearchString) {
            return (from i in theEntities.Issues
                    where i.Title.Contains(aSearchString)
                    orderby i.DateTimeStamp
                    select i).Take(10);
        }
    }
}