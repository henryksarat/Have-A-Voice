using System.Linq;

namespace HaveAVoice.Models.Repositories.UserFeatures {
    public class EntityHAVSearchRepository : HAVBaseRepository, IHAVSearchRepository {

        public string SearchResult(string aSearchString) {

            string searchResult = string.Empty;

            var results = (from a in GetEntities().Issues
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