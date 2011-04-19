using System.Collections.Generic;
using Social.BaseWebsite.Models;

namespace UniversityOfMe.Models.View {
    public class LoggedInListModel<T> : ILoggedInListModel<T> {
        private IEnumerable<T> Models;

        public IEnumerable<T> Get() {
            return Models;
        }

        public void Set(IEnumerable<T> aModel) {
            Models = aModel;
        }
    }
}