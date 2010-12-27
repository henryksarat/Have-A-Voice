using System.Linq;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVSearchRepository : IHAVSearchRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public string SearchResult(string aSearchString) {

            string searchResult = string.Empty;

            var results = (from a in theEntities.Issues
                           where a.Title.Contains(aSearchString)
                           orderby a.Title
                           select a).Take(10);

            foreach(Issue issue in results) {
                searchResult += string.Format("{0}|\r\n", issue.Title);   
            }

            return searchResult;
        }
    }
}