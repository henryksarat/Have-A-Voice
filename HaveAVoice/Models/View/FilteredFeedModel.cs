using System.Collections.Generic;
using System.Web.Mvc;
using Social.Generic.Constants;

namespace HaveAVoice.Models.View {
    public class FilteredFeedModel {
        public NavigationModel NavigationModel { get; private set; }
        public IEnumerable<FeedModel> FeedModels { get; set; }
        public IEnumerable<SelectListItem> States { get; private set; }

        public FilteredFeedModel(User aUser) {
            NavigationModel = new NavigationModel(aUser, SiteSection.Home);
            FeedModels = new List<FeedModel>();
            States = new SelectList(UnitedStates.STATES, Constants.SELECT);
        }
    }
}