using System.Collections.Generic;
using Social.BaseWebsite.Models;

namespace UniversityOfMe.Models.View {
    public class LoggedInListModel<T> : LoggedInModel, ILoggedInListModel<T> {
        public User User { get; private set; }
        private IEnumerable<T> Models;

        public LoggedInListModel(User aUser) : base(aUser) {
            User = aUser;
        }

        public IEnumerable<T> Get() {
            return Models;
        }

        public void Set(IEnumerable<T> aModel) {
            Models = aModel;
        }
    }
}